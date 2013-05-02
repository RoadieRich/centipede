using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;
using Centipede.Properties;
using CentipedeInterfaces;
using PythonEngine;
using ResharperAnnotations;
using PyEngine = PythonEngine.PythonEngine;

//  LINQ
//   \o/
// All the
// things

namespace Centipede
{
    public sealed class CentipedeCore : ICentipedeCore
    {
        #region Constructors

        public CentipedeCore(Dictionary<string, string> arguments)
        {
            Variables = PythonEngine.GetScope();
            
            Job = new CentipedeJob();
            this._arguments = arguments;
        }

        #endregion Constructors

        #region Events

        public event ActionErrorEvent ActionErrorOccurred;
        public event AfterLoadEvent AfterLoad;
        public event JobCompletedEvent JobCompleted;
        public event ActionEvent ActionAdded;
        public event ActionEvent ActionCompleted;
        public event ActionEvent ActionRemoved;
        public event ActionEvent BeforeAction;

        #endregion

        #region Fields

        public IAction CurrentAction { get; set; }

        [UsedImplicitly]
        private Dictionary<string, string> _arguments;

        #endregion

        #region Properties

        public CentipedeJob Job { get; set; }
        
        /// <summary>
        ///     Dictionary of Variables for use by actions
        /// </summary>
        public PythonScope Variables { get; private set; }

        public Form Window { get; set; }

        #endregion

        #region Methods

        public void Dispose()
        {
            foreach (IDisposable action in Job.Actions)
            {
                action.Dispose();
            }
        }

        /// <summary>
        ///     Run the job, starting with the first action added.
        /// </summary>
        [STAThread]
        public void RunJob(bool stepping = false)
        {
            PrepareStepping(stepping);

            _abortRequested = false;
            
            OnStartRun(new StartRunEventArgs(this._resetEvent));
            
            try
            {
                CurrentAction = Job.Actions.First();
            }
            catch (InvalidOperationException)
            {
                OnActionError(new ActionErrorEventArgs
                {
                    Action = CurrentAction,
                    Exception = new FatalActionException(Resources.Program_RunJob_No_Actions_Added)
                });
            }

            Boolean jobFailed = false;
            try
            {
                while (CurrentAction != null)
                {
                    ShouldPauseStepping();

                    ContinueState continueState = RunStep(CurrentAction);
                    
                    if (NeedsRetry(continueState))
                        continue;

                    ResetSteppingPause();

                    lock (this._abortRequested)
                    {
                        if ((bool)this._abortRequested)
                        {
                            throw new AbortOperationException();
                        }
                    }
                }
            }

            catch (AbortOperationException)
            {
                jobFailed = true;
            }
            OnCompleted(!jobFailed);
            FinishedStepping();
        }

        private void FinishedStepping()
        {
            IsStepping = false;
            this._resetEvent = null;
        }

        private void PrepareStepping(bool stepping)
        {
            IsStepping = stepping;
            if (stepping)
            {
                this._resetEvent = new ManualResetEvent(true);
            }
        }

        private void ResetSteppingPause()
        {
            if (IsStepping)
            {
                this._resetEvent.Reset();
            }
        }

        private void ShouldPauseStepping()
        {
            if (IsStepping)
            {
                this._resetEvent.WaitOne();
            }
        }

        private bool NeedsRetry(ContinueState continueState)
        {
            switch (continueState)
            {
            case ContinueState.Abort:
                throw new AbortOperationException();
            case ContinueState.Retry:
                return true;
            default:
                CurrentAction = CurrentAction.GetNext();
                return false;
            }
        }

        public event StartSteppingEvent StartRun;

        private void OnStartRun(StartRunEventArgs e)
        {
            if(e.ResetEvent == null)
                return;
            StartSteppingEvent handler = StartRun;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        [STAThread]
        private ContinueState RunStep(IAction currentAction)
        {
            try
            {
                ActionEventArgs args = new ActionEventArgs
                                       {
                                           Action = currentAction,
                                           Stepping = IsStepping
                                       };
                
                OnBeforeAction(args);
                currentAction.Run();
                OnAfterAction(args);

                return ContinueState.Continue;

            }
            catch (FatalActionException e)
            {
                ActionErrorEventArgs args = new ActionErrorEventArgs
                                            {
                                                Action = currentAction,
                                                Exception = new FatalActionException(
                                                    string.Format(Resources.CentipedeCore_RunJob_FatalError,
                                                                  e.Message, e)),
                                                Fatal = true
                                            };
                OnActionError(args);
                throw new AbortOperationException();
            }
            catch (Exception e)
            {
                ActionErrorEventArgs args = new ActionErrorEventArgs
                                            {
                                                Action = currentAction,
                                                Exception = (e as ActionException) ??
                                                            new ActionException(e, CurrentAction)
                                            };
                OnActionError(args);
                
                return args.Continue;
            }
        }

        private volatile Object _abortRequested = false;
        private IPythonEngine _pythonEngine = PyEngine.Instance;
        private ManualResetEvent _resetEvent;

        public bool IsStepping { get; private set; }

        public void AbortRun()
        {
            lock (_abortRequested)
            {
                _abortRequested = true;
            }
        }

        public IPythonEngine PythonEngine
        {
            get
            {
                return this._pythonEngine;
            }
            private set
            {
                this._pythonEngine = value;
            }
        }

        private void OnAfterAction(ActionEventArgs args)
        {
            ActionEvent handler = ActionCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnBeforeAction(ActionEventArgs e)
        {
            ActionEvent handler = BeforeAction;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnCompleted(bool completed)
        {
            JobCompletedEvent handler = JobCompleted;
            if (handler != null)
            {
                handler(this, new JobCompletedEventArgs { Completed = completed });
            }
        }

        private void OnActionRemoved(ActionEventArgs args)
        {
            var handler = ActionRemoved;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnActionError(ActionErrorEventArgs args)
        {
            ActionErrorEvent handler = ActionErrorOccurred;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        #region Action Methods

        /// <summary>
        ///     Add action to the job queue.  By default, it is added as the last action in the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="action">Action to add</param>
        /// <param name="index">(Optional) Index to add action at.  Defaults to end (-1).</param>
        public void AddAction(CentipedeJob job, IAction action, Int32 index = -1)
        {
            switch (index)
            {
            case -1:
                if (job.Actions.Count > 0)
                {
                    IAction last = job.Actions[job.Actions.Count - 1];

                    last.Next = action;
                }
                job.Actions.Add(action);
                index = Job.Actions.Count - 1;
                break;
            case 0:
                {
                    IAction oldFirst = job.Actions[0];
                    job.Actions.Insert(0, action);
                    action.Next = oldFirst;
                }
                break;
            default:
                {
                    IAction prevAction = job.Actions[index - 1];
                    IAction nextAction = job.Actions[index];
                    prevAction.Next = action;
                    action.Next = nextAction;
                    job.Actions.Insert(index, action);
                }
                break;
            }
            OnActionAdded(new ActionEventArgs
                          {
                              Action = action,
                              Index = index
                          });

        }

        private void OnActionAdded(ActionEventArgs args)
        {
            ActionEvent handler = ActionAdded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public void AddAction(IAction action, Int32 index = -1)
        {
            AddAction(Job, action, index);
        }

        /// <summary>
        ///     Remove action from the current job
        /// </summary>
        /// <param name="action"><see cref="Action"/> to remove</param>
        public void RemoveAction(IAction action)
        {
            IAction prevAction = Job.Actions.SingleOrDefault(a => a.Next == action);

            IAction nextAction = action.Next;

            if (prevAction != null)
            {
                prevAction.Next = nextAction;
            }
            Job.Actions.Remove(action);
            {

                OnActionRemoved(new ActionEventArgs
                                {
                                    Action = action
                                });
            }
        }

        public void Clear()
        {
            Variables.Clear();
            while (Job.Actions.Count > 0)
            {
                RemoveAction(Job.Actions.First());
            }
            Job.Author        = Settings.Default.DefaultAuthor;
            Job.AuthorContact = Settings.Default.DefaultContact;
            Job.FileName      = "";
            Job.InfoUrl       = "";
            Job.Name          = "";
            OnAfterLoad(EventArgs.Empty);
            
        }

        #endregion

        #region Save and Load methods

        /// <summary>
        ///     Save job
        /// </summary>
        /// <param name="filename">filename to save to</param>
        [Localizable(false)]
        public void SaveJob(String filename)
        {

            TakeBackup(filename);
            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot      = xmlDoc.CreateElement("CentipedeJob");
            XmlElement metaElement1 = xmlDoc.CreateElement("Metadata");
            XmlElement metaElement2 = xmlDoc.CreateElement("Name");
            
            metaElement2.AppendChild(xmlDoc.CreateTextNode(Job.Name));
            metaElement1.AppendChild(metaElement2);

            metaElement2            = xmlDoc.CreateElement("Author");
            XmlElement metaElement3 = xmlDoc.CreateElement("Name");
            metaElement3.AppendChild(xmlDoc.CreateTextNode(Job.Author));
            metaElement2.AppendChild(metaElement3);
            metaElement3            = xmlDoc.CreateElement("Contact");
            metaElement3.AppendChild(xmlDoc.CreateTextNode(Job.AuthorContact));
            metaElement2.AppendChild(metaElement3);
            metaElement1.AppendChild(metaElement2);

            metaElement2            = xmlDoc.CreateElement("Info");
            metaElement2.SetAttribute("Url", Job.InfoUrl);
            metaElement1.AppendChild(metaElement2);

            xmlRoot.AppendChild(metaElement1);
            
            xmlDoc.AppendChild(xmlRoot);

            XmlElement actionsElement = xmlDoc.CreateElement("Actions");
            xmlRoot.AppendChild(actionsElement);

            foreach (Action action in Job.Actions)
            {
                action.AddToXmlElement(actionsElement);
            }

            var settings = new XmlWriterSettings
                           {
                               Indent = true,
                               IndentChars = "  ",
                               NewLineChars = "\r\n",
                               NewLineHandling = NewLineHandling.Replace
                           };

            using (XmlWriter w = XmlWriter.Create(filename, settings))
            {
                xmlDoc.WriteTo(w);
            }
            Job.FileName = filename;
        }

        private void TakeBackup(string filePath)
        {
            if (!File.Exists(filePath))
            {
                return;
            }

            string directory = Path.GetDirectoryName(filePath);
            string filename = Path.GetFileName(filePath);
            string backupDir = Path.Combine(directory, @"backups");

            DirectoryInfo dir;
            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }
            
            dir = new DirectoryInfo(backupDir);
            int numberOfBackups = dir.GetFiles().Length;



            string backupFilename = string.Format(@"{0}.{1}", filename, numberOfBackups);

            File.Copy(filePath, Path.Combine(backupDir, backupFilename));

        }

        /// <summary>
        ///     Load a job with a given name
        /// </summary>
        /// <param name="jobFileName">Name of the job to load</param>
        [Localizable(false)]
        public void LoadJob(string jobFileName)
        {

            Clear();
            
            //var xmlDoc = new XmlDocument();
            if (!File.Exists(jobFileName))
            {
                throw new FileNotFoundException();
            }


            XPathDocument xPathDoc = new XPathDocument(jobFileName);
            XPathNavigator nav = xPathDoc.CreateNavigator();
            CentipedeJob job = new CentipedeJob(jobFileName, nav) { FileName = jobFileName };

            var it = nav.Select("//Actions/*");

            foreach (XPathNavigator actionElement in it)
            {
                AddAction(job, Action.FromXml(actionElement, Variables, this));
            }

            Job = job;
            
            OnAfterLoad(EventArgs.Empty);
        }

        private void OnAfterLoad(EventArgs eventArgs)
        {
            AfterLoadEvent handler = AfterLoad;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        #endregion

        #endregion
    }
}
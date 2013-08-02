using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Policy;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using Centipede.Actions;
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

        public CentipedeCore(List<string> arguments)
        {
            this.Variables = this.PythonEngine.GetScope();

            this.Job = new CentipedeJob();
            this._arguments = arguments.Where(String.IsNullOrWhiteSpace).ToList();
        }

        #endregion Constructors

        #region Events

        public event ActionEvent ActionAdded;

        private void OnActionAdded(ActionEventArgs args)
        {
            ActionEvent handler = this.ActionAdded;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event ActionEvent ActionCompleted;

        private void OnActionCompleted(ActionEventArgs args)
        {
            ActionEvent handler = this.ActionCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event ActionEvent ActionRemoved;

        private void OnActionRemoved(ActionEventArgs args)
        {
            ActionEvent handler = this.ActionRemoved;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event ActionEvent BeforeAction;

        private void OnBeforeAction(ActionEventArgs e)
        {
            ActionEvent handler = this.BeforeAction;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        public event ActionErrorEvent ActionErrorOccurred;

        private void OnActionErrorOccurred(ActionErrorEventArgs args)
        {
            ActionErrorEvent handler = this.ActionErrorOccurred;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        public event AfterLoadEvent AfterLoad;

        private void OnAfterLoad(EventArgs eventArgs)
        {
            AfterLoadEvent handler = this.AfterLoad;
            if (handler != null)
            {
                handler(this, eventArgs);
            }
        }

        public event JobCompletedEvent JobCompleted;

        private void OnJobCompleted(bool completed)
        {
            JobCompletedEvent handler = this.JobCompleted;
            if (handler != null)
            {
                handler(this,
                        new JobCompletedEventArgs
                        {
                            Completed = completed
                        });
            }
        }

        public event StartSteppingEvent StartRun;

        private void OnStartRun(StartRunEventArgs e)
        {
            if (e.ResetEvent == null)
            {
                return;
            }
            StartSteppingEvent handler = this.StartRun;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion

        #region Fields

        private readonly List<string> _arguments;

        private volatile Object _abortRequested = false;

        private IPythonEngine _pythonEngine = PyEngine.Instance;

        private ManualResetEvent _resetEvent;

        private Dictionary<FileInfo, List<Type>> _pluginFiles = new Dictionary<FileInfo, List<Type>>();

        #endregion

        #region Properties

        public IAction CurrentAction { get; set; }

        public CentipedeJob Job { get; set; }

        /// <summary>
        ///     Dictionary of Variables for use by actions
        /// </summary>
        public IPythonScope Variables { get; private set; }

        public object Tag { get; set; }

        public Dictionary<FileInfo, List<Type>> PluginFiles
        {
            get { return this._pluginFiles; }
            private set { this._pluginFiles = value; }
        }

        public List<string> Arguments { get { return _arguments; } }

        public IPythonEngine PythonEngine
        {
            get { return this._pythonEngine; }
            private set { this._pythonEngine = value; }
        }

        public bool IsStepping { get; private set; }

        #endregion

        #region Methods

        public void LoadActionPlugins()
        {
            var dir = new DirectoryInfo(Path.Combine(Application.StartupPath, Settings.Default.PluginFolder));

            IEnumerable<FileInfo> dlls = dir.EnumerateFiles(@"*.dll", SearchOption.AllDirectories);

            List<Type> actions = new List<Type>();
            foreach (FileInfo fi in dlls)
            {
                var evidence = new Evidence();
                var appDir = new ApplicationDirectory(Assembly.GetEntryAssembly().CodeBase);
                evidence.AddAssemblyEvidence(appDir);
                Assembly asm;
                try
                {
                    asm = Assembly.LoadFrom(fi.FullName);
                }
                catch (BadImageFormatException)
                {
                    continue;
                }

                Type[] typesInFile = asm.GetExportedTypes();

                var actionTypes = (from type in typesInFile
                                   where !type.IsAbstract
                                   where type.GetCustomAttributes(typeof(ActionCategoryAttribute), true).Any()
                                   select type);

                List<Type> actionTypeList = actionTypes.ToList();
                if (!actionTypeList.Any())
                {
                    continue;
                }
                
                this.PluginFiles.Add(fi, actionTypeList);

                var customDataTypes = from type in typesInFile
                                      where !type.IsAbstract
                                      where type.IsAssignableFrom(typeof(ICentipedeDataType))
                                      select type;

                foreach (Type t in customDataTypes)
                {
                    CentipedeSerializer.RegisterSerializableType(t);
                }
            }
        }

        public void Dispose()
        {
            foreach (IDisposable action in this.Job.Actions)
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
            this.PrepareStepping(stepping);

            this._abortRequested = false;

            this.OnStartRun(new StartRunEventArgs(this._resetEvent));

            try
            {
                this.CurrentAction = this.Job.Actions.First();
            }
            catch (InvalidOperationException)
            {
                this.OnActionErrorOccurred(new ActionErrorEventArgs
                                   {
                                       Action = this.CurrentAction,
                                       Exception = new FatalActionException(Resources.Program_RunJob_No_Actions_Added)
                                   });
            }

            Boolean jobFailed = false;
            try
            {
                while (this.CurrentAction != null)
                {
                    this.ShouldPauseStepping();

                    ContinueState continueState = this.RunStep(this.CurrentAction);

                    if (this.NeedsRetry(continueState))
                    {
                        continue;
                    }

                    this.ResetSteppingPause();

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
            this.OnJobCompleted(!jobFailed);
            this.FinishedStepping();
        }

        public void AbortRun()
        {
            lock (this._abortRequested)
            {
                this._abortRequested = true;
            }
        }

        private void FinishedStepping()
        {
            this.IsStepping = false;
            this._resetEvent = null;
        }

        private void PrepareStepping(bool stepping)
        {
            this.IsStepping = stepping;
            if (stepping)
            {
                this._resetEvent = new ManualResetEvent(true);
            }
        }

        private void ResetSteppingPause()
        {
            if (this.IsStepping)
            {
                this._resetEvent.Reset();
            }
        }

        private void ShouldPauseStepping()
        {
            if (this.IsStepping)
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
                    this.CurrentAction = this.CurrentAction.GetNext();
                    return false;
            }
        }

        [STAThread]
        private ContinueState RunStep(IAction currentAction)
        {
            try
            {
                var args = new ActionEventArgs
                           {
                               Action = currentAction,
                               Stepping = this.IsStepping
                           };

                this.OnBeforeAction(args);
                currentAction.Run();
                this.OnActionCompleted(args);

                return ContinueState.Continue;
            }
            catch (FatalActionException e)
            {
                var args = new ActionErrorEventArgs
                           {
                               Action = currentAction,
                               Exception = new FatalActionException(
                                   string.Format(Resources.CentipedeCore_RunJob_FatalError,
                                                 e.Message,
                                                 e)),
                               Fatal = true
                           };
                this.OnActionErrorOccurred(args);
                throw new AbortOperationException();
            }
            catch (Exception e)
            {
                var args = new ActionErrorEventArgs
                           {
                               Action = currentAction,
                               Exception = (e as ActionException) ??
                                           new ActionException(e, this.CurrentAction)
                           };
                this.OnActionErrorOccurred(args);

                return args.Continue;
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
                    index = this.Job.Actions.Count - 1;
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
            this.OnActionAdded(new ActionEventArgs
                               {
                                   Action = action,
                                   Index = index,
                                   LoadedSuccessfully = !(action is MissingAction)
                               });
        }

        public void AddAction(IAction action, Int32 index = -1)
        {
            this.AddAction(this.Job, action, index);
        }

        /// <summary>
        ///     Remove action from the current job
        /// </summary>
        /// <param name="action">
        ///     <see cref="Action" /> to remove
        /// </param>
        public void RemoveAction(IAction action)
        {
            IAction prevAction = this.Job.Actions.SingleOrDefault(a => a.Next == action);

            IAction nextAction = action.Next;

            if (prevAction != null)
            {
                prevAction.Next = nextAction;
            }
            this.Job.Actions.Remove(action);
            {
                this.OnActionRemoved(new ActionEventArgs
                                     {
                                         Action = action
                                     });
            }
        }

        public void Clear()
        {
            ((IDictionary<String, Object>)this.Variables).Clear();
            while (this.Job.Actions.Count > 0)
            {
                this.RemoveAction(this.Job.Actions.First());
            }
            this.Job.Author = Settings.Default.DefaultAuthor;
            this.Job.AuthorContact = Settings.Default.DefaultContact;
            this.Job.FileName = "";
            this.Job.InfoUrl = "";
            this.Job.Name = "";
            this.OnAfterLoad(EventArgs.Empty);
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
            this.TakeBackup(filename);
            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("CentipedeJob");
            XmlElement metaElement1 = xmlDoc.CreateElement("Metadata");
            XmlElement metaElement2 = xmlDoc.CreateElement("Name");

            metaElement2.AppendChild(xmlDoc.CreateTextNode(this.Job.Name));
            metaElement1.AppendChild(metaElement2);

            metaElement2 = xmlDoc.CreateElement("Author");
            XmlElement metaElement3 = xmlDoc.CreateElement("Name");
            metaElement3.AppendChild(xmlDoc.CreateTextNode(this.Job.Author));
            metaElement2.AppendChild(metaElement3);
            metaElement3 = xmlDoc.CreateElement("Contact");
            metaElement3.AppendChild(xmlDoc.CreateTextNode(this.Job.AuthorContact));
            metaElement2.AppendChild(metaElement3);
            metaElement1.AppendChild(metaElement2);

            metaElement2 = xmlDoc.CreateElement("Info");
            metaElement2.SetAttribute("Url", this.Job.InfoUrl);
            metaElement1.AppendChild(metaElement2);

            xmlRoot.AppendChild(metaElement1);

            xmlDoc.AppendChild(xmlRoot);

            XmlElement actionsElement = xmlDoc.CreateElement("Actions");
            xmlRoot.AppendChild(actionsElement);

            foreach (Action action in this.Job.Actions)
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
            this.Job.FileName = filename;
        }

        /// <summary>
        ///     Load a job with a given name
        /// </summary>
        /// <param name="jobFileName">Name of the job to load</param>
        [Localizable(false)]
        public void LoadJob(string jobFileName)
        {
            this.Clear();

            //var xmlDoc = new XmlDocument();
            if (!File.Exists(jobFileName))
            {
                throw new FileNotFoundException();
            }

            var doc = new XmlDocument();
            doc.Load(jobFileName);

            //XPathDocument xPathDoc = new XPathDocument(jobFileName);
            //XPathNavigator nav = xPathDoc.CreateNavigator();
            var job = new CentipedeJob(jobFileName, doc)
                      {
                          FileName = jobFileName
                      };

            //var it = nav.Select("//Actions/*");

            IEnumerable<XmlElement> it = doc.GetElementsByTagName("Actions")[0].ChildNodes.OfType<XmlElement>();

            foreach (XmlElement actionElement in it)
            {
                AddAction(job, Action.FromXml(actionElement, this.Variables, this));
            }

            this.Job = job;

            this.OnAfterLoad(EventArgs.Empty);
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

            if (!Directory.Exists(backupDir))
            {
                Directory.CreateDirectory(backupDir);
            }

            string backupFilename = string.Format(@"{0}-{1}{2}",
                                                  Path.GetFileNameWithoutExtension(filename),
                                                  DateTime.Now.ToString("yyyy'-'MM'-'dd'('ddd')-'HH'-'mm'-'ss"),
                                                  Path.GetExtension(filename));

            File.Copy(filePath, Path.Combine(backupDir, backupFilename));
        }

        #endregion

        #endregion
    }
}

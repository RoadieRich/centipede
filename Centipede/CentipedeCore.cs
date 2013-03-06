using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using Centipede.Properties;
using CentipedeInterfaces;
using ResharperAnnotations;

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
            Variables = new VariablesTable();
            
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
        ///     Dictionary of Variables for use by actions.  As much as I'd like to make types more intuitive,
        ///     I can't figure a way of doing it easily.
        /// </summary>
        public VariablesTable Variables { get; private set; }

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
            Mutex steppingPauseMutex = new Mutex();
            if (stepping)
            {
                OnStartStepping(new StartSteppingEventArgs(steppingPauseMutex));
            }

            if (!Job.Actions.Any())
            {
                ActionErrorEventArgs args = new ActionErrorEventArgs
                                            {
                                                    Action = this.CurrentAction,
                                                    Exception =
                                                            new ActionException(
                                                            Resources.Program_RunJob_No_Actions_Added)
                                            };
                OnActionError(args);

            }
            
            this.CurrentAction = Job.Actions.First();

            Boolean jobFailed = false;

            try
            {
                while (this.CurrentAction != null)
                {

                    ContinueState = ContinueState.Continue;

                    RunStep(this.CurrentAction);

                    if (stepping)
                    {
                        //something
                    }

                    switch (ContinueState)
                    {
                    case ContinueState.Abort:
                        throw new AbortOperationException();
                    case ContinueState.Retry:
                        continue;
                    case ContinueState.Continue:
                        this.CurrentAction = this.CurrentAction.GetNext();
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                    }
                }
            }

            catch (AbortOperationException)
            {
                jobFailed = true;
            }
            OnCompleted(!jobFailed);
        }

        public event StartSteppingEvent StartStepping;

        private void OnStartStepping(StartSteppingEventArgs e)
        {
            StartSteppingEvent handler = this.StartStepping;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void RunStep(IAction currentAction)
        {
            try
            {
                ActionEventArgs args = new ActionEventArgs
                                       {
                                            Action = currentAction
                                       };

                OnBeforeAction(args);
                currentAction.Run();
                OnAfterAction(args);

            }
            catch (FatalActionException e)
            {
                ActionErrorEventArgs args = new ActionErrorEventArgs
                                            {
                                                    Action = currentAction,
                                                    Exception =
                                                            new FatalActionException(
                                                            string.Format(Resources.CentipedeCore_RunJob_FatalError,
                                                                          e.Message, e))
                                            };
                OnActionError(args);
                throw new AbortOperationException();
            }
            catch (Exception e)
            {
                ActionErrorEventArgs args = new ActionErrorEventArgs()
                                            {
                                                    Action = currentAction,
                                                    Exception =
                                                            (e as ActionException) ?? new ActionException(e, this.CurrentAction)
                                            };
                OnActionError(args);
                
                this.ContinueState = args.Continue;
            }
        }

        private ContinueState ContinueState = ContinueState.Continue;

        private void OnAfterAction(ActionEventArgs args)
        {
            ActionEvent handler = this.ActionCompleted;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void OnBeforeAction(ActionEventArgs e)
        {
            ActionEvent handler = this.BeforeAction;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void OnCompleted(bool completed)
        {
            JobCompletedEvent handler = this.JobCompleted;
            if (handler != null)
            {
                handler(this, new JobCompletedEventArgs { Completed = completed });
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
            {
                ActionEvent handler = ActionAdded;
                if (handler != null)
                {
                    ActionEventArgs args = new ActionEventArgs
                                           {
                                                   Action = action,
                                                   Index = index
                                           };
                    handler(this, args);
                }
            }
        }

        public void AddAction(IAction action, Int32 index = -1)
        {
            AddAction(this.Job, action, index);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
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
                ActionEvent handler = ActionRemoved;
                if (handler != null)
                {
                    ActionEventArgs args = new ActionEventArgs()
                                           {
                                                   Action = action
                                           };
                    handler(this, args);
                }
            }
        }

        public void Clear()
        {
            Variables.Clear();
            while (Job.Actions.Count > 0)
            {
                RemoveAction(Job.Actions.First());
            }
            Job.Author = "";
            Job.AuthorContact = "";
            Job.FileName = "";
            Job.InfoUrl = "";
            Job.Name = "";
        }

        #endregion

        #region Save and Load methods

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        [Localizable(false)]
        public void SaveJob(String filename)
        {
            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("CentipedeJob");
            XmlElement metaElement1 = xmlDoc.CreateElement("Metadata");
            XmlElement metaElement2 = xmlDoc.CreateElement("Name");
            
            metaElement2.AppendChild(xmlDoc.CreateTextNode(Job.Name));
            metaElement1.AppendChild(metaElement2);

            metaElement2 = xmlDoc.CreateElement("Author");
            XmlElement metaElement3 = xmlDoc.CreateElement("Name");
            metaElement3.AppendChild(xmlDoc.CreateTextNode(Job.Author));
            metaElement2.AppendChild(metaElement3);
            metaElement3 = xmlDoc.CreateElement("Contact");
            metaElement3.AppendChild(xmlDoc.CreateTextNode(Job.AuthorContact));
            metaElement2.AppendChild(metaElement3);
            metaElement1.AppendChild(metaElement2);

            metaElement2 = xmlDoc.CreateElement("Info");
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

        /// <summary>
        ///     Load a job with a given name
        /// </summary>
        /// <param name="jobFileName">Name of the job to load</param>
        [Localizable(false)]
        public CentipedeJob LoadJob(string jobFileName)
        {

            Clear();
            CentipedeJob job = new CentipedeJob();

            //var xmlDoc = new XmlDocument();
            if (!File.Exists(jobFileName))
            {
                throw new FileNotFoundException();
            }


            XPathDocument xPathDoc = new XPathDocument(jobFileName);
            XPathNavigator nav = xPathDoc.CreateNavigator();

            job.FileName = jobFileName;

            job.Name = nav.SelectSingleNode("//Metadata/Name").Value;
            job.Author = nav.SelectSingleNode("//Metadata/Author/Name").Value;
            job.AuthorContact = nav.SelectSingleNode("//Metadata/Author/Contact").Value;
            job.InfoUrl = nav.SelectSingleNode("//Metadata/Info/@Url").Value;


            var it = nav.Select("//Actions/*");

            foreach (XPathNavigator actionElement in it)
            {
                AddAction(job, Action.FromXml(actionElement, Variables, this));
            }

            {
                AfterLoadEvent handler = AfterLoad;
                if (handler != null)
                {
                    handler(this, EventArgs.Empty);
                }
            }

            return job;
        }

        #endregion

        #endregion


        
    }

}
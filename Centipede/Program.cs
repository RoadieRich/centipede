using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using Centipede.Properties;


//  LINQ
//   \o/
// All the
// things

namespace Centipede
{
    public class Program : IDisposable
    {
        public delegate void ActionRemovedHandler(Action action);

        /// <summary>
        ///     Called after executing an action
        /// </summary>
        /// <param name="action">The action that has just been executed.</param>
        public delegate void ActionUpdateCallback(Action action);

        public delegate void AddActionCallback(Action action, Int32 index);

        public delegate void AfterLoadEventHandler(object sender, EventArgs e);

        /// <summary>
        ///     Handler for job completion
        /// </summary>
        /// <param name="succeeded">True if all actions completed successfully.</param>
        public delegate void CompletedHandler(Boolean succeeded);

        /// <summary>
        ///     Handler delegate for errors occuring in actions
        /// </summary>
        /// <param name="e">The exception that caused the error</param>
        /// <param name="nextAction">Set to the next action - useful for repeating actions</param>
        /// <returns>True if execution of Job should continue, false to halt</returns>
        public delegate Boolean ErrorHandler(ActionException e, out Action nextAction);

        /// <summary>
        ///     Dictionary of Variables for use by actions.  As much as I'd like to make types more intuitive,
        ///     I can't figure a way of doing it easily.
        /// </summary>
        public readonly Dictionary<String, Object> Variables = new Dictionary<String, Object>();

        public string JobFileName = "";
        public string JobName = "";

        public readonly List<Action> Actions = new List<Action>();
        private MainWindow _mainForm;
        private static volatile Program _instance;
        private static readonly object LockObject = new object();

        private Program()
        { }

        public Int32 JobComplexity
        {
            get
            {
                return Actions.Sum(action => action.Complexity);
            }
        }

        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            Instance._mainForm = new MainWindow();
            Application.Run(Instance._mainForm);
        }

        public static Program Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (LockObject)
                    {
                        if (_instance == null)
                        {
                            _instance = new Program();
                        }
                    }
                }
                return _instance;
            }
        }

        /// <summary>
        ///     Run the job, starting with the first action added.
        /// </summary>
        [STAThread]
        public void RunJob()
        {
            Action currentAction;

            if (Actions.Count < 1)
            {
                if (ActionErrorOccurred != null)
                {
                    ActionErrorOccurred(new ActionException(Resources.Program_RunJob_No_Actions_Added), out currentAction);
                }
                return;
            }

            currentAction = Actions[0];

            Boolean completed = true;

            while (currentAction != null)
            {
                try
                {
                    ActionUpdateCallback beforeActionHandler = BeforeAction;
                    if (beforeActionHandler != null)
                    {
                        beforeActionHandler(currentAction);
                    }

                    currentAction.Run();

                    ActionUpdateCallback afterActionHandler = ActionCompleted;
                    if (afterActionHandler != null)
                    {
                        afterActionHandler(currentAction);
                    }
                    currentAction = currentAction.GetNext();
                }
                catch (Exception e)
                {
                      ActionException ae;
                    if (e is FatalActionException)
                    {
                        completed = false;
                        ErrorHandler handler = ActionErrorOccurred;
                        if (handler != null)
                        {
                            ae = new ActionException(string.Format("Fatal error: cannot continue\n{0}", e.Message, e));
                            handler(e as FatalActionException, out currentAction);

                        }
                        break;
                    }
                  
                    if (e is ActionException)
                    {
                        ae = e as ActionException;
                    }
                    else
                    {
                        ae = new ActionException(e, currentAction);
                    }
                    {
                        ErrorHandler handler = ActionErrorOccurred;
                        if (handler != null)
                        {
                            if (!handler(ae, out currentAction))
                            {
                                completed = false;
                                break;
                            }
                        }
                    }
                }
            }

            CompletedHandler jobCompletedHandler = JobCompleted;
            if (jobCompletedHandler != null)
            {
                jobCompletedHandler(completed);
            }
        }

        public  event ActionUpdateCallback ActionCompleted = delegate { };
        public  event ActionUpdateCallback BeforeAction = delegate { };

        public  event ActionRemovedHandler ActionRemoved = delegate { };

        public  event CompletedHandler JobCompleted = delegate { };

        public  event ErrorHandler ActionErrorOccurred = delegate(ActionException e, out Action nextAction)
                                                                   {
                                                                       nextAction = null;
                                                                       return false;
                                                                   };

        public  event AddActionCallback ActionAdded;
        //{
        //    add
        //    {
        //        AddActionCallbacks.Add(value);
        //    }
        //    remove
        //    {
        //        AddActionCallbacks.Remove(value);
        //    }
            
        //}

        //private static List<AddActionCallback> AddActionCallbacks = new List<AddActionCallback>(); 

        /// <summary>
        ///     Add action to the job queue.  By default, it is added as the last action in the job.
        /// </summary>
        /// <param name="action">Action to add</param>
        /// <param name="index">(Optional) Index to add action at.  Defaults to end (-1).</param>
        public void AddAction(Action action, Int32 index = -1)
        {
            switch (index)
            {
            case -1:
                if (Actions.Count > 0)
                {
                    Action last = Actions[Actions.Count - 1];

                    last.Next = action;
                }
                Actions.Add(action);
                index = Actions.Count - 1;
                break;
            case 0:
                {
                    Action oldFirst = Actions[0];
                    Actions.Insert(0, action);
                    action.Next = oldFirst;
                }
                break;
            default:
                {
                    Action prevAction = Actions[index - 1];
                    Action nextAction = Actions[index];
                    prevAction.Next = action;
                    action.Next = nextAction;
                    Actions.Insert(index, action);
                }
                break;
            }
            AddActionCallback handler = ActionAdded;
            if (handler != null)
            {
                handler(action, index);
            }
        }

        internal void RemoveAction(Action action)
        {
            Action prevAction = Actions.SingleOrDefault(a => a.Next == action);

            Action nextAction = action.Next;

            

            if (prevAction != null)
            {
                prevAction.Next = nextAction;
            }
            Actions.Remove(action);
            ActionRemovedHandler handler = ActionRemoved;
            if (handler != null)
            {
                handler(action);
            }
        }

        /*
        internal static int GetIndexOf(Action action)
        {
            return Actions.IndexOf(action);
        }
*/

        [Localizable(false)]
        internal void SaveJob(String filename)
        {
            var xmlDoc = new XmlDocument();
            XmlElement xmlRoot = xmlDoc.CreateElement("CentipedeJob");
            xmlRoot.SetAttribute("Title", JobName);
            xmlDoc.AppendChild(xmlRoot);

            XmlElement actionsElement = xmlDoc.CreateElement("Actions");
            xmlRoot.AppendChild(actionsElement);

            foreach (Action action in Actions)
            {
                action.AddToXmlElement(actionsElement);
            }

            //XmlElement varsElement = xmlDoc.CreateElement("Variables");
            //foreach (var variableEntry in Variables)
            //{
            //    XmlElement curVarElement = xmlDoc.CreateElement(variableEntry.Value.GetType().Name);
            //    curVarElement.SetAttribute("Name", variableEntry.Key);
            //    curVarElement.SetAttribute("Value", variableEntry.Value.ToString());
            //    varsElement.AppendChild(curVarElement);
            //}
            //xmlRoot.AppendChild(varsElement);

            XmlWriterSettings settings = new XmlWriterSettings
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
            JobFileName = filename;
        }

        public  event AfterLoadEventHandler AfterLoad = delegate { };

        /// <summary>
        ///     Load a job with a given name
        /// </summary>
        /// <param name="jobFileName">Name of the job to load</param>
        [Localizable(false)]
        internal void LoadJob(String jobFileName)
        {
            Clear();

            var xmlDoc = new XmlDocument();
            if (!File.Exists(jobFileName))
            {
                return;
            }
            xmlDoc.Load(jobFileName);
            JobName = ((XmlElement)xmlDoc.ChildNodes[1]).GetAttribute("Title");
            foreach (
                    XmlElement actionElement in
                            xmlDoc.GetElementsByTagName("Actions").OfType<XmlElement>().Single().ChildNodes)
            {
                AddAction(Action.FromXml(actionElement, Variables));
            }

            Assembly asm = Assembly.GetExecutingAssembly();

            //foreach (
            //        XmlElement variableElement in
            //                xmlDoc.GetElementsByTagName("Variables").OfType<XmlElement>().Single())
            //{
            //    String name = variableElement.GetAttribute("name");
            //    Type type = asm.GetType(variableElement.LocalName);

            //    MethodInfo parseMethod = type.GetMethod("Parse", new[] { typeof (String) });

            //    object value = parseMethod == null
            //                           ? variableElement.GetAttribute("Value")
            //                           : parseMethod.Invoke(type,
            //                                                new object[] { variableElement.GetAttribute("Value") });
            //    Variables.Add(name, value);
            //}
            AfterLoadEventHandler handler = AfterLoad;
            MessageBox.Show(handler.ToString());
            if (handler != null)
            {
                handler(typeof (Program), EventArgs.Empty);
            }
        }

        
/*
        public Int32 JobLength
        {
            get
            {
                return Actions.Sum(a=>a.Complexity);
            }
        }
*/


        internal void Clear()
        {
            Variables.Clear();
            while (Actions.Count > 0)
            {
                RemoveAction(Actions.First());
            }
        }

        public void Dispose()
        {
            foreach (IDisposable action in Actions)
            {
                action.Dispose();
                    
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;


namespace Centipede
{
    public static class Program
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
        public static readonly Dictionary<String, Object> Variables = new Dictionary<String, Object>();

        public static string JobFileName = "";
        public static string JobName = "";

        public static readonly List<Action> Actions = new List<Action>();
        private static MainWindow _mainForm;

        public static Int32 JobComplexity
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

            _mainForm = new MainWindow();
            if (!_mainForm.IsDisposed)
            {
                Application.Run(_mainForm);
            }
        }

        /// <summary>
        ///     Run the job, starting with the first action added.
        /// </summary>
        [STAThread]
        public static void RunJob()
        {
            Action currentAction;

            if (Actions.Count < 1)
            {
                if (ActionErrorOccurred != null)
                {
                    ActionErrorOccurred(new ActionException("No Actions Added"), out currentAction);
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
                    if (e is ActionException)
                    {
                        ae = e as ActionException;
                    }
                    else
                    {
                        ae = new ActionException(e, currentAction);
                    }

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

            CompletedHandler jobCompletedHandler = JobCompleted;
            if (jobCompletedHandler != null)
            {
                jobCompletedHandler(completed);
            }
        }

        public static event ActionUpdateCallback ActionCompleted = delegate { };
        public static event ActionUpdateCallback BeforeAction = delegate { };

        public static event ActionRemovedHandler ActionRemoved = delegate { };

        public static event CompletedHandler JobCompleted = delegate { };

        public static event ErrorHandler ActionErrorOccurred = delegate(ActionException e, out Action nextAction)
                                                                   {
                                                                       nextAction = null;
                                                                       return false;
                                                                   };

        public static event AddActionCallback ActionAdded;
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
        public static void AddAction(Action action, Int32 index = -1)
        {
            if (index == -1)
            {
                if (Actions.Count > 0)
                {
                    Action last = Actions[Actions.Count - 1];

                    last.Next = action;
                }

                Actions.Add(action);
                index = Actions.Count - 1;
            }
            else
            {
                if (index == 0) //add at start of list
                {
                    Action oldFirst = Actions[0];
                    Actions.Insert(0, action);
                    action.Next = oldFirst;
                }
                else
                {
                    Action prevAction = Actions[index - 1];
                    Action nextAction = Actions[index];
                    prevAction.Next = action;
                    action.Next = nextAction;
                    Actions.Insert(index, action);
                }
            }
            AddActionCallback handler = ActionAdded;
            if (handler != null)
            {
                handler(action, index);
            }
        }

        internal static void RemoveAction(Action action)
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

        internal static void SaveJob(String filename)
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

            XmlElement varsElement = xmlDoc.CreateElement("Variables");
            foreach (var variableEntry in Variables)
            {
                XmlElement curVarElement = xmlDoc.CreateElement(variableEntry.Value.GetType().Name);
                curVarElement.SetAttribute("Name", variableEntry.Key);
                curVarElement.SetAttribute("Value", variableEntry.Value.ToString());
                varsElement.AppendChild(curVarElement);
            }
            xmlRoot.AppendChild(varsElement);

            using (XmlWriter w = XmlWriter.Create(filename))
            {
                xmlDoc.WriteTo(w);
            }
            JobFileName = filename;
        }

        public static event AfterLoadEventHandler AfterLoad = delegate { };

        /// <summary>
        ///     Load a job with a given name
        /// </summary>
        /// <param name="jobFileName">Name of the job to load</param>
        internal static void LoadJob(String jobFileName)
        {
            Clear();

            var xmlDoc = new XmlDocument();
            if (File.Exists(jobFileName))
            {
                xmlDoc.Load(jobFileName);
                JobName = (xmlDoc.ChildNodes[1] as XmlElement).GetAttribute("Title");
                foreach (
                        XmlElement actionElement in
                                xmlDoc.GetElementsByTagName("Actions").OfType<XmlElement>().Single().ChildNodes)
                {
                    AddAction(Action.FromXml(actionElement, Variables));
                }

                Assembly asm = Assembly.GetExecutingAssembly();

                foreach (
                        XmlElement variableElement in
                                xmlDoc.GetElementsByTagName("Variables").OfType<XmlElement>().Single())
                {
                    String name = variableElement.GetAttribute("name");
                    Type type = asm.GetType(variableElement.LocalName);

                    MethodInfo parseMethod = type.GetMethod("Parse", new[] { typeof (String) });

                    object value = parseMethod == null
                                           ? variableElement.GetAttribute("Value")
                                           : parseMethod.Invoke(type,
                                                                new object[] { variableElement.GetAttribute("Value") });
                    Variables.Add(name, value);
                }
                AfterLoadEventHandler handler = AfterLoad;
                if (handler != null)
                {
                    handler(typeof (Program), EventArgs.Empty);
                }
            }
        }

        
        public static Int32 JobLength
        {
            get
            {
                return Actions.Sum(a=>a.Complexity);
            }
        }


        internal static void Clear()
        {
            Variables.Clear();
            while (Actions.Count > 0)
            {
                RemoveAction(Actions.First());
            }
        }

        public static void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (IDisposable action in Actions)
                {
                    action.Dispose();
                    
                }
            }
        }
    }
}

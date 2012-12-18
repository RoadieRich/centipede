using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Reflection;
using System.IO;

namespace Centipede
{
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(String[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainForm = new MainWindow();
            if (!mainForm.IsDisposed)
                Application.Run(mainForm);
        }

        /// <summary>
        /// Dictionary of Variables for use by actions.  As much as I'd like to make types more intuitive, 
        /// I can't figure a way of doing it easily. 
        /// </summary>
        public static Dictionary<String, Object> Variables = new Dictionary<String, Object>();


        public static string JobFileName = "";
        public static string JobName = "";

        internal static List<Action> Actions = new List<Action>();
        internal static MainWindow mainForm;


        /// <summary>
        /// Run the job, starting with the first action added.
        /// </summary>
        /// <param name="updateCallback"><see cref="ActionUpdateCallback"/></param>
        /// <param name="completedHandler"><see cref="CompletedHandler"/></param>
        /// <param name="errorHandler"><see cref="ErrorHandler"/></param>
        [STAThread]
        public static void RunJob()
        {
            Action currentAction = null;

            if (Actions.Count < 1)
            {
                if (ActionErrorOccurred != null)
                {
                    ActionErrorOccurred(new ActionException("No Actions Added"), ref currentAction);
                }
                return;
            }
            
            currentAction = Actions[0];

            Boolean completed = true;

            while (currentAction != null)
            {
                try
                {
                    var beforeActionHandler = BeforeAction;
                    if (beforeActionHandler != null)
                    {
                        beforeActionHandler(currentAction);
                    }
                    currentAction.Run();

                    var afterActionHandler = ActionCompleted;
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

                    var handler = ActionErrorOccurred;
                    if (handler != null)
                    {
                        if (!handler(ae, ref currentAction))
                        {
                            completed = false;
                            break;
                        }
                    }
                }
            }

            var jobCompletedHandler = JobCompleted;
            if (jobCompletedHandler != null)
            {
                jobCompletedHandler(completed);
            }
        }


        /// <summary>
        /// Called after executing an action
        /// </summary>
        /// <param name="action">The action that has just been executed.</param>
        public delegate void ActionUpdateCallback(Action action);
        public static event ActionUpdateCallback ActionCompleted = delegate { };
        public static event ActionUpdateCallback BeforeAction = delegate { };
        public delegate void ActionRemovedHandler(Action action);
        public static event ActionRemovedHandler ActionRemoved = delegate { };

        /// <summary>
        /// Handler for job completion
        /// </summary>
        /// <param name="succeeded">True if all actions completed successfully.</param>
        public delegate void CompletedHandler(Boolean succeeded);
        public static event CompletedHandler JobCompleted = delegate { };

        /// <summary>
        /// Handler delegate for errors occuring in actions
        /// </summary>
        /// <param name="e">The exception that caused the error</param>
        /// <param name="nextAction">Set to the next action - useful for repeating actions</param>
        /// <returns>True if execution of Job should continue, false to halt</returns>
        public delegate Boolean ErrorHandler(ActionException e, ref Action nextAction);
        public static event ErrorHandler ActionErrorOccurred = delegate { return false; };

        public static Int32 JobComplexity
        {
            get
            {
                Int32 totalComplexity = 0;
                foreach (Action action in Actions)
                {
                    totalComplexity += action.Complexity;
                }
                return totalComplexity;
            }
        }
       
        [Flags]
        internal enum ActionsToTest
        {
            PythonAction = 1,
            DemoAction = 2,
            ErrorAction = 4,
            All = 1 | 2 | 4
        }

        /// <summary>
        /// Add some test actions to the queue.
        /// </summary>
        //internal static void SetupTestActions(ActionsToTest actions = ActionsToTest.All)
        //{

        //    JobFileName = "Test File";

        //    if (actions.HasFlag(ActionsToTest.PythonAction))
        //    {
        //        PythonAction testPythonAction = new PythonAction(Program.Variables);
        //        testPythonAction.Source = String.Join("\r\n",
        //                                new String[] {@"try:",
        //                                              @"    i = int(variables[""a""])",
        //                                              @"except: ",
        //                                              @"    i = 0",
        //                                              @"variables[""a""] = i+1"});

        //        testPythonAction.Comment = "Increase Variable i by one";

        //        Program.AddAction(testPythonAction);
        //    }

        //    if (actions.HasFlag(ActionsToTest.DemoAction))
        //    {
        //        DemoAction testDemoAction = new DemoAction(Program.Variables);
        //        testDemoAction.Comment = "Display a text box showing attirbute values";
        //        Program.AddAction(testDemoAction);
        //    }

        //    if (actions.HasFlag(ActionsToTest.ErrorAction))
        //    {
        //        PythonAction testErrorAction = new PythonAction(Program.Variables);
        //        testErrorAction.Comment = "Throw an error!";
        //        testErrorAction.Source = "raise Exception()";

        //        Program.AddAction(testErrorAction);
        //    }
        //}

        public delegate void AddActionCallback(Action action, Int32 index);
        public static event AddActionCallback ActionAdded;

        /// <summary>
        /// Add action to the job queue.  By default, it is added as the last action in the job.
        /// </summary>
        /// <param name="action">Action to add</param>
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
            var handler = ActionAdded;
            if (handler != null)
            {
                handler(action, index);
            }
        }


        internal static void RemoveAction(Action action)
        {
            Action prevAction = (from Action a in Actions
                                 where a.Next == action
                                 select a).SingleOrDefault();

            Action nextAction = action.Next;

            if (prevAction != null)
            {
                prevAction.Next = nextAction;
            }
            Actions.Remove(action);
            var handler = ActionRemoved;
            if (handler != null)
            {
                handler(action);
            }

        }

        internal static int GetIndexOf(Action action)
        {
            return Actions.IndexOf(action);
        }

        internal static void SaveJob(String filename)
        {
            XmlDocument xmlDoc = new XmlDocument();
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
            foreach (KeyValuePair<String, Object> variableEntry in Variables)
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

        public delegate void AfterLoadEventHandler(object sender, EventArgs e);
        public static event AfterLoadEventHandler AfterLoad = delegate { };


        /// <summary>
        /// Load a job with a given name
        /// </summary>
        /// <param name="jobName">Name of the job to load</param>
        internal static void LoadJob(String jobFileName)
        {
            Clear();

            XmlDocument xmlDoc = new XmlDocument();
            if (File.Exists(jobFileName))
            {
                xmlDoc.Load(jobFileName);
                JobName = (xmlDoc.ChildNodes[1] as XmlElement).GetAttribute("Title");
                foreach (XmlElement actionElement in xmlDoc.GetElementsByTagName("Actions").OfType<XmlElement>().Single().ChildNodes)
                {

                    AddAction(Action.FromXml(actionElement, Variables));
                }

                Assembly asm = Assembly.GetExecutingAssembly();



                foreach (XmlElement variableElement in xmlDoc.GetElementsByTagName("Variables").OfType<XmlElement>().Single())
                {
                    String name = variableElement.GetAttribute("name");
                    Type type = asm.GetType(variableElement.LocalName);

                    MethodInfo parseMethod = type.GetMethod("Parse", new Type[] { typeof(String) });

                    Object value;
                    if (parseMethod == null)
                    {
                        value = variableElement.GetAttribute("Value");
                    }
                    else
                    {
                        value = parseMethod.Invoke(type, new object[] { variableElement.GetAttribute("Value") });
                    }
                    Program.Variables.Add(name, value);

                }
                var handler = AfterLoad;
                if (handler != null)
                {
                    handler(typeof(Program), EventArgs.Empty);
                }
            }
        }

        public static Int32 JobLength
        {
            get
            {
                return Actions.Count;
            }
        }

        internal static void Clear()
        {
            Variables.Clear();
            foreach (Action action in Actions.ToArray())
            {
                RemoveAction(action);
            }
        }

        internal static void Dispose(bool disposing)
        {
            if (disposing)
            {
                foreach (Action action in Actions)
                {
                    action.Dispose();
                }
            }
        }
    }
}

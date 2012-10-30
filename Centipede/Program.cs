using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Centipede
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {

            //XXX:Testing only
            JobFileName = "Test File";
            SetupTestAction();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainWindow());
        }

        public static void LoadJob(string jobName)
        {
            throw new NotImplementedException();
        }

        public static Dictionary<String, Object> Variables = new Dictionary<string,object>();
        public static List<Action> Actions = new List<Action>();
        public static string JobFileName = "testing.100p";
        public static string JobName = "Testing";
        private static JobFile File = null;


        internal static void RunJob(UpdateCallback updateCallback = null, CompletedHandler completedHandler = null, ErrorHandler errorHandler = null)
        {

            Action currentAction;

            if (Actions.Count < 1)
            {
                errorHandler(new ActionException("No Actions Added"), out currentAction);
                return;
            }

            currentAction = Actions[0];

            if (updateCallback == null)
            {
                updateCallback = (a) => { };
            }

            Boolean completed = true;

            while (currentAction != null)
            {
                try
                {
                    currentAction.DoAction();
                    currentAction = currentAction.GetNext();
                }
                catch (ActionException e)
                {
                    if (!errorHandler(e, out currentAction))
                    {
                        completed = false;
                    }
                }
            }
            completedHandler(completed);
        }

        internal delegate void UpdateCallback(Action action);
        internal delegate void CompletedHandler(Boolean succeeded);
        internal delegate Boolean ErrorHandler(ActionException e, out Action nextAction);

        internal static void LoadJob()
        {
            throw new NotImplementedException();

            JobName = File.getName();
        }

        static void SetupTestAction()
        {
            Actions.Add(new PythonAction("Test Action", @"sys.stdout.write(""Hello World!"")"));
        }

    }
     
    public class ActionException : Exception
    {
        public ActionException(String message, Action action) : base(message)
        {
            ErrorAction = action;
        }
        
        public ActionException(Action action)
        {
            ErrorAction = action;
        }

        public ActionException(string message, Exception exception, Action action) 
            : base(message, exception)
        {
            ErrorAction = action;
        }

        public ActionException(string message)
            : base(message) { }

        public ActionException(Exception e, Action action) : base(e.Message, e)
        {
            ErrorAction = action;
        }

        public readonly Action ErrorAction = null;
    }


    public enum VarTypes
    {
        Integer,
        Float,
        String,
        Other
    }

    //class VarsList
    //{
    //    DataMember d;
    //}


}

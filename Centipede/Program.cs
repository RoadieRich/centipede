﻿using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Centipede
{
    delegate T id<T>(T arg);

    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //XXX:Testing only
            JobFileName = "Test File";
            //SetupTestAction();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            mainForm = new MainWindow();
            
            Application.Run(mainForm);
        }
        
        /// <summary>
        /// Load a job with a given name
        /// </summary>
        /// <param name="jobName">Name of the job to load</param>
        public static void LoadJob(string jobName)
        {
            throw new NotImplementedException();
        }

        //public class Variable
        //{
        //    public Int32 Index;
        //    public String Name;
        //    public Object Value;
        
        //    public Variable(Int32 index, String name, Object value)
        //    {
        //        Index = index;
        //        Name = name;
        //        Value = value;
        //    }

        //    public Variable()
        //    {
        //        Name = "(Name)";
        //        Value = "(Value)";
        //    }

        //    public static explicit operator Variable(KeyValuePair<String, Object> kvp)
        //    {
        //        return new Variable(-1, kvp.Key, kvp.Value);
        //    }
        //    public static implicit operator KeyValuePair<String, Object>(Variable v)
        //    {
        //        return new KeyValuePair<string, object>(v.Name, v.Value);
        //    }
        //}


        /// <summary>
        /// Dictionary of Variables for use by actions.  As much as I'd like to make types more intuitive, 
        /// I can't figure a way of doing it easily. 
        /// </summary>
        public static Dictionary<String, Object> Variables = new Dictionary<String, Object>();

        
        public static string JobFileName = "testing.100p";
        public static string JobName = "Testing";

        private static List<Action> Actions = new List<Action>();
        internal static MainWindow mainForm;
        //private static JobFile File = null;

        /// <summary>
        /// Run the job, starting with the first action added.
        /// </summary>
        /// <param name="updateCallback"><see cref="ActionUpdateCallback"/></param>
        /// <param name="completedHandler"><see cref="CompletedHandler"/></param>
        /// <param name="errorHandler"><see cref="ErrorHandler"/></param>
        public static void RunJob()
        {
            Action currentAction = null;

            if (Actions.Count < 1)
            {
                ActionErrorOccurred(new ActionException("No Actions Added"), ref currentAction);
                return;
            }

            currentAction = Actions[0];

            Boolean completed = true;

            while (currentAction != null)
            {
                try
                {
                    BeforeAction(currentAction);
                    currentAction.DoAction();
                    ActionCompleted(currentAction);
                    currentAction = currentAction.GetNext();
                }
                catch (ActionException e)
                {
                    if (!ActionErrorOccurred(e, ref currentAction))
                    {
                        completed = false;
                        break;
                    }
                }
            }

            JobCompleted(completed);
        }


        /// <summary>
        /// Called after executing an action
        /// </summary>
        /// <param name="action">The action that has just been executed.</param>
        public delegate void ActionUpdateCallback(Action action);
        public static event ActionUpdateCallback ActionCompleted;
        public static event ActionUpdateCallback BeforeAction;

        /// <summary>
        /// Handler for job completion
        /// </summary>
        /// <param name="succeeded">True if all actions completed successfully.</param>
        public delegate void CompletedHandler(Boolean succeeded);
        public static event CompletedHandler JobCompleted;

        /// <summary>
        /// Handler delegate for errors occuring in actions
        /// </summary>
        /// <param name="e">The exception that caused the error</param>
        /// <param name="nextAction">Set to the next action - useful for repeating actions</param>
        /// <returns>True if execution of Job should continue, false to halt</returns>
        public delegate Boolean ErrorHandler(ActionException e, ref Action nextAction);
        public static event ErrorHandler ActionErrorOccurred;

        public static void LoadJob()
        {
            throw new NotImplementedException();

            //JobName = File.getName();
        }

        static void SetupTestAction()
        {
            Program.AddAction(new PythonAction("pyact", 
@"try:
    i = int(variables[""a""])
except: 
    i = 0
variables[""a""] = i+1"));
            //Program.AddAction(new PythonAction("Test Action", @"sys.stdout.write(""Hello World!"")"));
        }

        public delegate void AddActionCallback(Action action, Int32 index);
        public static event AddActionCallback ActionAdded = new AddActionCallback((a,b) => {});

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

                ActionAdded(action, Actions.Count - 1);
            }
            else
            {
                Action prev = Actions[index];
                Action next = prev.Next;
                prev.Next = action;
                action.Next = next;
                Actions.Insert(index, action);
                ActionAdded(action, index);
            }
            
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
}

using System;
using System.ComponentModel;
using System.Threading;
using System.Windows.Forms;
using PythonEngine;
using ResharperAnnotations;

//    , ,,    ,, ,   , ,,    ,, ,   , ,,    ,, ,   , ,,    ,, ,   ,        ,---,
//   /  |\    /|  \ /  |\    /|  \ /  |\    /|  \ /  |\    /|  \ /   /===\/
// /=\==/-\==/-\==/-\==/-\==/-\==/-\==/-\==/-\==/-\==/-\==/-\==/-\==/   O \^
// \=/==\-/==\-/==\-/==\-/==\-/==\-/==\-/==\-/==\-/==\-/==\-/==\-/==\   O /v
//   \  | /   \|  / \  | /   \|  / \  | /   \|  / \  | /   \|  / \   \===/\
//    ` ``    `` `   ` ``    `` `   ` ``    `` `   ` ``    `` `   `        `---`

namespace CentipedeInterfaces
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public interface ICentipedeCore : IDisposable
    {   
        
        /// <summary>
        /// 
        /// </summary>
        event ActionEvent ActionAdded;
        
        /// <summary>
        /// 
        /// </summary>
        event ActionEvent ActionCompleted;
        
        /// <summary>
        /// 
        /// </summary>
        event ActionErrorEvent ActionErrorOccurred;
        
        /// <summary>
        /// 
        /// </summary>
        event ActionEvent ActionRemoved;
        
        /// <summary>
        /// 
        /// </summary>
        event AfterLoadEvent AfterLoad;
        
        /// <summary>
        /// 
        /// </summary>
        event ActionEvent BeforeAction;
        
        /// <summary>
        /// 
        /// </summary>
        event JobCompletedEvent JobCompleted;
     
        /// <summary>
        ///     Dictionary of Variables for use by actions.  As much as I'd like to make types more intuitive,
        ///     I can't figure a way of doing it easily.
        /// </summary>
        PythonScope Variables { get; }

        new void Dispose();

        /// <summary>
        ///     Run the job, starting with the first action added.
        /// </summary>
        [STAThread]
        void RunJob(bool stepping = false);

        /// <summary>
        ///     Add action to the job queue.  By default, it is added as the last action in the job.
        /// </summary>
        /// <param name="job"></param>
        /// <param name="action">Action to add</param>
        /// <param name="index">(Optional) Index to add action at.  Defaults to end (-1).</param>
        void AddAction(CentipedeJob job, IAction action, Int32 index = -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <param name="index"></param>
        void AddAction(IAction action, Int32 index = -1);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        void RemoveAction(IAction action);

        void Clear();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="filename"></param>
        [Localizable(false)]
        void SaveJob(String filename);

        /// <summary>
        ///     Load a job with a given name
        /// </summary>
        /// <param name="jobFileName">Name of the job to load</param>
        [Localizable(false)]
        void LoadJob(string jobFileName);

        CentipedeJob Job { get; set; }
        event StartSteppingEvent StartRun;

        IAction CurrentAction { get; set; }
        Boolean IsStepping{ get; }
        void AbortRun();

        IPythonEngine PythonEngine { get; }
        Form Window { get; set; }
    }

    public delegate void ActionRemovedHandler(IAction action);

    /// <summary>
    ///     Called after executing an action
    /// </summary>
    /// <param name="action">The action that has just been executed.</param>
    public delegate void ActionUpdateCallback(IAction action);

    public delegate void ActionEvent(object sender, ActionEventArgs e);

    public delegate void AddActionCallback(IAction action, Int32 index);

    public delegate void AfterLoadEvent(Object sender, EventArgs e);

    /// <summary>
    ///     Handler for job completion
    /// </summary>
    /// <param name="succeeded">True if all actions completed successfully.</param>
    public delegate void CompletedHandler(Boolean succeeded);

    public delegate void JobCompletedEvent(object sender, JobCompletedEventArgs e);

    public class JobCompletedEventArgs : EventArgs
    {
        public Boolean Completed { get; set; }
    }

    /// <summary>
    ///     Handler delegate for errors occuring in actions
    /// </summary>
    public delegate void ActionErrorEvent(object sender, ActionErrorEventArgs e);

    public delegate void StartSteppingEvent(object sender, StartRunEventArgs e);

    public class StartRunEventArgs : EventArgs
    {
        public ManualResetEvent ResetEvent { get; private set; }

        public StartRunEventArgs(ManualResetEvent resetEvent)
        {
            ResetEvent = resetEvent;
        }
        public StartRunEventArgs()
        {
            ResetEvent = null;
        }
    }
}
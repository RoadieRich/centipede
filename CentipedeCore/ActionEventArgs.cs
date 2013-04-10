using System;


namespace CentipedeInterfaces
{
    public class ActionEventArgs
    {
        public ActionEventArgs()
        {
            Index = -1;
        }

        public Boolean Stepping { get; set; }

        public virtual IAction Action { get; set; }

        public Int32 Index { get; set; }
    }

    public class ActionErrorEventArgs : ActionEventArgs
    {
        [ResharperAnnotations.UsedImplicitly]
        public IAction NextAction { get; set; }
        public Exception Exception { get; set; }
        public override IAction Action
        {
            get
            {
                return base.Action;
            }
            set
            {
                base.Action = value;
                NextAction = value.Next;
            }
        }
        public ContinueState Continue { get; set; }
        public Boolean Fatal { get; set; }
    }

    public enum ContinueState
    {
        Abort,
        Retry,
        Continue
    }
    
}
using System;


namespace CentipedeInterfaces
{
    public class ActionEventArgs
    {
        public ActionEventArgs()
        {
            Index = -1;
        }

        public virtual IAction Action { get; set; }

        public Int32 Index { get; set; }
    }

    public class ActionErrorEventArgs : ActionEventArgs
    {
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
        public Boolean Continue { get; set; }
    }
}
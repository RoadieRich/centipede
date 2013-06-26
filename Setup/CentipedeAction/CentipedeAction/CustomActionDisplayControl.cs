// Custom action display controls

// 0. References.  The only reference above and beyond those needed for the action itself is
// System.Windows.Forms

using System;
using Centipede.Actions;
using CentipedeInterfaces;


namespace CentipedeAction
{
    class CustomActionDisplayControl : ActionDisplayControl
    {
        // 1. Constructor.  Pass true as second argument to the contructor if all the argument types
        // are supported
        public CustomActionDisplayControl(IAction action) : base(action, generateArgumentFields: false)
        {
            
        }


        // This is standard notation to allow easy access to the 
        public new MyAction ThisAction
        {
            get
            {
                return (MyAction)base.ThisAction;
            }
            protected set
            {
                base.ThisAction = value;
            }
        }
    }
}

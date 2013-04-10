using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("Flow Control", DisplayName = "Exit Centipede", iconName = "exit")]
    class ExitAction:Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        /// <param name="c"></param>
        public ExitAction(IDictionary<string, object> v, ICentipedeCore c)
                : base("Exit Centipede", v, c)
        { }

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {
            if (GetCurrentCore().Window.InvokeRequired)
            {
                GetCurrentCore().Window.Invoke(new System.Action(GetCurrentCore().Window.Close));
            }
        }

        public override int Complexity
        {
            get
            {
                return 0;
            }
        }
    }
}

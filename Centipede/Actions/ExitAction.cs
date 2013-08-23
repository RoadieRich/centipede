using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("Flow Control", DisplayName = "Exit Centipede", IconName = "exit")]
    class ExitAction:Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public ExitAction(IDictionary<string, object> variables, ICentipedeCore c)
                : base("Exit Centipede", variables, c)
        { }

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        protected override void DoAction()
        {
            GetCurrentCore().Exit();
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

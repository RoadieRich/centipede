using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("Flow Control", displayName = "Exit Centipede")]
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


            if (Form.ActiveForm.InvokeRequired)
            {
                Form.ActiveForm.Invoke(new D(DoClose));
            }

            

        }

        public override int Complexity
        {
            get
            {
                return 0;
            }
        }

        private delegate void D();
        private void DoClose()
        {
            Form.ActiveForm.Close();
        }
    }
}

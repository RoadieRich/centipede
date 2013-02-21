using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Centipede.Actions
{
    [ActionCategory("Flow Control", displayName = "Exit Centipede")]
    class ExitAction:Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public ExitAction(IDictionary<string, object> v)
                : base("Exit Centipede", v)
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

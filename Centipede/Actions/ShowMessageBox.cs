using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centipede;
using System.Windows.Forms;

namespace Centipede
{
    [ActionCategory("UI", displayName="Show Messagebox")]
    class ShowMessageBox : Action
    {
        public ShowMessageBox(Dictionary<String,Object> v)
            : base("Show message box", v)
        { }

        [ActionArgument]
        public String Title = "Message";

        [ActionArgument]
        public String Message = "";

        private delegate void _showMessageBox();

        protected override void DoAction()
        {
            String parsedMessage = ParseStringForVariable(Message);
            String parsedTitle = ParseStringForVariable(Title);
            MainWindow.Instance.Invoke(new _showMessageBox(() => MessageBox.Show(MainWindow.Instance, parsedMessage, parsedTitle)));
        }
    }
}

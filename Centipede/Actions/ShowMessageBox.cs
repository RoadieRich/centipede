using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI", displayName="Show Messagebox")]
    class ShowMessageBox : Action
    {
        public ShowMessageBox(IDictionary<string, object> v, ICentipedeCore c)
            : base("Show message box", v,c)
        { }

        [ActionArgument]
        public String Title = "Message";

        [ActionArgument(displayName="Message")]
        public String MessageText = "";

        private delegate void ShowMessageBoxDelegate();

        protected override void DoAction()
        {
            String parsedMessage = ParseStringForVariable(MessageText);
            String parsedTitle = ParseStringForVariable(Title);
            MainWindow.Instance.Invoke(new ShowMessageBoxDelegate(() => MessageBox.Show(MainWindow.Instance, parsedMessage, parsedTitle)));
        }
    }
}

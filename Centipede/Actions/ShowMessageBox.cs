using System;
using System.Collections.Generic;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI", DisplayName="Show Messagebox")]
    class ShowMessageBox : Action
    {
        public ShowMessageBox(IDictionary<string, object> v, ICentipedeCore c)
            : base("Show message box", v,c)
        { }

        [ActionArgument]
        public String Title = "Message";

        [ActionArgument(DisplayName="Message")]
        public String MessageText = "";

        private delegate void ShowMessageBoxDelegate();

        protected override void DoAction()
        {
            Form form = GetCurrentCore().Window;

            String parsedMessage = ParseStringForVariable(MessageText);
            String parsedTitle = ParseStringForVariable(Title);
            form.Invoke(new Func<DialogResult>(() => MessageBox.Show(form, parsedMessage, parsedTitle)));
        }
    }
}

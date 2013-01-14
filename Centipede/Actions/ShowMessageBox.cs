using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Centipede.Actions
{
    [ActionCategory("UI", displayName="Show Messagebox")]
    class ShowMessageBox : Action
    {
        public ShowMessageBox(Dictionary<String,Object> v)
            : base("Show message box", v)
        { }

        [ActionArgument]
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
        public String Title = "Message";

        [ActionArgument(displayName="Message")]
        public String MessageText = "";

        // ReSharper restore FieldCanBeMadeReadOnly.Global
        // ReSharper restore ConvertToConstant.Global
        // ReSharper restore MemberCanBePrivate.Global
        private delegate void ShowMessageBoxDelegate();

        protected override void DoAction()
        {
            String parsedMessage = ParseStringForVariable(MessageText);
            String parsedTitle = ParseStringForVariable(Title);
            MainWindow.Instance.Invoke(new ShowMessageBoxDelegate(() => MessageBox.Show(MainWindow.Instance, parsedMessage, parsedTitle)));
        }
    }
}

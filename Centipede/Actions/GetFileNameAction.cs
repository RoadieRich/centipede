using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;


namespace Centipede.Actions
{
    [ActionCategory("UI", displayName="Get Filename")]
    class GetFileNameAction : Action
    {
        public GetFileNameAction(IDictionary<string, object> v)
            : base("Get Filename", v)
        {
            _handler = GetFileNameDialogue_FileOk;
        }

        [ActionArgument]
        public String DestinationVariable = "Filename";

        private delegate DialogResult ShowGetFileDialog();
        private readonly CancelEventHandler _handler;

        protected override void InitAction()
        {
            MainWindow.Instance.GetFileNameDialogue.FileOk += _handler;
        }

        protected override void DoAction()
        {
            MainWindow.Instance.Invoke(new ShowGetFileDialog(MainWindow.Instance.GetFileNameDialogue.ShowDialog));
        }

        private void GetFileNameDialogue_FileOk(object sender, CancelEventArgs e)
        {
            FileDialog dialog = sender as FileDialog;
            if (dialog == null)
            {
                throw new ArgumentException("sender");
            }
            Variables[ParseStringForVariable(DestinationVariable)] = dialog.FileName;
        }

        protected override void CleanupAction()
        {
            MainWindow.Instance.GetFileNameDialogue.FileOk -= _handler;
        }
    }
}

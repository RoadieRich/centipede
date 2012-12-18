using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Centipede;
using System.ComponentModel;

namespace Centipede
{
    [ActionCategory("UI", displayName="Get Filename")]
    class GetFileNameAction : Action
    {
        public GetFileNameAction(Dictionary<String, Object> v)
            : base("Get Filename", v)
        { _handler = new CancelEventHandler(GetFileNameDialogue_FileOk);}

        [ActionArgument]
        public String DestinationVariable = "Filename";


        private delegate DialogResult _showGetFileDialog();
        private CancelEventHandler _handler;

        protected override void InitAction()
        {
            MainWindow.Instance.GetFileNameDialogue.FileOk += _handler;
        }

        protected override void DoAction()
        {
            MainWindow.Instance.Invoke(new _showGetFileDialog(MainWindow.Instance.GetFileNameDialogue.ShowDialog));
        }

        private void GetFileNameDialogue_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Variables[ParseStringForVariable(DestinationVariable)] = (sender as FileDialog).FileName;
        }

        protected override void CleanupAction()
        {
            MainWindow.Instance.GetFileNameDialogue.FileOk -= _handler;
        }
    }
}

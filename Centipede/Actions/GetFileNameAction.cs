using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;


namespace Centipede.Actions
{
    [ActionCategory("UI", displayName="Get Filename")]
    class GetFileNameAction : Action
    {
        public GetFileNameAction(Dictionary<String, Object> v)
            : base("Get Filename", v)
        {
            _handler = GetFileNameDialogue_FileOk;
        }

        [ActionArgument]
// ReSharper disable ConvertToConstant.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
        public String DestinationVariable = "Filename";
// ReSharper restore FieldCanBeMadeReadOnly.Global
// ReSharper restore MemberCanBePrivate.Global
// ReSharper restore ConvertToConstant.Global


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
            Variables[ParseStringForVariable(DestinationVariable)] = (sender as FileDialog).FileName;
        }

        protected override void CleanupAction()
        {
            MainWindow.Instance.GetFileNameDialogue.FileOk -= _handler;
        }
    }
}

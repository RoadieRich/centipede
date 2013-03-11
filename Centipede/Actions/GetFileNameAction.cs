using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI", displayName="Get Filename")]
    class GetFileNameAction : Action
    {
        public GetFileNameAction(IDictionary<string, object> v, ICentipedeCore c)
            : base("Get Filename", v, c)
        {
            
        }

        [ActionArgument]
        public String DestinationVariable = "Filename";

        private delegate DialogResult ShowGetFileDialog();
        
        [ActionArgument(displayName = "Caption")]
        public String Caption = "";



        [ActionArgument(displayName = "Filter")]
        public String Filter = "";


        protected override void InitAction()
        {
            MainWindow.Instance.GetFileNameDialogue.FileOk += GetFileNameDialogue_FileOk;
            MainWindow.Instance.GetFileNameDialogue.Title = Caption;
            MainWindow.Instance.GetFileNameDialogue.Filter = Filter;
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
            MainWindow.Instance.GetFileNameDialogue.FileOk -= GetFileNameDialogue_FileOk;
        }
    }
}

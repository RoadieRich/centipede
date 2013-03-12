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

        [ActionArgument(Literal = true)]
        public String DestinationVariable = "Filename";

        [ActionArgument(Literal = true)]
        public String Caption = "Choose File";
        
        [ActionArgument(displayName = "Filter")]
        public String Filter = "All Files (*.*)|*.*";


        [ActionArgument(displayName = "Default filename")]
        public String DefaultFilename = "";

        private OpenFileDialog _dialog;

        protected override void InitAction()
        {
            this._dialog = ((MainWindow)Form.ActiveForm).GetFileNameDialogue;
            object oldFilename;
            bool hasOldFilename = Variables.TryGetValue(DestinationVariable, out oldFilename);

            
            
            this._dialog.FileOk  += GetFileNameDialogue_FileOk;
            this._dialog.Title    = Caption;
            this._dialog.Filter   = Filter;
            this._dialog.FileName = ParseStringForVariable(this.DefaultFilename);
            
            if (String.IsNullOrEmpty(this.DefaultFilename) && hasOldFilename)
            {
                this._dialog.FileName = oldFilename as String;
            }
        }

        protected override void DoAction()
        {
            DialogResult result =
                    (DialogResult)
                    Form.ActiveForm.Invoke(new Func<DialogResult>(
                                                   () => (this._dialog.ShowDialog())));
            if (result == DialogResult.Cancel)
            {
                throw new FatalActionException(string.Format("Cancel clicked on {0} dialog.", Caption), this);
            }
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
            this._dialog.FileOk -= GetFileNameDialogue_FileOk;
        }
    }
}

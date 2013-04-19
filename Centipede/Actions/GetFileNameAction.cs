using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI", DisplayName="Get Filename")]
    class GetFileNameAction : Action
    {
        public GetFileNameAction(IDictionary<string, object> variables, ICentipedeCore c)
            : base("Get Filename", variables, c)
        {
            
        }

        [ActionArgument(Literal = true)]
        public String DestinationVariable = "Filename";

        [ActionArgument(Literal = true)]
        public String Caption = "Choose File";
        
        [ActionArgument(DisplayName = "Filter")]
        public String Filter = "All Files (*.*)|*.*";


        [ActionArgument(DisplayName = "Default filename")]
        public String DefaultFilename = "";

        private OpenFileDialog _dialog;
        private MainWindow _mainWindow;

        protected override void InitAction()
        {
            this._mainWindow = ((MainWindow)GetCurrentCore().Window);
            this._dialog = this._mainWindow.GetFileNameDialogue;
            object oldFilename;
            bool hasOldFilename = Variables.TryGetValue(DestinationVariable, out oldFilename);



            this._mainWindow.GetFileNameDialogue.FileOk += GetFileNameDialogue_FileOk;
            this._mainWindow.GetFileNameDialogue.Title = Caption;
            this._mainWindow.GetFileNameDialogue.Filter = Filter;
            this._mainWindow.GetFileNameDialogue.FileName = ParseStringForVariable(this.DefaultFilename);
            
            if (String.IsNullOrEmpty(this.DefaultFilename) && hasOldFilename)
            {
                this._mainWindow.GetFileNameDialogue.FileName = oldFilename as String;
            }
        }

        protected override void DoAction()
        {
            DialogResult result =
                    (DialogResult)
                    _mainWindow.Invoke(new Func<Form, DialogResult>(_mainWindow.GetFileNameDialogue.ShowDialog),
                                       GetCurrentCore().Window);
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
            this._mainWindow.GetFileNameDialogue.FileOk -= GetFileNameDialogue_FileOk;
        }
    }
}

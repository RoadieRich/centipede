using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI", DisplayName = "Ask for Input (File Browser)")]
    class GetFileNameAction : Action
    {
        public GetFileNameAction(IDictionary<string, object> variables, ICentipedeCore c)
            : base("Ask for Input (File Browser)", variables, c)
        {
            
        }

        [ActionArgument(Literal = true, Usage = "(Optional) Text to display in the titlebar of the popup form")]
        public string Title = "Choose File";

        [ActionArgument(Usage = "(Optional) Message to display in the popup form")]
        public String Prompt = "Please browse to file";

        [ActionArgument(Literal = true, Usage = "(Required) Name of variable to store the chosen filename")]
        public String DestinationVariable = "Filename";
      
        [ActionArgument(DisplayName = "Filter", Usage = "(Optional) Filter the browse dialog to only show certain file types")]
        public String Filter = "All Files (*.*)|*.*";

        [ActionArgument(DisplayName = "Default filename", Usage = "(Optional) ")]
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
            FileDialog dialog = (FileDialog)sender;
            
            Variables[ParseStringForVariable(DestinationVariable)] = dialog.FileName;
        }

        protected override void CleanupAction()
        {
            this._mainWindow.GetFileNameDialogue.FileOk -= GetFileNameDialogue_FileOk;
        }
    }
}

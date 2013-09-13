using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("User Interface", DisplayName = "Ask for Input (File Browser)", IconName = "ui")]
    public class GetFileNameAction : UIAction
    {
        public GetFileNameAction(ICentipedeCore c)
            : base("Ask for Input (File Browser)", c)
        {
            this.Title = "Choose File";
            this.Prompt = "";
        }

        [ActionArgument(DisplayName = "Variable",
            Usage = "(Required) Name of variable to store the chosen filename")]
        public String DestinationVariable = "Filename";

        [ActionArgument(DisplayName = "Filter",
            Usage = "(Optional) Filter the browse dialog to only show certain file types")]
        public String Filter = "All Files (*.*)|*.*";

        private OpenFileDialog _dialog;
        private TextBox _tb;

        protected override IEnumerable<Control[]> GetControls()
        {
            string myFilter = ParseStringForVariable(Filter);
            if (String.IsNullOrEmpty(this.DestinationVariable))
            {
                throw new ActionException("No variable name provided", this);
            }

            object obj;
            string oldFilename = this.Variables.TryGetValue(this.DestinationVariable, out obj)
                                     ? (obj as string) ?? ""
                                     : "";

            this._dialog = new OpenFileDialog
                           {
                               Title = String.IsNullOrEmpty(Title)
                                           ? "Open File"
                                           : this.ParseStringForVariable(Title),
                               Filter = String.IsNullOrEmpty(myFilter)
                                            ? "All Files (*.*)|*.*"
                                            : myFilter
                           };

            _dialog.FileOk += GetFileNameDialogue_FileOk;

            _tb = new TextBox
                  {
                      Text = oldFilename,
                      Width = 300
                      //AutoSize = true
                  };

            var btn = new Button
                      {
                          Text = "Browse...",
                          AutoSize = true
                      };

            btn.Click += btnBrowse_Click;

            return new[] {new Control[] {_tb, btn}};

        }

        protected override void FormClosed(object sender, FormClosedEventArgs e)
        {
            if (((Form) sender).DialogResult == DialogResult.OK)
            {
                //Save
                this.Variables[this.DestinationVariable] = this._tb.Text;
            }
        }

        private void GetFileNameDialogue_FileOk(object sender, CancelEventArgs e)
        {
            _tb.Text = _dialog.FileName;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            _dialog.FileName = _tb.Text;
            _dialog.ShowDialog();
        }
    }
}

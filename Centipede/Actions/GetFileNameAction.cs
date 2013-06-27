using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.ComponentModel;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("User Interface", DisplayName = "Ask for Input (File Browser)", IconName = "ui")]
    class GetFileNameAction : Action
    {
        public GetFileNameAction(IDictionary<string, object> variables, ICentipedeCore c)
            : base("Ask for Input (File Browser)", variables, c)
        {
            
        }

        [ActionArgument(Usage = "(Optional) Text to display in the titlebar of the popup form")]
        public string Title = "Choose File";

        [ActionArgument(Usage = "(Optional) Message to display in the popup form")]
        public String Prompt = "";

        [ActionArgument(DisplayName = "Variable", Usage = "(Required) Name of variable to store the chosen filename")]
        public String DestinationVariable = "Filename";
      
        [ActionArgument(DisplayName = "Filter", Usage = "(Optional) Filter the browse dialog to only show certain file types")]
        public String Filter = "All Files (*.*)|*.*";

        private OpenFileDialog _dialog;
        private TableLayoutPanel _tableLayoutPanel;
        private Form _form;
        private TextBox _tb;

        protected override void DoAction()
        {

            string myTitle = ParseStringForVariable(Title); 
            string myPrompt = ParseStringForVariable(Prompt);
            string myDestinationVariable = ParseStringForVariable(DestinationVariable);
            string myFilter = ParseStringForVariable(Filter); 

            if (String.IsNullOrEmpty(myDestinationVariable))
            {
                throw new ActionException("No variable name provided", this);
            }
                        
            this._form = new Form
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                SizeGripStyle = SizeGripStyle.Hide,
                ShowIcon = false,
                ShowInTaskbar = false,
                Text = myTitle
            };

            object oldFilename;
            bool hasOldFilename = Variables.TryGetValue(myDestinationVariable, out oldFilename);

            this._dialog = new OpenFileDialog
            {
                Title = String.IsNullOrEmpty(myTitle) ? "Open File" : myTitle,
                Filter = String.IsNullOrEmpty(myFilter) ? "All Files (*.*)|*.*" : myFilter
            };

            _dialog.FileOk += GetFileNameDialogue_FileOk;

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                ColumnCount = 2,
                ColumnStyles = { new ColumnStyle(), new ColumnStyle(SizeType.AutoSize) }
            };

            this._tableLayoutPanel = tableLayoutPanel;

            Padding TablePadding = new Padding();
            TablePadding.All = 10;
            this._tableLayoutPanel.Padding = TablePadding;

            if (!String.IsNullOrEmpty(myPrompt))
            {
                Label label = new Label
                {
                    Text = myPrompt,
                    AutoSize = true
                };
                this._tableLayoutPanel.Controls.Add(label);
                this._tableLayoutPanel.SetColumnSpan(label, 2);
            }

            _tb = new TextBox
            {
                Text = (string) oldFilename,
                Width = 300
                //AutoSize = true
            };

            Button btn = new Button
            {
                Text = "Browse...",
                AutoSize = true
            };

            btn.Click += btnBrowse_Click;

            this._tableLayoutPanel.Controls.Add(_tb); 
            this._tableLayoutPanel.Controls.Add(btn); 

            Button btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Fill
            };

            this._form.AcceptButton = btnOK;

            this._tableLayoutPanel.Controls.Add(btnOK);
            this._tableLayoutPanel.SetColumnSpan(btnOK, 2);

            this._form.Controls.Add(this._tableLayoutPanel);

            _form.FormClosing += FormClosing;

            DialogResult result =
                (DialogResult)GetCurrentCore().
                    Window.Invoke(new Func<Form, DialogResult>(this._form.ShowDialog),
                                  GetCurrentCore().Window);

            switch (result)
            {
                case DialogResult.OK:
                    //Save
                    Variables[myDestinationVariable] = _tb.Text;
                    break;

                case DialogResult.Cancel:
                    throw new FatalActionException("User input cancelled", this);
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

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void FormClosing(object sender, FormClosingEventArgs a)
        {
            switch (_form.DialogResult)
            {
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    DialogResult result = MessageBox.Show("Cancelling user input will abort the current job.  Retry?", "Centipede", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
                    if (result == DialogResult.Retry) a.Cancel = true;
                    break;
            }
        }
    }
}

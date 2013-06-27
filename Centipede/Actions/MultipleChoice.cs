using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("User Interface", DisplayName = "Ask for Input (Multiple Choice)", IconName = "ui")]
    public class MultipleChoice : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="core"></param>
        public MultipleChoice(IDictionary<string, object> variables, ICentipedeCore core)
            : base("Ask for Input (Multiple Choice)", variables, core)
        { }

        [ActionArgument(Usage = "(Optional) Text to display in the titlebar of the popup form")]
        public string Title = "Choice";

        [ActionArgument(Usage = "(Optional) Message to display in the popup form")]
        public String Prompt = "Please select from the following choices";

        [ActionArgument(Usage = "(Required) List of choices, separated by commas")]
        public String Choices = "Choice 1, Choice 2, Choice 3";

        [ActionArgument(Usage = "Controls whether user input is by buttons or drop-down list")]
        public bool RadioButtons = true;

        [ActionArgument(DisplayName = "Variable (value)", Usage = "(Optional) Variable to be updated with the value of the selected choice")]
        public String ChoiceNameVar = "ChoiceResult";

        [ActionArgument(DisplayName = "Variable (index)", Usage = "(Optional) Variable to to be updated with the index number of the selected choice")]
        public String ChoiceIndexVar = "";

        private TableLayoutPanel _tableLayoutPanel;
        private Form _form;

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        /// <exception cref="FatalActionException">The job needs to halt</exception>
        protected override void DoAction()
        {
            string myTitle = ParseStringForVariable(Title); 
            string myPrompt = ParseStringForVariable(Prompt);
            string myChoices = ParseStringForVariable(Choices);
            string myChoiceNameVar = ParseStringForVariable(ChoiceNameVar);
            string myChoiceIndexVar = ParseStringForVariable(ChoiceIndexVar);

            if (String.IsNullOrEmpty(myChoices))
            {
                throw new ActionException("No choices provided", this);
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
            

            var choices = myChoices.Split(',');
            
            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel
                                                {
                                                    Dock = DockStyle.Fill,
                                                    AutoSize = true,
                                                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                                                    ColumnCount = 2,
                                                    RowCount = choices.Length
                                                               + (String.IsNullOrEmpty(Prompt) ? 1 : 2),
                                                    ColumnStyles = { new ColumnStyle(), new ColumnStyle(SizeType.AutoSize) }
                                                };
            //tableLayoutPanel.ColumnStyles[1].SizeType = SizeType.AutoSize;
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

            if (RadioButtons)
            {
                int i = 0;

                foreach (KeyValuePair<int, String> choice in choices.Enumerate())
                {
                    RadioButton radioButton = new RadioButton
                                              {
                                                  Text = choice.Value.Trim(),
                                                  Checked = i == 0 ? true : false,
                                                  Tag = i++,
                                                  AutoSize=true
                                              };

                    //radioButton.Width = 20 + TextRenderer.MeasureText(choice.Value.Trim(), radioButton.Font).Width;

                    this._tableLayoutPanel.Controls.Add(radioButton);
                    this._tableLayoutPanel.SetColumnSpan(radioButton, 2);
                }
            }
            else
            {
                ComboBox comboBox = new ComboBox
                                    {
                                        DropDownStyle = ComboBoxStyle.DropDown,
                                        Dock = DockStyle.Fill,
                                        AutoSize = true,
                                        //AutoCompleteMode = AutoCompleteMode.SuggestAppend
                                    };

                comboBox.Items.AddRange(choices.Select(s => s.Trim()));
                comboBox.SelectedIndex = 0;

                //comboBox.Width = 20 + choices.Max(s => TextRenderer.MeasureText(s.Trim(), comboBox.Font).Width);

                this._tableLayoutPanel.Controls.Add(comboBox);
                this._tableLayoutPanel.SetColumnSpan(comboBox, 2);    
            }

            this._tableLayoutPanel.ColumnCount = 2;
            int rows = this._tableLayoutPanel.RowCount;

            this._form.FormClosing += FormClosing;

            Button btnCancel = new Button
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
                Dock = DockStyle.Fill
            };

            this._form.CancelButton = btnCancel;

            Button btnOK = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Dock = DockStyle.Fill
            };

            this._form.AcceptButton = btnOK;

            //table.Controls.Add(btnCancel);  // Removed cancel button for consistent appearance when table columns resize
            this._tableLayoutPanel.Controls.Add(btnOK);
            this._tableLayoutPanel.SetColumnSpan(btnOK, 2);

            this._form.Controls.Add(this._tableLayoutPanel);

            DialogResult result =
                (DialogResult)GetCurrentCore().
                    Window.Invoke(new Func<Form, DialogResult>(this._form.ShowDialog),
                                  GetCurrentCore().Window);

            switch (result)
            {
                case DialogResult.OK:
                    // Save choice
                    if (RadioButtons)
                    {
                        RadioButton checkedButton =
                                    tableLayoutPanel.Controls.OfType<RadioButton>().First(button => button.Checked);

                        if (!String.IsNullOrEmpty(myChoiceNameVar))
                        {
                            Variables[myChoiceNameVar] = checkedButton.Text;
                        }
                        if (!String.IsNullOrEmpty(myChoiceIndexVar))
                        {
                            Variables[myChoiceIndexVar] = (int) checkedButton.Tag;
                        }
                    }
                    else
                    {
                        ComboBox comboBox = tableLayoutPanel.Controls.OfType<ComboBox>().First();

                        if (!String.IsNullOrEmpty(myChoiceNameVar))
                        {
                            Variables[myChoiceNameVar] = (string) comboBox.SelectedItem;
                        }
                        if (!String.IsNullOrEmpty(myChoiceIndexVar))
                        {
                            Variables[myChoiceIndexVar] = comboBox.SelectedIndex;
                        }
                    }
                    break;

                case DialogResult.Cancel:
                    throw new FatalActionException("User input cancelled", this);
            }
        }

        //-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

        private void FormClosing(object sender, FormClosingEventArgs a)
        {
            Form dialog = (Form)sender;

            switch (dialog.DialogResult)
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

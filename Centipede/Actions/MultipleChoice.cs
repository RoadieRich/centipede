using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI", DisplayName = "Ask for Input (Multiple Choice)")]
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

        [ActionArgument(Literal = true, Usage = "(Optional) Text to display in the titlebar of the popup form")]
        public string Title = "Choice";

        [ActionArgument(Usage = "(Optional) Message to display in the popup form")]
        public String Prompt = "Please select from the following choices";

        [ActionArgument(Usage = "(Required) List of choices, separated by commas")]
        public String Choices = "";

        [ActionArgument(Usage = "Controls whether user input is by buttons or drop-down list")]
        public bool RadioButtons;

        [ActionArgument(Usage = "(Optional) Variable to be updated with the value of the selected choice")]
        public String ChoiceNameVar = "";

        [ActionArgument(Usage = "(Optional) Variable to to be updated with the index number of the selected choice ")]
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
                             Text = this.Title
                         };
            
            if (String.IsNullOrEmpty(Choices))
            {
                throw new ActionException("No choices listed", this);
            }
            var choices = ParseStringForVariable(this.Choices).Split(',');
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

            var RowPadding = new System.Windows.Forms.Padding();
            RowPadding.All = 10;

            this._tableLayoutPanel.Padding = RowPadding;
            
            if (!String.IsNullOrEmpty(Prompt))
            {
                Label label = new Label
                              {
                                  Text = this.Prompt,
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
                                                  Tag = i++,
                                                  AutoSize=true
                                              };

                    radioButton.Width = 20 + TextRenderer.MeasureText(choice.Value.Trim(), radioButton.Font).Width;

                    this._tableLayoutPanel.Controls.Add(radioButton); //, 0, choice.Key);
                    this._tableLayoutPanel.SetColumnSpan(radioButton, 2);
                }

                this._form.FormClosing += RadioButtonFormOnFormClosing;
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

                comboBox.Width = 20 + choices.Max(s => TextRenderer.MeasureText(s.Trim(), comboBox.Font).Width);

                this._tableLayoutPanel.Controls.Add(comboBox);
                this._tableLayoutPanel.SetColumnSpan(comboBox, 2);

                this._form.FormClosing += CheckBoxFormOnFormClosing; //+= (sender, e) => { }
            }

            this._tableLayoutPanel.ColumnCount = 2;
            int rows = this._tableLayoutPanel.RowCount;


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

            if (result == DialogResult.Cancel)
            {
                throw new FatalActionException("Cancel Clicked", this);
            }
        }

        private void CheckBoxFormOnFormClosing(object sender, FormClosingEventArgs formClosingEventArgs)
        {
            Form form = (Form)sender;
            TableLayoutPanel tableLayoutPanel = form.Controls.OfType<TableLayoutPanel>().First();
            if (form.DialogResult ==  DialogResult.OK)
            {
                ComboBox comboBox = tableLayoutPanel.Controls.OfType<ComboBox>().First();
                SetVars((string)comboBox.SelectedItem, comboBox.SelectedIndex);
            }
        }

        private void SetVars(string item, int index)
        {
            if (!String.IsNullOrEmpty(this.ChoiceNameVar))
            {
                Variables[this.ChoiceNameVar] = item;
            }
            if (!String.IsNullOrEmpty(this.ChoiceIndexVar))
            {
                Variables[this.ChoiceIndexVar] = index;
            }
        }

        private void RadioButtonFormOnFormClosing(object sender, FormClosingEventArgs eventArgs)
        {
            Form form = (Form)sender;
            TableLayoutPanel tableLayoutPanel = form.Controls.OfType<TableLayoutPanel>().First();
            if (form.DialogResult == DialogResult.OK)
            {
                try
                {
                    RadioButton checkedButton =
                            tableLayoutPanel.Controls.OfType<RadioButton>().First(button => button.Checked);

                    SetVars(checkedButton.Text, (int)checkedButton.Tag);
                }

                catch (InvalidOperationException)
                {
                    MessageBox.Show("No choice selected, please try again");
                    eventArgs.Cancel = true;
                }
            }
        }
    }
}

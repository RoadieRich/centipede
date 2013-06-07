﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI", DisplayName = "Multiple Choice")]
    public class MultipleChoice : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="core"></param>
        public MultipleChoice(IDictionary<string, object> variables, ICentipedeCore core)
                : base("Multiple Choice", variables, core)
        { }

        [ActionArgument]
        public String Choices = "";

        [ActionArgument]
        public bool RadioButtons;

        [ActionArgument]
        public String ChoiceNameVar = "";

        [ActionArgument]
        public String ChoiceIndexVar = "";
        [ActionArgument(Literal = true)]
        public string Title = "Choice";

        [ActionArgument]
        public String Prompt = "";

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
            if (!String.IsNullOrEmpty(Prompt))
            {
                Label label = new Label
                              {
                                  Text = this.Prompt
                              };
                this._tableLayoutPanel.Controls.Add(label);
                this._tableLayoutPanel.SetColumnSpan(label, 2);
            }

            if (RadioButtons)
            {
                int i = 0;

                foreach (var choice in choices.Enumerate())
                {
                    RadioButton radioButton = new RadioButton
                                              {
                                                  Text = choice.Value.Trim(),
                                                  Tag = i++
                                              };
                    
                    this._tableLayoutPanel.Controls.Add(radioButton, 0, choice.Key);
                    this._tableLayoutPanel.SetColumnSpan(radioButton, 2);
                }

                this._form.FormClosing += RadioButtonFormOnFormClosing;
            }
            else
            {
                ComboBox comboBox = new ComboBox
                                    {
                                        DropDownStyle = ComboBoxStyle.DropDown,
                                        AutoSize = true,
                                        //AutoCompleteMode = AutoCompleteMode.SuggestAppend
                                    };

                comboBox.Items.AddRange(choices.Select(s => s.Trim()));

                this._tableLayoutPanel.Controls.Add(comboBox);
                this._tableLayoutPanel.SetColumnSpan(comboBox, 2);

                this._form.FormClosing += CheckBoxFormOnFormClosing; //+= (sender, e) => { }
            }

            this._tableLayoutPanel.ColumnCount = 2;
            int rows = this._tableLayoutPanel.RowCount;
            this._tableLayoutPanel.Controls.Add(new Button
                                                {
                                                    Text = "Cancel",
                                                    DialogResult = DialogResult.Cancel,
                                                    Dock = DockStyle.Fill
                                                }, 0, rows);
            this._tableLayoutPanel.Controls.Add(new Button
                                                {
                                                    Text = "OK",
                                                    DialogResult = DialogResult.OK,
                                                    Dock = DockStyle.Fill
                                                }, 1, rows);
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

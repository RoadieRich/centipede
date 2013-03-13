﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI", displayName = "Multiple Choice")]
    public class MultipleChoice : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="v"></param>
        /// <param name="core"></param>
        public MultipleChoice(IDictionary<string, object> v, ICentipedeCore core)
                : base("Multiple Choice", v, core)
        { }

        [ActionArgument]
        public String Choices = "";

        [ActionArgument]
        public bool RadioButtons;

        [ActionArgument]
        public String ChoiceNameVar = "";

        [ActionArgument]
        public String ChoiceIndexVar = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        /// <exception cref="ActionException">
        /// the action cannot be completed
        /// </exception>
        /// <exception cref="FatalActionException">The job needs to halt</exception>
        protected override void DoAction()
        {
            Form form = new Form
            {
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                StartPosition = FormStartPosition.CenterParent,
                MinimizeBox = false,
                MaximizeBox = false,
                SizeGripStyle = SizeGripStyle.Hide
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
                                                        RowCount = choices.Length + 1
                                                        
                                                };
            if (RadioButtons)
            {
                int i = 0;

                for (int index = 0; index < choices.Length; index++)
                {
                    RadioButton radioButton = new RadioButton
                                              {
                                                      Text = choices[index].Trim(),
                                                      Tag = i++
                                              };
                    
                    tableLayoutPanel.Controls.Add(radioButton, 0, index);
                    tableLayoutPanel.SetColumnSpan(radioButton, 2);
                }

                form.FormClosing += delegate
                                    {
                                        switch (form.DialogResult)
                                        {
                                        case DialogResult.OK:
                                            RadioButton radioButton = tableLayoutPanel.Controls.OfType<RadioButton>()
                                                                          .First(button => button.Checked);
                                            if (!String.IsNullOrEmpty(ChoiceNameVar))
                                            {
                                                Variables[ChoiceNameVar] = radioButton.Text;
                                            }
                                            if (!String.IsNullOrEmpty(ChoiceIndexVar))
                                            {
                                                Variables[ChoiceIndexVar] = (int)radioButton.Tag;
                                            }
                                            break;
                                        case DialogResult.Cancel:
                                            throw new FatalActionException("Cancel Clicked", this);
                                        }
                                    };
            }
            else
            {
                ComboBox comboBox = new ComboBox
                                    {
                                            DropDownStyle = ComboBoxStyle.DropDown
                                    };

                comboBox.Items.AddRange(choices.Select(s => s.Trim()));

                tableLayoutPanel.Controls.Add(comboBox);
                tableLayoutPanel.SetColumnSpan(comboBox, 2);

                form.FormClosing += delegate
                                    {
                                        switch (form.DialogResult)
                                        {
                                        case DialogResult.OK:
                                            if (String.IsNullOrEmpty(ChoiceNameVar))
                                            {
                                                Variables[ChoiceNameVar] = comboBox.SelectedItem;
                                            }
                                            if (String.IsNullOrEmpty(ChoiceIndexVar))
                                            {
                                                Variables[ChoiceIndexVar] = comboBox.SelectedIndex;
                                            }
                                            break;
                                        case DialogResult.Cancel:
                                            throw new FatalActionException("Cancel Clicked", this);
                                        }


                                    };
            }

            tableLayoutPanel.ColumnCount = 2;
            int rows = tableLayoutPanel.RowCount;
            tableLayoutPanel.Controls.Add(new Button
                                          {
                                                  Text = "Cancel",
                                                  DialogResult = DialogResult.Cancel,
                                                  Dock = DockStyle.Fill
                                          }, 0, rows);
            tableLayoutPanel.Controls.Add(new Button
                                          {
                                                  Text = "OK",
                                                  DialogResult = DialogResult.OK,
                                                  Dock = DockStyle.Fill
                                          }, 1, rows);
            form.Controls.Add(tableLayoutPanel);

            Form.ActiveForm.Invoke(new Func<DialogResult>(form.ShowDialog));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("UI")]
    public class MultipleChoice : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="v"></param>
        /// <param name="core"></param>
        public MultipleChoice(string name, IDictionary<string, object> v, ICentipedeCore core)
                : base(name, v, core)
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

            if (RadioButtons)
            {
                int i = 0;
                form.Controls.AddRange(this.Choices.Split(',')
                                           .Select(choice => new RadioButton
                                                             {
                                                                     Text = choice.Trim(),
                                                                     Tag = i++
                                                             }));
                form.FormClosing += delegate
                                    {
                                        switch (form.DialogResult)
                                        {
                                        case DialogResult.OK:
                                            RadioButton radioButton = form.Controls.OfType<RadioButton>()
                                                                          .First(button => button.Checked);
                                            if (String.IsNullOrEmpty(ChoiceNameVar))
                                            {
                                                Variables[ChoiceNameVar] = radioButton.Text;
                                            }
                                            if (String.IsNullOrEmpty(ChoiceIndexVar))
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

                comboBox.Items.AddRange(Choices.Split(',').Select(s => s.Trim()));
                form.Controls.Add(comboBox);

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

            form.Controls.Add(new TableLayoutPanel
            {
                ColumnCount = 2,
                GrowStyle = TableLayoutPanelGrowStyle.AddRows,
                Dock = DockStyle.Fill,
                AutoSize = true,
                AutoSizeMode = AutoSizeMode.GrowAndShrink,
                Controls = {
                                            new Button
                                            {
                                                    Text = "Cancel",
                                                    DialogResult = DialogResult.Cancel,
                                                    Dock = DockStyle.Fill
                                            },
                                            new Button
                                            {
                                                    Text = "OK",
                                                    DialogResult = DialogResult.OK,
                                                    Dock = DockStyle.Fill
                                            }
                }
            });
            
            form.ShowDialog();
        }
    }
}

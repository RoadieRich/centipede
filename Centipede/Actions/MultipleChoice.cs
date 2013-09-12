using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using CentipedeInterfaces;
using CentipedeInterfaces.Extensions;


namespace Centipede.Actions
{
    [ActionCategory("User Interface", DisplayName = "Ask for Input (Multiple Choice)",
        IconName = "ui")]
    public class MultipleChoice : UIAction
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="core"></param>
        public MultipleChoice(IDictionary<string, object> variables, ICentipedeCore core)
            : base("Ask for Input (Multiple Choice)", "select from", variables, core)
        {  }

        
        [ActionArgument(Usage = "(Required) List of choices, separated by commas")]
        public String Choices = "Choice 1, Choice 2, Choice 3";

        [ActionArgument(Usage = "Controls whether user input is by buttons or drop-down list")]
        public bool RadioButtons = true;

        [ActionArgument(DisplayName = "Variable (value)",
            Usage = "(Optional) Variable to be updated with the value of the selected choice")]
        public String ChoiceNameVar = "ChoiceResult";

        [ActionArgument(DisplayName = "Variable (index)",
            Usage =
                "(Optional) Variable to to be updated with the index number of the selected choice")
        ]
        public String ChoiceIndexVar = "";

        private IEnumerable<Control> _controls;
        

        protected override IEnumerable<Control[]> GetControls()
        {
            string myChoices = ParseStringForVariable(Choices);
            string myChoiceNameVar = ParseStringForVariable(ChoiceNameVar);
            string myChoiceIndexVar = ParseStringForVariable(ChoiceIndexVar);

            if (String.IsNullOrEmpty(myChoices))
            {
                throw new ActionException("No choices provided", this);
            }


            var choices = myChoices.Split(',').Select(s => s.Trim());

            if (RadioButtons)
            {
                _controls = choices.Select((choice, i) => new RadioButton
                                                          {
                                                              Text = choice.Trim(),
                                                              Checked = i == 0,
                                                              Tag = i,
                                                              AutoSize = true
                                                          }
                    );
            }
            else
            {
                var controlList = new List<Control>();
                var comboBox = new ComboBox
                               {
                                   DropDownStyle = ComboBoxStyle.DropDown,
                                   Dock = DockStyle.Fill,
                                   AutoSize = true,
                                   //AutoCompleteMode = AutoCompleteMode.SuggestAppend
                               };

                comboBox.Items.AddRange(choices.Select(s => s.Trim()));
                comboBox.SelectedIndex = 0;

                controlList.Add(comboBox);
                _controls = controlList;

            }

            return _controls.Select(c => new[]{c});
        }


        protected override void FormClosed(object sender, FormClosedEventArgs e)
        {
            if (((Form) sender).DialogResult != DialogResult.OK)
            {
                throw new FatalActionException("User input cancelled", this);
            }
            // Save choice

            string choiceName;
            int choiceIndex;
            if (this.RadioButtons)
            {
                RadioButton selectedButton =
                    this._controls.OfType<RadioButton>().First(button => button.Checked);

                choiceName = selectedButton.Text;
                choiceIndex = (int) selectedButton.Tag;
            }
            else
            {
                ComboBox comboBox = this._controls.OfType<ComboBox>().First();
                choiceIndex = comboBox.SelectedIndex;
                choiceName = comboBox.SelectedText;
            }

            if (!String.IsNullOrEmpty(this.ChoiceNameVar))
            {
                this.Variables[this.ParseStringForVariable(this.ChoiceNameVar)] = choiceName;
            }
            if (!String.IsNullOrEmpty(this.ChoiceIndexVar))
            {
                this.Variables[this.ParseStringForVariable(this.ChoiceIndexVar)] = choiceIndex;
            }
        }
    }
}

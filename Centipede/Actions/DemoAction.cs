using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Centipede.Properties;


namespace Centipede.Actions
{
    [ActionCategory("UI")]
    public class DemoAction : Action
    {
        [Localizable(false)]
        public DemoAction(Dictionary<String, Object> v)
            : base("Demo Action", v)
        {
            Test2 = "Text";
        }
        
        protected override void DoAction()
        {
            MessageBox.Show(String.Format("Test 1 attribute value: {0}\r\nTest 2 attribute value: {1}",
                                          Test1, 
                                          ParseStringForVariable(Test2)
                           ),
                           Resources.DemoAction_DoAction_Demo_Action_executed
            );
        }

        [ActionArgument(
            usage = "First Value to display (int)",
            displayName = "Test 1",
            onChangedHandlerName = "Test1Set"
        )]
        public Int32 Test1 = 1;

        public void Test1Set(Object sender, EventArgs e)
        {

            TextBox textBox = (TextBox)sender;
            Int32 oldVal = Test1;
            if (Int32.TryParse(textBox.Text, out Test1) || Int32.TryParse(ParseStringForVariable(textBox.Text), out Test1))
            {
                return;
            }
            textBox.Text = oldVal.ToString(CultureInfo.InvariantCulture);
            Test1 = oldVal;
        }

        [ActionArgument(
            usage = "Second Value to display (string)",
            displayName = "Test 2")]
        public String Test2 { get; set; }
    }

    
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using Centipede.Properties;
using CentipedeInterfaces;


namespace Centipede.Actions
{
    [ActionCategory("Demo", DisplayName="Demo Action")]
    public class DemoAction : Action
    {
        [Localizable(false)]
        public DemoAction(IDictionary<string, object> variables, ICentipedeCore c)
            : base("Demo Action", variables, c)
        {
            Test2 = "Text";
        }
        
        protected override void DoAction()
        {
            Form window = GetCurrentCore().Window;
            
            window.Invoke(new Func<DialogResult>(() => MessageBox.Show(window,
                              String.Format("Test 1 attribute value: {0}\r\nTest 2 attribute value: {1}",
                                            Test1,
                                            ParseStringForVariable(Test2)
                                  ),
                              Resources.DemoAction_DoAction_Demo_Action_executed
                          )));
            Message(MessageText);
        }

        [ActionArgument(
            Usage = "First Value to display (int)",
            DisplayName = "Test 1",
            OnChangedHandlerName = "Test1Set",
            Literal = true
        )]
        public Int32 Test1 = 1;

        
        [ActionArgument(DisplayName = "Message", Literal=true)]
        public String MessageText = "This is sent as a message";


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

            OnMessage(new MessageEventArgs(MessageText, MessageLevel.Action));
        }

        [ActionArgument(
            Usage = "Second Value to display (string)",
            DisplayName = "Test 2")]
        public String Test2 { get; set; }
    }

    
}

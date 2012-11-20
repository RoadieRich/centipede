using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Centipede
{
    [ActionCategory("UI")]
    public class DemoAction : Action
    {
        public DemoAction(Dictionary<String, Object> variables)
            : base("Demo Action", variables)
        { }
        
        public override void DoAction()
        {
            MessageBox.Show(String.Format("Test 1 attribute value: {0}\r\nTest 2 attribute value: {1}",
                                          Test1, 
                                          ParseStringForVariable(Test2)
                           )
                           ,
                           "Demo Action executed"
            );
        }

        [ActionArgument(
            usage = "First Value to display (int)",
            displayName = "Test 1",
            setterMethodName = "Test1Set"
        )]
        public Int32 Test1 = 1;

        public Boolean Test1Set(ref String s)
        {
            Int32 oldVal = Test1;
            if (Int32.TryParse(s, out Test1))
            {
                return true;
            }
            else
            {
                s = oldVal.ToString();
                Test1 = oldVal;
                return false;
            }
        }

        [ActionArgument(
            usage = "Second Value to display (string)",
            displayName = "Test 2")]
        public String Test2 = "attr 2";
    }

    
}

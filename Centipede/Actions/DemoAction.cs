using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Forms;
using Centipede.Properties;


namespace Centipede.Actions
{
    [ActionCategory("UI")]
    public class DemoAction : Action
    {
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
// ReSharper disable MemberCanBePrivate.Global
        public Int32 Test1 = 1;
// ReSharper restore MemberCanBePrivate.Global

// ReSharper disable UnusedMember.Global
        public Boolean Test1Set(ref String s)
// ReSharper restore UnusedMember.Global
        {


            Int32 oldVal = Test1;
            if (Int32.TryParse(s, out Test1) || Int32.TryParse(ParseStringForVariable(s), out Test1))
            {
                return true;
            }
            s = oldVal.ToString(CultureInfo.InvariantCulture);
            Test1 = oldVal;
            return false;
        }

        [ActionArgument(
            usage = "Second Value to display (string)",
            displayName = "Test 2")]
// ReSharper disable MemberCanBePrivate.Global
        public String Test2 { get; set; }
// ReSharper restore MemberCanBePrivate.Global
    }

    
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Centipede
{
    [ActionCategory("Other")]
    public class DemoAction : Action
    {
        public DemoAction(Dictionary<String, Object> variables)
            : base("Demo Action", variables)
        { }
        
        public override void DoAction()
        {
            MessageBox.Show(String.Format("Test 1 attribute value: {0}\r\nTest 2 attribute value: {1}",
                                          Test1, ParseStringForVariables(Test2)
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

        public Boolean Test1Set(String s)
        {
            return Int32.TryParse(s, out Test1);
        }

        [ActionArgument(
            usage = "Second Value to display (string)",
            displayName = "Test 2")]
        public String Test2 = "attr 2";
    }

    class DemoActionFactory : ActionFactory
    {
        public DemoActionFactory()
            : base("Demo Action")
        { }
        public override Action Generate()
        {
            return new DemoAction(Program.Variables);
        }
    }
}

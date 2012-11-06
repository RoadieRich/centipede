using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Centipede
{
    public class DemoAction : Action
    {
        public DemoAction()
            : base("Demo Action")
        {
            Attributes.Add("Test 1", 1);
            Attributes.Add("Test 2", "attr 2");
        }

        public override void DoAction()
        {
            MessageBox.Show(String.Format("Test 1 attribute value: {0}\r\nTest 2 attribute value: {1}",
                                          Attributes.ToArray().Select(
                                              kvp=>kvp.Value.ToString()
                                          )
                           ),
                           "Demo Action executed"
            );
        }
    }

    class DemoActionFactory : ActionFactory
    {
        DemoActionFactory()
            : base("Demo Action")
        { }
        public override Action Generate(string name)
        {
            return new DemoAction();
        }
    }
}

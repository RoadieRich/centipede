﻿using System;
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
                                          ParseAttribute<Int32>("Test 1"), ParseAttribute<String>("Test 2")
                           )
                           ,
                           "Demo Action executed"
            );
        }
    }

    class DemoActionFactory : ActionFactory
    {
        public DemoActionFactory()
            : base("Demo Action")
        { }
        public override Action Generate()
        {
            return new DemoAction();
        }
    }
}

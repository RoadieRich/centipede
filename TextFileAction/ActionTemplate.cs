using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centipede;

namespace Centipede.TextFileAction
{
    public class ActionTemplate : Action
    {
        public ActionTemplate(Dictionary<String, Object> variables)
            : base("ActionTemplate", variables)
        {

        }

        public override void DoAction()
        {
            throw new NotImplementedException();
        }
    }
}

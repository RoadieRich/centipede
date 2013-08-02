using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CentipedeInterfaces;

namespace Centipede.Actions
{

    [ActionCategory("Other Actions")]
    public class GetArguments : Action
    {
        public GetArguments(IDictionary<string, object> variables, ICentipedeCore core)
            : base("Get Arguments", variables, core)
        { }

        [ActionArgument]
        public string Destination = "args";

        protected override void DoAction()
        {
            Variables[Destination] = GetCurrentCore().Arguments;
        }
    }
}

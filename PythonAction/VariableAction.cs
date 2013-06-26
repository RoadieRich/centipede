using System;
using System.Collections.Generic;
using Centipede;
using CentipedeInterfaces;
using PythonEngine;
using Action = Centipede.Action;

namespace PyAction
{
    [ActionCategory("Flow Control", DisplayName="Variable", iconName=@"variable")]
    public class VariableAction : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public VariableAction(IDictionary<string, object> variables, ICentipedeCore c)
                : base("Variable", variables, c)
        { }

        [ActionArgument]
        public String Expresson = "";

        [ActionArgument(DisplayName = "Destination Variable Name")]
        public string DestinationVarName = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        protected override void DoAction()
        {
            IPythonEngine engine = GetCurrentCore().PythonEngine;
            var scope = engine.GetNewTypedScope(Variables);
            
            Variables[DestinationVarName] = engine.Evaluate<object>(Expresson, scope);
        }
    }
}

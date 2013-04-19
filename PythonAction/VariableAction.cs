using System;
using System.Collections.Generic;
using CentipedeInterfaces;
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
            var scope = PythonEngine.PythonEngine.Instance.GetNewTypedScope(Variables);
            Variables[DestinationVarName] = PythonEngine.PythonEngine.Instance.Evaluate<object>(Expresson, scope);
        }
    }
}

using System;
using System.Collections.Generic;
using Centipede;
using CentipedeInterfaces;
using PythonEngine;
using Action = Centipede.Action;

namespace PyAction
{
    [ActionCategory("Flow Control", DisplayName="Variable", IconName=@"variable")]
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

        [ActionArgument(DisplayName="Expression",Usage="(Required) A valid python expression")]
        public string Expression = "";

        [ActionArgument(DisplayName = "Destination Variable Name", Usage="(Required) Name of variable to store the result of the expression")]
        public string DestinationVarName = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        protected override void DoAction()
        {
            string myExpression = ParseStringForVariable(Expression);
            string myDestinationVarName = ParseStringForVariable(DestinationVarName);

            IPythonEngine engine = GetCurrentCore().PythonEngine;
            var scope = engine.GetNewTypedScope(Variables);

            Variables[myDestinationVarName] = engine.Evaluate<object>(myExpression, scope);
        }
    }
}

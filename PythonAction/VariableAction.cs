using System;
using System.Collections.Generic;
using Centipede;
using CentipedeInterfaces;
using PythonEngine;
using Action = Centipede.Action;

namespace PyAction
{
    [ActionCategory("Flow Control", DisplayName = "Assign to Variable", IconName = @"variable")]
    public class VariableAction : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <param name="c"></param>
        public VariableAction(IDictionary<string, object> variables, ICentipedeCore c)
            : base("Assign to Variable", variables, c)
        { }

        [ActionArgument(DisplayName = "Expression", Usage = "(Required) A valid python expression", Literal = true)]
        public string Expresson = "";

        [ActionArgument(DisplayName = "Variable", Usage = "(Required) Name of variable to store the expression result",
            Literal = true)]
        public string DestinationVarName = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        protected override void DoAction()
        {
            ParseStringForVariable(this.DestinationVarName);

            IPythonEngine engine = GetCurrentCore().PythonEngine;
            var scope = engine.GetNewTypedScope(Variables);

            Variables[DestinationVarName] = engine.Evaluate<object>(Expresson, scope);
        }
    }
}

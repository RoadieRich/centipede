using System;
using System.Collections.Generic;
using Centipede;
using Action = Centipede.Action;

// ReSharper disable UnusedMember.Global
// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable ConvertToConstant.Global
// ReSharper disable FieldCanBeMadeReadOnly.Global
namespace PyAction
{
    [ActionCategory("Flow Control", displayName="Variable", iconName=@"variable")]
    public class VariableAction : Action
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="v"></param>
        public VariableAction(Dictionary<string, object> v)
                : base("Variable", v)
        { }

        [ActionArgument]
        public String Expresson = "";

        [ActionArgument(displayName = "Destination Variable Name")]
        public string DestinationVarName = "";

        /// <summary>
        /// Perform the action
        /// </summary>
        protected override void DoAction()
        {
            var scope = PythonEngine.PythonEngine.Instance.GetNewScope(Variables);
            Variables[DestinationVarName] = PythonEngine.PythonEngine.Instance.Evaluate<object>(Expresson, scope);
        }
    }
}

using System;
using System.Collections.Generic;
using Centipede;
using CentipedeInterfaces;
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
        /// <param name="c"></param>
        public VariableAction(IDictionary<string, object> v, ICentipedeCore c)
                : base("Variable", v, c)
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

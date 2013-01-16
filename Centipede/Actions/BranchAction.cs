using System;
using System.Collections.Generic;


namespace Centipede.Actions
{
    /// <summary>
    /// 
    /// </summary>

    [ActionCategory("Flow Control", displayName="Branch", displayControl="BranchDisplayControl")]
    // ReSharper disable ClassNeverInstantiated.Global
    public class BranchAction : Action
    {
        /// <summary>
        /// Basic branch action - has two possible "next" actions, 
        /// which are chosen according to condition.
        /// </summary>
        /// <param name="v"></param>
        public BranchAction(Dictionary<String, Object> v)
            : base("Branch", v)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ActionArgument]
        public String ConditionSource = "True";

        /// <summary>
        /// 
        /// </summary>
        [ActionArgument(
            usage = "Next action if condition returns false",
            displayName = "Next Action if False"
        )]
        public Action NextIfFalse;

        private Boolean _result;

        /// <summary>
        /// 
        /// </summary>
        protected override void DoAction()
        {
            PythonEngine.PythonEngine pye = PythonEngine.PythonEngine.Instance;
            
            var scope = pye.GetNewScope(Variables);
            _result = pye.Evaluate<bool>(ConditionSource, scope);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Action GetNext()
        {
            return _result ? Next : NextIfFalse;
        }
    }
    // ReSharper restore ClassNeverInstantiated.Global
    
}

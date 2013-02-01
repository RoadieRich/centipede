using System;
using System.Collections.Generic;
using Centipede;
using Centipede.Properties;
using Action = Centipede.Action;


namespace PyAction
{
    /// <summary>
    /// 
    /// </summary>

    [ActionCategory("Flow Control", displayName = "Branch", displayControl = @"BranchDisplayControl", iconName = @"branch")]
    // ReSharper disable ClassNeverInstantiated.Global
    public class BranchAction : Action
    {
        /// <summary>
        /// Basic branch action - has two possible "next" actions, 
        /// which are chosen according to condition.
        /// </summary>
        /// <param name="v"></param>
        /// <remarks>
        /// Condition is a simple python expression, which is evalutated in a "safe" scope: 
        /// changes made to any (python) variables will be lost.
        /// </remarks>
        public BranchAction(Dictionary<String, Object> v)
            : base("Branch", v)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        [ActionArgument(displayName = @"Condition")]
        public String ConditionSource = @"False";

        /// <summary>
        /// 
        /// </summary>
        [ActionArgument(
            usage = @"Next action if condition returns true (otherwise, proceed normally)",
            displayName = @"Next Action if True"
        )]
        public Action NextIfTrue;

        private Boolean _result;

        /// <summary>
        /// 
        /// </summary>
        protected override void DoAction()
        {
            var pye = PythonEngine.PythonEngine.Instance;
            
            var scope = pye.GetNewScope(Variables);
            _result = pye.Evaluate<bool>(ConditionSource, scope);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Action GetNext()
        {
            return _result ? NextIfTrue : Next;
        }
    }

    public class BranchActionI18N
    {
        public String Base = @"Resources";
        public static Dictionary<String, String> NextIfTrue = new Dictionary<String, String>
                                                              {
                                                                      {
                                                                              @"Usage",
                                                                              @"BranchActionUsage"
                                                                      },
                                                                      {
                                                                              @"Display Name",
                                                                              @"BranchActionDisplayName"
                                                                      }
                                                              };

        public static Dictionary<String, String> Condition = new Dictionary<string, string>
                                                             {
                                                                     {
                                                                             @"Display Name",
                                                                             @"BranchActionI18N_Condition_DisplayName"
                                                                     }
                                                             };
    }


    // ReSharper restore ClassNeverInstantiated.Global
    
}

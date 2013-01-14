using System;
using System.Collections.Generic;
using System.Windows.Forms;


namespace Centipede.Actions
{
    /// <summary>
    /// 
    /// </summary>
// ReSharper disable ClassNeverInstantiated.Global
    public class BranchAction : Action
// ReSharper restore ClassNeverInstantiated.Global
    {

        /// <summary>
        /// Basic branch action - has two possible "next" actions, 
        /// which are chosen according to condition.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="variables"></param>
        /// <param name="condition"></param>
        public BranchAction(String name, Dictionary<String, Object> variables, BranchCondition condition)
            : base(name, variables)
        {
            Condition = condition;
        }

        /// <summary>
        /// 
        /// </summary>
        [ActionArgument]
        public BranchCondition Condition;

        /// <summary>
        /// 
        /// </summary>
        [ActionArgument(
            usage = "Next action if condition returns false",
            displayName = "Next Action if False"
        )]
// ReSharper disable UnassignedField.Global
// ReSharper disable MemberCanBePrivate.Global
        public Action NextIfFalse;
// ReSharper restore MemberCanBePrivate.Global
// ReSharper restore UnassignedField.Global

        private Boolean _result;

        /// <summary>
        /// 
        /// </summary>
        protected override void DoAction()
        {
            _result = Condition.Test(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Action GetNext()
        {
            if (_result)
            {
                return Next;
            }
            return NextIfFalse;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public abstract class BranchCondition
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="action"></param>
        /// <returns></returns>
        public abstract Boolean Test(Action action);

        public abstract Control[] MakeControls();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;


namespace Centipede
{
    /// <summary>
    /// 
    /// </summary>
    public class BranchAction : Action
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
        public Action NextIfFalse;

        private Boolean Result;

        /// <summary>
        /// 
        /// </summary>
        protected override void DoAction()
        {
            Result = Condition.Test(this);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override Action GetNext()
        {
            if (Result)
            {
                return Next;
            }
            else
            {
                return NextIfFalse;
            }
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
        protected BranchCondition()
        {

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="act"></param>
        /// <returns></returns>
        public abstract Boolean Test(Action act);

        public abstract Control[] MakeControls();
    }
}

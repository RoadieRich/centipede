using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Centipede
{
       
    public class BranchAction : Action
    {
        /// <summary>
        /// Basic branch action - has two possible "next" actions, 
        /// which are chosen according to condition.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="condition"></param>
        public BranchAction(String name, Dictionary<String,Object> variables, BranchCondition condition)
            : base(name, variables)
        {
            Condition = condition;
            Attributes = new Dictionary<string, object>();
        }

        [ActionArgument]
        public BranchCondition Condition;

        [ActionArgument(
            usage = "Next action if condition returns false", 
            displayName="Next Action if False"
        )]
        public Action NextIfFalse;

        private Boolean Result;
        public override void DoAction()
        {
            Result = Condition.Test(this);
        }

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



    public class BranchCondition
    {

        public BranchCondition()
        {
            
        }
        public virtual Boolean Test(Action act)
        {
            return true;
        }
    }

    
}

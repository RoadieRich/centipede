using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using IronPython.Runtime;


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
        public BranchAction(String name, BranchCondition condition)
            : base(name)
        {
            Condition = condition;
            Attributes = new Dictionary<string, object>();
        }


        public BranchCondition Condition;

        public Action NextIfFalse;

        private Boolean Result;
        public override void DoAction()
        {
            Result = Condition.Test();
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

    public class BranchActionFactory : ActionFactory
    {

        public override Action generate(string name)
        {
            return new BranchAction(name, null);
        }
    }


    public abstract class BranchCondition
    {
        protected BranchAction _branchAction;
        public BranchCondition(BranchAction action)
        {
            _branchAction = action;
            _branchAction.Attributes.Add("condition", this);
        }
        public abstract Boolean Test();
    }

    class PythonCondition : BranchCondition
    {
        
        public PythonCondition(BranchAction action, String source) : base(action)
        {
            
            Source = source;
            _branchAction.Attributes.Add("source", source);
        }

        public override Boolean Test()
        {
            PythonEngine engine = PythonEngine.GetInstance();
            return engine.Evaluate<Boolean>(Source);
        }

        public string Source;
    }
}

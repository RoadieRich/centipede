using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IronPython.Hosting;
using IronPython.Runtime;


namespace Centipede
{
    public class BranchActionFactory : ActionFactory
    {
        public BranchActionFactory() : base("Branch")
        { }
        public override Action Generate()
        {
            BranchAction branchAct = new BranchAction(Text);
            branchAct.Condition = new BranchCondition(branchAct);
            return branchAct;
        }
    }

    public class PythonBranchActionFactory : ActionFactory
    {
        public PythonBranchActionFactory() : base ("Python Branch")
        {
            Text="Python Branch";
        }
        public override Action Generate()
        {
            BranchAction branchAct = new BranchAction(Text);
            branchAct.Condition = new PythonCondition(branchAct);
            return branchAct;
        }
    }
    
    public class BranchAction : Action
    {
        /// <summary>
        /// Basic branch action - has two possible "next" actions, 
        /// which are chosen according to condition.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="condition"></param>
        public BranchAction(String name)
            : base(name)
        {
            Attributes = new Dictionary<string, object>();
        }


        public BranchCondition Condition;

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
        protected Action Action;

        public BranchCondition(Action act)
        {
            this.Action = act;
            
        }
        public virtual Boolean Test(Action act)
        {
            return true;
        }
    }

    class PythonCondition : BranchCondition
    {
        public PythonCondition(Action act) : base(act)
        {
            act.Attributes.Add("Source", "");
        }

        public override Boolean Test(Action act)
        {
            PythonEngine engine = PythonEngine.Instance;
            return engine.Evaluate<Boolean>(Source);
        }

        public string Source
        {
            get
            {
                return this.Action.Attributes["Source"] as String;
            }
            set
            {
                this.Action.Attributes["Source"] = value;
            }

        }

        public override String ToString()
        {
            return "";
        }
    }
}

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
        public override Action generate(string name)
        {
            return new BranchAction(name, new BranchCondition());
        }
    }

    public class PythonBranchActionFactory : ActionFactory
    {
        public PythonBranchActionFactory() : base ("Python Branch")
        {
            Text="Python Branch";
        }
        public override Action generate(string name)
        {
            return new BranchAction(name, new PythonCondition(""));
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

    class PythonCondition : BranchCondition
    {
        public PythonCondition(String source) : base()
        {
            Source = source;
        }

        public override Boolean Test(Action act)
        {
            PythonEngine engine = PythonEngine.GetInstance();
            return engine.Evaluate<Boolean>(Source);
        }

        public string Source;

        public String ToString()
        {
            return "";
        }
    }
}

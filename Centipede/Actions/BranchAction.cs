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
            return new BranchAction(Text, Program.Variables, new BranchCondition());
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
            return new BranchAction(Text, Program.Variables, new PythonCondition(""));
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

    class PythonCondition : BranchCondition
    {
        public PythonCondition(String source) : base()
        {
            Source = source;
        }

        public override Boolean Test(Action act)
        {
            return PythonEngine.Instance.Evaluate<Boolean>(Source);
        }

        public string Source;

        public override String ToString()
        {
            return "(Python condition)";
        }
    }
}

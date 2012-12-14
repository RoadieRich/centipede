using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centipede;
using Centipede.PyEngine;
using System.Resources;

namespace Centipede.PyAction
{

    [ActionCategory("Other Actions", iconName="pycon", displayName="Python Action")]
    public class PythonAction : Centipede.Action
    {

        public PythonAction(Dictionary<String, Object> variables)
            : base("Python Action", variables)
        { }

        [ActionArgument(usage = "Source code to be executed", setterMethodName="UpdateSource")]
        public String Source = "";
        //{
        //    get
        //    {
        //        return (String)Attributes["source"];
        //    }
        //    set
        //    {
        //        Attributes["source"] = value;
        //    }
        //}

        public Boolean UpdateSource(String newsource)
        {
            Source = newsource;
            this.Complexity = newsource.Split(System.Environment.NewLine.ToCharArray()).Length;
            return true;
        }
        protected override void DoAction()
        {
            PythonEngine engine = PythonEngine.Instance;
            if (!engine.VariableExists("variables"))
            {
                engine.SetVariable("variables", Variables);
            }
            try
            {
                engine.Execute(ParseStringForVariable(Source));
            }
            catch (PythonException e)
            {
                throw new ActionException(e, this);
            }
        }
    }

    class PythonCondition : BranchCondition
    {
        public PythonCondition(String source)
            : base()
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centipede;
using Centipede.PyEngine;

namespace Centipede.PyAction
{
    [Centipede.ActionCategory("Other Actions")]
    public class PythonAction : Centipede.Action
    {

        public PythonAction(Dictionary<String, Object> variables)
            : base("Python Action", variables)
        { }

        [ActionArgument(usage = "Source code to be executed")]
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
        public override void DoAction()
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
}

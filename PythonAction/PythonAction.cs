using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Centipede;
using Centipede.PyEngine;
using System.Resources;

namespace Centipede.PyAction
{

    [ActionCategory("Other Actions", 
        iconName="pycon", 
        displayName="Python Action", 
        displayControl="PythonDisplayControl"
        )]
    public class PythonAction : Centipede.Action
    {

        public PythonAction(Dictionary<String, Object> variables)
            : base("Python Action", variables)
        { }

        [ActionArgument(usage = "Source code to be executed", onTextChangedHandlerName = "UpdateSource")]
        public String Source
        {
            get
            {
                return this._source;
            }
            set
            {
                this._source = value;
                _complexity = value.Split(System.Environment.NewLine.ToCharArray()).Length;
            }
        }
        private int _complexity;
        private  string _source;

        public void UpdateSource(object sender, EventArgs e)
        {
            ScintillaNET.Scintilla control = sender as ScintillaNET.Scintilla;
            Source = control.Text;
            this._complexity = Source.Split(System.Environment.NewLine.ToCharArray()).Length;
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
        public override int Complexity
        {
            get
            {
                return _complexity;
            }
        }

    }

    //class PythonCondition : BranchCondition
    //{
    //    public PythonCondition(String source)
    //        : base()
    //    {
    //        Source = source;
    //    }

    //    public override Boolean Test(Action act)
    //    {
    //        return PythonEngine.Instance.Evaluate<Boolean>(Source);
    //    }

    //    public string Source;

    //    public override String ToString()
    //    {
    //        return "(Python condition)";
    //    }
    //}
}

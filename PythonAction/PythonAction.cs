using System;
using System.Collections.Generic;
using Centipede;
using PythonEngine;


namespace PyAction
{

    [ActionCategory("Other Actions", 
        iconName="pycon", 
        displayName="Python Action", 
        displayControl="PythonDisplayControl"
        )]
// ReSharper disable ClassNeverInstantiated.Global
    public class PythonAction : Centipede.Action
// ReSharper restore ClassNeverInstantiated.Global
    {

        public PythonAction(Dictionary<String, Object> variables)
            : base("Python Action", variables)
        { }

        [ActionArgument(usage = "Source code to be executed")]
        public String Source
        {
// ReSharper disable MemberCanBePrivate.Global
            get
// ReSharper restore MemberCanBePrivate.Global
            {
                return _source;
            }
            set
            {
                _source = value;
                _complexity = value.Split(Environment.NewLine.ToCharArray()).Length;
            }
        }
        private int _complexity;
        private string _source;

/*
        public void UpdateSource(object sender, EventArgs e)
        {
            ScintillaNET.Scintilla control = sender as ScintillaNET.Scintilla;
            Source = control.Text;
            this._complexity = Source.Split(System.Environment.NewLine.ToCharArray()).Length;
        }
*/
        protected override void DoAction()
        {
            PythonEngine.PythonEngine engine = PythonEngine.PythonEngine.Instance;
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

using System;
using System.Collections.Generic;
using System.Drawing;

namespace Centipede
{
    class PythonActionFactory : ActionFactory
    {
        public PythonActionFactory() : base()
        {
            Text = "Python Action";
            //this.ImageList.Images.Add(new Icon("Resources/pycon.ico"));
        }
        public override Action generate(String name)
        {
            return new PythonAction(name);
        }
    }
    
    class PythonAction : Action
    {

        public PythonAction(String name, String source="")
            : base(name)
        {
            Attributes = new Dictionary<string,object>();
            Attributes.Add("source", source);

        }

        public String Source
        {
            get
            {
                return (String)Attributes["source"];
            }
            set
            {
                Attributes["source"] = value;
            }
        }
        public override void DoAction()
        {
            PythonEngine engine = PythonEngine.GetInstance();
            try
            {
                engine.Execute((String)Attributes["source"]);
            }
            catch (PythonException e)
            {
                throw new ActionException(e, this);
            }
        }
    }
}

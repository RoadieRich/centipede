﻿using System;
using System.Collections.Generic;
using System.Drawing;


namespace Centipede
{
    class PythonActionFactory : ActionFactory
    {
        public PythonActionFactory() : base("Python Action")
        { }

        public override Action Generate()
        {
            return new PythonAction(Text);
        }
    }
    
    class PythonAction : Action
    {

        public PythonAction(String source="")
            : base("Python Action")
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
            PythonEngine engine = PythonEngine.Instance;
            try
            {
                engine.Execute(Attributes["source"] as String);
            }
            catch (PythonException e)
            {
                throw new ActionException(e, this);
            }
        }
    }
}

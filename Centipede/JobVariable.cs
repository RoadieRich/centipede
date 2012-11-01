using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Centipede
{
    public class JobVariable
    {
        public readonly JobVariable.Types Type;
        public Object Value;

        public JobVariable(JobVariable.Types type)
        {
            this.Type = type;
        }

        public JobVariable(JobVariable.Types type, Object value)
        {
            this.Type = type;
            this.Value = value;
        }


        public enum Types : byte
        {
            Integer,
            Float,
            String,
            Other
        }
    }

    namespace broken.old.stuff
    {
        public class JobVarBase
        { }
        
        public class JobVariable<T> : JobVarBase
        {
            public readonly String Name;
            public T Value;
            public readonly String TypeName;
            public String Unit = "";
            public String[] Units;


            protected JobVariable(String name, String typename)
            {
                Name = name;
                TypeName = typename;
            }

            public JobVariable(String name, T value)
            {
                Name = name;
                Value = value;
                TypeName = typeof(T).ToString();
            }

            public JobVariable(String name, String typename, T value, String unit)
            {
                Name = name;
                TypeName = typename;
                Value = value;
                Unit = unit;
            }

            public string getValueAsString()
            {
                return Value.ToString();
            }
            public T getValue()
            {
                return Value;
            }
        }
    }
}

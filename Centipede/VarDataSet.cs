using System.Linq;
using System.Collections.Generic;
    using System;
using System.Data;

namespace Centipede {
    using Variable = Program.Variable;
    
    public partial class VarDataSet {
        partial class VariablesDataTable
        {
            
            private Dictionary<String, Object> VarDict = Program.Variables;

            internal void Update()
            {
                foreach (Variable v in VarDict)
                {
                    if (!(this.Where((r) => (r.Name == v.Name)).Any()))
                    {
                        AddVariablesRow(v.Name, v.Value);
                    }
                    else
                    {
                        this.FindByName(v.Name).Value = v.Value;
                    }
                }
                foreach (VariablesRow r in this)
                {
                    Program.Variables[r.Name] = r.Value;
                }
            }
        }
    }
}

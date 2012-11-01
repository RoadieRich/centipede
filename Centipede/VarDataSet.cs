using System.Linq;
using System.Collections.Generic;
    using System;
using System.Data;

namespace Centipede {
    using Variable = Program.Variable;
    
    public partial class VarDataSet {
        partial class VariablesDataTable
        {
            
            private Dictionary<String, Variable> VarDict = Program.Variables;

            internal void Update()
            {
                //this.Clear();
                //foreach (KeyValuePair<String, Variable> kvp in VarDict)
                //{
                //    Variable v = kvp.Value;
                //    if (v.Name == null)
                //    {
                //        v.Name = kvp.Key;
                //    }
                //    if (v.Index != null)
                //    {
                //        if (v.Index >= 0)
                //        {
                //            VariablesRow newRow = (VariablesRow)NewRow();
                //            newRow.Value = v.Value;
                //            v.Index = newRow.index;
                //            newRow.Type = (Byte)JobVariable.Types.Other;

                //            AddVariablesRow(newRow);
                //        }
                //    }
                //    else
                //    {
                //        VariablesRow row = this.FindByindex(v.Index);
                //        row.Value =
                //    }
                //}
                Variable console = VarDict["console"];
                VarDict.Clear();
                foreach (VariablesRow r in this)
                {
                    VarDict[r.Name] = new Variable();
                    VarDict[r.Name].Value = r.Value;
                   // VarDict[r.Name].Index = r.index;
                    VarDict[r.Name].Name = r.Name;
                }
                VarDict["console"] = console;
            }
        }
    }
}

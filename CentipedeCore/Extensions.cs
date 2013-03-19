using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Centipede;
using Microsoft.Scripting.Hosting;
using Microsoft.Scripting.Runtime;


namespace CentipedeInterfaces
{
    public static class Extensions
    {
        public static Object Get(this IEnumerable<DataSet1.VariablesTableRow> table, String key)
        {
            return (from r in table
                    where r.Name == key
                    select r.Value).First();
        }

        public static void Set(this DataSet1.VariablesTableDataTable table, String key, Object value)
        {

            try
            {
                DataSet1.VariablesTableRow row = (from r in table
                                                  where r.Name == key
                                                  select r).First();
                row.Value = value;
            }
            catch (InvalidOperationException)
            {
                table.AddVariablesTableRow(key, value);
            }
        }

        public static String AsText(this MessageLevel e)
        {
            return DisplayTextAttribute.ToDisplayString(e);
        }
    }

}

using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Runtime.InteropServices;


namespace Centipede
{
    

    [System.ComponentModel.DesignerCategory("")]
    public abstract class ActionFactory : ListViewItem
    {
        public ActionFactory(String name)
            : base(name)
        { }

        /// <summary>
        /// Generate a new action of the given type
        /// </summary>
        /// <returns>The new action, with default attributes set</returns>
        public abstract Action Generate();

        /// <summary>
        /// A user-definable comment, e.g. to document the purpose of the action.
        /// </summary>
        public String Comment = "";

    }
    
    

}

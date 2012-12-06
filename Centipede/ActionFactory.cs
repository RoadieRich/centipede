using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Centipede
{
    class ActionFactory : ListViewItem
    {
        internal ActionFactory(String displayName, Type actionType) : base(displayName)
        {
            this.ActionType = actionType;
        }

        public ActionFactory(ActionCategoryAttribute catAttribute, Type pluginType)
        {
            String displayName = catAttribute.displayName != "" ? catAttribute.displayName : pluginType.Name;
            this.Text = displayName;
            this.ToolTipText = catAttribute.helpText;
            this.ActionType = pluginType;
        }

        private readonly Type ActionType;
        

        public Action Generate()
        {

            Type[] typeArray = new Type[1];
            typeArray[0] = typeof(Dictionary<String, Object>);
        
            return ActionType.GetConstructor(new Type[] {typeof(Dictionary<String, Object>)}).Invoke(new object[1]{Program.Variables}) as Action;
        }
    }
}

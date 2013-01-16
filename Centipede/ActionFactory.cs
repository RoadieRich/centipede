using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Centipede
{
    class ActionFactory : ListViewItem
    {
        internal ActionFactory(String displayName, Type actionType) : base(displayName)
        {
            _actionType = actionType;
            ImageKey = @"Generic";
        }

        public ActionFactory(ActionCategoryAttribute catAttribute, Type pluginType)
        {
            String displayName = catAttribute.displayName != "" ? catAttribute.displayName : pluginType.Name;
            Text = displayName;
            ToolTipText = catAttribute.helpText;
            _actionType = pluginType;
            ImageKey = @"Generic";
        }

        private readonly Type _actionType;

        public Action Generate()
        {

            var typeArray = new Type[1];
            typeArray[0] = typeof(Dictionary<String, Object>);
        
            return _actionType.GetConstructor(new[] {typeof(Dictionary<String, Object>)}).Invoke(new object[]{Program.Variables}) as Action;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Reflection;
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
            string displayName = String.IsNullOrEmpty(catAttribute.displayName) ? catAttribute.displayName : pluginType.Name;
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

            ConstructorInfo constructorInfo = _actionType.GetConstructor(new[] { typeof (Dictionary<String, Object>) });
            if (constructorInfo != null)
            {
                return constructorInfo.Invoke(new object[]{Program.Instance.Variables}) as Action;
            }
            return null;
        }
    }
}

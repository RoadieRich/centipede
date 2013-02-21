using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;

namespace Centipede
{
    class ActionFactory : ListViewItem
    {
        [ResharperAnnotations.UsedImplicitly]
        public static volatile Action.MessageHandlerDelegate MessageHandlerDelegate;

        internal ActionFactory(String displayName, Type actionType) : base(displayName)
        {
            _actionType = actionType;
            ImageKey = @"Generic";
        }

        public ActionFactory(ActionCategoryAttribute catAttribute, Type pluginType)
        {
            string displayName = !String.IsNullOrEmpty(catAttribute.displayName) ? catAttribute.displayName : pluginType.Name;
            Text = displayName;
            ToolTipText = catAttribute.helpText;
            _actionType = pluginType;
            ImageKey = @"Generic";
        }

        private readonly Type _actionType;

        public Action Generate()
        {

            var ctorTypes = new Type[1];
            ctorTypes[0] = typeof (IDictionary<String, Object>);

            ConstructorInfo constructorInfo = _actionType.GetConstructor(ctorTypes);
            Action instance = (Action)constructorInfo.Invoke(new object[] { MainWindow.Instance.Core.Variables });
            
            if (MessageHandlerDelegate != null)
            {
                instance.MessageHandler += MessageHandlerDelegate;
            }
            
            return instance;
        }

    }
}

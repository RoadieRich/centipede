using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows.Forms;
using CentipedeInterfaces;
using ResharperAnnotations;


namespace Centipede
{
    class ActionFactory : ListViewItem
    {
        [ResharperAnnotations.UsedImplicitly]
        public static volatile MessageEvent MessageEvent;

        [UsedImplicitly]
        internal ActionFactory(String displayName, Type actionType, ICentipedeCore core) : base(displayName)
        {
            _actionType = actionType;
            _core = core;
            ImageKey = @"Generic";
        }

        public ActionFactory(ActionCategoryAttribute catAttribute, Type pluginType, ICentipedeCore core)
        {
            string displayName = !String.IsNullOrEmpty(catAttribute.displayName) ? catAttribute.displayName : pluginType.Name;
            Text = displayName;
            ToolTipText = catAttribute.helpText;
            _actionType = pluginType;
            _core = core;
            ImageKey = @"Generic";
        }

        private readonly Type _actionType;
        private readonly ICentipedeCore _core;

        public Action Generate()
        {

            var ctorTypes = new[] { typeof (IDictionary<String, Object>), typeof (ICentipedeCore) };

            ConstructorInfo constructorInfo = _actionType.GetConstructor(ctorTypes);
            Action instance = (Action)constructorInfo.Invoke(new object[] { _core.Variables, _core });
            
            if (MessageEvent != null)
            {
                instance.MessageHandler += MessageEvent;
            }
            
            return instance;
        }

    }
}

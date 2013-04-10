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
        [UsedImplicitly]
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
            string displayName = !String.IsNullOrEmpty(catAttribute.DisplayName) ? catAttribute.DisplayName : pluginType.Name;
            Text = displayName;
            ToolTipText = catAttribute.Usage;
            _actionType = pluginType;
            _core = core;
            ImageKey = @"Generic";
        }

        private readonly Type _actionType;
        private readonly ICentipedeCore _core;

        public IAction Generate()
        {

            var ctorTypes1 = new[] { typeof (IDictionary<String, Object>), typeof (ICentipedeCore) };
            var ctorTypes2 = new[] { typeof(Dictionary<String, Object>), typeof(ICentipedeCore) };
            IAction instance = null;
            
            ConstructorInfo constructorInfo = this._actionType.GetConstructor(ctorTypes1) ??
                                              this._actionType.GetConstructor(ctorTypes2);
            if (constructorInfo != null)
            {
                instance = (IAction)constructorInfo.Invoke(new object[] { this._core.Variables, this._core });

                if (MessageEvent != null)
                {
                    instance.MessageHandler += MessageEvent;
                }
            }
            
            return instance;
        }

    }
}

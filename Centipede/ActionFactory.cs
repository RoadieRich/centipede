using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using CentipedeInterfaces;
using ResharperAnnotations;


namespace Centipede
{
    internal class ActionFactory : ListViewItem
    {
        [UsedImplicitly]
        public static volatile MessageEvent MessageHandler;

        [UsedImplicitly]
        internal ActionFactory(String displayName, Type actionType, ICentipedeCore core)
            : base(displayName)
        {
            _actionType = actionType;
            _core = core;
            ImageKey = @"Generic";
        }

        public ActionFactory(ActionCategoryAttribute catAttribute, Type pluginType, ICentipedeCore core)
        {
            string displayName = !String.IsNullOrEmpty(catAttribute.DisplayName)
                                     ? catAttribute.DisplayName
                                     : pluginType.Name;

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
            Type[][] ctorTypeSigs = new[]
                               {
                                        new[] {typeof (ICentipedeCore)},
                                        new[]
                                        {
                                            typeof (IDictionary<String, Object>),
                                            typeof (ICentipedeCore)
                                        },
                                        new[]
                                        {
                                            typeof (Dictionary<String, Object>),
                                            typeof (ICentipedeCore)
                                        },
                                        new[]
                                        {
                                            typeof (string), typeof (IDictionary<String, Object>),
                                            typeof (ICentipedeCore)
                                        },
                                        new[]
                                        {
                                            typeof (string), typeof (Dictionary<String, Object>),
                                            typeof (ICentipedeCore)
                                        }
                                    };

            var ctors = from types in ctorTypeSigs
                        let ctor = this._actionType.GetConstructor(types)
                        where ctor != null
                        select new
                               {
                                   Ctor = ctor,
                                   ArgCount = types.Length
                               };

            var constructorInfo = ctors.FirstOrDefault();
            if (constructorInfo == null)
            {
                throw new ActionException("Invalid Type");
            }

            object[] parameters;


            switch (constructorInfo.ArgCount)
            {
            case 1:
                parameters = new object[] {this._core};
                break;
            case 2:
                parameters = new object[] {this._core.Variables, this._core};
                break;
            default:
                parameters = new object[] {"", this._core.Variables, this._core};
                MessageHandler(this,
                               new MessageEventArgs
                               {
                                   Level = MessageLevel.Debug,
                                   Message =
                                       "Constructor Type signature contains \"string name\" parameter. Please contact plugin developer"
                               });
                break;
            }
            return (IAction)constructorInfo.Ctor.Invoke(parameters);
        }
    }

    //internal static class Exts
    //{
    //    public static T Invoke<T>(this ConstructorInfo constructorInfo, params object[] parameters)
    //    {
    //        return (T)constructorInfo.Invoke(parameters);
    //    }
    //}

}

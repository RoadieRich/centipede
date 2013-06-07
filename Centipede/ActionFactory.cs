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
        public static volatile MessageEvent MessageEvent;

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

#pragma warning disable 612,618 //displayName is obsolete but still supported until we completely eliminate it

                                     : !String.IsNullOrEmpty(catAttribute.displayName)
                                           ? catAttribute.displayName
                                           : pluginType.Name;
#pragma warning restore 612,618

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
                                        new[] { typeof(IDictionary<String, Object>), typeof(ICentipedeCore) },
                                        new[] { typeof(Dictionary<String, Object>), typeof(ICentipedeCore) },
                                        new[]
                                        { typeof(string), typeof(IDictionary<String, Object>), typeof(ICentipedeCore) },
                                        new[]
                                        { typeof(string), typeof(Dictionary<String, Object>), typeof(ICentipedeCore) }
                                    };

            ConstructorInfo constructorInfo =
                ctorTypeSigs.Select(types => this._actionType.GetConstructor(types)).FirstOrDefault();
            if (constructorInfo != null)
            {
                IAction instance;
                try
                {
                    instance = (IAction)constructorInfo.Invoke(new object[] { this._core.Variables, this._core });
                }
                catch (TargetParameterCountException)
                {
                    instance = (IAction)constructorInfo.Invoke(new object[] { "", this._core.Variables, this._core });
                }
                return instance;
            }
            throw new ActionException("Invalid Type");
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

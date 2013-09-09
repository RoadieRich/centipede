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
            var ctorParams = new List<object[]>
                             {
                                 new object[] {this._core},
                                 new object[]
                                 {
                                     this._core.Variables,
                                     this._core
                                 },
                                 new object[]
                                 {
                                     this._actionType.Name,
                                     this._core
                                 },
                                 new object[]
                                 {
                                     this._actionType.Name,
                                     this._core.Variables,
                                     this._core
                                 }
                             };

            ActionCtorWithArgs actionCtor = ctorParams.Select(this.Selector).FirstOrDefault(c => c.Valid);

            if (actionCtor == null)
            {
                throw new ActionException("Invalid Type");
            }

            if (actionCtor.HasOldSig)
            {
                MessageHandler(this,
                               new MessageEventArgs
                               {
                                   Level = MessageLevel.Debug,
                                   Message =
                                       "Constructor Type signature contains obsolete \"string name\" parameter. " +
                                       "Please contact plugin developer."
                               });
            }
            return actionCtor.Invoke();
        }

        private ActionCtorWithArgs Selector(object[] args)
        {
            var argTypes = args.Select(o => o.GetType()).ToArray();
            return new ActionCtorWithArgs(this._actionType.GetConstructor(argTypes), args);
        }

        internal class ActionCtorWithArgs
        {
            public ActionCtorWithArgs(ConstructorInfo ctor, object[] args)
            {
                this._ctor = ctor;
                this._args = args;
            }

            private readonly ConstructorInfo _ctor;
            private readonly object[] _args;

            public bool HasOldSig
            {
                get { return this._args.OfType<String>().Any(); }
            }

            public bool Valid
            {
                get { return this._ctor != null; }
            }

            public IAction Invoke()
            {
                return (IAction)this._ctor.Invoke(this._args);
            }
        }
    }
}

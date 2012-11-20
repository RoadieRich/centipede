using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Centipede
{
    class ActionFactory : ListViewItem
    {
        internal ActionFactory(String name, Type actionType) : base(name)
        {
            this.ActionType = actionType;
        }

        private readonly Type ActionType;

        public Action Generate()
        {
            return ActionType.GetConstructor(new Type[0]).Invoke(null) as Action;
        }
    }
}

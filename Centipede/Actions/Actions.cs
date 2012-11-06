using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Centipede
{
    public abstract class ActionFactory : ListViewItem
    {
        public ActionFactory(String name)
            : base(name)
        { }


        public abstract Action Generate(String name);

        public override String ToString()
        {
            return this.Text;
        }
    }

    
    /// <summary>
    /// Base Action class: all actions will subclass this
    /// </summary>
    public abstract class Action
    {
        public Action(String name, Object tag = null)
        {
            Name = name;
            Tag = tag;
        }

        public Dictionary<String, Object> Attributes = new Dictionary<string,object>();

        public readonly String Name;
        public String Comment = "";
        public Action Next = null;
        public Object Tag;

        public abstract void DoAction();


        public virtual Action GetNext()
        {
            return Next;
        }
    }



}

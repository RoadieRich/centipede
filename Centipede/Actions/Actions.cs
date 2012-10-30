using System;
using System.Collections.Generic;

namespace Centipede
{
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

        public Dictionary<String, Object> Attributes;

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

    public abstract class ActionFactory
    {
        public abstract Action generate(String name);
    }
}

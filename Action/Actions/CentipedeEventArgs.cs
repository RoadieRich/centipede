using System;
using System.Collections.Generic;


namespace Centipede.Actions
{
    /// <summary>
    /// 
    /// </summary>
// ReSharper disable ClassNeverInstantiated.Global
    public class CentipedeEventArgs : EventArgs
// ReSharper restore ClassNeverInstantiated.Global
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="program"></param>
        /// <param name="actions"></param>
        /// <param name="variables"></param>
        public CentipedeEventArgs(Type program, List<Action> actions, Dictionary<String, Object> variables)
        {
            Program = program;
            Actions = actions;
            Variables = variables;
        }

        /// <summary>
        /// 
        /// </summary>
// ReSharper disable MemberCanBePrivate.Global
        public readonly Type Program;

        /// <summary>
        /// 
        /// </summary>
        public readonly List<Action> Actions;

        /// <summary>
        /// 
        /// </summary>
        public readonly Dictionary<string, object> Variables;

// ReSharper restore MemberCanBePrivate.Global
    }
}
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
        /// <param name="actions"></param>
        /// <param name="variables"></param>
        public CentipedeEventArgs(List<Action> actions, Dictionary<String, Object> variables)
        {
            _actions = actions;
            _variables = variables;
        }

        
        /// <summary>
        /// 
        /// </summary>
        private readonly List<Action> _actions;

        /// <summary>
        /// 
        /// </summary>
        private readonly Dictionary<string, object> _variables;

// ReSharper restore MemberCanBePrivate.Global
        /// <summary>
        /// 
        /// </summary>
        public Dictionary<string, object> Variables
        {
            get
            {
                return _variables;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public List<Action> Actions
        {
            get
            {
                return _actions;
            }
        }
    }
}
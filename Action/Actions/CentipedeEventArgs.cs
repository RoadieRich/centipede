using System;
using System.Collections.Generic;
using ResharperAnnotations;


namespace Centipede.Actions
{
    /// <summary>
    /// 
    /// </summary>
    public class CentipedeEventArgs : EventArgs
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

        /// <summary>
        /// 
        /// </summary>
        [PublicAPI]
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
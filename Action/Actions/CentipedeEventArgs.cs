using System;
using System.Collections.Generic;
using ResharperAnnotations;


namespace Centipede.Actions
{
    /// <summary>
    /// The base Event arguments, containing a list of actions and variables
    /// </summary>
    public class CentipedeEventArgs : EventArgs
    {
        /// <summary>
        /// crteate a bnerw <see cref="CentipedeEventArgs"/>
        /// </summary>
        /// <param name="actions">Actions from the job</param>
        /// <param name="variables">variables from the job</param>
        public CentipedeEventArgs(List<Action> actions, IDictionary<String, Object> variables)
        {
            _actions = actions;
            _variables = variables;
        }

        private readonly List<Action> _actions;

        private readonly IDictionary<string, object> _variables;

        /// <summary>
        /// get the variables from the jiob
        /// </summary>
        [PublicAPI]
        public IDictionary<string, object> Variables
        {
            get
            {
                return _variables;
            }
        }

        /// <summary>
        /// get the actions from the job
        /// </summary>
        [UsedImplicitly]
        public List<Action> Actions
        {
            get
            {
                return _actions;
            }
        }

        /// <summary>
        /// em[ty event args
        /// </summary>
        public new static readonly CentipedeEventArgs Empty = new CentipedeEventArgs(null, null);
    }
}
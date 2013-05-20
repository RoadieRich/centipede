using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;

namespace PythonEngine
{
    public interface IPythonScope : IDictionary, IDictionary<string, object>, IBindingList
    {
        /// <summary>
        /// Determines if this context or any outer scope contains the defined name.
        /// </summary>
        /// <exception cref="T:System.ArgumentNullException">name is a null reference. </exception>
        /// <param name="name">The variable name to look for</param>
        /// <returns><see cref="bool"/> indicating whether the variable exists</returns>
        bool ContainsVariable(string name);

        /// <summary>
        /// Gets an array of variable names and their values stored in the scope.
        /// </summary>
        /// <returns><see cref="KeyValuePair{TKey,TValue}">KeyValuePairs</see> of variable names and values. 
        /// </returns>
        IEnumerable<KeyValuePair<string, object>> GetItems();

        /// <summary>
        /// Gets a value stored in the scope under the given name.
        /// </summary>
        /// <typeparam name="T">blah blah blah</typeparam>
        /// <exception cref="System.MissingMemberException">The specified name is not defined in the scope.</exception>
        /// <exception cref="System.ArgumentNullException">name is a null reference</exception>   
        T GetVariable<T>(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        dynamic GetVariable(string name);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        bool RemoveVariable(string name);

        /// <summary>
        /// Sets the name to the specified value.
        /// </summary>
        /// <exception cref="ArgumentNullException">name is a null reference.</exception>
        /// <param name="name"></param>
        /// <param name="value"></param>
        void SetVariable(string name, object value);

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetVariable<T>(string name, out T value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        bool TryGetVariable(string name, out dynamic value);

        /// <summary>
        /// 
        /// </summary>
        event PythonVariableChangedEventHandler PropertyChanged;
    }

    /// <summary>Raised when a variable is changed within the <see cref="PythonScope"/>.</summary>
    /// <param name="sender" />
    /// <param name="args" />
    public delegate void PythonVariableChangedEventHandler(object sender, PythonVariableChangedEventArgs args);

    /// <summary />
    public class PythonVariableChangedEventArgs : PropertyChangedEventArgs
    {
        private readonly PythonVariableChangedAction _action;

        /// <summary />
        public PythonVariableChangedAction Action
        {
            get
            {
                return this._action;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.ComponentModel.PropertyChangedEventArgs"/> class.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed. </param>
        /// <param name="action">What happened</param>
        public PythonVariableChangedEventArgs(string propertyName, PythonVariableChangedAction action)
            : base(propertyName)
        {
            _action = action;
        }
    }

    /// <summary />
    public enum PythonVariableChangedAction
    {
        /// <summary />
        Change,

        /// <summary />
        Add,

        /// <summary />
        Delete
    }

}
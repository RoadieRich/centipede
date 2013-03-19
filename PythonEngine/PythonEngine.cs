using System;
using System.Collections.Generic;
using IronPython.Hosting;
using IronPython.Runtime;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

[assembly: CLSCompliant(true)]
namespace PythonEngine
{
    /// <summary>
    ///     The Iron Python Engine.  Use the <code>variables</code> dictionary to access Job variables.
    /// </summary>
    //[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public class PythonEngine
    {
        private readonly ScriptEngine _pyEngine;
        private readonly PythonScope _pyScope;

        private PythonEngine()
        {
            //if (_pyEngine == null)
            {
                _pyEngine = Python.CreateEngine();
                _pyScope = new PythonScope(this._pyEngine.CreateScope());
                _pyScope.Scope.ImportModule(@"sys");
            }
        }

        /// <summary>
        ///     Execute python code
        /// </summary>
        /// <param name="code">The code to execute</param>
        /// <param name="scope">(optional)</param>
        /// <exception cref="PythonException"></exception>
        public void Execute(String code, PythonScope scope = null)
        {
            ScriptScope myscope = scope == null ? _pyScope : scope.Scope;

            ScriptSource source = _pyEngine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
            try
            {
                CompiledCode compiled = source.Compile();
                compiled.Execute(myscope);
            }
            catch (Exception e)
            {
                throw new PythonException(e);
            }
        }

        /// <summary>
        ///     Evaluate an expression, and return the result
        /// </summary>
        /// <typeparam name="T">The (C#) Type to coerce the value of the expression to</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <param name="scope">(Optional) the scope to evaluate the action in</param>
        /// <exception cref="PythonException"></exception>
        /// <returns>The result of the expression, coerced to type T</returns>
        
        public T Evaluate<T>(String expression, PythonScope scope = null)
        {
            ScriptScope myscope = scope == null ? _pyScope : scope.Scope;
            try
            {
                ScriptSource source = _pyEngine.CreateScriptSourceFromString(expression, SourceCodeKind.Expression);
                CompiledCode compiled = source.Compile();
                return compiled.Execute<T>(myscope);
            }
            catch (Exception e)
            {
                throw new PythonException(e);
            }
        }

        /// <summary>
        ///     Set a python variable, inside the Engine.
        /// </summary>
        /// <param name="name">Name of the variable to set</param>
        /// <param name="value">Value to set it to</param>
        public void SetVariable(String name, Object value)
        {
            _pyScope.SetVariable(name, value);
        }

        /// <summary>
        ///     Get the value of a variable inside the python engine.
        /// </summary>
        /// <param name="name">Variable name to get</param>
        /// <returns>The variable's value.  Will need casting to the correct type.</returns>
        /// <remarks>Avoids extra boxing/unboxing if the value is to be stored as an object reference.</remarks>
        public dynamic GetVariable(String name)
        {
            return _pyScope.GetVariable(name);
        }

        /// <summary>
        ///     Get a python variable, with a known type
        /// </summary>
        /// <typeparam name="T">The (c#) type to get the variable as</typeparam>
        /// <param name="name">Name of the variable to fetch</param>
        /// <returns>The value of the variable, cast to the appropriate C# type</returns>
        public T GetVariable<T>(String name)
        {
            return _pyScope.GetVariable<T>(name);
        }

        /// <summary>
        /// Currently unsupported
        /// </summary>
        /// <param name="code"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public CompiledCode Compile(String code, SourceCodeKind kind)
        {
            return _pyEngine.CreateScriptSourceFromString(code, kind).Compile();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Boolean VariableExists(String name)
        {
            return _pyScope.ContainsVariable(name);
        }

        

        /// <summary>
        ///     unused
        /// </summary>
        /// <param name="pyEngine"></param>
        /// <returns></returns>
        [Obsolete]
        public static implicit operator ScriptScope(PythonEngine pyEngine)
        {
            return pyEngine._pyScope;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        public PythonScope GetNewScope(IDictionary<String, Object> variables = null)
        {
            return variables != null && variables.Count < 1
                           ? new PythonScope(this._pyScope)
                           : new PythonScope(this._pyEngine.CreateScope(variables));
        }

        public PythonScope GetNewTypedScope(IDictionary<String, Object> variables = null)
        {
            if (variables == null)
            {
                return new PythonScope(this._pyScope);
            }
            ScriptScope scope = this._pyEngine.CreateScope();
            foreach (KeyValuePair<string, object> variable in variables)
            {
                scope.SetVariable(variable.Key, Convert.ChangeType(variable.Value, variable.Value.GetType()));
            }
            return new PythonScope(scope);
        }

        #region Singleton handling code

        private static volatile PythonEngine _instance;
        private static readonly Object SyncRoot = new Object();

        /// <summary>
        ///     The PyEngine Singleton.  <seealso href="http://msdn.microsoft.com/en-us/library/ff650316.aspx" />
        /// </summary>
        public static PythonEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (SyncRoot)
                    {
                        if (_instance == null)
                        {
                            _instance = new PythonEngine();
                        }
                    }
                }
                return _instance;
            }
        }

        #endregion

        public PythonScope GetScope()
        {
            return this._pyScope;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class PythonException : Exception
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public PythonException(Exception e)
                : base(String.Format(@"{0}: {1}", e.GetType().Name, e.Message), e)
        { }
    }
}

using System;
using System.Collections.Generic;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;


namespace PythonEngine
{
    /// <summary>
    ///     The Iron Python Engine.  Use the <code>variables</code> dictionary to access Job variables.
    /// </summary>
    public class PythonEngine
    {
        private readonly ScriptEngine _pyEngine;
        private readonly ScriptScope _pyScope;

        private PythonEngine()
        {
            if (_pyEngine == null)
            {
                _pyEngine = Python.CreateEngine();
                _pyScope = _pyEngine.CreateScope();
                _pyScope.ImportModule("sys");
            }
        }

        /// <summary>
        ///     Execute python code
        /// </summary>
        /// <param name="code">The code to execute</param>
        /// <exception cref="PythonException"></exception>
        public void Execute(String code)
        {
            ScriptSource source = _pyEngine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
            try
            {
                CompiledCode compiled = source.Compile();
                compiled.Execute(_pyScope);
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
        // ReSharper disable UnusedMember.Global
        public T Evaluate<T>(String expression, ScriptScope scope=null)
        {
            if (scope == null)
            {
                scope = _pyScope;
            }
            try
            {
                ScriptSource source = _pyEngine.CreateScriptSourceFromString(expression, SourceCodeKind.Expression);
                CompiledCode compiled = source.Compile();
                
                return compiled.Execute<T>(scope);
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
        public Object GetVariable(String name)
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

        public CompiledCode Compile(String code, SourceCodeKind kind)
        {
            return _pyEngine.CreateScriptSourceFromString(code, kind).Compile();
        }

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

        public ScriptScope GetNewScope(IDictionary<String, Object> variables = null)
        {
            return _pyEngine.CreateScope(variables);
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
    }

    public class PythonException : Exception
    {
        public PythonException(Exception e)
                : base(String.Format("{0}: {1}", e.GetType().Name, e.Message), e)
        { }
    }
}

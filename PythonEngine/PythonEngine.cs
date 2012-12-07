﻿using System;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;


namespace Centipede.PyEngine
{
    /// <summary>
    /// The Iron Python Engine.  Use the <code>variables</code> dictionary to access Job variables.
    /// </summary>
    public class PythonEngine
    {
        private ScriptEngine PyEngine = null;
        private ScriptScope PyScope = null;

        private PythonEngine()
        {
            if (PyEngine == null)
            {
                PyEngine = Python.CreateEngine();
                PyScope = PyEngine.CreateScope();
                PyScope.ImportModule("sys");

            }
        }

        /// <summary>
        /// Execute python code
        /// </summary>
        /// <param name="code">The code to execute</param>
        public void Execute(String code)
        {
            ScriptSource source = PyEngine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);;
            try
            {
                CompiledCode compiled = source.Compile();
                compiled.Execute(PyScope);
            }
            catch (Exception e)
            {
                throw new PythonException(e);
            }
        }

        /// <summary>
        /// Evaluate an expression, and return the result
        /// </summary>
        /// <typeparam name="T">The (C#) Type to coerce the value of the expression to</typeparam>
        /// <param name="expression">Expression to evaluate</param>
        /// <returns>The result of the expression, coerced to type T</returns>
        public T Evaluate<T>(String expression)
        {
            try
            {
                ScriptSource source = PyEngine.CreateScriptSourceFromString(expression, SourceCodeKind.Expression);
                CompiledCode compiled = source.Compile();
                return compiled.Execute<T>(PyScope);
            }
            catch (Exception e)
            {
                throw new PythonException(e);
            }
        }

        /// <summary>
        /// Set a python variable, inside the Engine.
        /// </summary>
        /// <param name="name">Name of the variable to set</param>
        /// <param name="value">Value to set it to</param>
        public void SetVariable(String name, Object value)
        {
            PyScope.SetVariable(name, value);
        }

        /// <summary>
        /// Get the value of a variable inside the python engine.
        /// </summary>
        /// <param name="name">Variable name to get</param>
        /// <returns>The variable's value.  Will need casting to the correct type.</returns>
        public Object GetVariable(String name)
        {
            return PyScope.GetVariable(name);
        }

        /// <summary>
        /// Get a python variable, with a known type
        /// </summary>
        /// <typeparam name="T">The (c#) type to get the variable as</typeparam>
        /// <param name="name">Name of the variable to fetch</param>
        /// <returns>The value of the variable, cast to the appropriate C# type</returns>
        public T GetVariable<T>(String name)
        {
            return PyScope.GetVariable<T>(name);
        }

        public CompiledCode Compile(String code, SourceCodeKind kind)
        {
            return PyEngine.CreateScriptSourceFromString(code, kind).Compile();
        }

        public Boolean VariableExists(String name)
        {
            return PyScope.ContainsVariable(name);
        }

        #region Singleton handling code

        private volatile static PythonEngine _instance;
        private static Object _syncRoot = new Object();

        /// <summary>
        /// The PyEngine Singleton.  <seealso href="http://msdn.microsoft.com/en-us/library/ff650316.aspx"/>
        /// </summary>
        public static PythonEngine Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_syncRoot)
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

        /// <summary>
        /// unused
        /// </summary>
        /// <param name="pyEngine"></param>
        /// <returns></returns>
        [Obsolete]
        public static implicit operator ScriptScope (PythonEngine pyEngine)
        {
            return pyEngine.PyScope;
        }
        
    }

    


    public class PythonException : Exception
    {
        public PythonException(Exception e)
            : base(String.Format("{0}: {1}", e.GetType().Name, e.Message), e)
        { }
    }
}


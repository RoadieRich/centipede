using System;

using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Centipede
{
    class PythonEngine
    {
        private ScriptEngine PyEngine = null;
        //private ScriptRuntime PyRuntime = null;
        private ScriptScope PyScope = null;

        private PythonEngine()
        {
            if (PyEngine == null)
            {
                PyEngine = Python.CreateEngine();
                PyScope = PyEngine.CreateScope();
                PyScope.ImportModule("sys");
                PyScope.SetVariable("sys.stdout", Program.Variables["console"]);
                PyScope.SetVariable("variables", Program.Variables);
            }
        }

        public void Execute(String code)
        {
            ScriptSource source = PyEngine.CreateScriptSourceFromString(code, SourceCodeKind.Statements);
            CompiledCode compiled = source.Compile();
            try
            {
                compiled.Execute(PyScope);
            }

            catch (Exception e)
            {
                throw new PythonException(e);
            }
        }

        public T Evaluate<T>(String expression)
        {
            ScriptSource source = PyEngine.CreateScriptSourceFromString(expression, SourceCodeKind.Expression);
            CompiledCode compiled = source.Compile();
            return compiled.Execute<T>(PyScope);
        }
        
        public void SetVariable(String name, Object value)
        {
            PyScope.SetVariable(name, value);
        }

        public Object GetVariable(String name)
        {
            return PyScope.GetVariable(name);
        }

        public T GetVariable<T>(String name)
        {
            return PyScope.GetVariable<T>(name);
        }

        
        #region Singleton handling code
        
        static private PythonEngine _instance = null;

        public static PythonEngine GetInstance()
        {
            if (_instance == null)
            {
                _instance = new PythonEngine();
            }

            return _instance;
        }
        #endregion
    }

    public class PythonException : Exception
    {
        public PythonException(Exception e) : base("Python Error: " + e.Message, e)
        { } 
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using ResharperAnnotations;


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
        private readonly ScriptScope _pyScope;

        internal ScriptScope Scope
        {
            get
            {
                return _pyScope;
            }
        }

        private PythonEngine()
        {
            //if (_pyEngine == null)
            {
                _pyEngine = Python.CreateEngine();
                _pyScope = this._pyEngine.CreateScope();
                _pyScope.ImportModule(@"sys");
                _pyScope.ImportModule(@"math");
                Execute(@"from math import *", _pyScope);
            }
        }

        private void Execute(string code, ScriptScope scope)
        {
            try
            {
                PythonByteCode compiled = Compile(code, PythonByteCode.SourceCodeType.Statements);
                //CompiledCode compiled = source.Compile();
                compiled.Execute(scope);
            }
            catch (Exception e)
            {
                throw new PythonParseException(e);
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
            ScriptScope myscope = scope != null ? scope.Scope : this._pyScope;
            Execute(code, myscope);
        }

        /// <summary>
        ///     Evaluate an expression, and return the result
        /// </summary>
        /// <typeparam name="T">The (C#) Type to coerce the value of the expression to</typeparam>
        /// <param name="expression">
        /// Expression to evaluate
        /// </param>
        /// <param name="scope">(Optional) the scope to evaluate the action in</param>
        /// <exception cref="PythonException"></exception>
        /// <returns>The result of the expression, coerced to type T</returns>
        public T Evaluate<T>(String expression, PythonScope scope = null)
        {
            
            return (T)Evaluate(expression, scope);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="scope"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        [UsedImplicitly]
        public T Evaluate<T>(PythonByteCode expression, PythonScope scope = null)
        {
            return (T)Evaluate(expression, scope);
        }

        /// <summary>
        ///     Evaluate an expression, and return the result
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <param name="scope">(Optional) the scope to evaluate the action in</param>
        /// <exception cref="PythonException"></exception>
        /// <returns>The result of the expression</returns>
        public dynamic Evaluate(String expression, PythonScope scope = null)
        {
            try
            {
                PythonByteCode compiled = Compile(expression, SourceCodeKind.Expression);
                return Evaluate(compiled, scope);
            }
            catch (PythonException)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new PythonException(e);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="kind"></param>
        /// <returns></returns>
        public PythonByteCode Compile(string expression, PythonByteCode.SourceCodeType kind)
        {
            return Compile(expression, (SourceCodeKind)kind);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        public dynamic Evaluate(PythonByteCode expression, PythonScope scope=null)
        {
            ScriptScope myscope = scope != null ? scope.Scope : this._pyScope;
            try
            {
                return expression.Execute(myscope);
            }
            catch (PythonException)
            {
                throw;
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
        [UsedImplicitly]
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
        [UsedImplicitly]
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
        [UsedImplicitly]
        internal PythonByteCode Compile(String code, SourceCodeKind kind)
        {
            CompiledCode compiledCode;
            try
            {
                compiledCode = this._pyEngine.CreateScriptSourceFromString(code, kind).Compile();
            }
            catch (SyntaxErrorException e)
            {
                throw new PythonParseException(e);
            }   
            
            return new PythonByteCode(compiledCode);
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
        public static explicit operator ScriptScope(PythonEngine pyEngine)
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        public PythonScope GetNewTypedScope(IEnumerable<KeyValuePair<string, object>> variables = null)
        {
            if (variables == null)
            {
                return new PythonScope(this._pyScope);
            }
            ScriptScope scope = this._pyEngine.CreateScope();
            foreach (KeyValuePair<string, object> variable in variables)
            {
                scope.SetVariable(variable.Key, variable.Value);
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public PythonScope GetScope()
        {
            return (PythonScope)this._pyScope;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class PythonByteCode
    {
        private readonly CompiledCode _code;

        internal PythonByteCode(CompiledCode code)
        {
            _code = code;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pythonScope"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Execute<T>(PythonScope pythonScope)
        {
            ScriptScope scope = pythonScope != null ? pythonScope.Scope : PythonEngine.Instance.Scope;
            return _code.Execute<T>(scope);
        }

        internal T Execute<T>(ScriptScope scope)
        {
            return _code.Execute<T>(scope);
        }

        /// <summary>
        /// Same as <see cref="Execute{T}"/>, but uses RTTI
        /// </summary>
        /// <param name="pythonScope"></param>
        /// <returns></returns>
        public dynamic Execute(PythonScope pythonScope)
        {
            ScriptScope scope = pythonScope != null ? pythonScope.Scope : PythonEngine.Instance.Scope;
            return _code.Execute(scope);
        }

        internal dynamic Execute(ScriptScope scope)
        {
            return _code.Execute(scope ?? PythonEngine.Instance.Scope);
        }

        /// <summary>
        /// 
        /// </summary>
        [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
        public enum SourceCodeType
        {
            /// <summary />
            [EditorBrowsable(EditorBrowsableState.Never)]
            Unspecified = SourceCodeKind.Unspecified, //0,

            /// <summary>The code is an expression.</summary>
            Expression = SourceCodeKind.Expression, //1,

            /// <summary>The code is a sequence of statements.</summary>
            Statements = SourceCodeKind.Statements, //2,

            /// <summary>     The code is a single statement.</summary>
            SingleStatement = SourceCodeKind.SingleStatement, //3,

            /// <summary> The code is a content of a file.</summary>
            File = SourceCodeKind.File, //4,

            /// <summary> The code is an interactive command.</summary>
            InteractiveCode = SourceCodeKind.InteractiveCode, //5,

            /// <summary> 
            /// The language parser auto-detects the kind. A syntax error is reported if it is not able to do so. 
            /// </summary>
            AutoDetect = SourceCodeKind.AutoDetect, //6,
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="compiledCode"></param>
        /// <returns></returns>
        public static explicit operator PythonByteCode(CompiledCode compiledCode)
        {
            return new PythonByteCode(compiledCode);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pythonByteCode"></param>
        /// <returns></returns>
        public static explicit operator CompiledCode(PythonByteCode pythonByteCode)
        {
            return pythonByteCode._code;
        }
    }

    internal static class PythonExtentions
    {
        public static SourceCodeKind ToSourceCodeKind(this PythonByteCode.SourceCodeType @this)
        {
            switch (@this)
            {
                case PythonByteCode.SourceCodeType.Unspecified:
                    return SourceCodeKind.Unspecified;
                case PythonByteCode.SourceCodeType.Expression:
                    return SourceCodeKind.Expression;
                case PythonByteCode.SourceCodeType.Statements:
                    return SourceCodeKind.Statements;
                case PythonByteCode.SourceCodeType.SingleStatement:
                    return SourceCodeKind.SingleStatement;
                case PythonByteCode.SourceCodeType.File:
                    return SourceCodeKind.File;
                case PythonByteCode.SourceCodeType.InteractiveCode:
                    return SourceCodeKind.InteractiveCode;
                case PythonByteCode.SourceCodeType.AutoDetect:
                    return SourceCodeKind.AutoDetect;
                default:
                    return default(SourceCodeKind);
            }
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

    /// <summary>
    /// 
    /// </summary>
    public class PythonParseException : PythonException
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="e"></param>
        public PythonParseException(Exception e)
                : base(e)
        { }
    }
}

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
    ///  The Iron Python Engine.
    /// </summary>
    public interface IPythonEngine
    {
        /// <summary>
        ///     Execute python code
        /// </summary>
        /// <param name="code">The code to execute</param>
        /// <param name="scope">(optional)</param>
        /// <exception cref="PythonException"></exception>
        void Execute(String code, PythonScope scope = null);

        /// <summary>
        ///     Evaluate an expression, and return the result
        /// </summary>
        /// <typeparam name="T">The (C#) Type to interpret the value of the expression as</typeparam>
        /// <param name="expression"> Expression to evaluate </param>
        /// <param name="scope">(Optional) the scope to evaluate the action in</param>
        /// <exception cref="PythonException"></exception>
        /// <returns>The result of the expression, coerced to type T</returns>
        /// <seealso cref="Evaluate(string,PythonScope)"/>
        T Evaluate<T>(String expression, PythonScope scope = null);

        /// <summary>
        /// Evaluate a compiled expression, and return the result, cast to type <typeparamref name="T"/>
        /// </summary>
        /// <param name="expression">The <see cref="IPythonByteCode">compiled expression</see> to execute</param>
        /// <param name="scope">The <see cref="PythonScope"/> to execute the value in</param>
        /// <typeparam name="T">The C# type to interpret the result as</typeparam>
        /// <returns></returns>
        /// <seealso cref="Evaluate(IPythonByteCode,PythonScope)"/>
        [UsedImplicitly]
        T Evaluate<T>(IPythonByteCode expression, PythonScope scope = null);

        /// <summary>
        ///     Evaluate an expression, and return the result
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <param name="scope">(Optional) the scope to evaluate the action in</param>
        /// <exception cref="PythonException"></exception>
        /// <returns>The result of the expression</returns>
        /// <seealso cref="Evaluate{T}(string,PythonScope)"/>
        dynamic Evaluate(String expression, PythonScope scope = null);

        /// <summary>
        /// Compile the <paramref name="expression"/> as the given <paramref name="type"/> of code.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IPythonByteCode Compile(string expression, PythonByteCode.SourceCodeType type);

        /// <summary>
        /// Evaluate the compiled expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <seealso cref="Evaluate{T}(IPythonByteCode,PythonScope)"/>
        dynamic Evaluate(IPythonByteCode expression, PythonScope scope=null);

        /// <summary>
        ///     Set a python variable, inside the Engine.
        /// </summary>
        /// <param name="name">Name of the variable to set</param>
        /// <param name="value">Value to set it to</param>
        void SetVariable(String name, Object value);

        /// <summary>
        ///     Get the value of a variable inside the python engine.
        /// </summary>
        /// <param name="name">Variable name to get</param>
        /// <returns>The variable's value.  Will need casting to the correct type.</returns>
        /// <remarks>Avoids extra boxing/unboxing if the value is to be stored as an object reference.</remarks>
        /// <seealso cref="GetVariable{T}"/>
        [UsedImplicitly]
        dynamic GetVariable(String name);

        /// <summary>
        ///     Get a python variable, with a known type
        /// </summary>
        /// <typeparam name="T">The (c#) type to get the variable as</typeparam>
        /// <param name="name">Name of the variable to fetch</param>
        /// <returns>The value of the variable, cast to the appropriate C# type</returns>
        /// <seealso cref="GetVariable"/>
        [UsedImplicitly]
        T GetVariable<T>(String name);

        /// <summary>
        /// determines whether a variable with the given <paramref name="name"/> exists wuithin the <see cref="DefaultScope"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Boolean VariableExists(String name);

        /// <summary>
        /// Create a new scope, either a clone of <see cref="DefaultScope"/>, or an unrelated scope from values in <paramref name="variables"/>.
        /// </summary>
        /// <param name="variables"></param>
        /// <returns>The new scope</returns>
        PythonScope GetNewScope(IDictionary<String, Object> variables = null);

        /// <summary>
        /// Create a new scope from values in <paramref name="variables"/>.
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        PythonScope GetNewTypedScope(IEnumerable<KeyValuePair<string, object>> variables);

        /// <summary>
        /// returns the <see cref="DefaultScope"/>
        /// </summary>
        /// <returns></returns>
        PythonScope GetScope();

        /// <summary>
        /// The default scope
        /// </summary>
        ScriptScope DefaultScope { get; }
    }

    /// <summary>
    /// The IronPython engine
    /// </summary>
    public class PythonEngine : IPythonEngine
    {
        private readonly ScriptEngine _pyEngine;
        private readonly ScriptScope _pyScope;

        private PythonEngine()
        {
            //if (_pyEngine == null)
            {
                _pyEngine = Python.CreateEngine();
                this._pyScope = this._pyEngine.CreateScope();
                this._pyScope.ImportModule(@"sys");
                this._pyScope.ImportModule(@"math");
            }
        }

        private void Execute(string code, ScriptScope scope)
        {
            try
            {
                PythonByteCode compiled = (PythonByteCode)Compile(code, PythonByteCode.SourceCodeType.Statements);
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
        /// Evaluate a compiled expression, and return the result, cast to type <typeparamref name="T"/>
        /// </summary>
        /// <param name="expression">The <see cref="IPythonByteCode">compiled expression</see> to execute</param>
        /// <param name="scope">The <see cref="PythonScope"/> to execute the value in</param>
        /// <typeparam name="T">The C# type to interpret the result as</typeparam>
        /// <returns></returns>
        /// <seealso cref="Evaluate(IPythonByteCode,PythonScope)"/>
        [UsedImplicitly]
        public T Evaluate<T>(IPythonByteCode expression, PythonScope scope = null)
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
        /// <seealso cref="Evaluate{T}(string,PythonScope)"/>
        public dynamic Evaluate(String expression, PythonScope scope = null)
        {
            try
            {
                IPythonByteCode compiled = Compile(expression, SourceCodeKind.Expression);
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
        /// Compile the <paramref name="expression"/> as the given <paramref name="type"/> of code.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public IPythonByteCode Compile(string expression, PythonByteCode.SourceCodeType type)
        {
            return Compile(expression, (SourceCodeKind)type);
        }

        /// <summary>
        /// Evaluate the compiled expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <seealso cref="Evaluate{T}(IPythonByteCode,PythonScope)"/>
        public dynamic Evaluate(IPythonByteCode expression, PythonScope scope=null)
        {
            ScriptScope myscope = scope != null ? scope.Scope : this._pyScope;
            try
            {
                return ((PythonByteCode)expression).Execute(myscope);
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
            this._pyScope.SetVariable(name, value);
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
            return this._pyScope.GetVariable(name);
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
            return this._pyScope.GetVariable<T>(name);
        }

        IPythonByteCode Compile(String code, SourceCodeKind kind)
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
        /// determines whether a variable with the given <paramref name="name"/> exists wuithin the <see cref="DefaultScope"/>.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Boolean VariableExists(String name)
        {
            return this._pyScope.ContainsVariable(name);
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
        /// Create a new scope, either a clone of <see cref="DefaultScope"/>, or an unrelated scope from values in <paramref name="variables"/>.
        /// </summary>
        /// <param name="variables"></param>
        /// <returns>The new scope</returns>
        public PythonScope GetNewScope(IDictionary<String, Object> variables = null)
        {
            return variables != null && variables.Count < 1
                           ? new PythonScope(this._pyScope)
                           : new PythonScope(this._pyEngine.CreateScope(variables));
        }

        /// <summary>Create a new scope from values in <paramref name="variables"/>.</summary>
        /// <param name="variables"></param>
        /// <returns></returns>        
        public PythonScope GetNewTypedScope([NotNull] IEnumerable<KeyValuePair<string, object>> variables)
        {
            ScriptScope scope = this._pyEngine.CreateScope();
            foreach (KeyValuePair<string, object> variable in variables)
            {
                scope.SetVariable(variable.Key, variable.Value);
            }
            return new PythonScope(scope);
        }

        #region Singleton handling code

        private static readonly Lazy<PythonEngine> InstanceLazy = new Lazy<PythonEngine>(() => new PythonEngine());

        /// <summary>
        ///     The PyEngine Singleton
        /// </summary>
        public static IPythonEngine Instance
        {
            get
            {
                return InstanceLazy.Value;
            }
        }

        #endregion

        /// <summary>
        /// returns the <see cref="DefaultScope"/>
        /// </summary>
        /// <returns></returns>
        public PythonScope GetScope()
        {
            return (PythonScope)this._pyScope;
        }

        /// <summary>
        /// The default scope
        /// </summary>
        public ScriptScope DefaultScope { get
        {
            return this._pyScope;
        }}
    }

    /// <summary>
    /// Compiled python bytecode
    /// </summary>
    public interface IPythonByteCode
    {
        /// <summary>
        /// Execute the bytecode in the given scope, interpretting the result as type <typeparamref name="T"/>
        /// </summary>
        /// <param name="pythonScope"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        T Execute<T>(PythonScope pythonScope);

        /// <summary>
        /// Same as <see cref="PythonByteCode.Execute{T}"/>, but uses RTTI
        /// </summary>
        /// <param name="pythonScope"></param>
        /// <returns></returns>
        dynamic Execute(PythonScope pythonScope);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <remarks>
    /// Ok, so this isn't nessecarily python byte code - the precise implementation is unspecified.
    /// </remarks>
    public class PythonByteCode : IPythonByteCode
    {
        private readonly CompiledCode _code;

        internal PythonByteCode(CompiledCode code)
        {
            _code = code;
        }

        /// <summary>
        /// Execute the bytecode in the given scope, interpretting the result as type <typeparamref name="T"/>
        /// </summary>
        /// <param name="pythonScope"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T Execute<T>(PythonScope pythonScope)
        {
            ScriptScope scope = pythonScope != null ? pythonScope.Scope : PythonEngine.Instance.DefaultScope;
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
            ScriptScope scope = pythonScope != null ? pythonScope.Scope : PythonEngine.Instance.DefaultScope;
            return _code.Execute(scope);
        }

        internal dynamic Execute(ScriptScope scope)
        {
            return _code.Execute(scope ?? PythonEngine.Instance.DefaultScope);
        }

        /// <summary>
        /// Defines a kind of the source code. The parser sets its initial state accordingly. 
        /// </summary>
        /// <seealso cref="SourceCodeKind"/>
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

/*
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
*/

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

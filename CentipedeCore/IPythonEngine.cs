using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Scripting;
using PythonEngine;
using ResharperAnnotations;

namespace Centipede
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
        void Execute(String code, IPythonScope scope = null);

        /// <summary>
        ///     Evaluate an expression, and return the result
        /// </summary>
        /// <typeparam name="T">The (C#) Type to interpret the value of the expression as</typeparam>
        /// <param name="expression"> Expression to evaluate </param>
        /// <param name="scope">(Optional) the scope to evaluate the action in</param>
        /// <exception cref="PythonException"></exception>
        /// <returns>The result of the expression, coerced to type T</returns>
        /// <seealso cref="Evaluate(string,PythonScope)"/>
        T Evaluate<T>(String expression, IPythonScope scope = null);

        /// <summary>
        /// Evaluate a compiled expression, and return the result, cast to type <typeparamref name="T"/>
        /// </summary>
        /// <param name="expression">The <see cref="IPythonByteCode">compiled expression</see> to execute</param>
        /// <param name="scope">The <see cref="PythonScope"/> to execute the value in</param>
        /// <typeparam name="T">The C# type to interpret the result as</typeparam>
        /// <returns></returns>
        /// <seealso cref="Evaluate(IPythonByteCode,PythonScope)"/>
        [UsedImplicitly]
        T Evaluate<T>(IPythonByteCode expression, IPythonScope scope = null);

        /// <summary>
        ///     Evaluate an expression, and return the result
        /// </summary>
        /// <param name="expression">Expression to evaluate</param>
        /// <param name="scope">(Optional) the scope to evaluate the action in</param>
        /// <exception cref="PythonException"></exception>
        /// <returns>The result of the expression</returns>
        /// <seealso cref="Evaluate{T}(string,PythonScope)"/>
        dynamic Evaluate(String expression, IPythonScope scope = null);

        /// <summary>
        /// Compile the <paramref name="expression"/> as the given <paramref name="type"/> of code.
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        IPythonByteCode Compile(string expression, SourceCodeType type);

        /// <summary>
        /// Evaluate the compiled expression
        /// </summary>
        /// <param name="expression"></param>
        /// <param name="scope"></param>
        /// <returns></returns>
        /// <seealso cref="Evaluate{T}(IPythonByteCode,PythonScope)"/>
        dynamic Evaluate(IPythonByteCode expression, IPythonScope scope=null);

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
        IPythonScope GetNewScope(IDictionary<String, Object> variables = null);

        /// <summary>
        /// Create a new scope from values in <paramref name="variables"/>.
        /// </summary>
        /// <param name="variables"></param>
        /// <returns></returns>
        IPythonScope GetNewTypedScope(IEnumerable<KeyValuePair<string, object>> variables);

        /// <summary>
        /// returns the <see cref="DefaultScope"/>
        /// </summary>
        /// <returns></returns>
        IPythonScope GetScope();

        /// <summary>
        /// The default scope
        /// </summary>
        IPythonScope DefaultScope { get; }
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
}
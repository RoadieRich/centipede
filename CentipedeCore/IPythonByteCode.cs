using System;

namespace PythonEngine
{
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
        T Execute<T>(IPythonScope pythonScope);

        /// <summary>
        /// Same as <see cref="PythonByteCode.Execute{T}"/>, but uses RTTI
        /// </summary>
        /// <param name="pythonScope"></param>
        /// <returns></returns>
        dynamic Execute(IPythonScope pythonScope);
    }
}
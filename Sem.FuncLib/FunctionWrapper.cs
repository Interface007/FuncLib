// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FunctionWrapper.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Combines a function together with the value for the actions parameter.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib
{
    using System;

    /// <summary>
    /// Combines a function that returns a function together with the value for the functions parameter.
    /// </summary>
    /// <typeparam name="TValue"> The type of the parameter of the function. </typeparam>
    /// <typeparam name="TRight"> The return type of the function. </typeparam>
    public struct FunctionWrapper<TValue, TRight>
    {
        /// <summary>
        /// The function that returns the function to be executed.
        /// </summary>
        private readonly Func<Func<TValue, TRight>, TValue, TRight> func;

        /// <summary>
        /// The value to use.
        /// </summary>
        private readonly TValue value;

        /// <summary>
        /// Initializes a new instance of the <see cref="FunctionWrapper{TValue,TRight}"/> struct.
        /// </summary>
        /// <param name="func">
        /// The function that returns the function to be executed.
        /// </param>
        /// <param name="value">
        /// The value to use.
        /// </param>
        internal FunctionWrapper(Func<Func<TValue, TRight>, TValue, TRight> func, TValue value)
        {
            this.func = func;
            this.value = value;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public TValue Value
        {
            get
            {
                return this.value;
            }
        }

        /// <summary>
        /// Gets the function to execute.
        /// </summary>
        internal Func<Func<TValue, TRight>, TValue, TRight> Func
        {
            get
            {
                return this.func;
            }
        }

        /// <summary>
        /// executes the function together with its value.
        /// </summary>
        /// <param name="action"> The action. </param>
        /// <returns> The result of the function call. </returns>
        internal TRight Execute(Func<TValue, TRight> action)
        {
            return this.func(action, this.value);
        }
    }
}
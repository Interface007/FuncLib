// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ExecutionCounter.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Simple class that will count the execution of a method.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib.Tests
{
    using System;

    /// <summary>
    /// Simple class that will count the execution of a method.
    /// </summary>
    public class ExecutionCounter
    {
        /// <summary>
        /// Gets or sets the count of calls.
        /// </summary>
        public int Count { get; set; }

        /// <summary>
        /// Executes a function and counts that.
        /// </summary>
        /// <param name="func"> The function to execute. </param>
        /// <typeparam name="TResult"> The type of the function result. </typeparam>
        /// <returns> The <see cref="TResult"/>. </returns>
        public TResult Action<TResult>(Func<TResult> func)
        {
            this.Count = this.Count + 1;
            return func();
        }

        /// <summary>
        /// Executes a function and counts that.
        /// </summary>
        /// <param name="func"> The function to execute. </param>
        /// <param name="value"> The function parameter. </param>
        /// <typeparam name="TValue"> The type of the function parameter. </typeparam>
        /// <typeparam name="TRight"> The type of the function result. </typeparam>
        /// <returns> The <see cref="TRight"/>. </returns>
        public TRight Action2<TValue, TRight>(Func<TValue, TRight> func, TValue value)
        {
            this.Count = this.Count + 1;
            return func(value);
        }

        /// <summary>
        /// Executes a function and prints the value to the console window.
        /// </summary>
        /// <param name="func"> The function to execute. </param>
        /// <param name="value"> The function parameter. </param>
        /// <typeparam name="TValue"> The type of the function parameter. </typeparam>
        /// <typeparam name="TRight"> The type of the function result. </typeparam>
        /// <returns> The <see cref="TRight"/>. </returns>
        public TRight Action3<TValue, TRight>(Func<TValue, TRight> func, TValue value)
        {
            Console.WriteLine("Data was [{0}]", value);
            return func(value);
        }
    }
}
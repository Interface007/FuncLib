// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ArrangeActAssert.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the ArrangeActAssert type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The "arrange act assert" bas class.
    /// </summary>
    /// <typeparam name="TTarget"> The type of the test target. </typeparam>
    /// <typeparam name="TData"> The type of the test data. </typeparam>
    /// <typeparam name="TResult">The type of the method call result from the test target.  </typeparam>
    public abstract class ArrangeActAssert<TTarget, TData, TResult>
    {
        /// <summary>
        /// Gets or sets the exception that may has been thrown while executing the test.
        /// </summary>
        public Exception Exception { get; set; }

        /// <summary>
        /// Gets or sets the data.
        /// </summary>
        public TData Data { get; set; }

        /// <summary>
        /// Gets or sets the test target.
        /// </summary>
        public TTarget Target { get; set; }

        /// <summary>
        /// Gets or sets the test execution result.
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// The "arrange act assert" execution.
        /// </summary>
        [TestInitialize]
        public void ArrActAss()
        {
            this.Data = this.Arrange();
            try
            {
                this.Result = this.Act();
            }
            catch (Exception ex)
            {
                this.Exception = ex;
            }
        }

        /// <summary>
        /// The act method that should execute the logic to be tested.
        /// </summary>
        /// <returns> The test result as <see cref="TResult"/>. </returns>
        protected abstract TResult Act();

        /// <summary>
        /// The arrange method should setup test data.
        /// </summary>
        /// <returns> The <see cref="TData"/>. </returns>
        protected virtual TData Arrange()
        {
            return default(TData);
        }
    }
}
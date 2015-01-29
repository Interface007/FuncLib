// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GivenASetOfNumber.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the GivenASetOfNumber type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib.Tests
{
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The given a set of number.
    /// </summary>
    public static class GivenASetOfNumber
    {
        /// <summary>
        /// The with a range of INTs.
        /// </summary>
        /// <typeparam name="TTarget"> The type of the test target </typeparam>
        /// <typeparam name="TResult"> The type of the method result. </typeparam>
        public abstract class WithRangeOfInts<TTarget, TResult> : ArrangeActAssert<TTarget, IEnumerable<int>, TResult>
        {
            /// <summary>
            /// The arrange method that sets up the test data.
            /// </summary>
            /// <returns> The <see cref="IEnumerable{T} "/>. </returns>
            protected override IEnumerable<int> Arrange()
            {
                return Enumerable.Range(0, 20);
            }
        }

        [TestClass]
        public class CalculatingOneDividedByN : WithRangeOfInts<Failable<int>, IEnumerable<int>>
        {
            [TestMethod]
            public void GeneratesAnError()
            {
                Assert.IsNotNull(this.Exception);
            }

            protected override IEnumerable<int> Act()
            {
                return this.Data.Select(x => (1 / x)).ToArray();
            }
        }

        [TestClass]
        public class CalculatingOneDividedByNAndSkipping1 : WithRangeOfInts<Failable<int>, IEnumerable<int>>
        {
            [TestMethod]
            public void Fails()
            {
                Assert.IsNotNull(this.Exception);
            }

            protected override IEnumerable<int> Act()
            {
                return this.Data.Select(x => (1 / x)).Skip(1).ToArray();
            }
        }

        [TestClass]
        public class CalculatingOneDividedByNAndUsingFailable : WithRangeOfInts<Failable<int>, IEnumerable<Failable<int>>>
        {
            [TestMethod]
            public void Succeeds()
            {
                Assert.IsNull(this.Exception);
            }

            protected override IEnumerable<Failable<int>> Act()
            {
                return this.Data.Select(x => new Failable<int>(() => 1 / x)).ToArray();
            }
        }
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="ClassEither.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   The tests for the class <see cref="Either{TOne,TTwo}" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib.Tests
{
    using System;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The tests for the class <see cref="Either{TOne,TTwo}"/>.
    /// </summary>
    public static class ClassEither
    {
        /// <summary>
        /// The casts from native.
        /// </summary>
        [TestClass]
        public class CastsFromNative
        {
            /// <summary>
            /// Cast succeeds for a class instance with first type parameter matching class type.
            /// </summary>
            [TestMethod]
            public void SucceedsForAClassInstanceWithFirstTypeParameterMatchingClassType()
            {
                var target = (Either<Sample, Exception>)new Sample(5);
                Assert.IsFalse(target.Is<Exception>());
                Assert.IsNotNull((Sample)target);
                Assert.IsTrue(target.Is<Sample>());
            }

            /// <summary>
            /// Cast succeeds for a class instance with second type parameter matching class type.
            /// </summary>
            [TestMethod]
            public void SucceedsForAClassInstanceWithSecondTypeParameterMatchingClassType()
            {
                var target = (Either<Exception, Sample>)new Sample(5);
                Assert.IsFalse(target.Is<Exception>());
                Assert.IsNotNull((Sample)target);
                Assert.IsTrue(target.Is<Sample>());
            }
        }

        /// <summary>
        /// Casting from null.
        /// </summary>
        [TestClass]
        public class CastsFromNull
        {
            /// <summary>
            /// Succeeds without exception.
            /// </summary>
            [TestMethod]
            public void Succeeds()
            {
                var sample = (Sample)null;
                // ReSharper disable once ExpressionIsAlwaysNull
                var value = (Either<Sample, Exception>)sample;
                Assert.IsNull((Sample)value);
                Assert.IsTrue(value.Is<Sample>());
            }
        }

        /// <summary>
        /// The method <see cref="Either{TOne,TTwo}.WhenIs"/>.
        /// </summary>
        [TestClass]
        public class WhenIs
        {
            /// <summary>
            /// Detects correct type for first type parameter.
            /// </summary>
            [TestMethod]
            public void DetectsCorrectTypeForFirstTypeParameter()
            {
                var invoked = false;
                var sample = (Sample)null;
                // ReSharper disable once ExpressionIsAlwaysNull
                var value = (Either<Sample, Exception>)sample;
                value.WhenIs<Sample>(x => invoked = true);
                Assert.IsTrue(invoked);
            }

            /// <summary>
            /// Detects correct type for second type parameter.
            /// </summary>
            [TestMethod]
            public void DetectsCorrectTypeForSecondTypeParameter()
            {
                var invoked = false;
                var sample = (Exception)null;
                // ReSharper disable once ExpressionIsAlwaysNull
                var value = (Either<Sample, Exception>)sample;
                value.WhenIs<Exception>(x => invoked = true);
                Assert.IsTrue(invoked);
            }

            /// <summary>
            /// Does not invoke action with non matching null value in type 1.
            /// </summary>
            [TestMethod]
            public void WithNonMatchingNullValue1()
            {
                var invoked = false;
                var sample = (Exception)null;
                // ReSharper disable once ExpressionIsAlwaysNull
                var value = (Either<Sample, Exception>)sample;
                value.WhenIs<Sample>(x => invoked = true);
                Assert.IsFalse(invoked);
            }

            /// <summary>
            /// Does not invoke action with non matching null value in type 2.
            /// </summary>
            [TestMethod]
            public void WhenIsWithNonMatchingNullValue2()
            {
                var invoked = false;
                var sample = (Sample)null;
                // ReSharper disable once ExpressionIsAlwaysNull
                var value = (Either<Sample, Exception>)sample;
                value.WhenIs<Exception>(x => invoked = true);
                Assert.IsFalse(invoked);
            }
        }

        /// <summary>
        /// The casts between equivalent classes with swapped type arguments.
        /// </summary>
        [TestClass]
        public class CastsBetweenEquivalent
        {
            /// <summary>
            /// Succeeds when value is not null.
            /// </summary>
            [TestMethod]
            public void SucceedsWhenValueIsNotNull()
            {
                var sample = new Sample(5);
                var value = (Either<Sample, Exception>)sample;
                var target = (Either<Exception, Sample>)value;
                Assert.AreEqual(5, ((Sample)target).Value);
                Assert.AreSame(sample, (Sample)target);
            }

            /// <summary>
            /// Succeeds when value is null.
            /// </summary>
            [TestMethod]
            public void SucceedsWhenValueIsNull()
            {
                Sample sample = null;
                // ReSharper disable once ExpressionIsAlwaysNull
                var value = (Either<Sample, Exception>)sample;
                var target = (Either<Exception, Sample>)value;
                Assert.IsNull((Sample)target);
            }
        }

        /// <summary>
        /// Testing for equivalence.
        /// </summary>
        [TestClass]
        public class TestingEquivalence
        {
            /// <summary>
            /// With same type parameters and same instance returns true.
            /// </summary>
            [TestMethod]
            public void EqualAndSameEqualTypes()
            {
                var sample = new Sample(5);
                var value1 = (Either<Sample, Exception>)sample;
                var value2 = (Either<Sample, Exception>)sample;

                Assert.AreEqual(value1, value2);
                Assert.AreSame((Sample)value2, (Sample)value1);
            }

            /// <summary>
            /// With swapped type parameters and same instance returns true.
            /// </summary>
            [TestMethod]
            public void EqualAndSameSwappedTypes()
            {
                var sample = new Sample(5);
                var value1 = (Either<Sample, Exception>)sample;
                var value2 = (Either<Exception, Sample>)sample;

                Assert.AreEqual(value1, value2);
                Assert.AreSame((Sample)value2, (Sample)value1);
            }

            /// <summary>
            /// With same type parameters and different instances returns false.
            /// </summary>
            [TestMethod]
            public void NotEqualAndSameTypes()
            {
                var sample1 = new Sample(5);
                var sample2 = new Sample(6);
                var value1 = (Either<Sample, Exception>)sample1;
                var value2 = (Either<Sample, Exception>)sample2;

                Assert.AreNotEqual(value1, value2);
                Assert.AreNotSame((Sample)value2, (Sample)value1);
            }

            /// <summary>
            /// With swapped type parameters and different instances returns false.
            /// </summary>
            [TestMethod]
            public void NotEqualAndSwappedTypes()
            {
                var sample1 = new Sample(5);
                var sample2 = new Sample(6);
                var value1 = (Either<Exception, Sample>)sample1;
                var value2 = (Either<Sample, Exception>)sample2;

                Assert.AreNotEqual(value1, value2);
                Assert.AreNotSame((Sample)value2, (Sample)value1);
            }

            /// <summary>
            /// With different values using the equals operator returns false.
            /// </summary>
            [TestMethod]
            public void NotEqualWithEqualsOperator()
            {
                var sample1 = new Sample(5);
                var sample2 = new Sample(6);
                var value1 = (Either<Exception, Sample>)sample1;
                var value2 = (Either<Exception, Sample>)sample2;

                Assert.IsFalse(value1 == value2);
            }

            /// <summary>
            /// With same instance using the equals operator returns true.
            /// </summary>
            [TestMethod]
            public void EqualWithEqualsOperator()
            {
                var sample1 = new Sample(6);
                var value1 = (Either<Exception, Sample>)sample1;
                var value2 = (Either<Exception, Sample>)sample1;

                Assert.IsTrue(value1 == value2);
            }

            /// <summary>
            /// With different instances using the "not equals" operator returns true.
            /// </summary>
            [TestMethod]
            public void NotEqualWithNotEqualsOperator()
            {
                var sample1 = new Sample(5);
                var sample2 = new Sample(6);
                var value1 = (Either<Exception, Sample>)sample1;
                var value2 = (Either<Exception, Sample>)sample2;

                Assert.IsTrue(value1 != value2);
            }

            /// <summary>
            /// With same instance using the "not equals" operator returns false.
            /// </summary>
            [TestMethod]
            public void EqualWithNotEqualsOperator()
            {
                var sample1 = new Sample(6);
                var value1 = (Either<Exception, Sample>)sample1;
                var value2 = (Either<Exception, Sample>)sample1;

                Assert.IsFalse(value1 != value2);
            }
        }
    }
}
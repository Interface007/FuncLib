// --------------------------------------------------------------------------------------------------------------------
// <copyright file="EitherProcessing.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   The tests for the processing of type <see cref="Either{TOne,TTwo}" />.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The tests for the processing of type <see cref="Either{TOne,TTwo}"/>.
    /// </summary>
    [TestClass]
    public class EitherProcessing
    {
        /// <summary>
        /// The can handle either case 4.
        /// Here we have a number of INTs and selecting a division into an <see cref="Either{TOne,TTwo}"/> by using 
        /// an <see cref="Func{T1, TResult}"/> that may throw an <see cref="DivideByZeroException"/>. The function
        /// <see cref="Extensions.Try{TValue,TResult1,TResult2}(System.Func{TValue,TResult1})"/>  does catch the
        /// exception and creates <see cref="Either{TOne,TTwo}"/> instances which can be processed "inline"
        /// </summary>
        [TestMethod]
        public void CanHandleEitherCase4()
        {
            var lastExceptionMessage = string.Empty;
            var res1 = new[] { 1, 2, 3, 4, 5, 6 }
                .Select(Extensions.Try<int, int, DivideByZeroException>(num => 100 / (num - 2)))
                .Select(x => x.WhenIs<Exception>(e => lastExceptionMessage = e.ToString()))
                .ToArray();

            Assert.AreEqual(6, res1.Count());
            Assert.IsTrue(lastExceptionMessage.StartsWith("System.DivideByZeroException"));
        }

        /// <summary>
        /// The can handle either case 5.
        /// Here we "try" to calculate using an extension to <see cref="IEnumerable{T}"/>
        /// </summary>
        [TestMethod]
        public void CanHandleEitherCase5()
        {
            var lastExceptionMessage = string.Empty;
            var res1 = new[] { 1, 2, 3, 4, 5, 6 }
                .Try(num => 100 / (num - 2))
                .Select(x => x.WhenIs<Exception>(e => lastExceptionMessage += e.ToString()))
                .ToArray();

            Assert.AreEqual(6, res1.Count());
            Assert.AreEqual(-100, (int)res1[0]);
            Assert.IsTrue(lastExceptionMessage.StartsWith("System.DivideByZeroException"));
        }

        /// <summary>
        /// Here we handle the case that the <see cref="Either{TLeft,TRight}"/> is a <see cref="DivideByZeroException"/>
        /// using a typed lambda. As you can see we can use a type that inherits from <see cref="Exception"/> when the
        /// type of the <see cref="Either{TLeft,TRight}"/> is <see cref="Exception"/>.
        /// </summary>
        [TestMethod]
        public void CanHandleIEnumerableOfEither()
        {
            var lastExceptionMessage = string.Empty;
            var res1 = new[] { 1, 2, 3, 4, 5, 6 }
                .Try(num => 100 / (num - 2))
                .WhenIs((DivideByZeroException e) => lastExceptionMessage = e.ToString())
                .ToArray();

            Assert.AreEqual(6, res1.Count());
            Assert.AreEqual(-100, (int)res1[0]);
            Assert.IsTrue(lastExceptionMessage
                            .StartsWith("System.DivideByZeroException"));
        }

        /// <summary>
        /// In this case we handle two different cases (integers and exceptions).
        /// </summary>
        [TestMethod]
        public void SplittingByType()
        {
            var logger = new ExceptionLogger();
            var counter = new ExecutionCounter();

            var res1 = new[] { "1", "12", "123", "1234", null, "12345" }
                .Try(value => counter.Action(() => 100 / (value.Length - 2)))
                .WhenIs(logger.LogInts)
                .WhenIs(logger.HandleException)
                .ToArray();

            Assert.AreEqual(6, res1.Count());           // we should have 6 return values
            Assert.AreEqual(2, logger.List.Count());    // out list of logs contains two exceptions
            Assert.AreEqual(4, logger.IntList.Count()); // out list of ints contains two integers
            Assert.AreEqual(6, counter.Count);          // we have only 6 executions of the calculation
            Assert.AreEqual(-100, (int)res1[0]);        // check for a successfull result
        }

        /// <summary>
        /// Here we "inject" two different actions in the processing of the integers.
        /// Be aware that <see cref="ExecutionCounter.Action2{TValue,TRight}"/> and
        /// <see cref="ExecutionCounter.Action2{TValue,TRight}"/> are called for each 
        /// element when the method <see cref="Calculate"/> is called by the extension method
        /// <see cref="Extensions.Try{TValue,TResult1,TResult2}(System.Func{TValue,TResult1})"/>,
        /// so skipping one item will also skip the execution of the "injected" methods 
        /// <see cref="ExecutionCounter.Action2{TValue,TRight}"/> and
        /// <see cref="ExecutionCounter.Action2{TValue,TRight}"/>.
        /// </summary>
        [TestMethod]
        public void PerformingActionsWhileCalculating()
        {
            var logger = new ExceptionLogger();
            var counter = new ExecutionCounter();

            var res1 = new[] { "1", "12", "123", "1234", null, "12345" }
                .WithAction<string, int>(counter.Action2)   // this is not optimal, since we need to specify types here
                .WithAction<string, int>(counter.Action3)   // this is not optimal, since we need to specify types here
                .Skip(1)
                .Try(Calculate)                             // here the value is calculated while calling the two actions defined above
                .WhenIs(logger.HandleException)
                .ToArray();

            Assert.AreEqual(5, res1.Count());           // we should have 6 return values
            Assert.AreEqual(5, counter.Count);          // we have only 6 executions of the calculation
            Assert.AreEqual(2, logger.List.Count());    // out list of logs contains two exceptions
            Assert.AreEqual(100, (int)res1[1]);        // check for a successfull result
        }

        [TestMethod]
        public void PerformingActionsFromAnArrayWhileCalculating()
        {
            var logger = new ExceptionLogger();
            var counter = new ExecutionCounter();

            var actions = new Func<Func<string, int>, string, int>[] { counter.Action2, counter.Action3 };
            
            var res1 = new[] { "1", "12", "123", "1234", null, "12345" }
                .WithActions(actions)   
                .Skip(1)
                .Try(Calculate)         
                .WhenIs(logger.HandleException)
                .ToArray();

            Assert.AreEqual(5, res1.Count());           // we should have 6 return values
            Assert.AreEqual(5, counter.Count);          // we have only 6 executions of the calculation
            Assert.AreEqual(2, logger.List.Count());    // out list of logs contains two exceptions
            Assert.AreEqual(100, (int)res1[1]);        // check for a successfull result
        }

        /// <summary>
        /// The method that implements the calculation.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The result. </returns>
        private static int Calculate(string value)
        {
            return 100 / (value.Length - 2);
        }
    }
}
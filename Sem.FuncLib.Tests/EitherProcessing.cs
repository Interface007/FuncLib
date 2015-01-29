namespace Sem.FuncLib.Tests
{
    using System;
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

        [TestMethod]
        public void SplittingByType()
        {
            var lastExceptionMessage = string.Empty;
            var logger = new ExceptionLogger();
            var counter = new ExecutionCounter();

            var res1 = new[] { "1", "12", "123", "1234", null, "12345" }
                .Try(value => counter.Action(() => 100 / (value.Length - 2)))
                .WhenIs(logger.HandleException)
                .WhenIs((DivideByZeroException e) => lastExceptionMessage += e.ToString())
                .ToArray();

            Assert.AreEqual(6, res1.Count());           // we should have 6 return values
            Assert.AreEqual(2, logger.List.Count());    // out list of logs contains two exceptions
            Assert.AreEqual(6, counter.Count);          // we have only 6 executions of the calculation
            Assert.AreEqual(-100, (int)res1[0]);        // check for a successfull result
            Assert.IsTrue(lastExceptionMessage          // and validate the execution for the DivideByZeroException
                            .StartsWith("System.DivideByZeroException"));
        }

        [TestMethod]
        public void SplittingByType2()
        {
            var lastExceptionMessage = string.Empty;
            var logger = new ExceptionLogger();
            var counter = new ExecutionCounter();

            Func<Func<string, string>, int, string> f = null;

            var res1 = new[] { "1", "12", "123", "1234", null, "12345" }
                .WithAction<string, int>(counter.Action2)   // this is not optimal, since we need to specify types here
                .WithAction<string, int>(counter.Action3)   // this is not optimal, since we need to specify types here
                .Try(Calculate)                             // here the value is calculated while calling the two actions defined above
                .WhenIs(logger.HandleException)
                .WhenIs((DivideByZeroException e) => lastExceptionMessage += e.ToString())
                .ToArray();

            Assert.AreEqual(6, res1.Count());           // we should have 6 return values
            Assert.AreEqual(6, counter.Count);          // we have only 6 executions of the calculation
            Assert.AreEqual(2, logger.List.Count());    // out list of logs contains two exceptions
            Assert.AreEqual(-100, (int)res1[0]);        // check for a successfull result
            Assert.IsTrue(lastExceptionMessage          // and validate the execution for the DivideByZeroException
                            .StartsWith("System.DivideByZeroException"));
        }

        private static int Calculate(string value)
        {
            return 100 / (value.Length - 2);
        }
    }

    public class ExecutionCounter
    {
        public TResult Action<TResult>(Func<TResult> func)
        {
            this.Count = this.Count + 1;
            return func();
        }

        public int Count { get; set; }

        public TRight Action2<TValue, TRight>(Func<TValue, TRight> func, TValue x)
        {
            this.Count = this.Count + 1;
            return func(x);
        }
        public TRight Action3<TValue, TRight>(Func<TValue, TRight> func, TValue x)
        {
            Console.WriteLine("Data was [{0}]", x);
            return func(x);
        }
    }
}
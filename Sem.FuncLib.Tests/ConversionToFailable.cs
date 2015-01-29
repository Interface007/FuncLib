namespace Sem.FuncLib.Tests
{
    using System;
    using System.Globalization;
    using System.Linq;

    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// The conversion from value to failable.
    /// </summary>
    [TestClass]
    public class ConversionToFailable
    {
        /// <summary>
        /// The failables can be checked using the <see cref="Failable{TValue}.Failed"/> property.
        /// </summary>
        [TestMethod]
        public void FailablesCanBeChecked()
        {
            var numbers = new[] { 1, 2, 3, 4, 5, 6 };
            var calc = numbers.Select(number => new Failable<int>(() => 100 / (number - 2)));
            var res1 = calc.Select(x => "result is " + (x.Failed ? "undefined" : x.Value.ToString(CultureInfo.InvariantCulture))).ToArray();
            Assert.AreEqual(6, res1.Length);
            Assert.AreEqual("result is undefined", res1[1]);
            Assert.AreEqual("result is -100", res1[0]);
            Assert.AreEqual("result is 100", res1[2]);
        }

        /// <summary>
        /// The can handle either case 1.
        /// </summary>
        [TestMethod]
        public void CanHandleEitherCase1()
        {
            var lastExceptionMessage = string.Empty;
            var res1 = new[] { 1, 2, 3, 4, 5, 6 }
                .Select(number => new Failable<int>(() => 100 / (number - 2)))
                .Select(x => x.ToEither())
                .WhenIs((Exception x) => lastExceptionMessage = x.ToString())
                .ToArray();

            Assert.AreEqual(6, res1.Count());
            Assert.IsTrue(lastExceptionMessage.StartsWith("System.DivideByZeroException"));
        }

        [TestMethod]
        public void CanHandleEitherCase2()
        {
            var lastExceptionMessage = string.Empty;
            var res1 = new[] { 1, 2, 3, 4, 5, 6 }
                .Select(number => new Failable<int>(() => 100 / (number - 2)))
                .Select(x => x.ToEither())
                .WhenIs((Exception x) => lastExceptionMessage = x.ToString())
                .ToArray();

            Assert.AreEqual(6, res1.Count());
            Assert.IsTrue(lastExceptionMessage.StartsWith("System.DivideByZeroException"));
        }

        [TestMethod]
        public void CanHandleEitherCase3()
        {
            var lastExceptionMessage = string.Empty;
            var res1 = new[] { 1, 2, 3, 4, 5, 6 }
                .Select(number => new Failable<int>(() => 100 / (number - 2)))
                .Select(x => x.ToEither())
                .Select(x => x.WhenIs<Exception>(e => lastExceptionMessage = e.ToString()))
                .ToArray();

            Assert.AreEqual(6, res1.Count());
            Assert.IsTrue(lastExceptionMessage.StartsWith("System.DivideByZeroException"));
        }
    }
}
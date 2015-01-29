// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Failable.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the Failable type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib
{
    using System;

    /// <summary>
    /// Represents a value of an exception that has been thrown while calculating the value.
    /// </summary>
    /// <typeparam name="TRight"> The type of the value </typeparam>
    public struct Failable<TRight>
    {
        /// <summary>
        /// The function to evaluate to determine the value of this instance.
        /// </summary>
        private readonly Func<TRight> valueFunc;

        /// <summary>
        /// The value of this instance.
        /// </summary>
        private TRight value;

        /// <summary>
        /// A value indicating whether the value still needs to be evaluated.
        /// </summary>
        private bool valueIsFunc;

        /// <summary>
        /// The exception that has been thrown (if it has been thrown).
        /// </summary>
        private Exception exception;

        /// <summary>
        /// Initializes a new instance of the <see cref="Failable{TRight}"/> struct.
        /// </summary>
        /// <param name="value"> The value. </param>
        public Failable(TRight value)
        {
            this.value = value;
            this.exception = null;
            this.valueIsFunc = false;
            this.valueFunc = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Failable{TRight}"/> struct.
        /// </summary>
        /// <param name="exception"> The exception. </param>
        public Failable(Exception exception)
        {
            this.value = default(TRight);
            this.exception = exception;
            this.valueIsFunc = false;
            this.valueFunc = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Failable{TRight}"/> struct.
        /// </summary>
        /// <param name="func"> The function to evaluate the value. </param>
        public Failable(Func<TRight> func)
        {
            this.valueFunc = func;
            this.exception = null;
            this.valueIsFunc = true;
            this.value = default(TRight);
        }

        /// <summary>
        /// Gets the exception that has been thrown.
        /// </summary>
        public Exception Exception
        {
            get
            {
                this.Resolve();
                return this.exception;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance represents a failed evaluation.
        /// </summary>
        public bool Failed
        {
            get
            {
                this.Resolve();
                return this.exception != null;
            }
        }

        /// <summary>
        /// Gets the value of this instance.
        /// </summary>
        /// <exception cref="AccessToFailedValueException"> If this instance represents an exception. </exception>
        public TRight Value
        {
            get
            {
                this.Resolve();
                if (this.exception != null)
                {
                    throw new AccessToFailedValueException("Trying to access failed value - you should test whether this value is failed before casting it or accessing the value.", this.exception);
                }

                return this.value;
            }
        }

        /// <summary>
        /// Converts a function of type <typeparamref name="TRight"/> to a <see cref="Failable{TRight}"/>.
        /// </summary>
        /// <param name="function"> The value. </param>
        /// <returns> The <see cref="Failable{TRight}"/> representing the result of <paramref name="function"/>. </returns>
        public static implicit operator Failable<TRight>(Func<TRight> function)
        {
            return new Failable<TRight>(function);
        }

        /// <summary>
        /// The converts a value into a <see cref="Failable{TRight}"/>.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The <see cref="Failable{TRight}"/> representing the value <paramref name="value"/> </returns>
        public static implicit operator Failable<TRight>(TRight value)
        {
            return new Failable<TRight>(value);
        }

        /// <summary>
        /// Converts a <see cref="Failable{TRight}"/> into its value.
        /// </summary>
        /// <param name="failable"> The <see cref="Failable{TRight}"/>. </param>
        /// <returns> The value. </returns>
        public static implicit operator TRight(Failable<TRight> failable)
        {
            failable.Resolve();
            return failable.Value;
        }

        /// <summary>
        /// Converts this type into an <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        /// <param name="failable"> The <see cref="Failable{TRight}"/> to convert. </param>
        /// <returns>
        /// </returns>
        public static implicit operator Either<Failable<TRight>, Exception>(Failable<TRight> failable)
        {
            failable.Resolve();
            return failable.Failed
                       ? new Either<Failable<TRight>, Exception>(failable.exception)
                       : new Either<Failable<TRight>, Exception>(failable.value);
        }

        /// <summary>
        /// Converts this instance to an <see cref="Either{TLeft,TRight}"/>.
        /// </summary>
        /// <returns> The <see cref="Either{TLeft,TRight}"/>. </returns>
        public Either<Failable<TRight>, Exception> ToEither()
        {
            this.Resolve();
            return this.Failed
                    ? new Either<Failable<TRight>, Exception>(this.exception)
                    : new Either<Failable<TRight>, Exception>(this.value);
        }

        /// <summary>
        /// Resolves the value from the function (if it needs to be resolved).
        /// </summary>
        private void Resolve()
        {
            if (!this.valueIsFunc)
            {
                return;
            }
            
            try
            {
                this.value = this.valueFunc();
                this.valueIsFunc = false;
            }
            catch (Exception ex)
            {
                this.value = default(TRight);
                this.exception = ex;
            }
        }
    }
}
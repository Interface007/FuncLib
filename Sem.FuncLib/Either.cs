// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Either.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the Either type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;

    /// <summary>
    /// This structure holds a value that is either of type <typeparamref name="TLeft"/> or <typeparamref name="TRight"/>.
    /// You can cast from and to both types.
    /// </summary>
    /// <typeparam name="TLeft"> One of the possible types. </typeparam>
    /// <typeparam name="TRight"> The other of the possible types. </typeparam>
    [DebuggerDisplay("{Value}")]
    public struct Either<TLeft, TRight> : IEquatable<Either<TLeft, TRight>>, IEither
    {
        /// <summary>
        /// Holds the instance of the value when the type is <see cref="TLeft"/>.
        /// </summary>
        private readonly TLeft instance1;

        /// <summary>
        /// Holds the instance of the value when the type is <see cref="TRight"/>.
        /// </summary>
        private readonly TRight instance2;

        /// <summary>
        /// The information which of the types has been selected.
        /// </summary>
        private readonly int typeNumber;

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TLeft,TRight}"/> struct. 
        /// </summary>
        /// <param name="instance">
        /// The instance of the value. 
        /// </param>
        public Either(object instance)
            : this(instance, typeof(object))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Either{TLeft,TRight}"/> struct. 
        /// </summary>
        /// <param name="instance">
        /// The instance of the result (either of type <typeparamref name="TLeft"/> or <typeparamref name="TRight"/>). 
        /// </param>
        /// <param name="type">
        /// The type of the object (useful when <paramref name="instance"/> is NULL). 
        /// </param>
        public Either(object instance, Type type)
        {
            if (type == typeof(TLeft) || instance is TLeft)
            {
                this.instance1 = (TLeft)instance;
                this.typeNumber = 1;
                this.instance2 = default(TRight);
                return;
            }

            if (type == typeof(TRight) || instance is TRight)
            {
                this.instance2 = (TRight)instance;
                this.typeNumber = 2;
                this.instance1 = default(TLeft);
                return;
            }

            this.instance1 = default(TLeft);
            this.instance2 = default(TRight);
            this.typeNumber = 0;
        }

        /// <summary>
        /// Gets the value.
        /// </summary>
        public object Value
        {
            get
            {
                return this.typeNumber == 1
                           ? (object)this.instance1
                           : this.instance2;
            }
        }

        /// <summary>
        /// The implicit cast from an <see cref="Either{TOne,TTwo}"/> to its underlying value type.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The value of the instance stored for object type <typeparamref name="TLeft"/> </returns>
        public static implicit operator TLeft(Either<TLeft, TRight> value)
        {
            if (value.typeNumber != 1)
            {
                throw new KeyNotFoundException(string.Format("There is no value of type {0} in this wrapper class.", typeof(TLeft).Name));
            }

            return value.instance1;
        }

        /// <summary>
        /// The implicit cast from an <see cref="Either{TOne,TTwo}"/> to its underlying value type.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> The value of the instance stored for object type <typeparamref name="TLeft"/> </returns>
        public static implicit operator TRight(Either<TLeft, TRight> value)
        {
            if (value.typeNumber != 2)
            {
                throw new KeyNotFoundException(string.Format("There is no value of type {0} in this wrapper class.", typeof(TRight).Name));
            }

            return value.instance2;
        }

        /// <summary>
        /// The implicit cast from a value of type <typeparamref name="TLeft"/> to the encapsulated value <see cref="Either{TOne,TTwo}"/>.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> A new instance of <see cref="Either{TOne,TTwo}"/> with the value set to<paramref name="value"/> </returns>
        public static implicit operator Either<TLeft, TRight>(TLeft value)
        {
            return new Either<TLeft, TRight>(value, typeof(TLeft));
        }

        /// <summary>
        /// The implicit cast from a value of type <typeparamref name="TRight"/> to the encapsulated value <see cref="Either{TOne,TTwo}"/>.
        /// </summary>
        /// <param name="value"> The value. </param>
        /// <returns> A new instance of <see cref="Either{TOne,TTwo}"/> with the value set to<paramref name="value"/> </returns>
        public static implicit operator Either<TLeft, TRight>(TRight value)
        {
            return new Either<TLeft, TRight>(value, typeof(TRight));
        }

        /// <summary>
        /// The implicit that simply inverses the order of the type parameters of this type.
        /// </summary>
        /// <param name="value"> The value to be converted. </param>
        /// <returns> A new instance of <see cref="Either{TOne,TTwo}"/> </returns>
        public static implicit operator Either<TLeft, TRight>(Either<TRight, TLeft> value)
        {
            return new Either<TLeft, TRight>(value.Value, value.Is<TLeft>() ? typeof(TLeft) : typeof(TRight));
        }

        /// <summary>
        /// Compares two instances of <see cref="Either{TOne,TTwo}"/> for equality of their <see cref="Value"/>.
        /// </summary>
        /// <param name="left"> The left instance of the comparison. </param>
        /// <param name="right"> The right instance of the comparison. </param>
        /// <returns> A value indicating whether the two instances contain equal values. </returns>
        public static bool operator ==(Either<TLeft, TRight> left, Either<TLeft, TRight> right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Compares two instances of <see cref="Either{TOne,TTwo}"/> for non-equality of their <see cref="Value"/>.
        /// </summary>
        /// <param name="left"> The left instance of the comparison. </param>
        /// <param name="right"> The right instance of the comparison. </param>
        /// <returns> A value indicating whether the two instances contain non-equal values. </returns>
        public static bool operator !=(Either<TLeft, TRight> left, Either<TLeft, TRight> right)
        {
            return !left.Equals(right);
        }

        /// <summary>
        /// Compares this instance of <see cref="Either{TOne,TTwo}"/> for equality of its <see cref="Value"/> with the <see cref="Value"/> of <paramref name="other"/>.
        /// </summary>
        /// <param name="other"> The instance to compare with this instance. </param>
        /// <returns> A value indicating whether the two instances contain equal values. </returns>
        public bool Equals(Either<TLeft, TRight> other)
        {
            return EqualityComparer<TLeft>.Default.Equals(this.instance1, other.instance1)
                && EqualityComparer<TRight>.Default.Equals(this.instance2, other.instance2);
        }

        /// <summary>
        /// Returns a value indicating whether the value is of type <typeparamref name="TTest"/>.
        /// </summary>
        /// <typeparam name="TTest"> The type to test for. </typeparam>
        /// <returns> A value indicating whether the current value is of type <typeparamref name="TTest"/>. </returns>
        public bool Is<TTest>()
        {
            return this.Value is TTest
                   || (this.typeNumber == 1 && typeof(TLeft) == typeof(TTest))
                   || (this.typeNumber == 2 && typeof(TRight) == typeof(TTest));
        }

        /// <summary>
        /// Compares this instance of <see cref="Either{TOne,TTwo}"/> for equality of its <see cref="Value"/> with the <see cref="Value"/> of <paramref name="obj"/>.
        /// </summary>
        /// <param name="obj"> The instance to compare with this instance. </param>
        /// <returns> A value indicating whether the two instances contain equal values. </returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return (obj is Either<TLeft, TRight> && this.Equals((Either<TLeft, TRight>)obj))
                || (obj is Either<TRight, TLeft> && this.Equals((Either<TRight, TLeft>)obj));
        }

        /// <summary>
        /// Returns the hash code for this instance.
        /// </summary>
        /// <returns> A 32-bit signed integer that is the hash code for this instance. </returns>
        /// <filterpriority>2</filterpriority>
        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TLeft>.Default.GetHashCode(this.instance1) * 397)
                      ^ EqualityComparer<TRight>.Default.GetHashCode(this.instance2);
            }
        }

        /// <summary>
        /// This invokes an <see cref="Action{T1}"/> when this instance can be casted into
        /// the type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to be tested for.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>This instance.</returns>
        public IEither WhenIs(Type type, Action<object> action)
        {
            if (this.CompareTypes(type, typeof(TLeft), this.instance1, 1))
            {
                action(this.instance1);
            }

            if (this.CompareTypes(type, typeof(TRight), this.instance2, 2))
            {
                action(this.instance2);
            }

            return this;
        }

        /// <summary>
        /// This invokes an <see cref="Action{T1}"/> when this instance can be casted into
        /// the type <typeparamref name="TType"/>.
        /// Since we need a similar functionality but with a stronger output type, we need to implement this interface
        /// explicitly - that way we can have the public function <see cref="WhenIs{T1}"/> with a return type
        /// of <see cref="Either{TOne,TTwo}"/> instead of <see cref="IEither"/>.
        /// </summary>
        /// <typeparam name="TType">The type to be tested for.</typeparam>
        /// <param name="action">The action to invoke.</param>
        /// <returns>This instance.</returns>
        IEither IEither.WhenIs<TType>(Action<TType> action)
        {
            this.WhenIs(typeof(TType), x => action((TType)x));
            return this;
        }

        /// <summary>
        /// This invokes an <see cref="Action{T1}"/> when this instance can be casted into
        /// the type <typeparamref name="TType"/>.
        /// </summary>
        /// <typeparam name="TType">The type to be tested for.</typeparam>
        /// <param name="action">The action to invoke.</param>
        /// <returns>This instance.</returns>
        public Either<TLeft, TRight> WhenIs<TType>(Action<TType> action)
        {
            this.WhenIs(typeof(TType), x => action((TType)x));
            return this;
        }

        /// <summary>
        /// Compares the type of an object to the current type of this instance.
        /// </summary>
        /// <param name="type1"> The type #1. </param>
        /// <param name="type2"> The type #2. </param>
        /// <param name="value"> The value that contains the type to compare to. </param>
        /// <param name="typeNum"> The type number to compare to. </param>
        /// <typeparam name="TType"> The concrete type to compare to. </typeparam>
        /// <returns> The value indicating whether the type of the value is equivalent to the type of this instance. </returns>
        private bool CompareTypes<TType>(Type type1, Type type2, TType value, int typeNum)
        {
            return this.typeNumber == typeNum 
                && (type2 == type1 || type1.IsAssignableFrom(type2) || ((object)value != null && value.GetType() == type1));
        }
    }
}
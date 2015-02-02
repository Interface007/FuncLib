// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IEither.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the IEither type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib
{
    using System;

    /// <summary>
    /// Declares an interface to provide basic functionality .
    /// </summary>
    public interface IEither
    {
        /// <summary>
        /// This invokes an <see cref="Action{T1}"/> when this instance can be casted into
        /// the type <paramref name="type"/>.
        /// </summary>
        /// <param name="type">The type to be tested for.</param>
        /// <param name="action">The action to invoke.</param>
        /// <returns>This instance.</returns>
        IEither WhenIs(Type type, Action<object> action);

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
        IEither WhenIs<TType>(Action<TType> action);
    }
}
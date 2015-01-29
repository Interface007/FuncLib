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

    public interface IEither
    {
        IEither WhenIs(Type type, Action<object> action);

        IEither WhenIs<TType>(Action<TType> action);
    }
}
// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Extensions.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the Extensions type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// The extensions for data handling with <see cref="Either{TOne,TTwo}"/>.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Executes an action <paramref name="action"/> for each element that is of type <typeparamref name="TType"/>.
        /// </summary>
        /// <param name="eitherSource"> The source of elements. </param>
        /// <param name="action"> The action to be executed. </param>
        /// <typeparam name="TEither"> The type of elements. </typeparam>
        /// <typeparam name="TType"> The type of elements that should be called with <paramref name="action"/>. </typeparam>
        /// <returns> An <see cref="IEnumerable{T}"/> that is a selector of the original <paramref name="eitherSource"/>. </returns>
        public static IEnumerable<TEither> WhenIs<TEither, TType>(this IEnumerable<TEither> eitherSource, Action<TType> action)
            where TEither : IEither
        {
            return eitherSource.Select(x => (TEither)x.WhenIs(action));
        }

        /// <summary>
        /// Executes the <see cref="Action{T}"/> for each item of <paramref name="eitherSource"/> that is of the matching type <typeparamref name="TLeft"/>.
        /// </summary>
        /// <param name="eitherSource"> The source of the <see cref="Either{TLeft,TRight}"/> elements. </param>
        /// <param name="action"> The action that determines the elements to use. </param>
        /// <typeparam name="TLeft"> The left type parameter of the <see cref="Either{TLeft,TRight}"/> </typeparam>
        /// <typeparam name="TRight"> The right type parameter of the <see cref="Either{TLeft,TRight}"/> </typeparam>
        /// <returns> The elements of <paramref name="eitherSource"/>. </returns>
        public static IEnumerable<Either<TLeft, TRight>> WhenIs<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> eitherSource, Action<TLeft> action)
        {
            return eitherSource.Select(x => x.WhenIs(action));
        }

        /// <summary>
        /// Executes the <see cref="Action{T}"/> for each item of <paramref name="eitherSource"/> that is of the matching type <typeparamref name="TLeft"/>.
        /// </summary>
        /// <param name="eitherSource"> The source of the <see cref="Either{TLeft,TRight}"/> elements. </param>
        /// <param name="action"> The action that determines the elements to use. </param>
        /// <typeparam name="TLeft"> The left type parameter of the <see cref="Either{TLeft,TRight}"/> </typeparam>
        /// <typeparam name="TRight"> The right type parameter of the <see cref="Either{TLeft,TRight}"/> </typeparam>
        /// <returns> The elements of <paramref name="eitherSource"/>. </returns>
        public static IEnumerable<Either<TLeft, TRight>> WhenIs<TLeft, TRight>(this IEnumerable<Either<TLeft, TRight>> eitherSource, Action<TRight> action)
        {
            return eitherSource.Select(x => x.WhenIs(action));
        }

        /// <summary>
        /// Calls the function <paramref name="func"/> and returns either the result or the exception.
        /// </summary>
        /// <param name="func"> The function to be called. </param>
        /// <typeparam name="TValue"> The value of the functions parameter. </typeparam>
        /// <typeparam name="TRight"> The type of the expected return value of the function. </typeparam>
        /// <typeparam name="TException"> The type of an expected exception. </typeparam>
        /// <returns> Either the result or the exception. </returns>
        public static Func<TValue, Either<TException, TRight>> Try<TValue, TRight, TException>(this Func<TValue, TRight> func)
            where TException : Exception
        {
            Func<TValue, Either<TException, TRight>> result =
                x =>
                {
                    try
                    {
                        return new Either<TException, TRight>(func(x));
                    }
                    catch (TException ex)
                    {
                        return new Either<TException, TRight>(ex);
                    }
                };

            return result;
        }

        /// <summary>
        /// Calls the function <paramref name="func"/> for each element of <paramref name="source"/> and 
        /// returns either the result or the exception.
        /// </summary>
        /// <param name="source"> The elements to use for the function call. </param>
        /// <param name="func"> The function to be called. </param>
        /// <typeparam name="TValue"> The value of the functions parameter. </typeparam>
        /// <typeparam name="TRight"> The type of the expected return value of the function. </typeparam>
        /// <returns> Either the result or the exception. </returns>
        public static IEnumerable<Either<Exception, TRight>> Try<TValue, TRight>(this IEnumerable<TValue> source, Func<TValue, TRight> func)
        {
            return source
                .Select(x =>
                    {
                        try
                        {
                            return new Either<Exception, TRight>(func(x));
                        }
                        catch (Exception ex)
                        {
                            return new Either<Exception, TRight>(ex);
                        }
                    });
        }

        /// <summary>
        /// Calls the function <paramref name="func"/> with each <see cref="FunctionWrapper{TValue,TRight}"/> of 
        /// <paramref name="source"/> and returns either the result or the exception.
        /// </summary>
        /// <param name="source"> The source of the <see cref="FunctionWrapper{TValue,TRight}"/>. </param>
        /// <param name="func"> The function to be called. </param>
        /// <typeparam name="TValue"> The value of the functions parameter. </typeparam>
        /// <typeparam name="TRight"> The type of the expected return value of the function. </typeparam>
        /// <returns> Either the result or the exception. </returns>
        public static IEnumerable<Either<Exception, TRight>> Try<TValue, TRight>(this IEnumerable<FunctionWrapper<TValue, TRight>> source, Func<TValue, TRight> func)
        {
            return source
                .Select(x =>
                {
                    try
                    {
                        return new Either<Exception, TRight>(x.Execute(func));
                    }
                    catch (Exception ex)
                    {
                        return new Either<Exception, TRight>(ex);
                    }
                });
        }

        public static IEnumerable<Either<TLeft, TRight>> Try<TValue, TLeft, TRight>(this IEnumerable<TValue> source, Func<TValue, TRight> func)
            where TLeft : Exception
        {
            return source
                .Select(x =>
                    {
                        try
                        {
                            return new Either<TLeft, TRight>(func(x));
                        }
                        catch (TLeft ex)
                        {
                            return new Either<TLeft, TRight>(ex);
                        }
                    });
        }

        public static IEnumerable<FunctionWrapper<TValue, TRight>> WithAction<TValue, TRight>(
            this IEnumerable<TValue> source,
            Func<Func<TValue, TRight>, TValue, TRight> func)
        {
            return source.Select(x => new FunctionWrapper<TValue, TRight>(func, x));
        }

        public static IEnumerable<FunctionWrapper<TValue, TRight>> WithActions<TValue, TRight>(
            this IEnumerable<TValue> source,
            IEnumerable<Func<Func<TValue, TRight>, TValue, TRight>> funcs)
        {
            var functions = funcs as Func<Func<TValue, TRight>, TValue, TRight>[] ?? funcs.ToArray();
            var functionWrappers = source.WithAction(functions[0]);
            return functionWrappers.WithActions(functions.Skip(1));
        }

        public static IEnumerable<FunctionWrapper<TValue, TRight>> WithActions<TValue, TRight>(
            this IEnumerable<FunctionWrapper<TValue, TRight>> source,
            IEnumerable<Func<Func<TValue, TRight>, TValue, TRight>> funcs)
        {
            var functions = funcs as Func<Func<TValue, TRight>, TValue, TRight>[] ?? funcs.ToArray();
            if (functions.Length == 0)
            {
                return source;
            }

            var functionWrappers = source.WithAction(functions[0]);
            return functionWrappers.WithActions(functions.Skip(1));
        }

        public static IEnumerable<FunctionWrapper<TValue, TRight>> WithAction<TValue, TRight>(
            this IEnumerable<FunctionWrapper<TValue, TRight>> source,
            Func<Func<TValue, TRight>, TValue, TRight> func)
        {
            return source.Select(x =>
                {
                    var xFunc = x.Func;
                    var xValue = x.Value;
                    return new FunctionWrapper<TValue, TRight>(
                        (f, v) => xFunc(v1 => func(f, v1), v),
                        xValue);
                });
        }
    }
}

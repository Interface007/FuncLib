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

        /// <summary>
        /// Calls the function <paramref name="func"/> with each <see cref="FunctionWrapper{TValue,TRight}"/> of 
        /// <paramref name="source"/> and returns either the result or the exception.
        /// </summary>
        /// <param name="source"> The source of the <see cref="FunctionWrapper{TValue,TRight}"/>. </param>
        /// <param name="func"> The function to be called. </param>
        /// <typeparam name="TValue"> The value of the functions parameter. </typeparam>
        /// <typeparam name="TLeft"> The left type of the expected <see cref="Either{TLeft,TRight}"/> return value of the function. </typeparam>
        /// <typeparam name="TRight"> The right type of the expected <see cref="Either{TLeft,TRight}"/> return value of the function. </typeparam>
        /// <returns> Either the result or the exception. </returns>
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

        /// <summary>
        /// Creates a <see cref="FunctionWrapper{TValue,TRight}"/> for each element of <paramref name="source"/>, that can be 
        /// called later (or can be combine with additional functions).
        /// </summary>
        /// <param name="source"> The source of the elements to wrap. </param>
        /// <param name="func"> The function to be called. </param>
        /// <typeparam name="TValue"> The value of the functions parameter. </typeparam>
        /// <typeparam name="TRight"> The type of the <see cref="FunctionWrapper{TValue,TRight}"/> return value of the function. </typeparam>
        /// <returns> A wrapper for invoking a function with a value. </returns>
        public static IEnumerable<FunctionWrapper<TValue, TRight>> WithAction<TValue, TRight>(
            this IEnumerable<TValue> source,
            Func<Func<TValue, TRight>, TValue, TRight> func)
        {
            return source.Select(x => new FunctionWrapper<TValue, TRight>(func, x));
        }

        /// <summary>
        /// Creates a new <see cref="FunctionWrapper{TValue,TRight}"/> for each element of <paramref name="source"/>, that is a 
        /// combination of the current function and the function <paramref name="func"/>.
        /// </summary>
        /// <param name="source"> The source of the elements to wrap. </param>
        /// <param name="func"> The function to be called. </param>
        /// <typeparam name="TValue"> The value of the functions parameter. </typeparam>
        /// <typeparam name="TRight"> The type of the <see cref="FunctionWrapper{TValue,TRight}"/> return value of the function. </typeparam>
        /// <returns> A wrapper for invoking a function with a value. </returns>
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

        /// <summary>
        /// Creates a <see cref="FunctionWrapper{TValue,TRight}"/> for each element of <paramref name="source"/>, that can be 
        /// called later (or can be combine with additional functions). This wrapper will contain a chain of all functions 
        /// from the parameter <paramref name="funcs"/>.
        /// </summary>
        /// <param name="source"> The source of the elements to wrap. </param>
        /// <param name="funcs"> The functions to be called. </param>
        /// <typeparam name="TValue"> The value of the functions parameter. </typeparam>
        /// <typeparam name="TRight"> The type of the <see cref="FunctionWrapper{TValue,TRight}"/> return value of the function. </typeparam>
        /// <returns> A wrapper for invoking a function with a value. </returns>
        public static IEnumerable<FunctionWrapper<TValue, TRight>> WithActions<TValue, TRight>(
            this IEnumerable<TValue> source,
            IEnumerable<Func<Func<TValue, TRight>, TValue, TRight>> funcs)
        {
            var functions = funcs as Func<Func<TValue, TRight>, TValue, TRight>[] ?? funcs.ToArray();
            var functionWrappers = source.WithAction(functions[0]);
            return functionWrappers.WithActions(functions.Skip(1));
        }

        /// <summary>
        /// Combines each <see cref="FunctionWrapper{TValue,TRight}"/> of <paramref name="source"/> with the new functions from 
        /// <paramref name="funcs"/>. The new wrapper will contain a chain of all functions from the parameter <paramref name="funcs"/>.
        /// </summary>
        /// <param name="source"> The source of the elements to wrap. </param>
        /// <param name="funcs"> The functions to be called. </param>
        /// <typeparam name="TValue"> The value of the functions parameter. </typeparam>
        /// <typeparam name="TRight"> The type of the <see cref="FunctionWrapper{TValue,TRight}"/> return value of the function. </typeparam>
        /// <returns> A wrapper for invoking a function with a value. </returns>
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
    }
}

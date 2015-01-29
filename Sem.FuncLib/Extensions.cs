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

        public static Func<TValue, Either<TResult1, TResult2>> Try<TValue, TResult1, TResult2>(this Func<TValue, TResult1> func)
            where TResult2 : Exception
        {
            Func<TValue, Either<TResult1, TResult2>> result =
                x =>
                {
                    try
                    {
                        return new Either<TResult1, TResult2>(func(x));
                    }
                    catch (TResult2 ex)
                    {
                        return new Either<TResult1, TResult2>(ex);
                    }
                };

            return result;
        }

        public static IEnumerable<Either<Exception, TRight>> Try<TValue, TRight>(this IEnumerable<ActionWrapper<TValue, TRight>> source, Func<TValue, TRight> func)
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

        ////public static IEnumerable<Either<Exception, TRight>> Try<TValue, TRight>(this IEnumerable<TValue> source, Func<TValue, TRight> func)
        ////{
        ////    return source
        ////        .Select(x =>
        ////        {
        ////            try
        ////            {
        ////                return new Either<Exception, TRight>(func(x));
        ////            }
        ////            catch (Exception ex)
        ////            {
        ////                return new Either<Exception, TRight>(ex);
        ////            }
        ////        });
        ////}

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

        public static IEnumerable<ActionWrapper<TValue, TRight>> WithAction<TValue, TRight>(
            this IEnumerable<TValue> source,
            Func<Func<TValue, TRight>, TValue, TRight> func)
        {
            return source.Select(x => new ActionWrapper<TValue, TRight>(func, x));
        }

        public static IEnumerable<ActionWrapper<TValue, TRight>> WithAction<TValue, TRight>(
            this IEnumerable<ActionWrapper<TValue, TRight>> source,
            Func<Func<TValue, TRight>, TValue, TRight> func)
        {
            return source.Select(x =>
                {
                    var xFunc = x.Func;
                    var xValue = x.Value;
                    return new ActionWrapper<TValue, TRight>(
                        (f, v) => xFunc(v1 => func(f, v1), v),
                        xValue);
                });
        }
    }
}

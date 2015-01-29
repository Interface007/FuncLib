// --------------------------------------------------------------------------------------------------------------------
// <copyright file="Of.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   A marker class for a type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib
{
    /// <summary>
    /// A marker class for a type.
    /// </summary>
    public class Of
    {
        public static TType Type<TType>()
        {
            return default(TType);
        }
    }
}
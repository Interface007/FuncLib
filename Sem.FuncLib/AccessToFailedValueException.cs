// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AccessToFailedValueException.cs" company="Sven Erik Matzen">
//   (c) Sven Erik Matzen
// </copyright>
// <summary>
//   Defines the AccessToFailedValueException type.
// </summary>
// --------------------------------------------------------------------------------------------------------------------

namespace Sem.FuncLib
{
    using System;

    /// <summary>
    /// The code did try to access a failed value.
    /// </summary>
    [Serializable]
    public class AccessToFailedValueException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AccessToFailedValueException"/> class.
        /// </summary>
        /// <param name="message"> The exception message describing the error. </param>
        /// <param name="innerException"> The inner exception causing the failure. </param>
        public AccessToFailedValueException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
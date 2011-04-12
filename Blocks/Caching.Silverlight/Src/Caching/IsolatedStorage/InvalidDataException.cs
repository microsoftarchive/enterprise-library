using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// Represents an error caused by invalid data.
    /// </summary>
    public class InvalidDataException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException"/> class.
        /// </summary>
        public InvalidDataException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public InvalidDataException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or 
        /// <see langword="null"/> if no inner exception is specified.</param>
        public InvalidDataException(string message, Exception inner) : base(message, inner) { }
    }
}

using System;
using System.IO;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Utility
{
    /// <summary>
    /// Represents an exception allocating storage.
    /// </summary>
    public class AllocationException : IOException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationException"/> class.
        /// </summary>
        public AllocationException() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public AllocationException(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="AllocationException"/> class.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or 
        /// <see langword="null"/> if no inner exception is specified.</param>
        public AllocationException(string message, Exception inner) : base(message, inner) { }
    }
}

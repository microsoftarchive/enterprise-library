//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// An exception that occurred during the exception handling process.
    /// </summary>
    [Serializable]
    public class ExceptionHandlingException : Exception
    {
        /// <summary>
        /// Initializes with defaults.
        /// </summary>
        public ExceptionHandlingException() : base()
        {
        }

        /// <summary>
        /// Initializes with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public ExceptionHandlingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes with a specified error 
        /// message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.
        /// </param>
        /// <param name="innerException">
        /// <para>The exception that is the cause of the current exception. If the innerException parameter is not a null reference, the current exception is raised in a catch block that handles the inner exception.</para>
        /// </param>
        public ExceptionHandlingException(string message, Exception innerException) : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes with serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or destination.
        /// </param>
        protected ExceptionHandlingException(SerializationInfo info, StreamingContext context) :
            base(info, context)
        {
        }
    }
}
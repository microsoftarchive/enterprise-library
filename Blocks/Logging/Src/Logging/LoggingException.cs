//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Logging block exception.
    /// </summary>
    [Serializable]
    [ComVisible(false)]

    public class LoggingException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the LoggingException class.
        /// </summary>
        public LoggingException() : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the LoggingException class 
        /// with a specified error message.
        /// </summary>
        /// <param name="message">A message that describes the error.</param>
        public LoggingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LoggingException class with a 
        /// specified error message and a reference to the inner exception that is 
        /// the cause of this exception.
        /// </summary>
        /// <param name="message">The error message that explains the reason for 
        /// the exception.
        /// </param>
        /// <param name="exception">The exception that is the cause of the current 
		/// exception.  If the innerException parameter is not a <see langword="null"/> reference, 
        /// the current exception is raised in a catch block that handles the inner 
        /// exception.
        /// </param>
        public LoggingException(string message, Exception exception) : base(message, exception)
        {
        }

        /// <summary>
        /// Initializes a new instance of the LoggingException class with 
        /// serialized data.
        /// </summary>
        /// <param name="info">The object that holds the serialized object data.</param>
        /// <param name="context">The contextual information about the source or 
        /// destination.
        /// </param>
        protected LoggingException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}
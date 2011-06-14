//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Indicates errors when retrieving configuration.
    /// </summary>
    public class ConfigurationErrorsException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationErrorsException"/> class.
        /// </summary>
        public ConfigurationErrorsException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationErrorsException"/> class with a message.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        public ConfigurationErrorsException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationErrorsException"/> class with a message and
        /// an inner exception.
        /// </summary>
        /// <param name="message">A message describing the error.</param>
        /// <param name="inner">The exception that is the cause of the current exception, or 
        /// <see langword="null"/> if no inner exception is specified.</param>
        public ConfigurationErrorsException(string message, Exception inner)
            : base(message, inner)
        { }
    }
}

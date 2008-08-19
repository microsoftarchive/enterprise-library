//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation
{
    /// <summary>
	/// Represents the WMI event fired when an exception is thrown while performing a validation operation.
	/// </summary>
    public class ValidationExceptionEvent : ValidationCalledEvent
    {
        string exceptionMessage;

        /// <summary>
		/// Initializes a new instance of the <see cref="ValidationExceptionEvent"/> class.
        /// </summary>
		/// <param name="typeBeingValidated">The name of the type being validated.</param>
		/// <param name="exceptionMessage">The message of the thrown exception.</param>
        public ValidationExceptionEvent(string typeBeingValidated, string exceptionMessage)
            : base(typeBeingValidated)
        {
            this.exceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// Gets the message of the thrown <see cref="Exception"/>.
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }
    }
}

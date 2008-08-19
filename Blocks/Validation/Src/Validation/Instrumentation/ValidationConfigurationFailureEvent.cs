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
    /// Represents the WMI event fired when an error in the configuration for the validation block is detected.
    /// </summary>
    public class ValidationConfigurationFailureEvent : ValidationEvent
    {
        private string exceptionMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationConfigurationFailureEvent"/> class.
        /// </summary>
        /// <param name="exceptionMessage">The message that represents the exception thrown when the configuration error was detected.</param>
        public ValidationConfigurationFailureEvent(string exceptionMessage)
        {
            this.exceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// Gets the message that represents the exception thrown when the configuration error was detected.
        /// </summary>
        public string ExceptionMessage
        {
            get { return this.exceptionMessage; }
        }
    }
}

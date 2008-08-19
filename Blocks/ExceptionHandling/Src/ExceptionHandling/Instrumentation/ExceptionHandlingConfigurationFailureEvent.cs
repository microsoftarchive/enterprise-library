//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation
{
    /// <summary>
    /// Represents the WMI event fired when an error in the configuration for the exception handling block is detected.
    /// </summary>
    public class ExceptionHandlingConfigurationFailureEvent : ExceptionHandlingEvent
    {
        private string policyName;
        private string exceptionMessage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingConfigurationFailureEvent"/> class.
        /// </summary>
        /// <param name="policyName">The name of the policy which contained configuration errors.</param>
        /// <param name="exceptionMessage">The message that represents the exception thrown when the configuration error was detected.</param>
        public ExceptionHandlingConfigurationFailureEvent(string policyName, string exceptionMessage)
        {
            this.policyName = policyName;
            this.exceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// Gets the name of the policy which contained configuration errors.
        /// </summary>
        public string PolicyName
        {
            get { return policyName; }
        }

        /// <summary>
        /// Gets the message that represents the exception thrown when the configuration error was detected.
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }
    }
}

//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Instrumentation
{
    /// <summary>
    /// Represents the WMI event fired when an error in the configuration for the security block is detected.
    /// </summary>
    public class SecurityConfigurationFailureEvent : SecurityEvent
    {
        private string exceptionMessage;
        
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityConfigurationFailureEvent"/> class.
        /// </summary>
		/// <param name="instanceName">The name of the provider for which an configuration failure is detected.</param>
        /// <param name="exceptionMessage">The message that represents the exception thrown when the configuration error was detected.</param>
		public SecurityConfigurationFailureEvent(string instanceName, string exceptionMessage)
			: base(instanceName)
        {
            this.exceptionMessage = exceptionMessage;
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

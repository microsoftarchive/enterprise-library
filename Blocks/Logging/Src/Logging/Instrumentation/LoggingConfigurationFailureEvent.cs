//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation
{
    /// <summary>
	/// Represents the WMI event fired when an error in the configuration for the logging block is detected.
	/// </summary>
    public class LoggingConfigurationFailureEvent : LoggingEvent
    {
        private string exceptionMessage;

        /// <summary>
		/// Initializes a new instance of the <see cref="LoggingConfigurationFailureEvent"/> class.
        /// </summary>
		/// <param name="exceptionMessage">The message that represents the exception thrown when the configuration error was detected.</param>
		public LoggingConfigurationFailureEvent(string exceptionMessage)
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

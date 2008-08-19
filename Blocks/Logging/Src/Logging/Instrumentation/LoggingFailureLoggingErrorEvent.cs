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
    /// Represents the WMI event fired when an error occurs that could not be logged to the errors special log source.
    /// </summary>
    public class LoggingFailureLoggingErrorEvent : LoggingEvent
    {
		private string errorMessage;
        private string exceptionMessage;

        /// <summary>
		/// Initializes a new instance of the <see cref="LoggingFailureLoggingErrorEvent"/> class.
        /// </summary>
		/// <param name="errorMessage">The message that describes the failure.</param>
        /// <param name="exceptionMessage">The message that represents the exception causing the failure.</param>
        public LoggingFailureLoggingErrorEvent(string errorMessage, string exceptionMessage)
        {
			this.errorMessage = errorMessage;
            this.exceptionMessage = exceptionMessage;
        }

		/// <summary>
		/// Gets the message that describes the failure.
		/// </summary>
		public string ErrorMessage
		{
			get { return errorMessage; }
		}
		
		/// <summary>
		/// Gets the message that represents the exception causing the failure.
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }
    }
}

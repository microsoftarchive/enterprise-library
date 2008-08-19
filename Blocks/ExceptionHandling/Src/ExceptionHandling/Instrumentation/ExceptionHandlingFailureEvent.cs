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
    /// Represents the WMI event fired when an error ocurred in the exception handling block.
    /// </summary>
    public class ExceptionHandlingFailureEvent : ExceptionHandlingEvent
    {
        string exceptionMessage;
		string instanceName;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionHandlingFailureEvent"/> class.
        /// </summary>
        /// <param name="instanceName">The name of the exception policy this failure ocurred in.</param>
        /// <param name="exceptionMessage">The message that represents the exception causing the failure.</param>
        public ExceptionHandlingFailureEvent(string instanceName, string exceptionMessage)
        {
			this.instanceName = instanceName;
            this.exceptionMessage = exceptionMessage;
        }

        /// <summary>
        /// Gets the message that represents the exception causing the failure.
        /// </summary>
        public string ExceptionMessage
        {
            get { return exceptionMessage; }
        }

		/// <summary>
        /// Gets the name of the exception policy this failure ocurred in.
		/// </summary>
		public string InstanceName
		{
			get { return instanceName; }
		}
	}
}

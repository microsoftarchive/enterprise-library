//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation
{
	/// <summary>
	/// Represents the WMI event fired when an error in the configuration for the caching block is detected.
	/// </summary>
	public class CacheConfigurationFailureEvent : CacheEvent
	{
		private string exceptionMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="CacheConfigurationFailureEvent"/> class.
		/// </summary>
		/// <param name="instanceName">Name of the <see cref="CacheManager"/> instance the failure ocurred in.</param>
		/// <param name="exceptionMessage">The message that represents the exception thrown when the configuration error was detected.</param>
		public CacheConfigurationFailureEvent(string instanceName, string exceptionMessage)
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

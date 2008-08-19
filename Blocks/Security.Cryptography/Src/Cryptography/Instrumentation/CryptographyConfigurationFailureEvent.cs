//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
	/// <summary>
	/// Represents the WMI event fired when an error in the configuration for the crypto block is detected.
	/// </summary>
	public class CryptographyConfigurationFailureEvent : CryptographyEvent
	{
		private string exceptionMessage;

		/// <summary>
		/// Initializes a new instance of the <see cref="CryptographyConfigurationFailureEvent"/> class.
		/// </summary>
		/// <param name="instanceName">Name of the requested cryptographic provider instance.</param>
		/// <param name="exceptionMessage">The message that represents the exception thrown when the configuration error was detected.</param>
		public CryptographyConfigurationFailureEvent(string instanceName, string exceptionMessage)
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

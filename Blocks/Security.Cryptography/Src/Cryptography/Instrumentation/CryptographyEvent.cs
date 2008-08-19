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

using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation
{
	/// <summary>
	/// Base class for the Enterprise Library Cryptography Application Block's WMI events.
	/// </summary>
	public abstract class CryptographyEvent : BaseWmiEvent
	{
		private string instanceName;

		/// <summary>
		/// Initializes a new instance of the <see cref="CryptographyEvent"/> class.
		/// </summary>
		/// <param name="instanceName">name of the cryptographic provider instance this event applies to.</param>
		protected CryptographyEvent(string instanceName)
		{
			this.instanceName = instanceName;
		}

		/// <summary>
		/// Gets the name of the cryptographic provider instance this event applies to.
		/// </summary>
		public string InstanceName
		{
			get { return instanceName; }
		}

	}
}

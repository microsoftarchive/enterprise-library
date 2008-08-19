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
	/// Represents the WMI event fired when a mismatch is detected during a hash comparison.
	/// </summary>
	public class HashMismatchDetectedEvent : CryptographyEvent
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="HashMismatchDetectedEvent"/> class.
		/// </summary>
		/// <param name="instanceName">name of the cryptographic provider instance this event applies to.</param>
		public HashMismatchDetectedEvent(string instanceName)
			: base(instanceName)
		{
		}
	}
}

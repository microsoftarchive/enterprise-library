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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
	/// <summary>
	/// Configuration for a reference to named <see cref="TraceListenerData"/>.
	/// </summary>
	public class TraceListenerReferenceData : NamedConfigurationElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="TraceListenerReferenceData"/> class with default values.
		/// </summary>
		public TraceListenerReferenceData()
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="TraceListenerReferenceData"/> class with a name.
		/// </summary>
		/// <param name="name">Name for trace listener.</param>
		public TraceListenerReferenceData(string name)
			: base(name)
		{ }
	}
}

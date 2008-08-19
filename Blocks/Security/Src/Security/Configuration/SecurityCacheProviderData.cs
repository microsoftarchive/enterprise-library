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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration
{
	/// <summary>
	/// Configuration data for the Security Cache.
	/// </summary>
	public class SecurityCacheProviderData : NameTypeConfigurationElement
	{
		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="SecurityCacheProviderData"/> class.
		/// </summary>
		public SecurityCacheProviderData()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="SecurityCacheProviderData"/> class.
		/// </summary>
		/// <param name="name">The name of the element.</param>
		/// <param name="type">The <see cref="Type"/> that this element is the configuration for.</param>
		public SecurityCacheProviderData(string name, Type type)
			: base(name, type)
		{
		}
	}
}
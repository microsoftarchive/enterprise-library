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

using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information for an instance of a concrete subclass of
	/// <see cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheStorageData"/>.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.CacheStorageData"/>
	/// <seealso cref="NamedConfigurationSetting"/>
	/// <seealso cref="ConfigurationSetting"/>
	[ManagementEntity]
	public abstract partial class CacheStorageSetting : NamedConfigurationSetting
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="CacheStorageSetting"/> class.
		/// </summary>
		protected CacheStorageSetting(string name)
			: base(name)
		{
		}

		/// <summary>
		/// Applies configuration changes received by the WMI in runtime
		/// </summary>
		/// <param name="sourceElement"></param>
		/// <returns></returns>
		protected override bool SaveChanges(System.Configuration.ConfigurationElement sourceElement)
		{
			return false;
		}
	}
}
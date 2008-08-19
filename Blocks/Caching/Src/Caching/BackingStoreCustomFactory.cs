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
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build an instance of <see cref="IBackingStore"/> described by a <see cref="CacheStorageData"/> configuration object.
    /// </summary>
	public class BackingStoreCustomFactory : AssemblerBasedCustomFactory<IBackingStore, CacheStorageData>
	{
		/// <summary>
        /// This field supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public static BackingStoreCustomFactory Instance = new BackingStoreCustomFactory();

		/// <summary>
        /// Returns the configuration object that represents the named <see cref="IBackingStore"/> instance in the specified <paramref name="configurationSource"/>.
		/// </summary>
		/// <param name="name">The name of the required instance.</param>
		/// <param name="configurationSource">The configuration source where to look for the configuration object.</param>
		/// <returns>The configuration object that represents the instance with name <paramref name="name"/> in the logging 
		/// configuration section from <paramref name="configurationSource"/></returns>
		/// <exception cref="ConfigurationErrorsException"><paramref name="configurationSource"/> does not contain 
		/// caching settings, or the <paramref name="name"/> does not exist in the caching settings.</exception>
		protected override CacheStorageData GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			CachingConfigurationView view = new CachingConfigurationView(configurationSource);
			return view.GetCacheStorageData(name);
		}
	}
}

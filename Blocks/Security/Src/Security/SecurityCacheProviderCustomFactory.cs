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
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build an instance of <see cref="ISecurityCacheProvider"/> described by a <see cref="SecurityCacheProviderData"/> configuration object.
    /// </summary>
	public class SecurityCacheProviderCustomFactory : AssemblerBasedCustomFactory<ISecurityCacheProvider, SecurityCacheProviderData>
	{

        /// <summary>
        /// Returns the configuration object that represents the named <see cref="ISecurityCacheProvider"/> instance in the specified <paramref name="configurationSource"/>.
        /// </summary>
        /// <param name="name">The name of the required instance.</param>
        /// <param name="configurationSource">The configuration source where to look for the configuration object.</param>
        /// <returns>The configuration object that represents the instance with name <paramref name="name"/> in the security 
        /// configuration section from <paramref name="configurationSource"/></returns>
        /// <exception cref="ConfigurationErrorsException"><paramref name="configurationSource"/> does not contain 
        /// security cache settings, or the <paramref name="name"/> does not exist in the caching settings.</exception>
		protected override SecurityCacheProviderData GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			return new SecurityConfigurationView(configurationSource).GetSecurityCacheProviderData(name);
		}
	}
}

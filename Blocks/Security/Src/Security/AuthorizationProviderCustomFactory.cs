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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the general process to build an <see cref="IAuthorizationProvider"/> object given a concrete sublcass of <see cref="AuthorizationProviderData"/>.
    /// </summary>
    /// <remarks>
    /// This type leverages the generic implementation from AssemblerBasedObjectFactory.
    /// </remarks>
	public class AuthorizationProviderCustomFactory : AssemblerBasedCustomFactory<IAuthorizationProvider, AuthorizationProviderData>
	{
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Looks up a specified <see cref="AuthorizationProvider"/>'s configuration from the given <paramref name="configurationSource"/>. 
        /// </summary>
        /// <param name="name">The name of the <see cref="AuthorizationProvider"/> for which the configuration should be looked up.</param>
        /// <param name="configurationSource">The configuration source which should be used.</param>
        /// <returns>The configuration for the specified <see cref="AuthorizationProvider"/>.</returns>
        protected override AuthorizationProviderData GetConfiguration(string name, IConfigurationSource configurationSource)
		{
			return new SecurityConfigurationView(configurationSource).GetAuthorizationProviderData(name);
		}
	}
}

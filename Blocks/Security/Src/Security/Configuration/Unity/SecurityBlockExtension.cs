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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Unity
{
	/// <summary>
	/// Container extension to the policies required to create the Security Application Block's
	/// objects described in the configuration file.
	/// </summary>
	public class SecurityBlockExtension : EnterpriseLibraryBlockExtension
	{
		/// <summary>
		/// Adds the policies describing the Security Application Block's objects.
		/// </summary>
		protected override void Initialize()
		{
			SecuritySettings securitySettings = (SecuritySettings) ConfigurationSource.GetSection(SecuritySettings.SectionName);

			if (securitySettings != null)
			{
				CreateProvidersPolicies<IAuthorizationProvider, AuthorizationProviderData>(
					Context.Policies,
					securitySettings.DefaultAuthorizationProviderName,
					securitySettings.AuthorizationProviders,
					ConfigurationSource);

				CreateProvidersPolicies<ISecurityCacheProvider, SecurityCacheProviderData>(
					Context.Policies,
					securitySettings.DefaultSecurityCacheProviderName,
					securitySettings.SecurityCacheProviders,
					ConfigurationSource);
			}
		}
	}
}

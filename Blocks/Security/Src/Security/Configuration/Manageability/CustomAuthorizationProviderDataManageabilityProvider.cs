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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="CustomAuthorizationProviderData"/> that
    /// splits policy overrides processing and WMI objects generation, performing approriate logging of 
    /// policy processing errors.
    /// </summary>
	public class CustomAuthorizationProviderDataManageabilityProvider
		: CustomProviderDataManageabilityProvider<CustomAuthorizationProviderData>
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="CustomAuthorizationProviderDataManageabilityProvider"/> class.
        /// </summary>
		public CustomAuthorizationProviderDataManageabilityProvider()
			: base(Resources.AuthorizationProviderPolicyNameTemplate)
		{
			CustomAuthorizationProviderDataWmiMapper.RegisterWmiTypes();
		}

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjects(CustomAuthorizationProviderData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			CustomAuthorizationProviderDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}

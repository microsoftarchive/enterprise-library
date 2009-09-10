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
    /// Represents a manageability provider for custom security cache provider.
    /// </summary>
	public class CustomSecurityCacheProviderDataManageabilityProvider 
		: CustomProviderDataManageabilityProvider<CustomSecurityCacheProviderData>
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="CustomSecurityCacheProviderDataManageabilityProvider"/> class.
        /// </summary>
		public CustomSecurityCacheProviderDataManageabilityProvider()
			: base(Resources.SecurityCacheProviderPolicyNameTemplate)
		{
			CustomSecurityCacheProviderDataWmiMapper.RegisterWmiTypes();
		}

	    /// <summary>
	    /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
	    /// configurationObject.
	    /// </summary>
	    /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
	    /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
	    protected override void GenerateWmiObjects(CustomSecurityCacheProviderData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			CustomSecurityCacheProviderDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}

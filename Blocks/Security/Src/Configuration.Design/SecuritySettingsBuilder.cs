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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// 
    /// </summary>
    public class SecuritySettingsBuilder
    {
        SecuritySettingsNode securitySettingsNode;
        SecuritySettings securityConfiguration;
        private IConfigurationUIHierarchy hierarchy;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="securitySettingsNode"></param>
        public SecuritySettingsBuilder(IServiceProvider serviceProvider, SecuritySettingsNode securitySettingsNode)
        {
            this.securitySettingsNode = securitySettingsNode;
            hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public SecuritySettings Build()
        {
            securityConfiguration = new SecuritySettings();
			if (!securitySettingsNode.RequirePermission)	// don't set if false
				securityConfiguration.SectionInformation.RequirePermission = securitySettingsNode.RequirePermission;
			securityConfiguration.DefaultAuthorizationProviderName = (securitySettingsNode.DefaultAuthorizationInstance == null) ? string.Empty : securitySettingsNode.DefaultAuthorizationInstance.Name;
            securityConfiguration.DefaultSecurityCacheProviderName = (securitySettingsNode.DefaultSecurityCacheInstance == null) ? string.Empty : securitySettingsNode.DefaultSecurityCacheInstance.Name;

            BuildAuthorizationProviders();
            BuildSecurityCacheProviders();
			BuildAuthorizationRules();
            return securityConfiguration;
        }

        private void BuildSecurityCacheProviders()
        {
            SecurityCacheProviderCollectionNode securityCacheCollectionNode = (SecurityCacheProviderCollectionNode) hierarchy.FindNodeByType(typeof(SecurityCacheProviderCollectionNode));
            foreach (SecurityCacheProviderNode cacheProviderNode in securityCacheCollectionNode.Nodes)
            {
                securityConfiguration.SecurityCacheProviders.Add(cacheProviderNode.SecurityCacheProviderData);
            }
        }

        private void BuildAuthorizationProviders()
        {
            AuthorizationProviderCollectionNode authorizationProviderCollectionNode = (AuthorizationProviderCollectionNode)hierarchy.FindNodeByType(typeof(AuthorizationProviderCollectionNode));
            foreach (AuthorizationProviderNode authorizationProviderNode in authorizationProviderCollectionNode.Nodes)
            {			
                securityConfiguration.AuthorizationProviders.Add(authorizationProviderNode.AuthorizationProviderData);				
            }
        }

		private void BuildAuthorizationRules()
		{
			foreach (AuthorizationRuleNode ruleNode in hierarchy.FindNodesByType(typeof(AuthorizationRuleNode)))
			{
				AuthorizationRuleProviderData data = (AuthorizationRuleProviderData)securityConfiguration.AuthorizationProviders.Get(ruleNode.Parent.Name);
				data.Rules.Add(ruleNode.AuthorizationRuleData);
			}
		}
    }
}

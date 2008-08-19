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
    sealed class SecuritySettingsNodeBuilder : NodeBuilder
    {
        private SecuritySettings settings;
        private ConfigurationNode authorizationProviderCollectionNode_;
        private ConfigurationNode securityCacheProviderCollectionNode_;
        private ConfigurationNode authorizationRuleProviderNode;
		private AuthorizationProviderNode defaultAuthorizationNode;
		private SecurityCacheProviderNode defaultSecurityCacheProviderNode;

        public SecuritySettingsNodeBuilder(IServiceProvider serviceProvider, SecuritySettings settings)
            :base(serviceProvider)
        {
            this.settings = settings;
        }

        public SecuritySettingsNode Build()
        {
            SecuritySettingsNode rootNode = new SecuritySettingsNode();

            AuthorizationProviderCollectionNode authorizationProviderCollectionNode = new AuthorizationProviderCollectionNode();
            this.authorizationProviderCollectionNode_ = authorizationProviderCollectionNode;
            settings.AuthorizationProviders.ForEach(new Action<AuthorizationProviderData>(BuildAuthorizationProviders));

            SecurityCacheProviderCollectionNode securityCacheProviderCollectionNode = new SecurityCacheProviderCollectionNode();
            this.securityCacheProviderCollectionNode_ = securityCacheProviderCollectionNode;
            settings.SecurityCacheProviders.ForEach(new Action<SecurityCacheProviderData>(BuildSecurityCacheProviders));

            rootNode.AddNode(authorizationProviderCollectionNode);
            rootNode.AddNode(securityCacheProviderCollectionNode);

			rootNode.DefaultAuthorizationInstance = defaultAuthorizationNode;
			rootNode.DefaultSecurityCacheInstance = defaultSecurityCacheProviderNode;
			
			rootNode.RequirePermission = settings.SectionInformation.RequirePermission;
			
			return rootNode;
        }

        private void BuildAuthorizationProviders(AuthorizationProviderData authorizationProviderData)
        {
            ConfigurationNode authorizationProviderNode = NodeCreationService.CreateNodeByDataType(authorizationProviderData.GetType(), new object[] { authorizationProviderData });
            if (null == authorizationProviderNode)
			{
                LogNodeMapError(authorizationProviderCollectionNode_, authorizationProviderData.GetType());
				return;
			}
            if (typeof(AuthorizationRuleProviderData) == authorizationProviderData.GetType())
            {
                AuthorizationRuleProviderData authorizationRuleProviderData = (AuthorizationRuleProviderData)authorizationProviderData;
                this.authorizationRuleProviderNode = (AuthorizationRuleProviderNode)authorizationProviderNode;

                authorizationRuleProviderData.Rules.ForEach(new Action<AuthorizationRuleData>(BuildAuthorizationRule));
            }
			if (authorizationProviderNode.Name == settings.DefaultAuthorizationProviderName) defaultAuthorizationNode = (AuthorizationProviderNode)authorizationProviderNode;
            authorizationProviderCollectionNode_.AddNode(authorizationProviderNode);
        }

        private void BuildSecurityCacheProviders(SecurityCacheProviderData securityProviderData)
        {
            ConfigurationNode securityProviderNode = NodeCreationService.CreateNodeByDataType(securityProviderData.GetType(), new object[] { securityProviderData });
            if (null == securityProviderNode)
            {
                LogNodeMapError(securityCacheProviderCollectionNode_, securityProviderData.GetType());
                return;
            }
			if (securityProviderNode.Name == settings.DefaultSecurityCacheProviderName) defaultSecurityCacheProviderNode = (SecurityCacheProviderNode)securityProviderNode;
            securityCacheProviderCollectionNode_.AddNode(securityProviderNode);
        }

        private void BuildAuthorizationRule(AuthorizationRuleData authorizationRule)
        {
            AuthorizationRuleNode ruleNode = new AuthorizationRuleNode(authorizationRule);

            authorizationRuleProviderNode.AddNode(ruleNode);
        }
    }
}

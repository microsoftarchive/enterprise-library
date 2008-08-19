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
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    sealed class SecurityCommandRegistrar : CommandRegistrar
    {		
        public SecurityCommandRegistrar(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
        }
        
        public override void Register()
        {
            AddSecurityCommand();
            AddDefaultCommands(typeof(SecuritySettingsNode));

            AddCustomAuthorizationRuleCommand();
            AddDefaultCommands(typeof(CustomAuthorizationProviderNode));

            AddAuthorizationRuleProviderCommand();
            AddDefaultCommands(typeof(AuthorizationRuleProviderNode));

            AddCustomSecurityCacheCommand();
            AddDefaultCommands(typeof(CustomSecurityCacheProviderNode));

            AddAuthorizationRuleCommand();
            AddDefaultCommands(typeof(AuthorizationRuleNode));
        }

        private void AddAuthorizationRuleCommand()
        {
            AddMultipleChildNodeCommand(Resources.AuthorizationRuleCommandName, 
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.AuthorizationRuleCommandName),
                typeof(AuthorizationRuleNode), typeof(AuthorizationRuleProviderNode));
        }

        private void AddCustomSecurityCacheCommand()
        {
            AddMultipleChildNodeCommand(Resources.CustomSecurityCacheNodeCommandName,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.CustomSecurityCacheNodeCommandName),
                typeof(CustomSecurityCacheProviderNode), typeof(SecurityCacheProviderCollectionNode));
        }

        private void AddAuthorizationRuleProviderCommand()
        {
            AddMultipleChildNodeCommand(Resources.AuthorizationRuleProviderCommandName,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.AuthorizationRuleProviderCommandName),
                typeof(AuthorizationRuleProviderNode), typeof(AuthorizationProviderCollectionNode));
        }

        private void AddCustomAuthorizationRuleCommand()
        {
            AddMultipleChildNodeCommand(Resources.CustomAuthorizationProviderCommandName,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.CustomAuthorizationProviderCommandName),
                typeof(CustomAuthorizationProviderNode), typeof(AuthorizationProviderCollectionNode));
        }

        private void AddSecurityCommand()
        {

            ConfigurationUICommand item = ConfigurationUICommand.CreateSingleUICommand(ServiceProvider, 
				Resources.AddConfigurationSectionCommandName,
				string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.AddConfigurationSectionCommandName),
				new AddSecuritySettingsNodeCommand(ServiceProvider),
                typeof(SecuritySettingsNode));

            AddUICommand(item, typeof(ConfigurationApplicationNode));
        }
    }
}

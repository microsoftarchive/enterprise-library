//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    internal class PolicyInjectionCommandRegistrar : CommandRegistrar
    {
        public PolicyInjectionCommandRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider)
        { }

        public override void Register()
        {
            AddPolicyInjectionSettingsCommands();

            AddPolicyCommands();

            AddAssemblyMatchingRuleCommands();

            AddCustomAttributeMatchingRuleCommands();

            AddCustomMatchingRuleCommands();

            AddMemberNameMatchingRuleCommands();

            AddMethodSignatureMatchingRuleCommands();

            AddNamespaceMatchingRuleCommands();

            AddParameterTypeMatchingRuleCommands();

            AddPropertyMatchingRuleCommands();

            AddReturnTypeMatchingRuleCommands();

            AddTagAttributeMatchingRuleCommands();

            AddTypeMatchingRuleCommands();
        }

        private void AddPolicyInjectionSettingsCommands()
        {
            AddDefaultCommands(typeof(PolicyInjectionSettingsNode));
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateSingleUICommand(
                ServiceProvider,
                Resources.AddPolicyInjectionSettingsCommandText,
                Resources.AddPolicyInjectionSettingsCommandTextLong,
                new AddPolicyInjectionSettingsCommand(ServiceProvider),
                typeof(PolicyInjectionSettingsNode));
            AddUICommand(cmd, typeof(ConfigurationApplicationNode));
        }

        private void AddPolicyCommands()
        {
            AddMoveUpDownCommands(typeof(PolicyNode));
            AddDefaultCommands(typeof(PolicyNode));
            ConfigurationUICommand cmd = ConfigurationUICommand.CreateMultipleUICommand(ServiceProvider,
                Resources.AddPolicyCommandText,
                Resources.AddPolicyCommandTextLong,
                new AddPolicyCommand(ServiceProvider),
                typeof(PolicyCollectionNode));

            AddUICommand(cmd, typeof(PolicyCollectionNode));
        }

        private void AddTagAttributeMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(TagAttributeMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.TagAttributeMatchingRuleCommandText,
                Resources.TagAttributeMatchingRuleCommandTextLong,
                typeof(TagAttributeMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddTypeMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(TypeMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddTypeMatchingRuleCommandText,
                Resources.AddTypeMatchingRuleTextLong,
                typeof(TypeMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddReturnTypeMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(ReturnTypeMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddReturnTypeMatchingRuleCommandText,
                Resources.AddReturnTypeMatchingRuleCommandTextLong,
                typeof(ReturnTypeMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddPropertyMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(PropertyMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddPropertyMatchingRuleCommandText,
                Resources.AddPropertyMatchingRuleCommandTextLong,
                typeof(PropertyMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddMethodSignatureMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(MethodSignatureMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddMethodSignatureMatchingRuleCommandText,
                Resources.AddMethodSignatureMatchingRuleCommandTextLong,
                typeof(MethodSignatureMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddNamespaceMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(NamespaceMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddNamespaceMatchingRuleCommandText,
                Resources.AddNamespaceMatchingRuleCommandTextLong,
                typeof(NamespaceMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddMemberNameMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(MemberNameMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddMemberNameMatchingRuleCommandText,
                Resources.AddMemberNameMatchingRuleCommandTextLong,
                typeof(MemberNameMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddCustomMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(CustomMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddCustomMatchingRuleCommandText,
                Resources.AddCustomMatchingRuleCommandTextLong,
                typeof(CustomMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddCustomAttributeMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(CustomAttributeMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddCustomAttributeMatchingRuleCommandText,
                Resources.AddCustomAttributeMatchingRuleCommandTextLong,
                typeof(CustomAttributeMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddAssemblyMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(AssemblyMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddAssemblyMatchingRuleCommandText,
                Resources.AddAssemblyMatchingRuleCommandTextLong,
                typeof(AssemblyMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }

        private void AddParameterTypeMatchingRuleCommands()
        {
            AddDefaultCommands(typeof(ParameterTypeMatchingRuleNode));
            AddMultipleChildNodeCommand(Resources.AddParameterTypeMatchingRuleCommandText,
                Resources.AddParameterTypeMatchingRuleCommandTextLong,
                typeof(ParameterTypeMatchingRuleNode),
                typeof(MatchingRuleCollectionNode));
        }
    }
}

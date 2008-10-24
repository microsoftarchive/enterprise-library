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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    sealed class PolicyInjectionSettingsBuilder
    {
        PolicyInjectionSettingsNode node;
        private IConfigurationUIHierarchy hierarchy;
        private PolicyInjectionSettings settings;

        public PolicyInjectionSettingsBuilder(IServiceProvider serviceProvider, PolicyInjectionSettingsNode node)
        {
            this.hierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
            this.node = node;
        }

        public PolicyInjectionSettings Build()
        {
            settings = new PolicyInjectionSettings();
            if (!node.RequirePermission)	// don't set if false
                settings.SectionInformation.RequirePermission = node.RequirePermission;

            foreach (PolicyNode policyNode in GetNodesInChildCollection<PolicyCollectionNode, PolicyNode>(node))
            {
                settings.Policies.Add(BuildPolicyData(policyNode));
            }
            return settings;
        }

        private PolicyData BuildPolicyData(PolicyNode policyNode)
        {
            PolicyData policyData = new PolicyData(policyNode.Name);
            foreach (MatchingRuleNode ruleNode in MatchingRuleNodesInPolicy(policyNode))
            {
                MatchingRuleData ruleData = ruleNode.GetConfigurationData();
                policyData.MatchingRules.Add(ruleData);
            }

            foreach (CallHandlerNode handlerNode in CallHandlerNodesInPolicy(policyNode))
            {
                CallHandlerData handlerData = handlerNode.CreateCallHandlerData();
                policyData.Handlers.Add(handlerData);
            }
            return policyData;
        }

        private T GetChildNode<T>(ConfigurationNode parentNode) where T : ConfigurationNode
        {
            T childNode = hierarchy.FindNodeByType(parentNode, typeof(T)) as T;
            return childNode;
        }

        private IEnumerable<TChild> GetNodesInChildCollection<TCollection, TChild>(ConfigurationNode parent)
            where TCollection : ConfigurationNode
            where TChild : ConfigurationNode
        {
            TCollection collectionNode = GetChildNode<TCollection>(parent);
            if (collectionNode != null)
            {
                foreach (TChild childNode in GetNodesInCollection<TChild>(collectionNode))
                {
                    yield return childNode;
                }
            }
        }

        private IEnumerable<TChild> GetNodesInCollection<TChild>(ConfigurationNode parent) where TChild : ConfigurationNode
        {
            foreach (ConfigurationNode childConfigurationNode in parent.Nodes)
            {
                TChild typedChildNode = childConfigurationNode as TChild;
                if (typedChildNode != null)
                {
                    yield return typedChildNode;
                }
            }
        }

        private IEnumerable<MatchingRuleNode> MatchingRuleNodesInPolicy(PolicyNode parent)
        {
            foreach (MatchingRuleNode childNode in GetNodesInChildCollection<MatchingRuleCollectionNode, MatchingRuleNode>(parent))
            {
                yield return childNode;
            }
        }

        private IEnumerable<CallHandlerNode> CallHandlerNodesInPolicy(PolicyNode parent)
        {
            foreach (CallHandlerNode childNode in GetNodesInChildCollection<CallHandlersCollectionNode, CallHandlerNode>(parent))
            {
                yield return childNode;
            }
        }
    }
}

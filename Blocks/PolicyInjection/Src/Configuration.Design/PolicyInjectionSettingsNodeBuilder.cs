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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    class PolicyInjectionSettingsNodeBuilder : NodeBuilder
    {
        private PolicyInjectionSettings settings;
        private IConfigurationUIHierarchy configurationHierarchy;

        public PolicyInjectionSettingsNodeBuilder(IServiceProvider serviceProvider, PolicyInjectionSettings settings)
            : base(serviceProvider)
        {
            this.settings = settings;
            configurationHierarchy = ServiceHelper.GetCurrentHierarchy(serviceProvider);
        }

        public PolicyInjectionSettingsNode Build()
        {
            PolicyInjectionSettingsNode node = new PolicyInjectionSettingsNode();
            InjectorCollectionNode injectorsNode = new InjectorCollectionNode();
            PolicyCollectionNode collectionNode = new PolicyCollectionNode();
            node.AddNode(injectorsNode);
            node.AddNode(collectionNode);

            foreach (InjectorData injectorData in settings.Injectors)
            {
                AddInjector(injectorData, injectorsNode);
            }
            foreach (PolicyData policyData in settings.Policies)
            {
                PolicyNode policyNode = new PolicyNode(policyData);

                AddMatchingRules(policyData, policyNode);

                AddHandlers(policyData, policyNode);

                collectionNode.AddNode(policyNode);
            }

            node.RequirePermission = settings.SectionInformation.RequirePermission;

            return node;
        }

        private void AddInjector(InjectorData injectorData, InjectorCollectionNode injectorsNode)
        {
            InjectorNode injectorNode =
                NodeCreationService.CreateNodeByDataType(injectorData.GetType(), new object[] { injectorData }) as InjectorNode;
            if (injectorNode == null)
            {
                LogNodeMapError(injectorsNode, injectorData.GetType());
            }
            else
            {
                injectorsNode.AddNode(injectorNode);
                if (injectorData.Name == this.settings.Injectors.DefaultInjector)
                {
                    injectorsNode.DefaultInjector = injectorNode;
                }
            }
        }

        private void AddMatchingRules(PolicyData policyData, PolicyNode policyNode)
        {
            MatchingRuleCollectionNode matchingRuleCollectionNode = new MatchingRuleCollectionNode();
            foreach (MatchingRuleData matchingRule in policyData.MatchingRules)
            {
                ConfigurationNode matchingRuleNode = base.NodeCreationService.CreateNodeByDataType(matchingRule.GetType(), new object[] { matchingRule });
                if (matchingRuleNode == null)
                {
                    base.LogNodeMapError(matchingRuleCollectionNode, matchingRule.GetType());
                    break;
                }
                matchingRuleCollectionNode.AddNode(matchingRuleNode);
            }
            policyNode.AddNode(matchingRuleCollectionNode);
        }

        private void AddHandlers(PolicyData policyData, PolicyNode policyNode)
        {
            CallHandlersCollectionNode callHandlersCollectionNode = new CallHandlersCollectionNode();
            foreach (CallHandlerData handler in policyData.Handlers)
            {
                ConfigurationNode handlerNode = base.NodeCreationService.CreateNodeByDataType(handler.GetType(), new object[] { handler });
                if (handlerNode == null)
                {
                    base.LogNodeMapError(callHandlersCollectionNode, handler.GetType());
                    break;
                }
                callHandlersCollectionNode.AddNode(handlerNode);

                CallHandlerNode typedHandlerNode = handlerNode as CallHandlerNode;
                if (typedHandlerNode != null)
                {
                    typedHandlerNode.ResolveNodeReferences(configurationHierarchy);
                }
            }
            policyNode.AddNode(callHandlersCollectionNode);
        }
    }
}

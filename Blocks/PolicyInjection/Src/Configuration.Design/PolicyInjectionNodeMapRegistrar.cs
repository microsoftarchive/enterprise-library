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
    internal class PolicyInjectionNodeMapRegistrar : NodeMapRegistrar
    {
        public PolicyInjectionNodeMapRegistrar(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        public override void Register()
        {
            AddMultipleNodeMap(Resources.AssemblyMatchingRule,
                               typeof(AssemblyMatchingRuleNode),
                               typeof(AssemblyMatchingRuleData));
            AddMultipleNodeMap(Resources.CustomAttributeMatchingRuleNodeName,
                               typeof(CustomAttributeMatchingRuleNode),
                               typeof(CustomAttributeMatchingRuleData));
            AddMultipleNodeMap(Resources.CustomMatchingRuleNodeName,
                               typeof(CustomMatchingRuleNode),
                               typeof(CustomMatchingRuleData));
            AddMultipleNodeMap(Resources.MemberNameMatchingRuleNodeName,
                               typeof(MemberNameMatchingRuleNode),
                               typeof(MemberNameMatchingRuleData));
            AddMultipleNodeMap(Resources.MethodSignatureMatchingRuleNodeName,
                               typeof(MethodSignatureMatchingRuleNode),
                               typeof(MethodSignatureMatchingRuleData));
            AddMultipleNodeMap(Resources.NamespaceMatchingRuleNodeName,
                               typeof(NamespaceMatchingRuleNode),
                               typeof(NamespaceMatchingRuleData));
            AddMultipleNodeMap(Resources.ParameterTypeMatchingRuleNodeName,
                               typeof(ParameterTypeMatchingRuleNode),
                               typeof(ParameterTypeMatchingRuleData));
            AddMultipleNodeMap(Resources.PropertyMatchingRuleNodeName,
                               typeof(PropertyMatchingRuleNode),
                               typeof(PropertyMatchingRuleData));
            AddMultipleNodeMap(Resources.ReturnTypeMatchingRuleNodeName,
                               typeof(ReturnTypeMatchingRuleNode),
                               typeof(ReturnTypeMatchingRuleData));
            AddMultipleNodeMap(Resources.TagAttributeMatchingRuleNodeName,
                               typeof(TagAttributeMatchingRuleNode),
                               typeof(TagAttributeMatchingRuleData));
            AddMultipleNodeMap(Resources.TypeMatchingRuleNodeName,
                               typeof(TypeMatchingRuleNode),
                               typeof(TypeMatchingRuleData));
        }
    }
}

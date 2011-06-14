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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    [DeploymentItem("test.exe.config")]
    public class CustomAttributeMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {
#if !SILVERLIGHT
        [TestMethod]
        public void CanSerializeTypeMatchingRule()
        {
            CustomAttributeMatchingRuleData customAttributeMatchingRule = new CustomAttributeMatchingRuleData
                {
                    Name =  "MatchesMyAttribute", 
                    AttributeTypeName = "Namespace.MyAttribute, Assembly", 
                    SearchInheritanceChain = true
                };

            CustomAttributeMatchingRuleData deserializedRule = SerializeAndDeserializeMatchingRule(customAttributeMatchingRule) as CustomAttributeMatchingRuleData;

            Assert.IsNotNull(deserializedRule);
            Assert.AreEqual(customAttributeMatchingRule.Name, deserializedRule.Name);
            Assert.AreEqual(customAttributeMatchingRule.TypeName, deserializedRule.TypeName);
            Assert.AreEqual(customAttributeMatchingRule.SearchInheritanceChain, deserializedRule.SearchInheritanceChain);
        }
#endif

        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            CustomAttributeMatchingRuleData customAttributeMatchingRule = new CustomAttributeMatchingRuleData
                {
                    Name =  "MatchesMyAttribute", 
                    AttributeTypeName = "Namespace.MyAttribute, Assembly", 
                    SearchInheritanceChain = true
                };
            TypeRegistration registration = customAttributeMatchingRule.GetRegistrations("").First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }
}

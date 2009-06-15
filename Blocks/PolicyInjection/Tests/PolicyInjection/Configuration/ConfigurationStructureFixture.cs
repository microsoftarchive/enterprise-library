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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    [TestClass]
    public class ConfigurationFixture
    {
        const string policy1Name = "policy1";
        const string policy2Name = "policy2";

        const string handler1Name = "handler1";

        const string matchingRule1Name = "rule1";

        [TestMethod]
        public void CanSerializeConfigurationFixture()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();

            PolicyData policy1 = new PolicyData(policy1Name);
            PolicyData policy2 = new PolicyData(policy2Name);
            settings.Policies.Add(policy1);
            settings.Policies.Add(policy2);

            CustomCallHandlerData handler1 = new CustomCallHandlerData(handler1Name, typeof(CallCountHandler));
            handler1.SetAttributeValue("customHandlerAttribute", "customHandlerAttributeValue");

            CustomMatchingRuleData customMatchingRule = new CustomMatchingRuleData(matchingRule1Name, typeof(TypeMatchingAssignmentRule));
            customMatchingRule.SetAttributeValue("customMatchingRuleAttribute", "customMatchingRuleAttributeValue");

            policy1.Handlers.Add(handler1);
            policy1.MatchingRules.Add(customMatchingRule);

            Dictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections.Add(PolicyInjectionSettings.SectionName, settings);

            IConfigurationSource configurationSource = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            PolicyInjectionSettings deserializedSection = (PolicyInjectionSettings)configurationSource.GetSection(PolicyInjectionSettings.SectionName);

            Assert.AreEqual(2, deserializedSection.Policies.Count);

            PolicyData deserializedPolicy1 = deserializedSection.Policies.Get(0);
            Assert.IsNotNull(deserializedPolicy1);
            Assert.AreEqual(policy1Name, deserializedPolicy1.Name);

            CallHandlerData deserializedHandler = deserializedPolicy1.Handlers.Get(0);
            Assert.IsNotNull(deserializedHandler);
            Assert.IsNotNull(deserializedHandler as CustomCallHandlerData);
            Assert.AreEqual(handler1Name, deserializedHandler.Name);
            Assert.AreEqual(typeof(CallCountHandler), deserializedHandler.Type);
            Assert.AreEqual("customHandlerAttributeValue", (string)deserializedHandler.ElementInformation.Properties["customHandlerAttribute"].Value);

            Assert.AreEqual(policy2Name, deserializedSection.Policies.Get(1).Name);

            MatchingRuleData deserializedMatchingRule = deserializedPolicy1.MatchingRules.Get(0);
            Assert.IsNotNull(deserializedMatchingRule as CustomMatchingRuleData);
            Assert.AreEqual(matchingRule1Name, deserializedMatchingRule.Name);
            Assert.AreEqual(typeof(TypeMatchingAssignmentRule), deserializedMatchingRule.Type);
            Assert.AreEqual("customMatchingRuleAttributeValue", (string)deserializedMatchingRule.ElementInformation.Properties["customMatchingRuleAttribute"].Value);
        }
    }
}

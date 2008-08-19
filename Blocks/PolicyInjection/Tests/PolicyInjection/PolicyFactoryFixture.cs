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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests
{
    [TestClass]
    public class PolicyFactoryFixture
    {
        [TestMethod]
        public void CanCreateEmptyPolicy()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));

            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
        }

        [TestMethod]
        public void CreatingPolicyWithoutConfigurationReturnsEmptyPolicySet()
        {
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policies = factory.Create();
            Assert.IsNotNull(policies);
            Assert.AreEqual(1, policies.Count);
        }

        [TestMethod, ExpectedException(typeof(ArgumentNullException))]
        public void CreatingPolicyFactoryWithNullSourceThrows()
        {
            new PolicySetFactory(null);
        }

        [TestMethod]
        public void CanCreatePolicyWithAssemblyMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new AssemblyMatchingRuleData("RuleName", "assemblyName"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, policy.RuleSet.Count);
            Assert.AreEqual(typeof(AssemblyMatchingRule), policy.RuleSet[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithMemberNameMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new MemberNameMatchingRuleData("RuleName", "memberName"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, policy.RuleSet.Count);
            Assert.AreEqual(typeof(MemberNameMatchingRule), policy.RuleSet[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithMethodSignatureMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new MethodSignatureMatchingRuleData("RuleName", "memberName"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, policy.RuleSet.Count);
            Assert.AreEqual(typeof(MethodSignatureMatchingRule), policy.RuleSet[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithNamespaceMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new NamespaceMatchingRuleData("RuleName", "namespaceName"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, policy.RuleSet.Count);
            Assert.AreEqual(typeof(NamespaceMatchingRule), policy.RuleSet[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithReturnTypeMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new ReturnTypeMatchingRuleData("RuleName", "returnType"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, policy.RuleSet.Count);
            Assert.AreEqual(typeof(ReturnTypeMatchingRule), policy.RuleSet[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithTagMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new TagAttributeMatchingRuleData("RuleName", "tag"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, policy.RuleSet.Count);
            Assert.AreEqual(typeof(TagAttributeMatchingRule), policy.RuleSet[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithTypeMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new TypeMatchingRuleData("RuleName", "returnType"));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, policy.RuleSet.Count);
            Assert.AreEqual(typeof(TypeMatchingRule), policy.RuleSet[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithCustomMatchingRule()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).MatchingRules.Add(new CustomMatchingRuleData("alwaysTrue", typeof(AlwaysMatchingRule)));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, policy.RuleSet.Count);
            Assert.AreEqual(typeof(AlwaysMatchingRule), policy.RuleSet[0].GetType());
        }

        [TestMethod]
        public void CanCreatePolicyWithCustomCallHandler()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("Policy"));
            settings.Policies.Get(0).Handlers.Add(new CustomCallHandlerData("callCountHandler", typeof(CallCountHandler)));
            DictionaryConfigurationSource dictSource = new DictionaryConfigurationSource();
            dictSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictSource);
            PolicySet policySet = factory.Create();
            Assert.IsNotNull(policySet);

            RuleDrivenPolicy policy = (RuleDrivenPolicy)policySet[1];
            Assert.IsNotNull(policy);
            Assert.AreEqual("Policy", policy.Name);
            Assert.AreEqual(1, policy.Handlers.Count);
            Assert.AreEqual(typeof(CallCountHandler), policy.Handlers[0].GetType());
        }
    }
}
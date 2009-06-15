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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class PolicyInjectionSettingsFixture
    {
        [TestMethod]
        [DeploymentItem("OldStyle.config")]
        public void SkipsInjectorsElement()
        {
            IConfigurationSource source = new FileConfigurationSource("OldStyle.config");

            PolicyInjectionSettings settings
                = (PolicyInjectionSettings)source.GetSection(PolicyInjectionSettings.SectionName);

            Assert.IsNotNull(settings);
            Assert.AreEqual(3, settings.Policies.Count);
        }
    }

    [TestClass]
    public class GivenAnEmptySection
    {
        private PolicyInjectionSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new PolicyInjectionSettings();
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenReturnsNoRegistrations()
        {
            var registrations = settings.GetRegistrations(null);

            Assert.AreEqual(0, registrations.Count());
        }
    }

    [TestClass]
    public class GivenASectionWithAnEmptyPolicy
    {
        private PolicyInjectionSettings settings;

        [TestInitialize]
        public void Setup()
        {
            settings = new PolicyInjectionSettings();
            settings.Policies.Add(new PolicyData("policy 1") { });
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenReturnsOneRegistrationForInjectionPolicy()
        {
            var registrations = settings.GetRegistrations(null);

            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).Count());
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenReturnsNoOtherPolicies()
        {
            var registrations = settings.GetRegistrations(null);

            Assert.AreEqual(0, registrations.Where(tr => tr.ServiceType != typeof(InjectionPolicy)).Count());
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenRegistrationForInjectionPolicyIsForRuleData()
        {
            var registrations = settings.GetRegistrations(null);

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(InjectionPolicy))
                .ForName("policy 1")
                .ForImplementationType(typeof(InjectionFriendlyRuleDrivenPolicy));
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenRegistrationForInjectionPolicyInjectsNoMatchingRulesOrHandlers()
        {
            string[] handlerNames;

            var registrations = settings.GetRegistrations(null);

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("policy 1")
                .WithContainerResolvedEnumerableConstructorParameter<IMatchingRule>(new string[0])
                .WithValueConstructorParameter(out handlerNames)
                .VerifyConstructorParameters();

            CollectionAssert.AreEqual(new string[0], handlerNames);
        }
    }

    [TestClass]
    public class GivenASectionWithAMatchingRule
    {
        private PolicyInjectionSettings settings;

        [TestInitialize]
        public void Setup()
        {
            var policyData = new PolicyData("policy 1");
            policyData.MatchingRules.Add(
                new TypeMatchingRuleData("matching rule 1", typeof(object).AssemblyQualifiedName));

            settings = new PolicyInjectionSettings();
            settings.Policies.Add(policyData);
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenReturnsOneRegistrationForInjectionPolicy()
        {
            var registrations = settings.GetRegistrations(null);

            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).Count());
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenRegistrationForInjectionPolicyIsForRuleData()
        {
            var registrations = settings.GetRegistrations(null);

            registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).ElementAt(0)
                .AssertForServiceType(typeof(InjectionPolicy))
                .ForName("policy 1")
                .ForImplementationType(typeof(InjectionFriendlyRuleDrivenPolicy));
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenRegistrationForInjectionPolicyInjectsNoMatchingRulesOrHandlers()
        {
            string[] handlerNames;

            var registrations = settings.GetRegistrations(null);

            registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("policy 1")
                .WithContainerResolvedEnumerableConstructorParameter<IMatchingRule>(
                    new[] { "matching rule 1-policy 1" })
                .WithValueConstructorParameter(out handlerNames)
                .VerifyConstructorParameters();

            CollectionAssert.AreEqual(new string[0], handlerNames);
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenReturnsOneRegistrationForMatchingRule()
        {
            var registrations = settings.GetRegistrations(null);

            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(IMatchingRule)).Count());
        }


        [TestMethod]
        public void WhenCreatingRegistration_ThenRegistrationForMatchingRuleIsForConfiguredRuleWithSyntheticName()
        {
            var registrations = settings.GetRegistrations(null);

            registrations.Where(tr => tr.ServiceType == typeof(IMatchingRule)).ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("matching rule 1-policy 1")
                .ForImplementationType(typeof(TypeMatchingRule));
        }
    }

    [TestClass]
    public class GivenASectionWithAMatchingRuleAndACustomCallHandler
    {
        private PolicyInjectionSettings settings;

        [TestInitialize]
        public void Setup()
        {
            var policyData = new PolicyData("policy 1");
            policyData.MatchingRules.Add(
                new TypeMatchingRuleData("matching rule 1", typeof(object).AssemblyQualifiedName));
            policyData.Handlers.Add(
                new CustomCallHandlerData("handler 1", typeof(CallCountHandler).AssemblyQualifiedName));

            settings = new PolicyInjectionSettings();
            settings.Policies.Add(policyData);
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenReturnsOneRegistrationForInjectionPolicy()
        {
            var registrations = settings.GetRegistrations(null);

            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).Count());
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenRegistrationForInjectionPolicyIsForRuleData()
        {
            var registrations = settings.GetRegistrations(null);

            registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).ElementAt(0)
                .AssertForServiceType(typeof(InjectionPolicy))
                .ForName("policy 1")
                .ForImplementationType(typeof(InjectionFriendlyRuleDrivenPolicy));
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenRegistrationForInjectionPolicyInjectsNoMatchingRulesOrHandlers()
        {
            string[] handlerNames;

            var registrations = settings.GetRegistrations(null);

            registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("policy 1")
                .WithContainerResolvedEnumerableConstructorParameter<IMatchingRule>(
                    new[] { "matching rule 1-policy 1" })
                .WithValueConstructorParameter(out handlerNames)
                .VerifyConstructorParameters();

            CollectionAssert.AreEqual(new[] { "handler 1-policy 1" }, handlerNames);
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenReturnsOneRegistrationForMatchingRule()
        {
            var registrations = settings.GetRegistrations(null);

            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(IMatchingRule)).Count());
        }


        [TestMethod]
        public void WhenCreatingRegistration_ThenRegistrationForMatchingRuleIsForConfiguredRuleWithSyntheticName()
        {
            var registrations = settings.GetRegistrations(null);

            registrations.Where(tr => tr.ServiceType == typeof(IMatchingRule)).ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("matching rule 1-policy 1")
                .ForImplementationType(typeof(TypeMatchingRule));
        }

        [TestMethod]
        public void WhenCreatingRegistration_ThenReturnsOneRegistrationForCallHandler()
        {
            var registrations = settings.GetRegistrations(null);

            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(ICallHandler)).Count());
        }


        [TestMethod]
        public void WhenCreatingRegistration_ThenRegistrationForCallHandlerIsForConfiguredHandlerWithSyntheticName()
        {
            var registrations = settings.GetRegistrations(null);

            registrations.Where(tr => tr.ServiceType == typeof(ICallHandler)).ElementAt(0)
                .AssertForServiceType(typeof(ICallHandler))
                .ForName("handler 1-policy 1")
                .ForImplementationType(typeof(CallCountHandler));
        }
    }
}

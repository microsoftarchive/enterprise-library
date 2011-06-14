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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class GivenANewPolicyData
    {
        private PolicyData policyData;

        [TestInitialize]
        public void Setup()
        {
            policyData = new PolicyData { Name = "policy" };
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenReturnsSingleRegistration()
        {
            var registrations = policyData.GetRegistrations();

            Assert.AreEqual(1, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenRegistrationIsForRuleDrivenPolicy()
        {
            var registrations = policyData.GetRegistrations();

            registrations.ElementAt(0)
                .AssertForServiceType(typeof(InjectionPolicy))
                .ForName("policy")
                .ForImplementationType(typeof(InjectionFriendlyRuleDrivenPolicy));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenRegistrationForInjectionPolicyInjectsNoMatchingRulesOrHandlers()
        {
            string[] handlerNames;

            var registrations = policyData.GetRegistrations();

            registrations.ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("policy")
                .WithContainerResolvedEnumerableConstructorParameter<IMatchingRule>(new string[0])
                .WithValueConstructorParameter(out handlerNames)
                .VerifyConstructorParameters();

            CollectionAssert.AreEqual(new string[0], handlerNames);
        }
    }

    [TestClass]
    public class GivenAPolicyDataWithAMatchingRule
    {
        private PolicyData policyData;

        [TestInitialize]
        public void Setup()
        {
            policyData = new PolicyData { Name = "policy" };
            policyData.MatchingRules.Add(new TypeMatchingRuleData
                {
                    Name = "type",
                    Matches = { new MatchData { Match = typeof(object).AssemblyQualifiedName }
                }
            });
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenReturnsTwoRegistrations()
        {
            var registrations = policyData.GetRegistrations();

            Assert.AreEqual(2, registrations.Count());
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenReturnsOneRegistrationForInjectionPolicy()
        {
            var registrations = policyData.GetRegistrations();

            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).Count());
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenReturnsOneRegistrationForMatchingRule()
        {
            var registrations = policyData.GetRegistrations();

            Assert.AreEqual(1, registrations.Where(tr => tr.ServiceType == typeof(IMatchingRule)).Count());
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenRegistrationIsForRuleDrivenPolicy()
        {
            var registrations = policyData.GetRegistrations();

            registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).ElementAt(0)
                .AssertForServiceType(typeof(InjectionPolicy))
                .ForName("policy")
                .ForImplementationType(typeof(InjectionFriendlyRuleDrivenPolicy));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenPolicyRegistrationConfiguresInjectsResolvedRule()
        {
            string[] matchingRuleNames;
            string[] handlerNames;

            var registrations = policyData.GetRegistrations();

            registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter("policy")
                .WithContainerResolvedEnumerableConstructorParameter<IMatchingRule>(out matchingRuleNames)
                .WithValueConstructorParameter(out handlerNames)
                .VerifyConstructorParameters();

            Assert.AreEqual(1, matchingRuleNames.Length);
            Assert.IsTrue(matchingRuleNames[0].StartsWith("type-policy"));
            CollectionAssert.AreEqual(new string[0], handlerNames);
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationIsForTypeMatchingRule()
        {
            var registrations = policyData.GetRegistrations();

            registrations.Where(tr => tr.ServiceType == typeof(IMatchingRule)).ElementAt(0)
                .AssertForServiceType(typeof(IMatchingRule))
                .ForName("type-policy")
                .ForImplementationType(typeof(TypeMatchingRule));
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenMatchingRuleRegistrationConfiguresInjectsMatches()
        {
            IEnumerable<MatchingInfo> matches;

            var registrations = policyData.GetRegistrations();

            registrations.Where(tr => tr.ServiceType == typeof(IMatchingRule)).ElementAt(0)
                .AssertConstructor()
                .WithValueConstructorParameter(out matches)
                .VerifyConstructorParameters();

            Assert.AreEqual(1, matches.Count());
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, matches.ElementAt(0).Match);
        }

        [TestMethod]
        public void WhenCreatesRegistrations_ThenPolicyRegistrationHasTransientLifetime()
        {
            var registrations = policyData.GetRegistrations();
            var registration = registrations.Where(tr => tr.ServiceType == typeof(InjectionPolicy)).First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }
    }
}

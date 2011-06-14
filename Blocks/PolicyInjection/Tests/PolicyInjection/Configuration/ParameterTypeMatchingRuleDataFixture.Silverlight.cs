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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    [DeploymentItem("test.exe.config")]
    public class ParameterTypeMatchingRuleDataFixture : MatchingRuleDataFixtureBase
    {
        [TestMethod]
        public void ShouldCreateCorrectMatchingRule()
        {
            PolicyData policyData = new PolicyData { Name = "Validate Parameters" };
            policyData.Handlers.Add(new ValidationCallHandlerData { Name = "Foo" });
            ParameterTypeMatchingRuleData matchingRuleData = GetParameterTypeMatchingRuleData();
            policyData.MatchingRules.Add(matchingRuleData);

            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            settings.Policies.Add(policyData);

            DictionaryConfigurationSource configSource = new DictionaryConfigurationSource();
            configSource.Add(PolicyInjectionSettings.SectionName, settings);

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            settings.ConfigureContainer(container, configSource);

            InjectionFriendlyRuleDrivenPolicy policy = container.Resolve<InjectionFriendlyRuleDrivenPolicy>("Validate Parameters");
            
            // Assert that the method are correctly affected by the policy
            Assert.IsTrue(policy.Matches(new MethodImplementationInfo(null, typeof(MockObjectWithCustomMethods).GetMethod("MethodWithIntInput"))));
            Assert.IsTrue(policy.Matches(new MethodImplementationInfo(null, typeof(MockObjectWithCustomMethods).GetMethod("MethodWithIntOutput"))));
            Assert.IsTrue(policy.Matches(new MethodImplementationInfo(null, typeof(MockObjectWithCustomMethods).GetMethod("MethodWithStringInput"))));
            Assert.IsTrue(policy.Matches(new MethodImplementationInfo(null, typeof(MockObjectWithCustomMethods).GetMethod("MethodWithDateTimeReturnValue"))));
            Assert.IsFalse(policy.Matches(new MethodImplementationInfo(null, typeof(MockObjectWithCustomMethods).GetMethod("MethodWithDoubleInputAndReturnValue"))));
        }

        [TestMethod]
        public void MatchingRuleHasTransientLifetime()
        {
            ParameterTypeMatchingRuleData ruleData = GetParameterTypeMatchingRuleData();
            TypeRegistration registration = ruleData.GetRegistrations("").First();

            Assert.AreEqual(TypeRegistrationLifetime.Transient, registration.Lifetime);
        }

        ParameterTypeMatchingRuleData GetParameterTypeMatchingRuleData()
        {
            return new ParameterTypeMatchingRuleData
            {
                Name = "Parameter Matching Rule",
                Matches = 
                    {
                        new ParameterTypeMatchData{ Match ="System.String", ParameterKind = ParameterKind.Input },
                        new ParameterTypeMatchData{ Match ="int32", ParameterKind = ParameterKind.InputOrOutput, IgnoreCase = true},
                        new ParameterTypeMatchData{ Match ="DateTime", ParameterKind = ParameterKind.ReturnValue, IgnoreCase = false},
                    }
            };
        }

        public class MockObjectWithCustomMethods
        {
            public void MethodWithIntInput(int param1) { }
            public void MethodWithIntOutput(out int param1) { param1 = 0; }
            public void MethodWithStringInput(string param1) { }
            public DateTime MethodWithDateTimeReturnValue() { return DateTime.MinValue; }
            public double MethodWithDoubleInputAndReturnValue(double param1) { return 0D; }
        }
        
        void AssertMatchDataEqual(ParameterTypeMatchData original,
                                  ParameterTypeMatchData rehydrated,
                                  string message,
                                  params object[] args)
        {
            Assert.AreEqual(original.Match, rehydrated.Match, message, args);
            Assert.AreEqual(original.ParameterKind, rehydrated.ParameterKind, message, args);
            Assert.AreEqual(original.IgnoreCase, rehydrated.IgnoreCase, message, args);
        }

        void AssertMatchDataEqual(ParameterTypeMatchData original,
                                  ParameterTypeMatchingInfo info,
                                  string message,
                                  params object[] args)
        {
            Assert.AreEqual(original.Match, info.Match, message, args);
            Assert.AreEqual(original.ParameterKind, info.Kind, message, args);
            Assert.AreEqual(original.IgnoreCase, info.IgnoreCase, message, args);
        }
    }
}

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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests
{
    [TestClass]
    public class ValidationCallHandlerFixture
    {
        [TestMethod]
        [DeploymentItem("Validation.config")]
        public void CanCreateValidationCallHandlerThroughFactory()
        {
            ValidationCallHandlerData validationCallHandler = new ValidationCallHandlerData("validationHandler");
            PolicySet policies = CreatePolicySetContainingCallHandler(validationCallHandler);

            ICallHandler runtimeHandler = ((RuleDrivenPolicy)policies[1]).Handlers[0];

            Assert.IsNotNull(runtimeHandler);
        }

        [TestMethod]
        [DeploymentItem("Validation.config")]
        public void ValidationCallHandlerDoesNothingIfValidationPasses()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Attributes, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Create<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(false, false));
        }

        [TestMethod]
        [DeploymentItem("Validation.config")]
        [ExpectedException(typeof(ArgumentValidationException))]
        public void ValidationCallHandlerThrowsArgumentValidationExceptionIfValidationFailsFromMetaData()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Attributes, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Create<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(true, false));
        }

        [TestMethod]
        [DeploymentItem("Validation.config")]
        [ExpectedException(typeof(ArgumentValidationException))]
        public void ValidationCallHandlerThrowsArgumentValidationExceptionIfValidationFailsFromConfiguration()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Configuration, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Create<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(false, true));
        }

        [TestMethod]
        [DeploymentItem("Validation.config")]
        public void ValidationCallHandlerIgnoresAttributeValidationIfSpecificationSourceIsConfig()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Configuration, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Create<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(true, false));
        }

        [TestMethod]
        [DeploymentItem("Validation.config")]
        public void ValidationCallHandlerIgnoresConfigurationValidationIfSpecificationSourceIsAttributes()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Attributes, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Create<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(false, true));
        }

        [TestMethod]
        [DeploymentItem("Validation.config")]
        [ExpectedException(typeof(ArgumentValidationException))]
        public void ShouldThrowIfValidationOnParameterAttributesFails()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Both,
                                new TypeMatchingRule("TestObject"));

            TestObject target = factory.Create<TestObject>(false, false);
            target.GetValueByKey(null);
        }

        [TestMethod]
        [DeploymentItem("Validation.config")]
        public void ShouldNotThrowIfValidationOnParameterAttributePasses()
        {
            RemotingPolicyInjector factory = new RemotingPolicyInjector();
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Both,
                                new TypeMatchingRule("TestObject"));

            TestObject target = factory.Create<TestObject>(false, false);
            target.GetValueByKey("key");
        }

        static PolicySet CreatePolicySetContainingCallHandler(ValidationCallHandlerData validationCallHandler)
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            PolicyData policyData = new PolicyData("policy");
            policyData.MatchingRules.Add(new CustomMatchingRuleData("match everything", typeof(AlwaysMatchingRule)));
            policyData.Handlers.Add(validationCallHandler);
            settings.Policies.Add(policyData);

            DictionaryConfigurationSource dictionaryConfigurationSource = new DictionaryConfigurationSource();
            dictionaryConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            PolicySetFactory factory = new PolicySetFactory(dictionaryConfigurationSource);
            PolicySet policies = factory.Create();
            return policies;
        }

        void AddValidationPolicy(PolicyInjector factory,
                                 string ruleSet,
                                 SpecificationSource specificationSource,
                                 IMatchingRule matchingRule)
        {
            RuleDrivenPolicy policy = new RuleDrivenPolicy();
            policy.RuleSet.Add(matchingRule);
            policy.Handlers.Add(new ValidationCallHandler(ruleSet, specificationSource));

            factory.Policies.Add(policy);
        }
    }

    public class ValidationFixtureTarget : MarshalByRefObject
    {
        public void AcceptTest(TestObject testObject) {}
    }

    public class TestObject : MarshalByRefObject
    {
        bool failAttributeBasedValidation;
        bool failConfigurationBasedValidation;

        public TestObject(bool failAttributeBasedValidation,
                          bool failConfigurationBasedValidation)
        {
            this.failAttributeBasedValidation = failAttributeBasedValidation;
            this.failConfigurationBasedValidation = failConfigurationBasedValidation;
        }

        [DomainValidator(1, 2, 3)]
        public int DomainValue
        {
            get
            {
                if (failAttributeBasedValidation)
                {
                    return 4;
                }
                return 1;
            }
        }

        public int DomainValue2
        {
            get
            {
                if (failConfigurationBasedValidation)
                {
                    return 4;
                }
                return 1;
            }
        }

        public int GetValueByKey([NotNullValidator] string key)
        {
            return 42;
        }
    }
}
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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests
{
    [TestClass]
    public class ValidationCallHandlerFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            ValidationFactory.SetDefaultConfigurationValidatorFactory(new SystemConfigurationSource(false));
        }

        [TestCleanup]
        public void TestCleanup()
        {
            ValidationFactory.Reset();
        }

        [TestMethod]
        public void CanCreateValidationCallHandlerThroughFactory()
        {
            ValidationCallHandlerData validationCallHandler = new ValidationCallHandlerData("validationHandler");
            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();

            RuleDrivenPolicy policy = CreatePolicySetContainingCallHandler(validationCallHandler, container);

            ICallHandler runtimeHandler
                = (policy.GetHandlersFor(new MethodImplementationInfo(null, (MethodInfo)MethodBase.GetCurrentMethod()), container)).ElementAt(0);

            Assert.IsNotNull(runtimeHandler);
        }

        [TestMethod]
        public void ValidationCallHandlerDoesNothingIfValidationPasses()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<ValidationFixtureTarget>(new TransparentProxyInterceptor());
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Attributes, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Resolve<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(false, false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentValidationException))]
        public void ValidationCallHandlerThrowsArgumentValidationExceptionIfValidationFailsFromMetaData()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<ValidationFixtureTarget>(new TransparentProxyInterceptor());
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Attributes, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Resolve<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(true, false));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentValidationException))]
        public void ValidationCallHandlerThrowsArgumentValidationExceptionIfValidationFailsFromConfiguration()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<ValidationFixtureTarget>(new TransparentProxyInterceptor());
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Configuration, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Resolve<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(false, true));
        }

        [TestMethod]
        public void ValidationCallHandlerIgnoresAttributeValidationIfSpecificationSourceIsConfig()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<ValidationFixtureTarget>(new TransparentProxyInterceptor());
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Configuration, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Resolve<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(true, false));
        }

        [TestMethod]
        public void ValidationCallHandlerIgnoresConfigurationValidationIfSpecificationSourceIsAttributes()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<ValidationFixtureTarget>(new TransparentProxyInterceptor());
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Attributes, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Resolve<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(false, true));
        }

        [TestMethod]
        public void ValidationCallHandlerIgnoresConfigurationAndAttributesValidationIfSpecificationSourceIsParameterAttributesOnly()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<ValidationFixtureTarget>(new TransparentProxyInterceptor());
            AddValidationPolicy(factory, string.Empty, SpecificationSource.ParameterAttributesOnly, new TypeMatchingRule("ValidationFixtureTarget"));
            ValidationFixtureTarget target = factory.Resolve<ValidationFixtureTarget>();

            target.AcceptTest(new TestObject(true, true));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentValidationException))]
        public void ShouldThrowIfValidationOnParameterAttributesFails()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<TestObject>(new TransparentProxyInterceptor());
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Both,
                                new TypeMatchingRule("TestObject"));

            TestObject target = factory.BuildUp<TestObject>(new TestObject(false, false));
            target.GetValueByKey(null);
        }

        [TestMethod]
        public void ShouldNotThrowIfValidationOnParameterAttributePasses()
        {
            IUnityContainer factory = new UnityContainer().AddNewExtension<Interception>();
            factory.Configure<Interception>().SetDefaultInterceptorFor<TestObject>(new TransparentProxyInterceptor());
            AddValidationPolicy(factory, string.Empty, SpecificationSource.Both,
                                new TypeMatchingRule("TestObject"));

            TestObject target = factory.BuildUp<TestObject>(new TestObject(false, false));
            target.GetValueByKey("key");
        }

        static RuleDrivenPolicy CreatePolicySetContainingCallHandler(
            ValidationCallHandlerData validationCallHandler,
            IUnityContainer container)
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();
            PolicyData policyData = new PolicyData("policy");
            policyData.MatchingRules.Add(new CustomMatchingRuleData("match everything", typeof(AlwaysMatchingRule)));
            policyData.Handlers.Add(validationCallHandler);
            settings.Policies.Add(policyData);

            DictionaryConfigurationSource dictionaryConfigurationSource = new DictionaryConfigurationSource();
            dictionaryConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            settings.ConfigureContainer(container);

            return container.Resolve<RuleDrivenPolicy>("policy");
        }

        void AddValidationPolicy(IUnityContainer factory,
                                 string ruleSet,
                                 SpecificationSource specificationSource,
                                 IMatchingRule matchingRule)
        {

            factory.Configure<Interception>().AddPolicy("Noop")
                .AddMatchingRule(matchingRule)
                .AddCallHandler(new ValidationCallHandler(ruleSet, specificationSource));
        }
    }

    public class ValidationFixtureTarget : MarshalByRefObject
    {
        public void AcceptTest(TestObject testObject) { }
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

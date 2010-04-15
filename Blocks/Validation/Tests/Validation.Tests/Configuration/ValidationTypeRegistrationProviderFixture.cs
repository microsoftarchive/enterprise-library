//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class GivenTypeRegistrationsBasedOnEmptyConfigurationSource
    {
        private IEnumerable<TypeRegistration> registrations;
        private IConfigurationSource configurationSource;

        [TestInitialize]
        public void Given()
        {
            var typeRegistrationProvider = new ValidationTypeRegistrationProvider();
            configurationSource = new DictionaryConfigurationSource();
            registrations = typeRegistrationProvider.GetRegistrations(configurationSource);
        }

        [TestMethod]
        public void ThenContainsRegistrationForValidationInstrumentationProvider()
        {
            var instrumentationProviderRegistration =
                registrations.Single(r => r.ServiceType == typeof(IValidationInstrumentationProvider));
            instrumentationProviderRegistration
                .AssertForServiceType(typeof(IValidationInstrumentationProvider))
                .IsDefault()
                .ForImplementationType(typeof(ValidationInstrumentationProvider));
        }

        [TestMethod]
        public void ThenValidationInstrumentationProviderUsesDefaultInstrumentationSettings()
        {
            var instrumentationProviderRegistration =
                registrations.Single(r => r.ServiceType == typeof(IValidationInstrumentationProvider));

            instrumentationProviderRegistration
                .AssertConstructor()
                .WithValueConstructorParameter(false)
                .WithValueConstructorParameter(false)
                .WithValueConstructorParameter(string.Empty)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenContainsRegistrationForDefaultNamedAttributeValidatorFactory()
        {
            var attributeFactoryRegistration =
               registrations.Single(r => r.ServiceType == typeof(AttributeValidatorFactory));

            attributeFactoryRegistration
                .AssertForServiceType(typeof(AttributeValidatorFactory))
                .IsDefault()
                .ForImplementationType(typeof(AttributeValidatorFactory));
        }

        [TestMethod]
        public void ThenAttributeRegistrationTargetsConstructorWithConfigurationSource()
        {
            var attributeFactoryRegistration =
               registrations.Single(r => r.ServiceType == typeof(AttributeValidatorFactory));

            attributeFactoryRegistration
                .AssertConstructor()
                .WithContainerResolvedParameter<IValidationInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenContainsRegistrationForDefaultNamedConfigurationValidatorFactory()
        {
            var configurationFactoryRegistration =
               registrations.Single(r => r.ServiceType == typeof(ConfigurationValidatorFactory));

            configurationFactoryRegistration
                .AssertForServiceType(typeof(ConfigurationValidatorFactory))
                .IsDefault()
                .ForImplementationType(typeof(ConfigurationValidatorFactory));
        }


        [TestMethod]
        public void ThenConfigurationRegistrationTargetsConstructorWithConfigurationSource()
        {
            var configurationFactoryRegistration =
               registrations.Single(r => r.ServiceType == typeof(ConfigurationValidatorFactory));

            configurationFactoryRegistration
                .AssertConstructor()
                .WithValueConstructorParameter(configurationSource)
                .WithContainerResolvedParameter<IValidationInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenContainsRegistrationForFactoryInstance()
        {
            var factoryRegistration = registrations.Single(r => r.ImplementationType == typeof(CompositeValidatorFactory));
            factoryRegistration
                .AssertForServiceType(typeof(ValidatorFactory))
                .IsDefault()
                .ForImplementationType(typeof(CompositeValidatorFactory));
        }

        [TestMethod]
        public void ThenCompositeRegistrationTargetsConstructorWithConfigurationSource()
        {
            var factoryRegistration = registrations.Single(r => r.ImplementationType == typeof(CompositeValidatorFactory));

            factoryRegistration
                .AssertConstructor()
                .WithContainerResolvedParameter<IValidationInstrumentationProvider>(null)
                .WithContainerResolvedParameter<AttributeValidatorFactory>(null)
                .WithContainerResolvedParameter<ConfigurationValidatorFactory>(null)
                .WithContainerResolvedParameter<ValidationAttributeValidatorFactory>(null)
                .VerifyConstructorParameters();
        }
    }

    [TestClass]
    public class GivenUpdatedTypeRegistrationsBasedOnEmptyConfigurationSource
    {
        private IEnumerable<TypeRegistration> registrations;
        private IConfigurationSource configurationSource;

        [TestInitialize]
        public void Given()
        {
            var typeRegistrationProvider = new ValidationTypeRegistrationProvider();
            configurationSource = new DictionaryConfigurationSource();
            registrations = typeRegistrationProvider.GetUpdatedRegistrations(configurationSource);
        }

        [TestMethod]
        public void ThenContainsRegistrationForValidationInstrumentationProvider()
        {
            var instrumentationProviderRegistration =
                registrations.Single(r => r.ServiceType == typeof(IValidationInstrumentationProvider));
            instrumentationProviderRegistration
                .AssertForServiceType(typeof(IValidationInstrumentationProvider))
                .IsDefault()
                .ForImplementationType(typeof(ValidationInstrumentationProvider));
        }

        [TestMethod]
        public void ThenValidationInstrumentationProviderUsesDefaultInstrumentationSettings()
        {
            var instrumentationProviderRegistration =
                registrations.Single(r => r.ServiceType == typeof(IValidationInstrumentationProvider));

            instrumentationProviderRegistration
                .AssertConstructor()
                .WithValueConstructorParameter(false)
                .WithValueConstructorParameter(false)
                .WithValueConstructorParameter(string.Empty)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenContainsRegistrationForDefaultNamedAttributeValidatorFactory()
        {
            var attributeFactoryRegistration =
               registrations.Single(r => r.ServiceType == typeof(AttributeValidatorFactory));

            attributeFactoryRegistration
                .AssertForServiceType(typeof(AttributeValidatorFactory))
                .IsDefault()
                .ForImplementationType(typeof(AttributeValidatorFactory));
        }

        [TestMethod]
        public void ThenAttributeRegistrationTargetsConstructorWithConfigurationSource()
        {
            var attributeFactoryRegistration =
               registrations.Single(r => r.ServiceType == typeof(AttributeValidatorFactory));

            attributeFactoryRegistration
                .AssertConstructor()
                .WithContainerResolvedParameter<IValidationInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenContainsRegistrationForDefaultNamedConfigurationValidatorFactory()
        {
            var configurationFactoryRegistration =
               registrations.Single(r => r.ServiceType == typeof(ConfigurationValidatorFactory));

            configurationFactoryRegistration
                .AssertForServiceType(typeof(ConfigurationValidatorFactory))
                .IsDefault()
                .ForImplementationType(typeof(ConfigurationValidatorFactory));
        }


        [TestMethod]
        public void ThenConfigurationRegistrationTargetsConstructorWithConfigurationSource()
        {
            var configurationFactoryRegistration =
               registrations.Single(r => r.ServiceType == typeof(ConfigurationValidatorFactory));

            configurationFactoryRegistration
                .AssertConstructor()
                .WithValueConstructorParameter(configurationSource)
                .WithContainerResolvedParameter<IValidationInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void ThenContainsRegistrationForFactoryInstance()
        {
            var factoryRegistration = registrations.Single(r => r.ImplementationType == typeof(CompositeValidatorFactory));
            factoryRegistration
                .AssertForServiceType(typeof(ValidatorFactory))
                .IsDefault()
                .ForImplementationType(typeof(CompositeValidatorFactory));
        }

        [TestMethod]
        public void ThenCompositeRegistrationTargetsConstructorWithConfigurationSource()
        {
            var factoryRegistration = registrations.Single(r => r.ImplementationType == typeof(CompositeValidatorFactory));

            factoryRegistration
                .AssertConstructor()
                .WithContainerResolvedParameter<IValidationInstrumentationProvider>(null)
                .WithContainerResolvedParameter<AttributeValidatorFactory>(null)
                .WithContainerResolvedParameter<ConfigurationValidatorFactory>(null)
                .WithContainerResolvedParameter<ValidationAttributeValidatorFactory>(null)
                .VerifyConstructorParameters();
        }
    }
    [TestClass]
    public class GivenContainerConfiguredForValidationWithNoConfigurationSourceSpecified
    {
        private IUnityContainer container;

        [TestInitialize]
        public void Given()
        {
            container = new UnityContainer();
            var typeRegistrationProvider = new ValidationTypeRegistrationProvider();
            var configurator = new UnityContainerConfigurator(container);
            configurator.RegisterAll(
                new DictionaryConfigurationSource(),
                typeRegistrationProvider);
        }

        [TestMethod]
        public void WhenValidatorIsRetrieved_ThenShouldProvideValidatorFromConfiguration()
        {
            var validatingObject = container.Resolve<SomeValidatingObject>();
            Assert.IsTrue(validatingObject.DoValidation());
        }

        internal class SomeValidatingObject
        {
            private Validator<TestObjectWithFailingAttributesOnProperties> validator;

            public SomeValidatingObject(CompositeValidatorFactory validationFactory)
            {
                validator = validationFactory.CreateValidator<TestObjectWithFailingAttributesOnProperties>("RuleA");
            }

            public bool DoValidation()
            {
                ValidationResults results = validator.Validate(new TestObjectWithFailingAttributesOnProperties());
                return !results.IsValid;
            }
        }
    }

    [TestClass]
    public class GivenContainerConfiguredForValidationFromSpecifiedConfigurationSource
    {
        private IUnityContainer container;

        [TestInitialize]
        public void Given()
        {
            container = new UnityContainer();
            var configurationSource = new DictionaryConfigurationSource();
            ValidationSettings settings = new ValidationSettings();
            configurationSource.Add(ValidationSettings.SectionName, settings);
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(BaseTestDomainObject));
            settings.Types.Add(typeReference);
            typeReference.DefaultRuleset = "RuleA";
            ValidationRulesetData ruleData = new ValidationRulesetData("RuleA");
            typeReference.Rulesets.Add(ruleData);
            ValidatedPropertyReference propertyReference1 = new ValidatedPropertyReference("Property1");
            ruleData.Properties.Add(propertyReference1);
            MockValidatorData validator11 = new MockValidatorData("validator1", true);
            propertyReference1.Validators.Add(validator11);
            validator11.MessageTemplate = "message-from-config1-RuleA";
            MockValidatorData validator12 = new MockValidatorData("validator2", true);
            propertyReference1.Validators.Add(validator12);
            validator12.MessageTemplate = "message-from-config2-RuleA";
            MockValidatorData validator13 = new MockValidatorData("validator3", false);
            propertyReference1.Validators.Add(validator13);
            validator13.MessageTemplate = "message-from-config3-RuleA";

            var typeRegistrationProvider = new ValidationTypeRegistrationProvider();
            var configurator = new UnityContainerConfigurator(container);
            configurator.RegisterAll(configurationSource, typeRegistrationProvider);
        }

        [TestMethod]
        public void WhenValidatorIsRetrieved_ThenShouldProvideValidatorFromConfiguration()
        {
            var validatingObject = container.Resolve<SomeValidatingObject>();
            Assert.IsTrue(validatingObject.DoValidation());
        }

        internal class SomeValidatingObject
        {
            private Validator<BaseTestDomainObject> validator;

            public SomeValidatingObject(CompositeValidatorFactory validationFactory)
            {
                validator = validationFactory.CreateValidator<BaseTestDomainObject>();
            }

            public bool DoValidation()
            {
                ValidationResults results = validator.Validate(new BaseTestDomainObject());
                return !results.IsValid;
            }
        }
    }





}

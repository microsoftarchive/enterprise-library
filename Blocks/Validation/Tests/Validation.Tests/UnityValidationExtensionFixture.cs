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

using System;
using System.Configuration;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    /// <summary>
    /// Summary description for UnityValidationExtensionFixture
    /// </summary>
    [TestClass]
    [Ignore]    // TODO move extension to other project, assumes validator classes are registered explicitly
    public class UnityValidationExtensionFixture
    {
        private IUnityContainer container;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer();
            var configurationSource =
                new SystemConfigurationSource();
            EnterpriseLibraryContainer.ConfigureContainer(new UnityContainerConfigurator(container), configurationSource);
        }

        [TestCleanup]
        public void Teardown()
        {
            container.Dispose();
        }

        [TestMethod]
        public void AddingEntlibExtensionAutomaticallyLoadsValidationExtension()
        {
            Assert.IsNotNull(container.Configure<ValidationBlockExtension>());
        }

        [TestMethod]
        public void CanResolveValidatorThroughContainer()
        {
            var validator = container.Resolve<Validator<TestObjectWithFailingAttributesOnProperties>>();

            Assert.IsNotNull(validator);
        }

        [TestMethod]
        public void ResolvedValidatorValidatesCorrectly()
        {
            var validator = container.Resolve<Validator<TestObjectWithFailingAttributesOnProperties>>();

            ValidationResults validationResults = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(4, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message2"));
        }

        [TestMethod]
        public void CanResolveValidatorBasedOnRuleset()
        {
            var validator = container.Resolve<Validator<TestObjectWithFailingAttributesOnProperties>>("RuleA");

            AssertValidatorIsForRuleSetA(validator);
        }

        [TestMethod]
        public void CanGetValidatorBasedOnAttributesOnly()
        {
            container.RegisterType<Validator<TestObjectWithFailingAttributesOnProperties>>(
                new InjectionValidationSource(ValidationSpecificationSource.Attributes));

            var validator = container.Resolve<Validator<TestObjectWithFailingAttributesOnProperties>>();

            AssertValidatorIsBasedOnAttributesOnly(validator);
        }

        [TestMethod]
        public void CanGetValidatorBasedOnConfigurationOnly()
        {
            container.RegisterType<Validator<TestObjectWithFailingAttributesOnProperties>>(
                new InjectionValidationSource(ValidationSpecificationSource.Configuration));

            var validator = container.Resolve<Validator<TestObjectWithFailingAttributesOnProperties>>();

            AssertValidatorIsBasedOnConfigOnly(validator);
        }

        [TestMethod]
        public void CanControlValidationSourceWithAttributesOnProperties()
        {
            var o = container.Resolve<ObjectWithValidatorProperties>();

            AssertValidatorIsBasedOnAttributesOnly(o.Val1);
        }

        [TestMethod]
        public void CanControlValidationSourceAndRuleSetWithAttributesOnProperties()
        {
            var o = container.Resolve<ObjectWithValidatorProperties>();

            ValidationResults validationResults
                = o.Val2.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        [TestMethod]
        public void CanControlValidationSourceAndRuleSetWhenConfiguringInjectionInAPI()
        {
            container.RegisterType<ObjectWithValidatorPropertiesAndNoAttributes>(
                new InjectionProperty("Val1",
                    new ValidatorParameter<TestObjectWithFailingAttributesOnProperties>("RuleA", ValidationSpecificationSource.Attributes)));

            var o = container.Resolve<ObjectWithValidatorPropertiesAndNoAttributes>();

            ValidationResults validationResults
                = o.Val1.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void APIThrowsWhenConfiguringWithMismatchedValidatorTypes()
        {
            container.RegisterType<ObjectWithValidatorPropertiesAndNoAttributes>(
                new InjectionProperty("Val1", new ValidatorParameter<object>()));
        }

        [TestMethod]
        public void CanOverrideValidatorSourceAtResolveTime()
        {
            container.RegisterType<ObjectWithValidatorPropertiesAndNoAttributes>(
                new InjectionProperty("Val1"));

            var o = container.Resolve<ObjectWithValidatorPropertiesAndNoAttributes>(
                new PropertyOverride("Val1",
                    new ValidatorParameter<TestObjectWithFailingAttributesOnProperties>(
                    "RuleA",
                    ValidationSpecificationSource.Attributes))
                .OnType<ObjectWithValidatorPropertiesAndNoAttributes>());

            AssertValidatorIsBasedOnAttributesOnlyForRuleSetA(o.Val1);
        }

        [TestMethod]
        public void AddingTheValidationBlockExtensionDoesNotConfigureTheContainer()
        {
            var container = new UnityContainer();

            container.AddNewExtension<ValidationBlockExtension>();

            Assert.AreEqual(1, container.Registrations.Count());        // account for the container registering itself
        }

        [TestMethod]
        public void CanConfigureValidationSourceFromUnityConfigurationToReadOnlyAttributes()
        {
            ConfigureContainer("attributesOnly");

            var result = container.Resolve<ObjectWithValidatorProperties>();

            AssertValidatorIsBasedOnAttributesOnly(result.Val1);
        }

        [TestMethod]
        public void CanConfigureValidationSourceFromUnityConfigurationToReadOnlyConfiguration()
        {
            ConfigureContainer("configOnly");

            var result = container.Resolve<ObjectWithValidatorProperties>();

            AssertValidatorIsBasedOnConfigOnly(result.Val1);
        }

        [TestMethod]
        public void CanConfigureValidationSourceFromUnityConfigurationForRulesetA()
        {
            ConfigureContainer("ruleSetA");

            var result = container.Resolve<ObjectWithValidatorProperties>();

            AssertValidatorIsForRuleSetA(result.Val1);
        }

        [TestMethod]
        public void CanConfigureValidationSourceAndRulesetFromUnityConfiguration()
        {
            ConfigureContainer("attributesOnlyRuleSetA");

            var result = container.Resolve<ObjectWithValidatorProperties>();

            AssertValidatorIsBasedOnAttributesOnlyForRuleSetA(result.Val1);
        }

        private void ConfigureContainer(string containerConfigName)
        {
            var section = (UnityConfigurationSection)ConfigurationManager.GetSection("unity");
            container.LoadConfiguration(section, containerConfigName);
        }

        private void AssertValidatorIsBasedOnAttributesOnly(Validator<TestObjectWithFailingAttributesOnProperties> v)
        {
            ValidationResults validationResults = v.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(2, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message2"));

        }

        private void AssertValidatorIsBasedOnConfigOnly(Validator<TestObjectWithFailingAttributesOnProperties> v)
        {
            ValidationResults validationResults = v.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(2, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2"));
        }

        private void AssertValidatorIsForRuleSetA(Validator<TestObjectWithFailingAttributesOnProperties> v)
        {
            var validationResults = v.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            var resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(3, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        private void AssertValidatorIsBasedOnAttributesOnlyForRuleSetA(Validator<TestObjectWithFailingAttributesOnProperties> v)
        {
            ValidationResults validationResults = v.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(1, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }

        public class ObjectWithValidatorProperties
        {
            [ValidatorDependency(ValidationSource = ValidationSpecificationSource.Attributes)]
            public Validator<TestObjectWithFailingAttributesOnProperties> Val1 { get; set; }

            [ValidatorDependency("RuleA", ValidationSource = ValidationSpecificationSource.Attributes)]
            public Validator<TestObjectWithFailingAttributesOnProperties> Val2 { get; set; }
        }

        public class ObjectWithValidatorPropertiesAndNoAttributes
        {
            public Validator<TestObjectWithFailingAttributesOnProperties> Val1 { get; set; }

            public Validator<TestObjectWithFailingAttributesOnProperties> Val2 { get; set; }
        }
    }
}

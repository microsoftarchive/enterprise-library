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
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests
{
    /// <summary>
    /// Summary description for UnityValidationExtensionFixture
    /// </summary>
    [TestClass]
    public class UnityValidationExtensionFixture
    {
        private IUnityContainer container;

        [TestInitialize]
        public void Setup()
        {
            container = new UnityContainer()
                .AddNewExtension<EnterpriseLibraryCoreExtension>();
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

            ValidationResults validationResults
                = validator.Validate(new TestObjectWithFailingAttributesOnProperties());

            Assert.IsFalse(validationResults.IsValid);
            IDictionary<string, ValidationResult> resultsMapping = ValidationTestHelper.GetResultsMapping(validationResults);
            Assert.AreEqual(3, resultsMapping.Count);
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config1-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message-from-config2-RuleA"));
            Assert.IsTrue(resultsMapping.ContainsKey("message1-RuleA"));
        }
    }
}

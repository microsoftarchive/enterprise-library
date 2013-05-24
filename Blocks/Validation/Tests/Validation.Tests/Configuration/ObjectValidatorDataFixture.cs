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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class ObjectValidatorDataFixture
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
        public void CanDeserializeSerializedInstanceWithNameOnly()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            ObjectValidatorData rwValidatorData = new ObjectValidatorData("validator1");
            rwSettings.Validators.Add(rwValidatorData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(ObjectValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(string.Empty, ((ObjectValidatorData)roSettings.Validators.Get(0)).TargetRuleset);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameAndTargetRuleset()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            ObjectValidatorData rwValidatorData = new ObjectValidatorData("validator1");
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.TargetRuleset = "ruleset";

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(ObjectValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual("ruleset", ((ObjectValidatorData)roSettings.Validators.Get(0)).TargetRuleset);
            }
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObject()
        {
            ObjectValidatorData rwValidatorData = new ObjectValidatorData("validator1");
            rwValidatorData.TargetRuleset = "ruleset";

            Validator validator =
                ((IValidatorDescriptor)rwValidatorData)
                    .CreateValidator(
                        typeof(ObjectValidatorDataFixture),
                        null,
                        null,
                        ValidationFactory.DefaultCompositeValidatorFactory);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ObjectValidator), validator.GetType());
            Assert.AreSame(typeof(ObjectValidatorDataFixture), ((ObjectValidator)validator).TargetType);
            Assert.AreEqual("ruleset", ((ObjectValidator)validator).TargetRuleset);
            Assert.AreEqual(null, ((ObjectValidator)validator).MessageTemplate);
        }
    }
}

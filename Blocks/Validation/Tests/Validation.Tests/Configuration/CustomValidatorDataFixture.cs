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
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class CustomValidatorDataFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameOnly()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            CustomValidatorData rwValidatorData = new CustomValidatorData("validator1", typeof(MockCustomValidator));
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
                Assert.AreSame(typeof(MockCustomValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(0, ((CustomValidatorData)roSettings.Validators.Get(0)).Attributes.Count);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithValuesSet()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            CustomValidatorData rwValidatorData = new CustomValidatorData("validator1", typeof(MockCustomValidator));
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.Attributes.Add("returnFailure", "true");
            rwValidatorData.Attributes.Add("foo", "bar");

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(MockCustomValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(2, ((CustomValidatorData)roSettings.Validators.Get(0)).Attributes.Count);
                Assert.AreEqual("true", ((CustomValidatorData)roSettings.Validators.Get(0)).Attributes["returnFailure"]);
                Assert.AreEqual("bar", ((CustomValidatorData)roSettings.Validators.Get(0)).Attributes["foo"]);
            }
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObject()
        {
            CustomValidatorData rwValidatorData = new CustomValidatorData("validator1", typeof(MockCustomValidator));
            rwValidatorData.Attributes.Add("returnFailure", "true");
            rwValidatorData.Attributes.Add("foo", "bar");

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(MockCustomValidator), validator.GetType());
            Assert.AreEqual(true, ((MockCustomValidator)validator).ReturnFailure);
            Assert.AreEqual(MockCustomValidator.DefaultMockValidatorMessageTemplate, ((MockCustomValidator)validator).MessageTemplate);
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            CustomValidatorData rwValidatorData = new CustomValidatorData("validator1", typeof(MockCustomValidator));
            rwValidatorData.Attributes.Add("returnFailure", "true");
            rwValidatorData.Attributes.Add("foo", "bar");
            rwValidatorData.MessageTemplate = "message template override";

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(MockCustomValidator), validator.GetType());
            Assert.AreEqual(true, ((MockCustomValidator)validator).ReturnFailure);
            Assert.AreEqual("message template override", ((MockCustomValidator)validator).MessageTemplate);
        }
    }
}

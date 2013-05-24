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
using Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class ValidatorDataFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameAndType()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);
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
                Assert.AreSame(typeof(MockValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).MessageTemplate);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).MessageTemplateResourceName);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).MessageTemplateResourceTypeName);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).Tag);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameAndTypeAndMessageTemplate()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.MessageTemplate = "message template";

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual("message template", roSettings.Validators.Get(0).MessageTemplate);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).MessageTemplateResourceName);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).MessageTemplateResourceTypeName);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).Tag);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameAndTypeAndResourceMessageTemplateInformation()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.MessageTemplateResourceName = "message template name";
            rwValidatorData.MessageTemplateResourceTypeName = "my type";

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).MessageTemplate);
                Assert.AreEqual("message template name", roSettings.Validators.Get(0).MessageTemplateResourceName);
                Assert.AreEqual("my type", roSettings.Validators.Get(0).MessageTemplateResourceTypeName);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).Tag);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameAndTypeAndTag()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.Tag = "tag";

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).MessageTemplate);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).MessageTemplateResourceName);
                Assert.AreEqual(string.Empty, roSettings.Validators.Get(0).MessageTemplateResourceTypeName);
                Assert.AreEqual("tag", roSettings.Validators.Get(0).Tag);
            }
        }

        [TestMethod]
        public void GetsNullForMessageTemplateIfNeitherLiteralOrResourceTemplateIsSet()
        {
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);

            Assert.AreEqual(null, rwValidatorData.GetMessageTemplate());
        }

        [TestMethod]
        public void GetsLiteralTemplateForMessageTemplateIfLiteralTemplateIsSet()
        {
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);
            rwValidatorData.MessageTemplate = "message template";

            Assert.AreEqual("message template", rwValidatorData.GetMessageTemplate());
        }

        [TestMethod]
        public void GetsResourceBasedTemplateForMessageTemplateIfResourceBasedTemplateTypeAndNameAreSet()
        {
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);
            rwValidatorData.MessageTemplateResourceName = "TestMessageTemplate";
            rwValidatorData.MessageTemplateResourceTypeName = typeof(Resources).AssemblyQualifiedName;

            Assert.AreEqual(Resources.TestMessageTemplate, rwValidatorData.GetMessageTemplate());
        }

        [TestMethod]
        public void GetsLiteralTemplateForMessageTemplateIfBothLiteralOrResourceTemplateAreSet()
        {
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);
            rwValidatorData.MessageTemplate = "message template";
            rwValidatorData.MessageTemplateResourceName = "TestMessageTemplate";
            rwValidatorData.MessageTemplateResourceTypeName = typeof(Resources).AssemblyQualifiedName;

            Assert.AreEqual("message template", rwValidatorData.GetMessageTemplate());
        }

        [TestMethod]
        public void CreatesValidatorWithNullTagIfTagNotSet()
        {
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.AreEqual(string.Empty, rwValidatorData.Tag);
            Assert.IsNotNull(validator);
            Assert.IsNull(validator.Tag);
        }

        [TestMethod]
        public void CreatesValidatorWithTagIfTagSet()
        {
            string tag = new string(new char[] { 'a', 'b', 'c' });
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);
            rwValidatorData.Tag = tag;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(tag, validator.Tag);
        }

        [TestMethod]
        public void CreatesValidatorWithSpecifiedTemplate()
        {
            ValidatorData rwValidatorData = new MockValidatorData("validator1", false);
            rwValidatorData.MessageTemplateResourceName = "TestMessageTemplate";
            rwValidatorData.MessageTemplateResourceTypeName = typeof(Resources).AssemblyQualifiedName;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreEqual(Resources.TestMessageTemplate, validator.MessageTemplate);
        }
    }
}

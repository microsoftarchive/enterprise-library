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
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class RegexValidatorDataFixture
    {
        const string RegexResourceName1 = "Regex1";
        const string RegexResourceName2 = "Regex2";
        const string RegexResourceName3 = "Regex3";

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
            RegexValidatorData rwValidatorData = new RegexValidatorData("validator1");
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
                Assert.AreSame(typeof(RegexValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual("", ((RegexValidatorData)roSettings.Validators.Get(0)).Pattern);
                Assert.AreEqual("", ((RegexValidatorData)roSettings.Validators.Get(0)).PatternResourceName);
                Assert.AreEqual(null, ((RegexValidatorData)roSettings.Validators.Get(0)).PatternResourceType);
                Assert.AreEqual(RegexOptions.None, ((RegexValidatorData)roSettings.Validators.Get(0)).Options);
                Assert.AreEqual(false, ((RegexValidatorData)roSettings.Validators.Get(0)).Negated);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithValuesSet()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            RegexValidatorData rwValidatorData = new RegexValidatorData("validator1");
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.Pattern = "pattern";
            rwValidatorData.PatternResourceName = RegexResourceName1;
            rwValidatorData.PatternResourceType = typeof(Resources);
            rwValidatorData.Negated = true;
            rwValidatorData.Options = RegexOptions.Multiline | RegexOptions.IgnoreCase;

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(RegexValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual("pattern", ((RegexValidatorData)roSettings.Validators.Get(0)).Pattern);
                Assert.AreEqual(RegexResourceName1, ((RegexValidatorData)roSettings.Validators.Get(0)).PatternResourceName);
                Assert.AreEqual(typeof(Resources), ((RegexValidatorData)roSettings.Validators.Get(0)).PatternResourceType);
                Assert.AreEqual(RegexOptions.Multiline | RegexOptions.IgnoreCase, ((RegexValidatorData)roSettings.Validators.Get(0)).Options);
                Assert.AreEqual(true, ((RegexValidatorData)roSettings.Validators.Get(0)).Negated);
            }
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObject()
        {
            RegexValidatorData rwValidatorData = new RegexValidatorData("validator1");
            rwValidatorData.Pattern = "pattern";
            rwValidatorData.PatternResourceName = RegexResourceName1;
            rwValidatorData.PatternResourceType = typeof(Resources);
            rwValidatorData.Options = RegexOptions.Multiline | RegexOptions.IgnoreCase;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(RegexValidator), validator.GetType());
            Assert.AreEqual("pattern", ((RegexValidator)validator).Pattern);
            Assert.AreEqual(RegexResourceName1, ((RegexValidator)validator).PatternResourceName);
            Assert.AreEqual(typeof(Resources), ((RegexValidator)validator).PatternResourceType);
            Assert.AreEqual(RegexOptions.Multiline | RegexOptions.IgnoreCase, ((RegexValidator)validator).Options);
            Assert.AreEqual(EnterpriseLibrary.Validation.Properties.Resources.RegexValidatorNonNegatedDefaultMessageTemplate, ((RegexValidator)validator).MessageTemplate);
            Assert.AreEqual(false, ((RegexValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateNegatedValidatorFromConfigurationObject()
        {
            RegexValidatorData rwValidatorData = new RegexValidatorData("validator1");
            rwValidatorData.Pattern = "pattern";
            rwValidatorData.PatternResourceName = RegexResourceName1;
            rwValidatorData.PatternResourceType = typeof(Resources);
            rwValidatorData.Negated = true;
            rwValidatorData.Options = RegexOptions.Multiline | RegexOptions.IgnoreCase;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(RegexValidator), validator.GetType());
            Assert.AreEqual("pattern", ((RegexValidator)validator).Pattern);
            Assert.AreEqual(RegexResourceName1, ((RegexValidator)validator).PatternResourceName);
            Assert.AreEqual(typeof(Resources), ((RegexValidator)validator).PatternResourceType);
            Assert.AreEqual(RegexOptions.Multiline | RegexOptions.IgnoreCase, ((RegexValidator)validator).Options);
            Assert.AreEqual(EnterpriseLibrary.Validation.Properties.Resources.RegexValidatorNegatedDefaultMessageTemplate, ((RegexValidator)validator).MessageTemplate);
            Assert.AreEqual(true, ((RegexValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            RegexValidatorData rwValidatorData = new RegexValidatorData("validator1");
            rwValidatorData.Pattern = "pattern";
            rwValidatorData.Options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            rwValidatorData.PatternResourceName = RegexResourceName1;
            rwValidatorData.PatternResourceType = typeof(Resources);
            rwValidatorData.MessageTemplate = "message template override";

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(RegexValidator), validator.GetType());
            Assert.AreEqual("pattern", ((RegexValidator)validator).Pattern);
            Assert.AreEqual(RegexResourceName1, ((RegexValidator)validator).PatternResourceName);
            Assert.AreEqual(typeof(Resources), ((RegexValidator)validator).PatternResourceType);
            Assert.AreEqual(RegexOptions.Multiline | RegexOptions.IgnoreCase, ((RegexValidator)validator).Options);
            Assert.AreEqual("message template override", ((RegexValidator)validator).MessageTemplate);
            Assert.AreEqual(false, ((RegexValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateNegatedValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            RegexValidatorData rwValidatorData = new RegexValidatorData("validator1");
            rwValidatorData.Pattern = "pattern";
            rwValidatorData.PatternResourceName = RegexResourceName1;
            rwValidatorData.PatternResourceType = typeof(Resources);
            rwValidatorData.Negated = true;
            rwValidatorData.Options = RegexOptions.Multiline | RegexOptions.IgnoreCase;
            rwValidatorData.MessageTemplate = "message template override";

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(RegexValidator), validator.GetType());
            Assert.AreEqual("pattern", ((RegexValidator)validator).Pattern);
            Assert.AreEqual(RegexResourceName1, ((RegexValidator)validator).PatternResourceName);
            Assert.AreEqual(typeof(Resources), ((RegexValidator)validator).PatternResourceType);
            Assert.AreEqual(RegexOptions.Multiline | RegexOptions.IgnoreCase, ((RegexValidator)validator).Options);
            Assert.AreEqual("message template override", ((RegexValidator)validator).MessageTemplate);
            Assert.AreEqual(true, ((RegexValidator)validator).Negated);
        }
    }
}

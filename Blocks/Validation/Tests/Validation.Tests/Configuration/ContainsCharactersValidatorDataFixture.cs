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
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class ContainsCharactersValidatorDataFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameOnly()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            ContainsCharactersValidatorData rwValidatorData = new ContainsCharactersValidatorData("validator1");
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
                Assert.AreSame(typeof(ContainsCharactersValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(string.Empty, ((ContainsCharactersValidatorData)roSettings.Validators.Get(0)).CharacterSet);
                Assert.AreEqual(ContainsCharacters.Any, ((ContainsCharactersValidatorData)roSettings.Validators.Get(0)).ContainsCharacters);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithValuesSet()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            ContainsCharactersValidatorData rwValidatorData = new ContainsCharactersValidatorData("validator1");
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.CharacterSet = "abc";
            rwValidatorData.ContainsCharacters = ContainsCharacters.All;
            rwValidatorData.Negated = true;

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(ContainsCharactersValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual("abc", ((ContainsCharactersValidatorData)roSettings.Validators.Get(0)).CharacterSet);
                Assert.AreEqual(ContainsCharacters.All, ((ContainsCharactersValidatorData)roSettings.Validators.Get(0)).ContainsCharacters);
                Assert.AreEqual(true, ((ContainsCharactersValidatorData)roSettings.Validators.Get(0)).Negated);
            }
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObject()
        {
            ContainsCharactersValidatorData rwValidatorData = new ContainsCharactersValidatorData("validator1");
            rwValidatorData.CharacterSet = "abc";
            rwValidatorData.ContainsCharacters = ContainsCharacters.All;
            rwValidatorData.Negated = true;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ContainsCharactersValidator), validator.GetType());
            Assert.AreEqual("abc", ((ContainsCharactersValidator)validator).CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, ((ContainsCharactersValidator)validator).ContainsCharacters);
            Assert.AreEqual(true, ((ContainsCharactersValidator)validator).Negated);
            Assert.AreEqual(Resources.ContainsCharactersNegatedDefaultMessageTemplate, validator.MessageTemplate);
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            ContainsCharactersValidatorData rwValidatorData = new ContainsCharactersValidatorData("validator1");
            rwValidatorData.CharacterSet = "abc";
            rwValidatorData.ContainsCharacters = ContainsCharacters.All;
            rwValidatorData.Negated = true;
            rwValidatorData.MessageTemplate = "message template override";

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(ContainsCharactersValidator), validator.GetType());
            Assert.AreEqual("abc", ((ContainsCharactersValidator)validator).CharacterSet);
            Assert.AreEqual(ContainsCharacters.All, ((ContainsCharactersValidator)validator).ContainsCharacters);
            Assert.AreEqual(true, ((ContainsCharactersValidator)validator).Negated);
            Assert.AreEqual("message template override", validator.MessageTemplate);
        }
    }
}

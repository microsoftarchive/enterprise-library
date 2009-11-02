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
    public class NotNotNullValidatorDataFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedInstance()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            ValidatorData rwValidatorData = new NotNullValidatorData("validator1");
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
                Assert.AreSame(typeof(NotNullValidator), roSettings.Validators.Get(0).Type);
            }
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObject()
        {
            NotNullValidatorData rwValidatorData = new NotNullValidatorData("validator1");

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(NotNullValidator), validator.GetType());
            Assert.AreEqual(Resources.NonNullNonNegatedValidatorDefaultMessageTemplate, ((NotNullValidator)validator).MessageTemplate);
            Assert.AreEqual(false, ((NotNullValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateNegatedValidatorFromConfigurationObject()
        {
            NotNullValidatorData rwValidatorData = new NotNullValidatorData("validator1");
            rwValidatorData.Negated = true;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(NotNullValidator), validator.GetType());
            Assert.AreEqual(Resources.NonNullNegatedValidatorDefaultMessageTemplate, ((NotNullValidator)validator).MessageTemplate);
            Assert.AreEqual(true, ((NotNullValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            NotNullValidatorData rwValidatorData = new NotNullValidatorData("validator1");
            rwValidatorData.MessageTemplate = "message template override";

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(NotNullValidator), validator.GetType());
            Assert.AreEqual("message template override", ((NotNullValidator)validator).MessageTemplate);
        }
    }
}

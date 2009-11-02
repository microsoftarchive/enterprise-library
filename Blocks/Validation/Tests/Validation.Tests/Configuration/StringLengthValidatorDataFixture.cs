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
    public class StringLengthValidatorDataFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameOnly()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            StringLengthValidatorData rwValidatorData = new StringLengthValidatorData("validator1");
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
                Assert.AreSame(typeof(StringLengthValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(0, ((StringLengthValidatorData)roSettings.Validators.Get(0)).LowerBound);
                Assert.AreEqual(RangeBoundaryType.Ignore, ((StringLengthValidatorData)roSettings.Validators.Get(0)).LowerBoundType);
                Assert.AreEqual(0, ((StringLengthValidatorData)roSettings.Validators.Get(0)).UpperBound);
                Assert.AreEqual(RangeBoundaryType.Inclusive, ((StringLengthValidatorData)roSettings.Validators.Get(0)).UpperBoundType);
                Assert.AreEqual(false, ((StringLengthValidatorData)roSettings.Validators.Get(0)).Negated);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithValuesSet()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            StringLengthValidatorData rwValidatorData = new StringLengthValidatorData("validator1");
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.LowerBound = 10;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = 20;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Inclusive;

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(StringLengthValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(10, ((StringLengthValidatorData)roSettings.Validators.Get(0)).LowerBound);
                Assert.AreEqual(RangeBoundaryType.Exclusive, ((StringLengthValidatorData)roSettings.Validators.Get(0)).LowerBoundType);
                Assert.AreEqual(20, ((StringLengthValidatorData)roSettings.Validators.Get(0)).UpperBound);
                Assert.AreEqual(RangeBoundaryType.Inclusive, ((StringLengthValidatorData)roSettings.Validators.Get(0)).UpperBoundType);
                Assert.AreEqual(false, ((StringLengthValidatorData)roSettings.Validators.Get(0)).Negated);
            }
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObject()
        {
            StringLengthValidatorData rwValidatorData = new StringLengthValidatorData("validator1");
            rwValidatorData.LowerBound = 10;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = 20;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Inclusive;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(StringLengthValidator), validator.GetType());
            Assert.AreEqual(10, ((StringLengthValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((StringLengthValidator)validator).LowerBoundType);
            Assert.AreEqual(20, ((StringLengthValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, ((StringLengthValidator)validator).UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNonNegatedDefaultMessageTemplate, ((StringLengthValidator)validator).MessageTemplate);
            Assert.AreEqual(false, ((StringLengthValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateNegatedValidatorFromConfigurationObject()
        {
            StringLengthValidatorData rwValidatorData = new StringLengthValidatorData("validator1");
            rwValidatorData.LowerBound = 10;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = 20;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Inclusive;
            rwValidatorData.Negated = true;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(StringLengthValidator), validator.GetType());
            Assert.AreEqual(10, ((StringLengthValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((StringLengthValidator)validator).LowerBoundType);
            Assert.AreEqual(20, ((StringLengthValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, ((StringLengthValidator)validator).UpperBoundType);
            Assert.AreEqual(Resources.StringLengthValidatorNegatedDefaultMessageTemplate, ((StringLengthValidator)validator).MessageTemplate);
            Assert.AreEqual(true, ((StringLengthValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            StringLengthValidatorData rwValidatorData = new StringLengthValidatorData("validator1");
            rwValidatorData.LowerBound = 10;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = 20;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Inclusive;
            rwValidatorData.MessageTemplate = "message template override";

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(StringLengthValidator), validator.GetType());
            Assert.AreEqual(10, ((StringLengthValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((StringLengthValidator)validator).LowerBoundType);
            Assert.AreEqual(20, ((StringLengthValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, ((StringLengthValidator)validator).UpperBoundType);
            Assert.AreEqual("message template override", ((StringLengthValidator)validator).MessageTemplate);
            Assert.AreEqual(false, ((StringLengthValidator)validator).Negated);
        }

        [TestMethod]
        public void CanCreateNegatedValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            StringLengthValidatorData rwValidatorData = new StringLengthValidatorData("validator1");
            rwValidatorData.LowerBound = 10;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = 20;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Inclusive;
            rwValidatorData.MessageTemplate = "message template override";
            rwValidatorData.Negated = true;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(StringLengthValidator), validator.GetType());
            Assert.AreEqual(10, ((StringLengthValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((StringLengthValidator)validator).LowerBoundType);
            Assert.AreEqual(20, ((StringLengthValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, ((StringLengthValidator)validator).UpperBoundType);
            Assert.AreEqual("message template override", ((StringLengthValidator)validator).MessageTemplate);
            Assert.AreEqual(true, ((StringLengthValidator)validator).Negated);
        }
    }
}

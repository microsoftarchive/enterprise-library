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
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class RangeValidatorDataFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameOnly()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            RangeValidatorData rwValidatorData = new RangeValidatorData("validator1");
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
                Assert.AreSame(typeof(RangeValidator), roSettings.Validators.Get(0).Type);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithValuesSet()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            RangeValidatorData rwValidatorData = new RangeValidatorData("validator1");
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.Negated = true;
            rwValidatorData.LowerBound = "12";
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = "24";
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
                Assert.AreSame(typeof(RangeValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(true, ((RangeValidatorData)roSettings.Validators.Get(0)).Negated);
                Assert.AreEqual("12", ((RangeValidatorData)roSettings.Validators.Get(0)).LowerBound);
                Assert.AreEqual(RangeBoundaryType.Exclusive, ((RangeValidatorData)roSettings.Validators.Get(0)).LowerBoundType);
                Assert.AreEqual("24", ((RangeValidatorData)roSettings.Validators.Get(0)).UpperBound);
                Assert.AreEqual(RangeBoundaryType.Inclusive, ((RangeValidatorData)roSettings.Validators.Get(0)).UpperBoundType);
            }
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObject()
        {
            RangeValidatorData rwValidatorData = new RangeValidatorData("validator1");
            rwValidatorData.Negated = true;
            rwValidatorData.LowerBound = "12";
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = "24";
            rwValidatorData.UpperBoundType = RangeBoundaryType.Inclusive;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(typeof(int), null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(RangeValidator), validator.GetType());
            Assert.AreEqual(Resources.RangeValidatorNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, ((RangeValidator)validator).Negated);
            Assert.AreEqual(12, ((RangeValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((RangeValidator)validator).LowerBoundType);
            Assert.AreEqual(24, ((RangeValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, ((RangeValidator)validator).UpperBoundType);
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            RangeValidatorData rwValidatorData = new RangeValidatorData("validator1");
            rwValidatorData.MessageTemplate = "message template override";
            rwValidatorData.Negated = true;
            rwValidatorData.LowerBound = "12";
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = "24";
            rwValidatorData.UpperBoundType = RangeBoundaryType.Inclusive;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(typeof(string), null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(RangeValidator), validator.GetType());
            Assert.AreEqual("message template override", validator.MessageTemplate);
            Assert.AreEqual(true, ((RangeValidator)validator).Negated);
            Assert.AreEqual("12", ((RangeValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((RangeValidator)validator).LowerBoundType);
            Assert.AreEqual("24", ((RangeValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, ((RangeValidator)validator).UpperBoundType);
        }
    }
}
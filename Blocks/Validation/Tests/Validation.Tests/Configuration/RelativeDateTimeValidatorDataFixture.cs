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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class RelativeDateTimeValidatorDataFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameOnly()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            RelativeDateTimeValidatorData rwValidatorData = new RelativeDateTimeValidatorData("validator1");
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
                Assert.AreSame(typeof(RelativeDateTimeValidator), roSettings.Validators.Get(0).Type);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithValuesSet()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            RelativeDateTimeValidatorData rwValidatorData = new RelativeDateTimeValidatorData("validator1");
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.Negated = true;
            rwValidatorData.LowerBound = 2;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.LowerUnit = DateTimeUnit.Year;
            rwValidatorData.UpperBound = 3;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Ignore;
            rwValidatorData.UpperUnit = DateTimeUnit.Month;

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                MockValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as MockValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Validators.Count);
                Assert.AreEqual("validator1", roSettings.Validators.Get(0).Name);
                Assert.AreSame(typeof(RelativeDateTimeValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(true, ((RelativeDateTimeValidatorData)roSettings.Validators.Get(0)).Negated);
                Assert.AreEqual(2, ((RelativeDateTimeValidatorData)roSettings.Validators.Get(0)).LowerBound);
                Assert.AreEqual(RangeBoundaryType.Exclusive, ((RelativeDateTimeValidatorData)roSettings.Validators.Get(0)).LowerBoundType);
                Assert.AreEqual(DateTimeUnit.Year, ((RelativeDateTimeValidatorData)roSettings.Validators.Get(0)).LowerUnit);
                Assert.AreEqual(3, ((RelativeDateTimeValidatorData)roSettings.Validators.Get(0)).UpperBound);
                Assert.AreEqual(RangeBoundaryType.Ignore, ((RelativeDateTimeValidatorData)roSettings.Validators.Get(0)).UpperBoundType);
                Assert.AreEqual(DateTimeUnit.Month, ((RelativeDateTimeValidatorData)roSettings.Validators.Get(0)).UpperUnit);
            }
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObject()
        {
            RelativeDateTimeValidatorData rwValidatorData = new RelativeDateTimeValidatorData("validator1");
            rwValidatorData.Negated = true;
            rwValidatorData.Negated = true;
            rwValidatorData.LowerBound = 2;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.LowerUnit = DateTimeUnit.Year;
            rwValidatorData.UpperBound = 3;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Ignore;
            rwValidatorData.UpperUnit = DateTimeUnit.Month;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(RelativeDateTimeValidator), validator.GetType());
            Assert.AreEqual(Resources.RelativeDateTimeNegatedDefaultMessageTemplate, validator.MessageTemplate);
            Assert.AreEqual(true, ((RelativeDateTimeValidator)validator).Negated);
            Assert.AreEqual(2, ((RelativeDateTimeValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((RelativeDateTimeValidator)validator).LowerBoundType);
            Assert.AreEqual(DateTimeUnit.Year, ((RelativeDateTimeValidator)validator).LowerUnit);
            Assert.AreEqual(3, ((RelativeDateTimeValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, ((RelativeDateTimeValidator)validator).UpperBoundType);
            Assert.AreEqual(DateTimeUnit.Month, ((RelativeDateTimeValidator)validator).UpperUnit);
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            RelativeDateTimeValidatorData rwValidatorData = new RelativeDateTimeValidatorData("validator1");
            rwValidatorData.MessageTemplate = "message template override";
            rwValidatorData.Negated = true;
            rwValidatorData.Negated = true;
            rwValidatorData.Negated = true;
            rwValidatorData.LowerBound = 2;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.LowerUnit = DateTimeUnit.Year;
            rwValidatorData.UpperBound = 3;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Ignore;
            rwValidatorData.UpperUnit = DateTimeUnit.Month;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(RelativeDateTimeValidator), validator.GetType());
            Assert.AreEqual("message template override", validator.MessageTemplate);
            Assert.AreEqual(true, ((RelativeDateTimeValidator)validator).Negated);
            Assert.AreEqual(2, ((RelativeDateTimeValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((RelativeDateTimeValidator)validator).LowerBoundType);
            Assert.AreEqual(DateTimeUnit.Year, ((RelativeDateTimeValidator)validator).LowerUnit);
            Assert.AreEqual(3, ((RelativeDateTimeValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Ignore, ((RelativeDateTimeValidator)validator).UpperBoundType);
            Assert.AreEqual(DateTimeUnit.Month, ((RelativeDateTimeValidator)validator).UpperUnit);
        }
    }
}

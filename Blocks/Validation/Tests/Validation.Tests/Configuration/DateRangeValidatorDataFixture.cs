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
    public class DateRangeValidatorDataFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameOnly()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            DateTimeRangeValidatorData rwValidatorData = new DateTimeRangeValidatorData("validator1");
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
                Assert.AreSame(typeof(DateTimeRangeValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(default(DateTime), ((DateTimeRangeValidatorData)roSettings.Validators.Get(0)).LowerBound);
                Assert.AreEqual(RangeBoundaryType.Ignore, ((DateTimeRangeValidatorData)roSettings.Validators.Get(0)).LowerBoundType);
                Assert.AreEqual(default(DateTime), ((DateTimeRangeValidatorData)roSettings.Validators.Get(0)).UpperBound);
                Assert.AreEqual(RangeBoundaryType.Inclusive, ((DateTimeRangeValidatorData)roSettings.Validators.Get(0)).UpperBoundType);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithValuesSet()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            MockValidationSettings rwSettings = new MockValidationSettings();
            DateTimeRangeValidatorData rwValidatorData = new DateTimeRangeValidatorData("validator1");
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.LowerBound = lowerBound;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = upperBound;
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
                Assert.AreSame(typeof(DateTimeRangeValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(lowerBound, ((DateTimeRangeValidatorData)roSettings.Validators.Get(0)).LowerBound);
                Assert.AreEqual(RangeBoundaryType.Exclusive, ((DateTimeRangeValidatorData)roSettings.Validators.Get(0)).LowerBoundType);
                Assert.AreEqual(upperBound, ((DateTimeRangeValidatorData)roSettings.Validators.Get(0)).UpperBound);
                Assert.AreEqual(RangeBoundaryType.Inclusive, ((DateTimeRangeValidatorData)roSettings.Validators.Get(0)).UpperBoundType);
            }
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObject()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidatorData rwValidatorData = new DateTimeRangeValidatorData("validator1");
            rwValidatorData.LowerBound = lowerBound;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = upperBound;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Inclusive;

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(DateTimeRangeValidator), validator.GetType());
            Assert.AreEqual(lowerBound, ((DateTimeRangeValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((DateTimeRangeValidator)validator).LowerBoundType);
            Assert.AreEqual(upperBound, ((DateTimeRangeValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, ((DateTimeRangeValidator)validator).UpperBoundType);
            Assert.AreEqual(Resources.RangeValidatorNonNegatedDefaultMessageTemplate, ((DateTimeRangeValidator)validator).MessageTemplate);
        }

        [TestMethod]
        public void CanCreateValidatorFromConfigurationObjectWithMessageTemplateOverride()
        {
            DateTime lowerBound = new DateTime(2006, 1, 1);
            DateTime upperBound = new DateTime(2006, 1, 10);

            DateTimeRangeValidatorData rwValidatorData = new DateTimeRangeValidatorData("validator1");
            rwValidatorData.LowerBound = lowerBound;
            rwValidatorData.LowerBoundType = RangeBoundaryType.Exclusive;
            rwValidatorData.UpperBound = upperBound;
            rwValidatorData.UpperBoundType = RangeBoundaryType.Inclusive;
            rwValidatorData.MessageTemplate = "message template override";

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(null, null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(DateTimeRangeValidator), validator.GetType());
            Assert.AreEqual(lowerBound, ((DateTimeRangeValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((DateTimeRangeValidator)validator).LowerBoundType);
            Assert.AreEqual(upperBound, ((DateTimeRangeValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, ((DateTimeRangeValidator)validator).UpperBoundType);
            Assert.AreEqual("message template override", ((DateTimeRangeValidator)validator).MessageTemplate);
        }
    }
}

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
using System.Globalization;
using System;

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
            rwValidatorData.Culture = CultureInfo.GetCultureInfo("nl-NL");
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
                Assert.AreEqual(CultureInfo.GetCultureInfo("nl-NL"), rwValidatorData.Culture);
            }
        }

        [TestMethod]
        public void CanCreateValidatorWithSpecificCulture()
        {
            MockRangeValidatorData validatorData = new MockRangeValidatorData();
            
            validatorData.Culture = CultureInfo.GetCultureInfo("nl-NL");
            validatorData.LowerBound = "12,4";
            validatorData.LowerBoundType = RangeBoundaryType.Inclusive;
            validatorData.UpperBound = "24,4";
            validatorData.UpperBoundType = RangeBoundaryType.Inclusive;

            var validator = validatorData.CreateValidator(typeof(double));
            Assert.IsFalse(validator.Validate(12.3d).IsValid);
            Assert.IsTrue(validator.Validate(13.3d).IsValid);
        }

        [TestMethod]
        public void CanCreateValidatorWithoutCulture()
        {
            MockRangeValidatorData validatorData = new MockRangeValidatorData();

            validatorData.Culture = null;
            validatorData.LowerBound = (12.4d).ToString(CultureInfo.CurrentCulture);
            validatorData.LowerBoundType = RangeBoundaryType.Inclusive;
            validatorData.UpperBound = (22.4d).ToString(CultureInfo.CurrentCulture);
            validatorData.UpperBoundType = RangeBoundaryType.Inclusive;

            var validator = validatorData.CreateValidator(typeof(double));
            Assert.IsFalse(validator.Validate(12.3d).IsValid);
            Assert.IsTrue(validator.Validate(13.3d).IsValid);
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

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(typeof(int), null, null, null);

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

            Validator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(typeof(string), null, null, null);

            Assert.IsNotNull(validator);
            Assert.AreSame(typeof(RangeValidator), validator.GetType());
            Assert.AreEqual("message template override", validator.MessageTemplate);
            Assert.AreEqual(true, ((RangeValidator)validator).Negated);
            Assert.AreEqual("12", ((RangeValidator)validator).LowerBound);
            Assert.AreEqual(RangeBoundaryType.Exclusive, ((RangeValidator)validator).LowerBoundType);
            Assert.AreEqual("24", ((RangeValidator)validator).UpperBound);
            Assert.AreEqual(RangeBoundaryType.Inclusive, ((RangeValidator)validator).UpperBoundType);
        }


        private class MockRangeValidatorData : RangeValidatorData
        {
            public Validator CreateValidator(Type targetType)
            {
                return base.DoCreateValidator(targetType);
            }
        }
    }
}

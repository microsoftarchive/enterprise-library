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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class PropertyComparisonValidatorDataFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedInstanceWithNameOnly()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            PropertyComparisonValidatorData rwValidatorData = new PropertyComparisonValidatorData("validator1");
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
                Assert.AreSame(typeof(PropertyComparisonValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(ComparisonOperator.Equal, ((PropertyComparisonValidatorData)roSettings.Validators.Get(0)).ComparisonOperator);
                Assert.AreEqual("", ((PropertyComparisonValidatorData)roSettings.Validators.Get(0)).PropertyToCompare);
                Assert.AreEqual(false, ((PropertyComparisonValidatorData)roSettings.Validators.Get(0)).Negated);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedInstanceWithValuesSet()
        {
            MockValidationSettings rwSettings = new MockValidationSettings();
            PropertyComparisonValidatorData rwValidatorData = new PropertyComparisonValidatorData("validator1");
            rwSettings.Validators.Add(rwValidatorData);
            rwValidatorData.ComparisonOperator = ComparisonOperator.GreaterThanEqual;
            rwValidatorData.PropertyToCompare = "property";
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
                Assert.AreSame(typeof(PropertyComparisonValidator), roSettings.Validators.Get(0).Type);
                Assert.AreEqual(ComparisonOperator.GreaterThanEqual, ((PropertyComparisonValidatorData)roSettings.Validators.Get(0)).ComparisonOperator);
                Assert.AreEqual("property", ((PropertyComparisonValidatorData)roSettings.Validators.Get(0)).PropertyToCompare);
                Assert.AreEqual(true, ((PropertyComparisonValidatorData)roSettings.Validators.Get(0)).Negated);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void CreateValidatorWithNoPropertyThrows()
        {
            PropertyComparisonValidatorData rwValidatorData = new PropertyComparisonValidatorData("validator1");

            ((IValidatorDescriptor)rwValidatorData).CreateValidator(typeof(string),
                                                                    typeof(PropertyComparisonValidatorDataFixtureTestClass),
                                                                    new ReflectionMemberValueAccessBuilder(),
                                                                    null);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void CreateValidatorWithInvalidPropertyThrows()
        {
            PropertyComparisonValidatorData rwValidatorData = new PropertyComparisonValidatorData("validator1");
            rwValidatorData.PropertyToCompare = "Property";

            PropertyComparisonValidator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(typeof(string),
                                                                                                            typeof(PropertyComparisonValidatorDataFixtureTestClass),
                                                                                                            new ReflectionMemberValueAccessBuilder(),
                                                                                                            null) as PropertyComparisonValidator;

            Assert.IsNotNull(validator);
            Assert.AreEqual("PublicProperty", ((PropertyValueAccess)validator.ValueAccess).PropertyInfo.Name);
            Assert.AreEqual(ComparisonOperator.NotEqual, validator.ComparisonOperator);
            Assert.AreEqual(true, validator.Negated);
        }

        [TestMethod]
        public void CanCreateValidator()
        {
            PropertyComparisonValidatorData rwValidatorData = new PropertyComparisonValidatorData("validator1");
            rwValidatorData.PropertyToCompare = "PublicProperty";
            rwValidatorData.Negated = true;
            rwValidatorData.ComparisonOperator = ComparisonOperator.NotEqual;

            PropertyComparisonValidator validator = ((IValidatorDescriptor)rwValidatorData).CreateValidator(typeof(string),
                                                                                                            typeof(PropertyComparisonValidatorDataFixtureTestClass),
                                                                                                            new ReflectionMemberValueAccessBuilder(),
                                                                                                            null) as PropertyComparisonValidator;

            Assert.IsNotNull(validator);
            Assert.AreEqual("PublicProperty", ((PropertyValueAccess)validator.ValueAccess).PropertyInfo.Name);
            Assert.AreEqual(ComparisonOperator.NotEqual, validator.ComparisonOperator);
            Assert.AreEqual(true, validator.Negated);
        }

        public class PropertyComparisonValidatorDataFixtureTestClass
        {
            string property;

            public string PublicProperty
            {
                get { return property; }
                set { property = value; }
            }
        }
    }
}

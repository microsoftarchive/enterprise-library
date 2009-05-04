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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Tests.Configuration
{
    [TestClass]
    public class ConfigurationSectionSerializationFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedEmptySection()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(0, roSettings.Types.Count);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedSectionWithEmptyType()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            rwSettings.Types.Add(new ValidatedTypeReference(typeof(string)));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Types.Count);
                Assert.AreEqual(typeof(string).FullName, roSettings.Types.Get(0).Name);
                Assert.AreEqual("", roSettings.Types.Get(0).DefaultRuleset);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedSectionWithEmptyTypeWithDefaultRule()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            ValidatedTypeReference typeReference = new ValidatedTypeReference(typeof(string));
            rwSettings.Types.Add(typeReference);
            typeReference.DefaultRuleset = "defaultRule";

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Types.Count);
                Assert.AreEqual(typeof(string).FullName, roSettings.Types.Get(0).Name);
                Assert.AreEqual("defaultRule", roSettings.Types.Get(0).DefaultRuleset);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedSectionWithManyEmptyTypes()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            rwSettings.Types.Add(new ValidatedTypeReference(typeof(string)));
            rwSettings.Types.Add(new ValidatedTypeReference(typeof(int)));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(2, roSettings.Types.Count);
                Assert.AreEqual(typeof(string).FullName, roSettings.Types.Get(0).Name);
                Assert.AreEqual(typeof(int).FullName, roSettings.Types.Get(1).Name);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedSectionWithTypeWithEmptyNamedRule()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            ValidatedTypeReference rwStringType = new ValidatedTypeReference(typeof(string));
            rwSettings.Types.Add(rwStringType);
            rwStringType.Rulesets.Add(new ValidationRulesetData("ruleset"));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Types.Count);
                Assert.AreEqual(typeof(string).FullName, roSettings.Types.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Count);
                Assert.AreEqual("ruleset", roSettings.Types.Get(0).Rulesets.Get(0).Name);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedSectionWithTypeWithValidatorsForNamedRule()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            ValidatedTypeReference rwStringType = new ValidatedTypeReference(typeof(string));
            rwSettings.Types.Add(rwStringType);
            ValidationRulesetData rwValidationRule = new ValidationRulesetData("ruleset");
            rwStringType.Rulesets.Add(rwValidationRule);
            ValidatorData rwValidatorData = new MockValidatorData("validator1", true);
            rwValidationRule.Validators.Add(rwValidatorData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Types.Count);
                Assert.AreEqual(typeof(string).FullName, roSettings.Types.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Count);
                Assert.AreEqual("ruleset", roSettings.Types.Get(0).Rulesets.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Validators.Count);
                Assert.AreEqual("validator1", roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).GetType());
                Assert.AreEqual(true, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0)).ReturnFailure);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedSectionWithNamedRuleSpecifyingValidatorForProperty()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            ValidatedTypeReference rwStringType = new ValidatedTypeReference(typeof(string));
            rwSettings.Types.Add(rwStringType);
            ValidationRulesetData rwValidationRule = new ValidationRulesetData("ruleset");
            rwStringType.Rulesets.Add(rwValidationRule);
            ValidatorData rwValidatorData = new MockValidatorData("validator1", true);
            rwValidationRule.Validators.Add(rwValidatorData);
            ValidatedPropertyReference rwValidationPropertyReference = new ValidatedPropertyReference("System.String.Length");
            rwValidationRule.Properties.Add(rwValidationPropertyReference);
            ValidatorData rwPropertyValidatorData = new MockValidatorData("validator2", false);
            rwValidationPropertyReference.Validators.Add(rwPropertyValidatorData);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Types.Count);
                Assert.AreEqual(typeof(string).FullName, roSettings.Types.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Count);
                Assert.AreEqual("ruleset", roSettings.Types.Get(0).Rulesets.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Validators.Count);
                Assert.AreEqual("validator1", roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).GetType());
                Assert.AreEqual(true, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0)).ReturnFailure);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Properties.Count);
                Assert.AreEqual("System.String.Length", roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Count);
                Assert.AreEqual("validator2", roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Get(0).GetType());
                Assert.AreEqual(false, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Get(0)).ReturnFailure);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedSectionWithNamedRuleSpecifyingMultipleValidatorsForProperty()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            ValidatedTypeReference rwStringType = new ValidatedTypeReference(typeof(string));
            rwSettings.Types.Add(rwStringType);
            ValidationRulesetData rwValidationRule = new ValidationRulesetData("ruleset");
            rwStringType.Rulesets.Add(rwValidationRule);
            ValidatorData rwValidatorData = new MockValidatorData("validator1", true);
            rwValidationRule.Validators.Add(rwValidatorData);
            ValidatedPropertyReference rwValidationPropertyReference = new ValidatedPropertyReference("System.String.Length");
            rwValidationRule.Properties.Add(rwValidationPropertyReference);
            ValidatorData rwPropertyValidatorData1 = new MockValidatorData("ruleset-validator1", false);
            rwValidationPropertyReference.Validators.Add(rwPropertyValidatorData1);
            ValidatorData rwPropertyValidatorData2 = new MockValidatorData("ruleset-validator2", false);
            rwValidationPropertyReference.Validators.Add(rwPropertyValidatorData2);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Types.Count);
                Assert.AreEqual(typeof(string).FullName, roSettings.Types.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Count);
                Assert.AreEqual("ruleset", roSettings.Types.Get(0).Rulesets.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Validators.Count);
                Assert.AreEqual("validator1", roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).GetType());
                Assert.AreEqual(true, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0)).ReturnFailure);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Properties.Count);
                Assert.AreEqual("System.String.Length", roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Name);
                Assert.AreEqual(2, roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Count);
                Assert.AreEqual("ruleset-validator1", roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Get(0).GetType());
                Assert.AreEqual(false, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Get(0)).ReturnFailure);
                Assert.AreEqual("ruleset-validator2", roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Get(1).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Get(1).GetType());
                Assert.AreEqual(false, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Properties.Get(0).Validators.Get(1)).ReturnFailure);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedSectionWithNamedRuleSpecifyingMultipleValidatorsForMethod()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            ValidatedTypeReference rwStringType = new ValidatedTypeReference(typeof(string));
            rwSettings.Types.Add(rwStringType);
            ValidationRulesetData rwValidationRule = new ValidationRulesetData("ruleset");
            rwStringType.Rulesets.Add(rwValidationRule);
            ValidatorData rwValidatorData = new MockValidatorData("validator1", true);
            rwValidationRule.Validators.Add(rwValidatorData);
            ValidatedMethodReference rwValidationMethodReference = new ValidatedMethodReference("System.Object.GetHashCode");
            rwValidationRule.Methods.Add(rwValidationMethodReference);
            ValidatorData rwMethodValidatorData1 = new MockValidatorData("ruleset-validator1", false);
            rwValidationMethodReference.Validators.Add(rwMethodValidatorData1);
            ValidatorData rwMethodValidatorData2 = new MockValidatorData("ruleset-validator2", false);
            rwValidationMethodReference.Validators.Add(rwMethodValidatorData2);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Types.Count);
                Assert.AreEqual(typeof(string).FullName, roSettings.Types.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Count);
                Assert.AreEqual("ruleset", roSettings.Types.Get(0).Rulesets.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Validators.Count);
                Assert.AreEqual("validator1", roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).GetType());
                Assert.AreEqual(true, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0)).ReturnFailure);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Methods.Count);
                Assert.AreEqual("System.Object.GetHashCode", roSettings.Types.Get(0).Rulesets.Get(0).Methods.Get(0).Name);
                Assert.AreEqual(2, roSettings.Types.Get(0).Rulesets.Get(0).Methods.Get(0).Validators.Count);
                Assert.AreEqual("ruleset-validator1", roSettings.Types.Get(0).Rulesets.Get(0).Methods.Get(0).Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Methods.Get(0).Validators.Get(0).GetType());
                Assert.AreEqual(false, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Methods.Get(0).Validators.Get(0)).ReturnFailure);
                Assert.AreEqual("ruleset-validator2", roSettings.Types.Get(0).Rulesets.Get(0).Methods.Get(0).Validators.Get(1).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Methods.Get(0).Validators.Get(1).GetType());
                Assert.AreEqual(false, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Methods.Get(0).Validators.Get(1)).ReturnFailure);
            }
        }

        [TestMethod]
        public void CanDeserializeSerializedSectionWithNamedRuleSpecifyingMultipleValidatorsForField()
        {
            ValidationSettings rwSettings = new ValidationSettings();
            ValidatedTypeReference rwStringType = new ValidatedTypeReference(typeof(string));
            rwSettings.Types.Add(rwStringType);
            ValidationRulesetData rwValidationRule = new ValidationRulesetData("ruleset");
            rwStringType.Rulesets.Add(rwValidationRule);
            ValidatorData rwValidatorData = new MockValidatorData("validator1", true);
            rwValidationRule.Validators.Add(rwValidatorData);
            ValidatedFieldReference rwValidationFieldReference = new ValidatedFieldReference("System.Object.GetHashCode");
            rwValidationRule.Fields.Add(rwValidationFieldReference);
            ValidatorData rwFieldValidatorData1 = new MockValidatorData("ruleset-validator1", false);
            rwValidationFieldReference.Validators.Add(rwFieldValidatorData1);
            ValidatorData rwFieldValidatorData2 = new MockValidatorData("ruleset-validator2", false);
            rwValidationFieldReference.Validators.Add(rwFieldValidatorData2);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[ValidationSettings.SectionName] = rwSettings;

            using (ConfigurationFileHelper configurationFileHelper = new ConfigurationFileHelper(sections))
            {
                IConfigurationSource configurationSource = configurationFileHelper.ConfigurationSource;

                ValidationSettings roSettings = configurationSource.GetSection(ValidationSettings.SectionName) as ValidationSettings;

                Assert.IsNotNull(roSettings);
                Assert.AreEqual(1, roSettings.Types.Count);
                Assert.AreEqual(typeof(string).FullName, roSettings.Types.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Count);
                Assert.AreEqual("ruleset", roSettings.Types.Get(0).Rulesets.Get(0).Name);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Validators.Count);
                Assert.AreEqual("validator1", roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0).GetType());
                Assert.AreEqual(true, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Validators.Get(0)).ReturnFailure);
                Assert.AreEqual(1, roSettings.Types.Get(0).Rulesets.Get(0).Fields.Count);
                Assert.AreEqual("System.Object.GetHashCode", roSettings.Types.Get(0).Rulesets.Get(0).Fields.Get(0).Name);
                Assert.AreEqual(2, roSettings.Types.Get(0).Rulesets.Get(0).Fields.Get(0).Validators.Count);
                Assert.AreEqual("ruleset-validator1", roSettings.Types.Get(0).Rulesets.Get(0).Fields.Get(0).Validators.Get(0).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Fields.Get(0).Validators.Get(0).GetType());
                Assert.AreEqual(false, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Fields.Get(0).Validators.Get(0)).ReturnFailure);
                Assert.AreEqual("ruleset-validator2", roSettings.Types.Get(0).Rulesets.Get(0).Fields.Get(0).Validators.Get(1).Name);
                Assert.AreSame(typeof(MockValidatorData), roSettings.Types.Get(0).Rulesets.Get(0).Fields.Get(0).Validators.Get(1).GetType());
                Assert.AreEqual(false, ((MockValidatorData)roSettings.Types.Get(0).Rulesets.Get(0).Fields.Get(0).Validators.Get(1)).ReturnFailure);
            }
        }
    }
}

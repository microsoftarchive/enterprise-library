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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests
{
    [TestClass]
    public class ValidationConfigurationDesignManagerFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void OpenAndSaveConfiguration()
        {
            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            IErrorLogService s = ServiceHelper.GetErrorService(ServiceProvider);
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(ValidationSettingsNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TypeNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(RuleSetNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(SelfNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(FieldNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(MethodNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PropertyNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(AndCompositeValidatorNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(OrCompositeValidatorNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(CustomValidatorNode)).Count);
            Assert.AreEqual(8, ApplicationNode.Hierarchy.FindNodesByType(typeof(DateRangeValidatorNode)).Count);
            Assert.AreEqual(12, ApplicationNode.Hierarchy.FindNodesByType(typeof(NotNullValidatorNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(RegexValidatorNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(StringLengthValidatorNode)).Count);
            Assert.AreEqual(8, ApplicationNode.Hierarchy.FindNodesByType(typeof(RangeValidatorNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(ContainsCharactersValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(DomainValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(EnumConversionValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(RelativeDateTimeValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TypeConversionValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PropertyComparisonValidatorNode)).Count);

            ApplicationNode.Hierarchy.Save();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            ApplicationNode.Hierarchy.Load();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);
            ApplicationNode.Hierarchy.Open();
            Assert.AreEqual(0, ServiceHelper.GetErrorService(ServiceProvider).ConfigurationErrorCount);

            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(ValidationSettingsNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TypeNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(RuleSetNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(SelfNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(FieldNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(MethodNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PropertyNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(AndCompositeValidatorNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(OrCompositeValidatorNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(CustomValidatorNode)).Count);
            Assert.AreEqual(8, ApplicationNode.Hierarchy.FindNodesByType(typeof(DateRangeValidatorNode)).Count);
            Assert.AreEqual(12, ApplicationNode.Hierarchy.FindNodesByType(typeof(NotNullValidatorNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(RegexValidatorNode)).Count);
            Assert.AreEqual(4, ApplicationNode.Hierarchy.FindNodesByType(typeof(StringLengthValidatorNode)).Count);
            Assert.AreEqual(8, ApplicationNode.Hierarchy.FindNodesByType(typeof(RangeValidatorNode)).Count);
            Assert.AreEqual(2, ApplicationNode.Hierarchy.FindNodesByType(typeof(ContainsCharactersValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(DomainValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(EnumConversionValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(RelativeDateTimeValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(TypeConversionValidatorNode)).Count);
            Assert.AreEqual(1, ApplicationNode.Hierarchy.FindNodesByType(typeof(PropertyComparisonValidatorNode)).Count);
        }

        void ConfigurationAction()
        {
            throw new Exception("The method or operation is not implemented.");
        }

        [TestMethod]
        public void BuildContextTest()
        {
            ValidationConfigurationDesignManager designManager = new ValidationConfigurationDesignManager();
            designManager.Register(ServiceProvider);
            designManager.Open(ServiceProvider);

            DictionaryConfigurationSource dictionarySource = new DictionaryConfigurationSource();
            designManager.BuildConfigurationSource(ServiceProvider, dictionarySource);
            Assert.IsTrue(dictionarySource.Contains("validation"));
        }
    }
}

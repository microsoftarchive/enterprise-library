//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SysConfig = System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Tests
{
    [TestClass]
    public class ConfigurationFixture
    {
        [TestMethod]
        public void CanReadAndWriteLoggingHandler()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test.exe.config";
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            config.Sections.Remove(ExceptionHandlingSettings.SectionName);
            config.Sections.Add(ExceptionHandlingSettings.SectionName, CreateSettings());
            config.Save(ConfigurationSaveMode.Full);

            config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            ExceptionHandlingSettings settings = (ExceptionHandlingSettings)config.Sections[ExceptionHandlingSettings.SectionName];
            FaultContractExceptionHandlerData data = (FaultContractExceptionHandlerData)settings.ExceptionPolicies.Get("test").ExceptionTypes.Get("test").ExceptionHandlers.Get("test");
            config.Sections.Remove(ExceptionHandlingSettings.SectionName);
            config.Save(ConfigurationSaveMode.Full);

            Assert.AreEqual("test", data.Name);
            Assert.AreEqual(typeof(object).AssemblyQualifiedName, data.FaultContractType);
            Assert.AreEqual("my exception message", data.ExceptionMessage);
            Assert.AreEqual(2, data.PropertyMappings.Count);
            Assert.AreEqual("source1", data.PropertyMappings.Get("property1").Source);
            Assert.AreEqual("source2", data.PropertyMappings.Get("property2").Source);
        }

        static ExceptionHandlingSettings CreateSettings()
        {
            FaultContractExceptionHandlerData logData = new FaultContractExceptionHandlerData("test");
            logData.FaultContractType = typeof(object).AssemblyQualifiedName;
            logData.ExceptionMessage = "my exception message";
            logData.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("property1", "source1"));
            logData.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData("property2", "source2"));

            ExceptionTypeData typeData = new ExceptionTypeData("test", typeof(Exception), PostHandlingAction.None);
            typeData.ExceptionHandlers.Add(logData);
            ExceptionPolicyData policy = new ExceptionPolicyData("test");
            policy.ExceptionTypes.Add(typeData);
            ExceptionHandlingSettings settings = new ExceptionHandlingSettings();
            settings.ExceptionPolicies.Add(policy);
            return settings;
        }
    }
}

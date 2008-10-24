//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ConfigurationApplicationDataFixture
    {
        static readonly string baseDirectory = @"C:\";
        static readonly string configurationFilePath = @"C:\Foo.config";

        ConfigurationApplicationFile data;

        [TestInitialize]
        public void TestInitialize()
        {
            data = new ConfigurationApplicationFile(baseDirectory, configurationFilePath);
        }

        [TestMethod]
        public void BaseDirectoryIsSetCorrectly()
        {
            Assert.AreEqual(baseDirectory, data.BaseDirectory);
        }

        [TestMethod]
        public void ConfigurationFilePathIsSetCorrectly()
        {
            Assert.AreEqual(configurationFilePath, data.ConfigurationFilePath);
        }

        [TestMethod]
        public void CanSetFromCurrentAppDomain()
        {
            ConfigurationApplicationFile appDomainData = ConfigurationApplicationFile.FromCurrentAppDomain();

            Assert.AreEqual(AppDomain.CurrentDomain.BaseDirectory, appDomainData.BaseDirectory);
            Assert.AreEqual(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, appDomainData.ConfigurationFilePath);
        }
    }
}

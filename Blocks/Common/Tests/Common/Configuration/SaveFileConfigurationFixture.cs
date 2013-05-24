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
using System.Configuration;
using System.IO;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class SaveFileConfigurationFixture
    {
        string file;

        [TestInitialize]
        public void TestInitialize()
        {
            file = CreateFile();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            if (File.Exists(file)) File.Delete(file);
        }

        [TestMethod]
        public void CanSaveConfigurationSectionToFile()
        {
            FileConfigurationSource source = new FileConfigurationSource(file, false);
            source.Save(TestConfigurationSection.SectionName, CreateTestSection());

            ValidateConfiguration(file);
        }

        string CreateFile()
        {
            string tempFile = Path.Combine(Directory.GetCurrentDirectory(), @"app.config");
            XmlDocument doc = new XmlDocument();
            XmlElement elem = doc.CreateElement("configuration");
            doc.AppendChild(elem);
            doc.Save(tempFile);
            return tempFile;
        }

        void ValidateConfiguration(string configFile)
        {
            TestConfigurationSection section = GetSection(configFile);

            Assert.AreEqual(true, section.BoolValue);
            Assert.AreEqual(42, section.IntValue);
        }

        TestConfigurationSection GetSection(string configFile)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            TestConfigurationSection section = (TestConfigurationSection)config.GetSection(TestConfigurationSection.SectionName);
            return section;
        }

        [TestMethod]
        public void TryToSaveWithAFileConfigurationSaveParameter()
        {
            FileConfigurationSource source = new FileConfigurationSource(file, false);
            source.Add(TestConfigurationSection.SectionName, CreateTestSection());

            ValidateConfiguration(file);
        }

        [TestMethod]
        public void TryToSaveWithConfigurationMultipleTimes()
        {
            string tempFile = CreateFile();
            try
            {
                using (var source = new FileConfigurationSource(tempFile, false))
                {
                    source.Add(TestConfigurationSection.SectionName, CreateTestSection());
                    ValidateConfiguration(tempFile);
                    source.Add(TestConfigurationSection.SectionName, CreateTestSection());
                    ValidateConfiguration(tempFile);
                    source.Add(TestConfigurationSection.SectionName, CreateTestSection());
                    ValidateConfiguration(tempFile);
                }
            }
            finally
            {
                if (File.Exists(tempFile)) File.Delete(tempFile);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToSaveWithNullOrEmptySectionNameThrows()
        {
            FileConfigurationSource source = new FileConfigurationSource(file, false);
            source.Save(null, CreateTestSection());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryToSaveWithNullSectionThrows()
        {
            FileConfigurationSource source = new FileConfigurationSource(file, false);
            source.Save(TestConfigurationSection.SectionName, null);
        }

        TestConfigurationSection CreateTestSection()
        {
            return new TestConfigurationSection { BoolValue = true, IntValue = 42 };
        }
    }

    public class TestConfigurationSection : ConfigurationSection
    {
        public const string SectionName = "testSection";

        [ConfigurationProperty("boolValue")]
        public bool BoolValue
        {
            get { return (bool)this["boolValue"]; }
            set { this["boolValue"] = value; }
        }

        [ConfigurationProperty("intValue")]
        public int IntValue
        {
            get { return (int)this["intValue"]; }
            set { this["intValue"] = value; }
        }
    }
}

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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
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
            source.Save(InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());

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
            InstrumentationConfigurationSection section = GetSection(configFile);

            Assert.IsTrue(section.PerformanceCountersEnabled);
            Assert.IsTrue(section.WmiEnabled);
            Assert.IsTrue(section.EventLoggingEnabled);
        }

        InstrumentationConfigurationSection GetSection(string configFile)
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = configFile;
            System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            InstrumentationConfigurationSection section = (InstrumentationConfigurationSection)config.GetSection(InstrumentationConfigurationSection.SectionName);
            return section;
        }

        [TestMethod]
        public void TryToSaveWithAFileConfigurationSaveParameter()
        {
            FileConfigurationSource source = new FileConfigurationSource(file, false);
            source.Add(InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());

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
                    source.Add(InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
                    ValidateConfiguration(tempFile);
                    source.Add(InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
                    ValidateConfiguration(tempFile);
                    source.Add(InstrumentationConfigurationSection.SectionName, CreateInstrumentationSection());
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
            source.Save(null, CreateInstrumentationSection());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void TryToSaveWithNullSectionThrows()
        {
            FileConfigurationSource source = new FileConfigurationSource(file, false);
            source.Save(InstrumentationConfigurationSection.SectionName, null);
        }

        InstrumentationConfigurationSection CreateInstrumentationSection()
        {
            return new InstrumentationConfigurationSection(true, true, true, "fooApplicationName");
        }
    }
}

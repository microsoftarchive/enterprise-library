//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.Formatters
{
    [TestClass]
    public class JsonLogFormatterFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        private static ILogFormatter GetFormatter(string name, IConfigurationSource configurationSource)
        {
            var settings = LoggingSettings.GetLoggingSettings(configurationSource);
            return settings.Formatters.Get(name).BuildFormatter();
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.Formatters.Add(new JsonLogFormatterData("jsonFormatter1") { Formatting = JsonFormatting.Indented });
            rwLoggingSettings.Formatters.Add(new JsonLogFormatterData("jsonFormatter2"));

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = rwLoggingSettings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            LoggingSettings roLoggingSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.AreEqual(2, roLoggingSettings.Formatters.Count);
            Assert.IsNotNull(roLoggingSettings.Formatters.Get("jsonFormatter1"));
            Assert.AreEqual(JsonFormatting.Indented, ((JsonLogFormatterData)roLoggingSettings.Formatters.Get("jsonFormatter1")).Formatting);
            Assert.AreSame(typeof(JsonLogFormatterData), roLoggingSettings.Formatters.Get("jsonFormatter1").GetType());
            Assert.AreSame(typeof(JsonLogFormatter), roLoggingSettings.Formatters.Get("jsonFormatter1").Type);
            Assert.IsNotNull(roLoggingSettings.Formatters.Get("jsonFormatter2"));
            Assert.AreEqual(JsonFormatting.None, ((JsonLogFormatterData)roLoggingSettings.Formatters.Get("jsonFormatter2")).Formatting);
            Assert.AreSame(typeof(JsonLogFormatterData), roLoggingSettings.Formatters.Get("jsonFormatter2").GetType());
            Assert.AreSame(typeof(JsonLogFormatter), roLoggingSettings.Formatters.Get("jsonFormatter2").Type);
        }

        [TestMethod]
        public void CanCreateFormatter()
        {
            FormatterData data = new JsonLogFormatterData("ignore");
            LoggingSettings settings = new LoggingSettings();
            settings.Formatters.Add(data);
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(LoggingSettings.SectionName, settings);

            ILogFormatter formatter = GetFormatter("ignore", configurationSource);

            Assert.IsNotNull(formatter);
            Assert.AreEqual(formatter.GetType(), typeof(JsonLogFormatter));
        }

        [TestMethod]
        public void CanDeserializeFormattedEntry()
        {
            LogEntry entry = CommonUtil.GetDefaultLogEntry();
            entry.Message = "message";
            entry.Title = "title";
            entry.Categories = new List<string>(new string[] { "cat1", "cat2", "cat3" });

            string serializedLogEntryText = new JsonLogFormatter(JsonFormatting.Indented).Format(entry);
            LogEntry deserializedEntry = JsonLogFormatter.Deserialize<LogEntry>(serializedLogEntryText);

            Assert.IsNotNull(deserializedEntry);
            Assert.IsFalse(ReferenceEquals(entry, deserializedEntry));
            Assert.AreEqual(entry.Categories.Count, deserializedEntry.Categories.Count);
            foreach (string category in entry.Categories)
            {
                Assert.IsTrue(deserializedEntry.Categories.Contains(category));
            }
            Assert.AreEqual(entry.Message, deserializedEntry.Message);
            Assert.AreEqual(entry.Title, deserializedEntry.Title);
        }

        [TestMethod]
        public void CanDeserializeFormattedCustomEntry()
        {
            var entry = new CustomLogEntry();
            entry.TimeStamp = DateTime.MaxValue;
            entry.Title = "My custom message title";
            entry.Message = "My custom message body";
            entry.Categories = new List<string>(new string[] { "CustomFormattedCategory", "OtherCategory" });
            entry.AcmeCoField1 = "apple";
            entry.AcmeCoField2 = "orange";
            entry.AcmeCoField3 = "lemon";

            string serializedLogEntryText = new JsonLogFormatter().Format(entry);
            var deserializedEntry = JsonLogFormatter.Deserialize<CustomLogEntry>(serializedLogEntryText);

            Assert.IsNotNull(deserializedEntry);
            Assert.IsFalse(ReferenceEquals(entry, deserializedEntry));
            Assert.AreEqual(entry.Categories.Count, deserializedEntry.Categories.Count);
            foreach (string category in entry.Categories)
            {
                Assert.IsTrue(deserializedEntry.Categories.Contains(category));
            }
            Assert.AreEqual(entry.Message, deserializedEntry.Message);
            Assert.AreEqual(entry.Title, deserializedEntry.Title);
            Assert.AreEqual(entry.AcmeCoField1, deserializedEntry.AcmeCoField1);
            Assert.AreEqual(entry.AcmeCoField2, deserializedEntry.AcmeCoField2);
            Assert.AreEqual(entry.AcmeCoField3, deserializedEntry.AcmeCoField3);
        }

        class CustomLogEntry : LogEntry
        {
            public string AcmeCoField1 = string.Empty;
            public string AcmeCoField2 = string.Empty;
            public string AcmeCoField3 = string.Empty;
        }
    }
}

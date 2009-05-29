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
using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
    [TestClass]
    public class FormatterEmailTraceListenerConfigurationFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        private static TraceListener GetListener(string name, IConfigurationSource configurationSource)
        {
            var container = EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource);
            return container.GetInstance<TraceListener>(name);
        }

        [TestMethod]
        public void ListenerDataIsCreatedCorrectly()
        {
            var listenerData =
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter");

            Assert.AreSame(typeof(EmailTraceListener), listenerData.Type);
            Assert.AreSame(typeof(EmailTraceListenerData), listenerData.ListenerDataType);
            Assert.AreEqual("listener", listenerData.Name);
            Assert.AreEqual("smtphost", listenerData.SmtpServer);
            Assert.AreEqual("formatter", listenerData.Formatter);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            var rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.TraceListeners.Add(
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter"));
            rwLoggingSettings.TraceListeners.Add(
                new EmailTraceListenerData("listener2", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter"));

            var fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test.exe.config";
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(LoggingSettings.SectionName);
            rwConfiguration.Sections.Add(LoggingSettings.SectionName, rwLoggingSettings);

            File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
            rwConfiguration.Save();

            System.Configuration.Configuration roConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            var roLoggingSettings = roConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;

            Assert.AreEqual(2, roLoggingSettings.TraceListeners.Count);
            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener"));
            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener2"));
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenName()
        {
            var listenerData =
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter");

            var helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);
            helper.loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));

            TraceListener listener = GetListener("listener", helper.configurationSource);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNotNull(((EmailTraceListener)listener).Formatter);
            Assert.AreEqual(((EmailTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)((EmailTraceListener)listener).Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenConfiguration()
        {
            var listenerData =
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter");

            var helper = new MockLogObjectsHelper();
            helper.loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));
            helper.loggingSettings.TraceListeners.Add(listenerData);

            TraceListener listener = GetListener(listenerData.Name, helper.configurationSource);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNotNull(((EmailTraceListener)listener).Formatter);
            Assert.AreEqual(((EmailTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)((EmailTraceListener)listener).Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenConfigurationWithoutFormatter()
        {
            var listenerData =
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, null);

            var helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);

            TraceListener listener = GetListener(listenerData.Name, helper.configurationSource);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNull(((EmailTraceListener)listener).Formatter);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFile()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.Formatters.Add(new TextFormatterData("formatter", "foobar template"));
            loggingSettings.TraceListeners.Add(
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter"));

            TraceListener listener = GetListener("listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings));

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNotNull(((EmailTraceListener)listener).Formatter);
            Assert.AreEqual(((EmailTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("foobar template", ((TextFormatter)((EmailTraceListener)listener).Formatter).Template);
        }
    }
}

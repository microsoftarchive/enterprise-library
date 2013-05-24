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
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
    [TestClass]
    public class FormattedEmailTraceListenerConfigurationFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
            ConfigurationTestHelper.ConfigurationFileName = "test.exe.config";
        }

        private static TraceListener GetListener(string name, IConfigurationSource configurationSource)
        {
            var settings = LoggingSettings.GetLoggingSettings(configurationSource);
            return settings.TraceListeners.Get(name).BuildTraceListener(settings);
        }

        private EmailTraceListenerData CreateDefaultData()
        {
            return new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter");
        }

        [TestMethod]
        public void ListenerDataIsCreatedCorrectly()
        {
            EmailTraceListenerData listenerData = CreateDefaultData();

            Assert.AreSame(typeof(EmailTraceListener), listenerData.Type);
            Assert.AreSame(typeof(EmailTraceListenerData), listenerData.ListenerDataType);
            Assert.AreEqual("listener", listenerData.Name);
            Assert.AreEqual("smtphost", listenerData.SmtpServer);
            Assert.AreEqual("formatter", listenerData.Formatter);
        }

        [TestMethod]
        public void AuthenticationModeIsNoneWhenUnspecified()
        {
            EmailTraceListenerData listenerData = CreateDefaultData();

            Assert.AreEqual(EmailAuthenticationMode.None, listenerData.AuthenticationMode);
        }

        [TestMethod]
        public void UseSSLDefaultsToFalseWhenUnspecified()
        {
            EmailTraceListenerData listenerData = CreateDefaultData();

            Assert.IsFalse(listenerData.UseSSL);
        }

        [TestMethod]
        public void CanUseSSLWhenUnauthenticated()
        {
            var listenerData = new EmailTraceListenerData("listener",
                "obviously.bad.email.address@127.0.0.1", "logging@entlib.com",
                "EntLib-Logging:", "has occurred",
                "smtphost", 25,
                "formatter", TraceOptions.None, SourceLevels.All,
                EmailAuthenticationMode.None, null, null, true);

            Assert.IsTrue(listenerData.UseSSL);
        }

        [TestMethod]
        public void CanSetUserNameAndPassword()
        {
            var listenerData = new EmailTraceListenerData("listener",
                "obviously.bad.email.address@127.0.0.1", "logging@entlib.com",
                "EntLib-Logging:", "has occurred",
                "smtphost", 25,
                "formatter", TraceOptions.None, SourceLevels.All,
                EmailAuthenticationMode.UserNameAndPassword, "user", "secret", true);

            Assert.AreEqual(EmailAuthenticationMode.UserNameAndPassword, listenerData.AuthenticationMode);
            Assert.AreEqual("user", listenerData.UserName);
            Assert.AreEqual("secret", listenerData.Password);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            const string name = "name";
            const string toAddress = "obviously.bad.email.address@127.0.0.1";
            const string fromAddress = "logging@entlib.com";
            const string subjectStarter = "EntLib-Logging:";
            const string subjectEnder = "has occurred";
            const string server = "smtphost";
            const int port = 25;
            const string formatter = "formatter";
            const string user = "user";
            const string password = "secret";

            var data = new EmailTraceListenerData
                {
                    Name = name,
                    ToAddress = toAddress,
                    FromAddress = fromAddress,
                    SubjectLineStarter = subjectStarter,
                    SubjectLineEnder = subjectEnder,
                    SmtpServer = server,
                    SmtpPort = port,
                    Formatter = formatter,
                    TraceOutputOptions = TraceOptions.Callstack,
                    Filter = SourceLevels.Critical,
                    AuthenticationMode = EmailAuthenticationMode.UserNameAndPassword,
                    UserName = user,
                    Password = password,
                    UseSSL = true
                };

            LoggingSettings settings = new LoggingSettings();
            settings.TraceListeners.Add(data);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            LoggingSettings roSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.AreEqual(1, roSettings.TraceListeners.Count);

            var loadedData = (EmailTraceListenerData)roSettings.TraceListeners.Get(name);

            Assert.IsNotNull(loadedData);
            Assert.AreEqual(TraceOptions.Callstack, loadedData.TraceOutputOptions);
            Assert.AreEqual(SourceLevels.Critical, loadedData.Filter);
            Assert.AreSame(typeof(EmailTraceListenerData), loadedData.GetType());
            Assert.AreSame(typeof(EmailTraceListenerData), loadedData.ListenerDataType);
            Assert.AreSame(typeof(EmailTraceListener), loadedData.Type);
            Assert.AreEqual(formatter, loadedData.Formatter);
            Assert.AreEqual(fromAddress, loadedData.FromAddress);
            Assert.AreEqual(port, loadedData.SmtpPort);
            Assert.AreEqual(server, loadedData.SmtpServer);
            Assert.AreEqual(subjectEnder, loadedData.SubjectLineEnder);
            Assert.AreEqual(subjectStarter, loadedData.SubjectLineStarter);
            Assert.AreEqual(toAddress, loadedData.ToAddress);
            Assert.AreEqual(EmailAuthenticationMode.UserNameAndPassword, loadedData.AuthenticationMode);
            Assert.AreEqual(user, loadedData.UserName);
            Assert.AreEqual(password, loadedData.Password);
            Assert.IsTrue(loadedData.UseSSL);
            Assert.AreEqual(false, loadedData.Asynchronous);
            Assert.AreEqual(30000, loadedData.AsynchronousBufferSize);
            Assert.AreEqual(System.Threading.Timeout.InfiniteTimeSpan, loadedData.AsynchronousDisposeTimeout);
            Assert.AreEqual(null, loadedData.AsynchronousMaxDegreeOfParallelism);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfigurationWithNonDefaultAsyncValues()
        {
            const string name = "name";
            const string toAddress = "obviously.bad.email.address@127.0.0.1";
            const string fromAddress = "logging@entlib.com";
            const string subjectStarter = "EntLib-Logging:";
            const string subjectEnder = "has occurred";
            const string server = "smtphost";
            const int port = 25;
            const string formatter = "formatter";
            const string user = "user";
            const string password = "secret";

            var data = new EmailTraceListenerData
            {
                Name = name,
                ToAddress = toAddress,
                FromAddress = fromAddress,
                SubjectLineStarter = subjectStarter,
                SubjectLineEnder = subjectEnder,
                SmtpServer = server,
                SmtpPort = port,
                Formatter = formatter,
                TraceOutputOptions = TraceOptions.Callstack,
                Filter = SourceLevels.Critical,
                AuthenticationMode = EmailAuthenticationMode.UserNameAndPassword,
                UserName = user,
                Password = password,
                UseSSL = true,
                Asynchronous = true,
                AsynchronousBufferSize = 300,
                AsynchronousDisposeTimeout = TimeSpan.FromSeconds(100),
                AsynchronousMaxDegreeOfParallelism = 200,
            };

            LoggingSettings settings = new LoggingSettings();
            settings.TraceListeners.Add(data);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            LoggingSettings roSettings = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.AreEqual(1, roSettings.TraceListeners.Count);

            var loadedData = (EmailTraceListenerData)roSettings.TraceListeners.Get(name);

            Assert.IsNotNull(loadedData);
            Assert.AreEqual(TraceOptions.Callstack, loadedData.TraceOutputOptions);
            Assert.AreEqual(SourceLevels.Critical, loadedData.Filter);
            Assert.AreSame(typeof(EmailTraceListenerData), loadedData.GetType());
            Assert.AreSame(typeof(EmailTraceListenerData), loadedData.ListenerDataType);
            Assert.AreSame(typeof(EmailTraceListener), loadedData.Type);
            Assert.AreEqual(formatter, loadedData.Formatter);
            Assert.AreEqual(fromAddress, loadedData.FromAddress);
            Assert.AreEqual(port, loadedData.SmtpPort);
            Assert.AreEqual(server, loadedData.SmtpServer);
            Assert.AreEqual(subjectEnder, loadedData.SubjectLineEnder);
            Assert.AreEqual(subjectStarter, loadedData.SubjectLineStarter);
            Assert.AreEqual(toAddress, loadedData.ToAddress);
            Assert.AreEqual(EmailAuthenticationMode.UserNameAndPassword, loadedData.AuthenticationMode);
            Assert.AreEqual(user, loadedData.UserName);
            Assert.AreEqual(password, loadedData.Password);
            Assert.IsTrue(loadedData.UseSSL);
            Assert.AreEqual(true, loadedData.Asynchronous);
            Assert.AreEqual(300, loadedData.AsynchronousBufferSize);
            Assert.AreEqual(TimeSpan.FromSeconds(100), loadedData.AsynchronousDisposeTimeout);
            Assert.AreEqual(200, loadedData.AsynchronousMaxDegreeOfParallelism);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfigurationContainingTwoListeners()
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
            EmailTraceListenerData listenerData =
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter");

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);
            helper.loggingSettings.Formatters.Add(new TextFormatterData("formatter", "some template"));

            TraceListener listener = GetListener("listener", helper.configurationSource);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNotNull(((EmailTraceListener)listener).Formatter);
            Assert.AreEqual(((EmailTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("some template", ((TextFormatter)((EmailTraceListener)listener).Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFile()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.Formatters.Add(new TextFormatterData("formatter", "some template"));
            loggingSettings.TraceListeners.Add(
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, "formatter"));

            TraceListener listener = GetListener("listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings));

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNotNull(((EmailTraceListener)listener).Formatter);
            Assert.AreEqual(((EmailTraceListener)listener).Formatter.GetType(), typeof(TextFormatter));
            Assert.AreEqual("some template", ((TextFormatter)((EmailTraceListener)listener).Formatter).Template);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFileWithoutFormatter()
        {
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.TraceListeners.Add(
                new EmailTraceListenerData("listener", "obviously.bad.email.address@127.0.0.1", "logging@entlib.com", "EntLib-Logging:",
                                           "has occurred", "smtphost", 25, null));

            TraceListener listener = GetListener("listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings));

            Assert.IsNotNull(listener);
            Assert.AreEqual(listener.GetType(), typeof(EmailTraceListener));
            Assert.IsNull(((EmailTraceListener)listener).Formatter);
        }
    }
}

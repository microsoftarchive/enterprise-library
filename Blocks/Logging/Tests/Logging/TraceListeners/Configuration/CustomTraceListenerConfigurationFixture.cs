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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
    [TestClass]
    public class CustomTraceListenerConfigurationFixture
    {
        const string initializationData = "custom initialization data";
        const string attributeValue = "value";

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

        [TestMethod]
        public void CanCreateInstanceFromGivenName()
        {
            CustomTraceListenerData listenerData
                = new CustomTraceListenerData("listener", typeof(MockCustomTraceListener), initializationData);

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);
            TraceListener listener = GetListener("listener", helper.configurationSource);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(typeof(MockCustomTraceListener), listener.GetType());
            Assert.AreEqual(initializationData, ((MockCustomTraceListener)listener).initData);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreationOfCustomTraceListenerWithoutRequiredSignatureConstructorThrows()
        {
            CustomTraceListenerData listenerData
                = new CustomTraceListenerData("listener", typeof(MockCustomTraceListenerWithInvalidConstructor), initializationData);

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);
            TraceListener listener = GetListener("listener", helper.configurationSource);
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenConfigurationWithAttributes()
        {
            CustomTraceListenerData listenerData
                = new CustomTraceListenerData("listener", typeof(MockCustomTraceListener), initializationData);
            listenerData.SetAttributeValue(MockCustomTraceListener.AttributeKey, attributeValue);

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);

            TraceListener listener = GetListener("listener", helper.configurationSource);

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);

            var innerListener = (MockCustomTraceListener)listener;

            Assert.AreEqual(initializationData, innerListener.initData);
            Assert.AreEqual(attributeValue, innerListener.Attribute);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFile()
        {
            CustomTraceListenerData listenerData
                = new CustomTraceListenerData("listener", typeof(MockCustomTraceListener), initializationData);
            listenerData.SetAttributeValue(MockCustomTraceListener.AttributeKey, attributeValue);
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.TraceListeners.Add(listenerData);

            TraceListener listener =
                GetListener("listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings));

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);

            var innerListener = (MockCustomTraceListener)listener;

            Assert.AreEqual(initializationData, innerListener.initData);
            Assert.IsNull(innerListener.Formatter);
            Assert.AreEqual(attributeValue, innerListener.Attribute);
        }

        [TestMethod]
        public void CanCreateInstanceWithFormatterFromConfigurationFile()
        {
            CustomTraceListenerData listenerData
                = new CustomTraceListenerData("listener", typeof(MockCustomTraceListener), initializationData);
            listenerData.Formatter = "formatter";
            listenerData.SetAttributeValue(MockCustomTraceListener.AttributeKey, attributeValue);
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.TraceListeners.Add(listenerData);
            loggingSettings.Formatters.Add(new TextFormatterData("formatter", "template"));

            TraceListener listener =
                GetListener("listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings));

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);

            var innerListener = (MockCustomTraceListener)listener;

            Assert.AreEqual(initializationData, innerListener.initData);
            Assert.IsNotNull(innerListener.Formatter);
            Assert.AreSame(typeof(TextFormatter), innerListener.Formatter.GetType());
            Assert.AreEqual("template", ((TextFormatter)(innerListener.Formatter)).Template);
            Assert.AreEqual(attributeValue, innerListener.Attribute);
        }

        [TestMethod]
        public void CanCreateInstanceWithoutInitializationDataFromConfigurationFile()
        {
            CustomTraceListenerData listenerData = new CustomTraceListenerData();
            listenerData.Name = "listener";
            listenerData.Type = typeof(MockCustomTraceListener);
            listenerData.ListenerDataType = typeof(CustomTraceListenerData);
            listenerData.SetAttributeValue(MockCustomTraceListener.AttributeKey, attributeValue);
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.TraceListeners.Add(listenerData);

            TraceListener listener =
                GetListener("listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings));

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);

            var innerListener = (MockCustomTraceListener)listener;

            Assert.IsNull(innerListener.initData);
            Assert.IsNull(innerListener.Formatter);
            Assert.AreEqual(attributeValue, innerListener.Attribute);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFileUsingSystemDiagnosticsTraceListenerData()
        {
            SystemDiagnosticsTraceListenerData listenerData
                = new SystemDiagnosticsTraceListenerData("listener", typeof(MockCustomTraceListener), initializationData, TraceOptions.Callstack);
            listenerData.SetAttributeValue(MockCustomTraceListener.AttributeKey, attributeValue);
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.TraceListeners.Add(listenerData);

            TraceListener listener = GetListener("listener", CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings));

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(TraceOptions.Callstack, listener.TraceOutputOptions);

            var innerListener = (MockCustomTraceListener)listener;

            Assert.AreEqual(initializationData, innerListener.initData);
            Assert.AreEqual(attributeValue, innerListener.Attribute);
        }

        [TestMethod]
        public void CanUseCustomTraceListenerWithSystemDiagnosticsConfiguration()
        {
            TraceSource source = new TraceSource("customProvider");
            Assert.AreEqual(2, source.Listeners.Count);

            TraceListener listener = source.Listeners["listener"];

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener", listener.Name);
            Assert.AreEqual(typeof(MockCustomTraceListener), listener.GetType());
            Assert.IsNull(((MockCustomTraceListener)listener).Formatter);
            Assert.AreEqual(attributeValue, ((MockCustomTraceListener)listener).Attribute);
        }

        [TestMethod]
        public void CanDeserializeSerializedConfiguration()
        {
            string name = "name";
            string initData = "init data";
            string attributeName = "attribute";
            string attributeValue = "value";

            CustomTraceListenerData data = new CustomTraceListenerData(name, typeof(MockCustomTraceListener), initData, TraceOptions.Callstack);
            data.SetAttributeValue(attributeName, attributeValue);

            LoggingSettings settings = new LoggingSettings();
            settings.TraceListeners.Add(data);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            LoggingSettings roSettigs = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.AreEqual(1, roSettigs.TraceListeners.Count);
            Assert.IsNotNull(roSettigs.TraceListeners.Get(name));
            Assert.AreEqual(TraceOptions.Callstack, ((CustomTraceListenerData)roSettigs.TraceListeners.Get(name)).TraceOutputOptions);
            Assert.AreSame(typeof(CustomTraceListenerData), roSettigs.TraceListeners.Get(name).GetType());
            Assert.AreSame(typeof(CustomTraceListenerData), roSettigs.TraceListeners.Get(name).ListenerDataType);
            Assert.AreSame(typeof(MockCustomTraceListener), roSettigs.TraceListeners.Get(name).Type);
            Assert.AreEqual(attributeValue, ((CustomTraceListenerData)roSettigs.TraceListeners.Get(name)).Attributes[attributeName]);
        }
    }
}

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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
    [TestClass]
    public class DelimitedListTraceListenerConfigurationFixture
    {
        [TestInitialize]
        public void SetUp()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenName()
        {
            SystemDiagnosticsTraceListenerData listenerData
                = new SystemDiagnosticsTraceListenerData("listener", typeof(DelimitedListTraceListener), "log.txt");

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);

            var container = EnterpriseLibraryContainer.CreateDefaultContainer(helper.configurationSource);

            TraceListener listener = container.GetInstance<TraceListener>("listener\u200cimplementation");

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener\u200cimplementation", listener.Name);
            Assert.AreEqual(typeof(DelimitedListTraceListener), listener.GetType());
        }

        [TestMethod]
        public void CanCreateInstanceFromGivenConfigurationWithAttributes()
        {
            SystemDiagnosticsTraceListenerData listenerData
                = new SystemDiagnosticsTraceListenerData("listener", typeof(DelimitedListTraceListener), "log.txt");
            listenerData.SetAttributeValue("delimiter", "||");

            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(listenerData);

            var container = EnterpriseLibraryContainer.CreateDefaultContainer(helper.configurationSource);

            TraceListener listener = container.GetInstance<TraceListener>("listener\u200cimplementation");

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener\u200cimplementation", listener.Name);
            Assert.IsInstanceOfType(listener, typeof(AttributeSettingTraceListenerWrapper));

            var innerListener =
                (DelimitedListTraceListener)((AttributeSettingTraceListenerWrapper)listener).InnerTraceListener;
            Assert.AreEqual("||", innerListener.Delimiter);
        }

        [TestMethod]
        public void CanCreateInstanceFromConfigurationFile()
        {
            SystemDiagnosticsTraceListenerData listenerData
                = new SystemDiagnosticsTraceListenerData("listener", typeof(DelimitedListTraceListener), "log.txt");
            listenerData.SetAttributeValue("delimiter", "||");
            LoggingSettings loggingSettings = new LoggingSettings();
            loggingSettings.TraceListeners.Add(listenerData);

            var container = EnterpriseLibraryContainer.CreateDefaultContainer(CommonUtil.SaveSectionsAndGetConfigurationSource(loggingSettings));

            TraceListener listener = container.GetInstance<TraceListener>("listener\u200cimplementation");

            Assert.IsNotNull(listener);
            Assert.AreEqual("listener\u200cimplementation", listener.Name);
            Assert.IsInstanceOfType(listener, typeof(AttributeSettingTraceListenerWrapper));

            var innerListener =
                (DelimitedListTraceListener)((AttributeSettingTraceListenerWrapper)listener).InnerTraceListener;

            Assert.AreEqual("||", innerListener.Delimiter);
        }
    }
}

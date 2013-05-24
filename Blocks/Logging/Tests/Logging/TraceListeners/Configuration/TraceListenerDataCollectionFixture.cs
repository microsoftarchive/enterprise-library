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

using System.Configuration;
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
    [TestClass]
    public class TraceListenerDataCollectionFixture
    {
        [TestMethod]
        public void CanDeserializeSerializedCollection()
        {
            LoggingSettings rwLoggingSettings = new LoggingSettings();
            rwLoggingSettings.TraceListeners.Add(new FormattedEventLogTraceListenerData("listener1", CommonUtil.EventLogSourceName, "formatter"));
            rwLoggingSettings.TraceListeners.Add(new SystemDiagnosticsTraceListenerData("listener2", typeof(FormattedEventLogTraceListener), CommonUtil.EventLogSourceName));
            rwLoggingSettings.TraceListeners.Add(new SystemDiagnosticsTraceListenerData("listener3", typeof(XmlWriterTraceListener), "log.txt"));

            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test.exe.config";
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            rwConfiguration.Sections.Remove(LoggingSettings.SectionName);
            rwConfiguration.Sections.Add(LoggingSettings.SectionName, rwLoggingSettings);

            File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
            rwConfiguration.Save();

            System.Configuration.Configuration roConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
            LoggingSettings roLoggingSettings = roConfiguration.GetSection(LoggingSettings.SectionName) as LoggingSettings;

            Assert.AreEqual(3, roLoggingSettings.TraceListeners.Count);

            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener1"));
            Assert.AreEqual(roLoggingSettings.TraceListeners.Get("listener1").GetType(), typeof(FormattedEventLogTraceListenerData));
            Assert.AreSame(roLoggingSettings.TraceListeners.Get("listener1").Type, typeof(FormattedEventLogTraceListener));

            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener2"));
            Assert.AreEqual(roLoggingSettings.TraceListeners.Get("listener2").GetType(), typeof(SystemDiagnosticsTraceListenerData));
            Assert.AreSame(roLoggingSettings.TraceListeners.Get("listener2").Type, typeof(FormattedEventLogTraceListener));

            Assert.IsNotNull(roLoggingSettings.TraceListeners.Get("listener3"));
            Assert.AreEqual(roLoggingSettings.TraceListeners.Get("listener3").GetType(), typeof(SystemDiagnosticsTraceListenerData));
            Assert.AreSame(roLoggingSettings.TraceListeners.Get("listener3").Type, typeof(XmlWriterTraceListener));
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void DeserializationOfInvalidTraceListenerDataTypeThrows()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test-tracelistenerinvalidtype.exe.config";
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            rwConfiguration.GetSection(LoggingSettings.SectionName);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void DeserializationOfMissingTraceListenerDataTypeThrows()
        {
            ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
            fileMap.ExeConfigFilename = "test-tracelistenermissingtype.exe.config";
            System.Configuration.Configuration rwConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);

            rwConfiguration.GetSection(LoggingSettings.SectionName);
        }
    }
}

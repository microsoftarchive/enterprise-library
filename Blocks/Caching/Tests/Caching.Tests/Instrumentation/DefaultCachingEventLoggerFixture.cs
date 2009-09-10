//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Configuration;
using System.Diagnostics;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Instrumentation.Tests
{
    [TestClass]
    public class DefaultCachingEventLogFixture
    {
        const string instanceName = "test";
        const string exceptionMessage = "exception message";

        [TestMethod]
        public void CanBuildDefaultLogger()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(InstrumentationConfigurationSection.SectionName, new InstrumentationConfigurationSection(true, true, true, "fooApplicationName"));
            configurationSource.Add(CacheManagerSettings.SectionName, new CacheManagerSettings());

            DefaultCachingEventLogger logger
                =
                EnterpriseLibraryContainer.CreateDefaultContainer(configurationSource).GetInstance
                    <DefaultCachingEventLogger>();

            Assert.IsNotNull(logger);
        }

        [TestMethod]
        public void DefaultLoggerWritesToEventLog()
        {
            DefaultCachingEventLogger logger
                = new DefaultCachingEventLogger(true, false);
            ConfigurationErrorsException exception = new ConfigurationErrorsException(exceptionMessage);

            using (var eventLog = new EventLogTracker(GetEventLog()))
            {
                logger.LogConfigurationError(instanceName, exception);

                var newEntries = from entry in eventLog.NewEntries()
                                 where entry.Message.IndexOf(exceptionMessage) > -1
                                 select entry;

                Assert.AreEqual(1, newEntries.ToList().Count);
            }
        }

        [TestMethod]
        public void DefaultLoggerFiresWmiEvent()
        {
            DefaultCachingEventLogger logger
                = new DefaultCachingEventLogger(false, true);
            ConfigurationErrorsException exception = new ConfigurationErrorsException(exceptionMessage);

            using (WmiEventWatcher eventListener = new WmiEventWatcher(1))
            {
                logger.LogConfigurationError(instanceName, exception);

                eventListener.WaitForEvents();
                Assert.AreEqual(1, eventListener.EventsReceived.Count);
                Assert.AreEqual("CacheConfigurationFailureEvent", eventListener.EventsReceived[0].ClassPath.ClassName);
                Assert.AreEqual(instanceName, eventListener.EventsReceived[0].GetPropertyValue("InstanceName"));
            }
        }

        static EventLog GetEventLog()
        {
            return new EventLog("Application", ".", DefaultCachingEventLogger.EventLogSourceName);
        }
    }
}

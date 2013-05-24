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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class TraceSourceDataFixture
    {
        [TestInitialize]
        public void TestInitialize()
        {
            AppDomain.CurrentDomain.SetData("APPBASE", Environment.CurrentDirectory);
        }

        [TestMethod]
        public void CanDeserializeSerializedDefaultConfiguration()
        {
            string name = "name";
            bool autoFlush = true;

            TraceSourceData data = new TraceSourceData(name, SourceLevels.Critical);
            data.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            data.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            LoggingSettings settings = new LoggingSettings();
            settings.TraceSources.Add(data);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            LoggingSettings roSettigs = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.AreEqual(1, roSettigs.TraceSources.Count);
            Assert.IsNotNull(roSettigs.TraceSources.Get(name));
            Assert.AreEqual(SourceLevels.Critical, roSettigs.TraceSources.Get(name).DefaultLevel);
            Assert.AreEqual(autoFlush, roSettigs.TraceSources.Get(name).AutoFlush);
            Assert.AreEqual(2, roSettigs.TraceSources.Get(name).TraceListeners.Count);
            Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener1"));
            Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener2"));
        }

        [TestMethod]
        public void CanDeserializeSerializedWithAutoFlushConfiguration()
        {
            string name = "name";
            bool autoFlush = false;

            TraceSourceData data = new TraceSourceData(name, SourceLevels.Critical, false);
            data.TraceListeners.Add(new TraceListenerReferenceData("listener1"));
            data.TraceListeners.Add(new TraceListenerReferenceData("listener2"));

            LoggingSettings settings = new LoggingSettings();
            settings.TraceSources.Add(data);

            IDictionary<string, ConfigurationSection> sections = new Dictionary<string, ConfigurationSection>();
            sections[LoggingSettings.SectionName] = settings;
            IConfigurationSource configurationSource
                = ConfigurationTestHelper.SaveSectionsInFileAndReturnConfigurationSource(sections);

            LoggingSettings roSettigs = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            Assert.AreEqual(1, roSettigs.TraceSources.Count);
            Assert.IsNotNull(roSettigs.TraceSources.Get(name));
            Assert.AreEqual(SourceLevels.Critical, roSettigs.TraceSources.Get(name).DefaultLevel);
            Assert.AreEqual(autoFlush, roSettigs.TraceSources.Get(name).AutoFlush);
            Assert.AreEqual(2, roSettigs.TraceSources.Get(name).TraceListeners.Count);
            Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener1"));
            Assert.IsNotNull(roSettigs.TraceSources.Get(name).TraceListeners.Get("listener2"));
        }
    }

    [TestClass]
    public class GivenTraceSourceDataWithNoTraceListenerReferences
    {
        private TraceSourceData data;

        [TestInitialize]
        public void Setup()
        {
            data = new TraceSourceData("source", SourceLevels.Error, true);
        }
    }

    [TestClass]
    public class GivenTraceSourceDataWithTraceListenerReferences
    {
        private TraceSourceData data;

        [TestInitialize]
        public void Setup()
        {
            data =
                new TraceSourceData("source", SourceLevels.Error, true)
                {
                    TraceListeners = 
                    { 
                        new TraceListenerReferenceData("listener1"), 
                        new TraceListenerReferenceData("listener2")
                    }
                };
        }
    }
}

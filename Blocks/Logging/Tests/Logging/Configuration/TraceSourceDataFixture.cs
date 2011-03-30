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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if !SILVERLIGHT
using System.Diagnostics;
#else
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
#endif

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Tests
{
    [TestClass]
    public class TraceSourceDataFixture
    {
#if !SILVERLIGHT
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
#endif
    }

    [TestClass]
    public class GivenTraceSourceDataWithNoTraceListenerReferences
    {
        private TraceSourceData data;

        [TestInitialize]
        public void Setup()
        {
            data = new TraceSourceData { Name = "source", DefaultLevel = SourceLevels.Error, AutoFlush = true };
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationIsForLogSourceWithTheSuppliedName()
        {
            data.GetRegistrations()
                .AssertForServiceType(typeof(LogSource))
                .ForName("source")
                .ForImplementationType(typeof(LogSource));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            data.GetRegistrations()
                .AssertConstructor()
                .WithValueConstructorParameter("source")
                .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new string[0])
                .WithValueConstructorParameter(SourceLevels.Error)
                .WithValueConstructorParameter(true)
                .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
                .VerifyConstructorParameters();
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
                new TraceSourceData
                {
                    Name = "source",
                    DefaultLevel = SourceLevels.Error,
                    AutoFlush = true,
                    TraceListeners = 
                    { 
                        new TraceListenerReferenceData{ Name = "listener1"}, 
                        new TraceListenerReferenceData{ Name = "listener2"}
                    }
                };
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationIsForLogSourceWithTheSuppliedName()
        {
            data.GetRegistrations()
                .AssertForServiceType(typeof(LogSource))
                .ForName("source")
                .ForImplementationType(typeof(LogSource));
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationHasTheExpectedConstructorParameters()
        {
            data.GetRegistrations()
                .AssertConstructor()
                .WithValueConstructorParameter("source")
                .WithContainerResolvedEnumerableConstructorParameter<TraceListener>(new[] { "listener1", "listener2" })
                .WithValueConstructorParameter(SourceLevels.Error)
                .WithValueConstructorParameter(true)
                .WithContainerResolvedParameter<ILoggingInstrumentationProvider>(null)
                .VerifyConstructorParameters();
        }

        [TestMethod]
        public void WhenCreatesRegistration_ThenCreatedRegistrationIsATransient()
        {
            Assert.AreEqual(TypeRegistrationLifetime.Transient, data.GetRegistrations().Lifetime);
        }
    }
}

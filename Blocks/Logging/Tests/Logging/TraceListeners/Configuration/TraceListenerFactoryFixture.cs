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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners.Tests;
using Microsoft.Practices.ObjectBuilder2;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.Configuration
{
    [TestClass]
    public class TraceListenerFactoryFixture
    {
        IBuilderContext context;
        ConfigurationReflectionCache reflectionCache;

        [TestInitialize]
        public void SetUp()
        {
			context = new BuilderContext(new StrategyChain(), null, null, new PolicyList(), null, null);
            reflectionCache = new ConfigurationReflectionCache();
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void CreationWithNullLoggingSettingsThrows()
        {
            DictionaryConfigurationSource configurationSource = new DictionaryConfigurationSource();

            TraceListenerCustomFactory.Instance.Create(context, "listener", configurationSource, reflectionCache);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void CreationWithMissingTraceListenerDataThrows()
        {
            MockLogObjectsHelper helper = new MockLogObjectsHelper();

            TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);
        }

        [TestMethod]
        public void CreationWithExistingTraceListenerDataSucceeeds()
        {
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(new MockTraceListenerData("listener"));

            TraceListener listener
                = TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);

            Assert.IsNotNull(listener);
            Assert.AreSame(typeof(MockTraceListener), listener.GetType());
        }

        /// <summary>
        /// Default Filter is ALL so no filter must be applied to the trace listener
        /// </summary>
        [TestMethod]
        public void CreatedTraceListenerWithDefaultFilter()
        {
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            helper.loggingSettings.TraceListeners.Add(new MockTraceListenerData("listener"));

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);

            Assert.IsNull(listener.Filter);
        }

        /// <summary>
        /// If the Filter.All is selected then no filter must be applied to the trace listener
        /// </summary>
        [TestMethod]
        public void CreatedTraceListenerWithAllFilterOption()
        {
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            MockTraceListenerData data = new MockTraceListenerData("listener");
            data.Filter = SourceLevels.All;

            helper.loggingSettings.TraceListeners.Add(data);

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);

            Assert.IsNull(listener.Filter);
        }

        [TestMethod]
        public void CreatedTraceListenerHasCorrectFilter()
        {
            MockLogObjectsHelper helper = new MockLogObjectsHelper();
            MockTraceListenerData data = new MockTraceListenerData("listener");
            data.Filter = SourceLevels.Critical;

            helper.loggingSettings.TraceListeners.Add(data);

            TraceListener listener = TraceListenerCustomFactory.Instance.Create(context, "listener", helper.configurationSource, reflectionCache);

            Assert.IsNotNull(listener.Filter);

            EventTypeFilter filter = (EventTypeFilter)listener.Filter;

            Assert.AreEqual(data.Filter, filter.EventType);
        }
    }
}
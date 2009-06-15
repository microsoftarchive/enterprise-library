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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Tests.Configuration.Unity
{
    [TestClass]
    public class FormattedDatabaseTraceListenerPolicyCreatorFixture
    {
        private LoggingSettings loggingSettings;
        private ConnectionStringsSection connectionStringsSection;
        private DictionaryConfigurationSource configurationSource;

        [TestInitialize]
        public void SetUp()
        {
            loggingSettings = new LoggingSettings();
            connectionStringsSection = new ConnectionStringsSection();
            configurationSource = new DictionaryConfigurationSource();
            configurationSource.Add(LoggingSettings.SectionName, loggingSettings);
            configurationSource.Add("connectionStrings", connectionStringsSection);
        }

        private IUnityContainer CreateContainer()
        {
            return new UnityContainer()
                .AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
        }

        [TestMethod]
        public void CanCreatePoliciesForProvider()
        {
            FormattedDatabaseTraceListenerData listenerData
                = new FormattedDatabaseTraceListenerData("listener", "write", "add", "database", "");
            listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
            listenerData.Filter = SourceLevels.Error;
            loggingSettings.TraceListeners.Add(listenerData);

            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings("database", "foo=bar;", "System.Data.SqlClient"));

            using (var container = CreateContainer())
            {
                FormattedDatabaseTraceListener createdObject =
                    (FormattedDatabaseTraceListener)container.Resolve<TraceListener>("listener\u200cimplementation");

                Assert.IsNotNull(createdObject);
                Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
                Assert.IsNotNull(createdObject.Filter);
                Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
                Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
                Assert.IsNull(createdObject.Formatter);
            }
        }

        [TestMethod]
        public void CanCreatePoliciesForProviderWithFormatter()
        {
            FormattedDatabaseTraceListenerData listenerData
                = new FormattedDatabaseTraceListenerData("listener", "write", "add", "database", "formatter");
            listenerData.TraceOutputOptions = TraceOptions.Callstack | TraceOptions.ProcessId;
            listenerData.Filter = SourceLevels.Error;
            loggingSettings.TraceListeners.Add(listenerData);

            FormatterData formatterData = new TextFormatterData("formatter", "template");
            loggingSettings.Formatters.Add(formatterData);

            connectionStringsSection.ConnectionStrings.Add(
                new ConnectionStringSettings("database", "foo=bar;", "System.Data.SqlClient"));

            using (var container = CreateContainer())
            {
                FormattedDatabaseTraceListener createdObject =
                    (FormattedDatabaseTraceListener)container.Resolve<TraceListener>("listener\u200cimplementation");

                Assert.IsNotNull(createdObject);
                Assert.AreEqual(listenerData.TraceOutputOptions, createdObject.TraceOutputOptions);
                Assert.IsNotNull(createdObject.Filter);
                Assert.IsInstanceOfType(createdObject.Filter, typeof(EventTypeFilter));
                Assert.AreEqual(listenerData.Filter, ((EventTypeFilter)createdObject.Filter).EventType);
                Assert.IsNotNull(createdObject.Formatter);
                Assert.AreSame(typeof(TextFormatter), createdObject.Formatter.GetType());
                Assert.AreEqual("template", ((TextFormatter)createdObject.Formatter).Template);
            }
        }
    }
}

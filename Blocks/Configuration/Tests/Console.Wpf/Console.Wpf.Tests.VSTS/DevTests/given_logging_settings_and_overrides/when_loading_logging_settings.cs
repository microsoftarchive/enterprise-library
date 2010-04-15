//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_logging_settings_and_overrides
{
    public abstract class LoggingSettingsContext : ContainerContext
    {
        protected SectionViewModel LoggingSectionViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceBuilder sourceBuilder = new ConfigurationSourceBuilder();

            sourceBuilder
                .ConfigureLogging()
                .WithOptions
                .FilterCustom<MockLogFilter>("filter")
                .LogToCategoryNamed("General")
                .SendTo
                .SystemDiagnosticsListener("listener")
                .SendTo
                .Msmq("msmqlistener")
                .LogToCategoryNamed("Other")
                .SendTo
                .EventLog("eventlog"); ;

            DesignDictionaryConfigurationSource source = new DesignDictionaryConfigurationSource();
            sourceBuilder.UpdateConfigurationWithReplace(source);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);

            LoggingSectionViewModel = sourceModel.Sections.Where(x => x.SectionName == LoggingSettings.SectionName).First();
        }

    }

    internal class MockLogFilter : ILogFilter
    {
        #region ILogFilter Members

        public bool Filter(Microsoft.Practices.EnterpriseLibrary.Logging.LogEntry log)
        {
            throw new NotImplementedException();
        }

        public string Name
        {
            get { throw new NotImplementedException(); }
        }

        #endregion
    }

    [TestClass]
    public class when_loading_logging_settings : LoggingSettingsContext
    {
        ElementViewModel customSystemDiagnostic;

        protected override void Act()
        {
            customSystemDiagnostic = LoggingSectionViewModel.DescendentConfigurationsOfType<SystemDiagnosticsTraceListenerData>().FirstOrDefault();
        }

        [TestMethod]
        public void then_the_system_diagnostic_trace_listener_type_name_is_not_readonly()
        {
            var typeNameProperty = customSystemDiagnostic.Properties.FirstOrDefault(p => p.PropertyName == "TypeName");
            Assert.IsFalse(typeNameProperty.ReadOnly);
        }
    }
}

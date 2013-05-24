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
using System.Linq;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.HostAdapter;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Hosting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_host_adapter
{
    public abstract class given_host_adapter : ContainerContext
    {
        protected ISingleHierarchyConfigurationUIHostAdapter HostAdapter;
        protected SectionViewModel LoggingViewModel;
        protected ElementViewModel TraceListener;

        protected override void Arrange()
        {
            base.Arrange();

            HostAdapter = new SingleHierarchyConfigurationUIHostAdapter(new HostAdapterConfiguration(AppDomain.CurrentDomain.BaseDirectory), null);
            ConfigurationSourceModel sourceModel = (ConfigurationSourceModel)HostAdapter.GetService(typeof(ConfigurationSourceModel));

            sourceModel.AddSection(
                LoggingSettings.SectionName,
                new LoggingSettings
                {
                    DefaultCategory = "category",
                    TraceSources = { new TraceSourceData("category", System.Diagnostics.SourceLevels.Critical) },
                    TraceListeners = { new FormattedEventLogTraceListenerData("event log", "source", "formatter") },
                    Formatters = { new CustomFormatterData("formatter", "custom formatter type") }
                });

            LoggingViewModel = sourceModel.Sections.Single();
            TraceListener = LoggingViewModel.GetDescendentsOfType<TraceListenerData>().First();
        }
    }
}

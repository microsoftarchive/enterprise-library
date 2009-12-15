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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Console.Wpf.Tests.VSTS.Mocks;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;

namespace Console.Wpf.Tests.VSTS.DevTests.given_logging_settings_and_overrides
{
    public abstract class given_logging_settings_and_overrides : ContainerContext
    {
        protected SectionViewModel LoggingSectionViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceBuilder sourceBuilder = new ConfigurationSourceBuilder();
            sourceBuilder.ConfigureLogging().LogToCategoryNamed("General").SendTo.EventLog("Listener");

            DesignDictionaryConfigurationSource source = new DesignDictionaryConfigurationSource();
            sourceBuilder.UpdateConfigurationWithReplace(source);

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.Load(source);
            sourceModel.NewEnvironment();

            LoggingSectionViewModel = sourceModel.Sections.Where(x => x.SectionName == LoggingSettings.SectionName).First();
        }
    }

    [TestClass]
    public class when_overriding_tracelistener_references : given_logging_settings_and_overrides
    {
        Property overridesProperty;
        protected override void Act()
        {
            var traceSourceData = LoggingSectionViewModel.GetDescendentsOfType<TraceSourceData>().First();
            overridesProperty = traceSourceData.Properties.Where(x => x.PropertyName.Contains("Override")).First();
            overridesProperty.Value = true;
        }

        [TestMethod]
        public void then_overridden_listeners_have_special_editor()
        {
            var overriddenTraceListeners = overridesProperty.ChildProperties.Where(x => x.PropertyName == "TraceListeners").First();
            Assert.IsInstanceOfType(overriddenTraceListeners.Visual, typeof(OverridenTraceListenerCollectionEditor));
        }
    }
}

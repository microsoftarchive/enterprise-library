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
using Console.Wpf.Tests.VSTS.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Logging.given_logging_configuration
{
    public abstract class given_system_diagnostics_trace_listener_data : LoggingConfigurationContext
    {
        protected ElementViewModel DiagnosticsTraceListner;
        protected override void Arrange()
        {
            base.Arrange();

            ConfigurationSourceModel sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.New();

            var loggingSection = SectionViewModel.CreateSection(Container, LoggingSettings.SectionName, base.LoggingSection);
            DiagnosticsTraceListner = loggingSection.GetDescendentsOfType<SystemDiagnosticsTraceListenerData>().First();
        }
    }

    [TestClass]
    public class when_accessing_properties_on_system_diagnostics_listener : given_system_diagnostics_trace_listener_data
    {
        IEnumerable<Property> properties;

        protected override void Act()
        {
            properties = DiagnosticsTraceListner.Properties;
        }

        [TestMethod]
        public void then_listener_has_initdata_property()
        {
            Assert.IsTrue(properties.Any(x => x.PropertyName == "InitData"));
        }

        [TestMethod]
        public void then_listener_has_no_attributes()
        {
            Assert.IsFalse(properties.Any(x => x.PropertyName == "Attributes"));
        }

        [TestMethod]
        public void then_listener_initdata_can_be_overwritten()
        {
            var initData = properties.Single(x => x.PropertyName == "InitData");
            Assert.IsTrue(((IEnvironmentalOverridesProperty)initData).SupportsOverride);
            Assert.AreEqual("initializeData", ((IEnvironmentalOverridesProperty)initData).PropertyAttributeName);
        }
    }
}

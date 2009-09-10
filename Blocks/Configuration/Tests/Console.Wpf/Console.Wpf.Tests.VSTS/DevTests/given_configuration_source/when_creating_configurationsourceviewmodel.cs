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
using Console.Wpf.Tests.VSTS.TestSupport;
using Console.Wpf.ViewModel;
using Console.Wpf.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Moq;
using System.ComponentModel.Design;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source
{

    [TestClass]
    public class when_creating_configurationsourceviewmodel : ArrangeActAssert
    {
        private DictionaryConfigurationSource configSource;
        private ConfigurationSourceViewModel configurationSourceViewModel;

        protected override void Arrange()
        {

            var builder = new TestConfigurationBuilder();
            configSource = new DictionaryConfigurationSource();
            builder.AddExceptionSettings()
                .Build(configSource);

            ServiceContainer container = new ServiceContainer();

            var mockLocator = new Mock<ConfigurationSectionLocator>();
            mockLocator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { ExceptionHandlingSettings.SectionName });

            container.AddService(typeof (ConfigurationSectionLocator), mockLocator.Object);

            configurationSourceViewModel = new ConfigurationSourceViewModel(container);
        }

        protected override void Act()
        {
            configurationSourceViewModel.Open(configSource);
        }

        [TestMethod]
        public void then_configuration_model_contains_exception_section()
        {
            Assert.AreEqual(1, configurationSourceViewModel.Sections.Count(s => s.Name == Resources.SectionDisplayName));
        }

        [TestMethod]
        public void then_does_not_createsection_if_non_existant()
        {
            Assert.AreEqual(1, configurationSourceViewModel.Sections.Count());
        }
    }
}

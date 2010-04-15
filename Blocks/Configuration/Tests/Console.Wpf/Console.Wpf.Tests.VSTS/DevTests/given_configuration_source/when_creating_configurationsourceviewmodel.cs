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

using System.Linq;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;

namespace Console.Wpf.Tests.VSTS.DevTests.given_configuration_source
{

    [TestClass]
    public class when_creating_configurationsourceviewmodel : ContainerContext
    {
        private DesignDictionaryConfigurationSource configSource;
        private ConfigurationSourceModel configurationSourceViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            var builder = new TestConfigurationBuilder();
            configSource = new DesignDictionaryConfigurationSource();
            builder.AddExceptionSettings()
                .Build(configSource);

            var mockLocator = new Mock<ConfigurationSectionLocator>();
            mockLocator.Setup(x => x.ConfigurationSectionNames).Returns(new[] { ExceptionHandlingSettings.SectionName });

            Container.RegisterInstance<ConfigurationSectionLocator>(mockLocator.Object);

            configurationSourceViewModel = Container.Resolve<ConfigurationSourceModel>();
        }

        protected override void Act()
        {
            configurationSourceViewModel.Load(configSource);
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

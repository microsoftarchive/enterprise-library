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
using Console.Wpf.Tests.VSTS.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Console.Wpf.Tests.VSTS.DevTests.given_satelite_provider_add_command
{
    [TestClass]
    public class when_executing_satelite_provider : LoggingConfigurationContext
    {
        private ConfigurationSourceModel configurationSourceModel;

        protected override void Arrange()
        {
            base.Arrange();
            this.Container.RegisterInstance(new Mock<IAssemblyDiscoveryService>().Object);
        }

        protected override void Act()
        {
            configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            var loggingViewModel = configurationSourceModel.AddSection(LoggingSettings.SectionName, LoggingSection);
            var tracelistenersCollection = loggingViewModel.GetDescendentsOfType<TraceListenerDataCollection>().First();
            var addDBtracelinerCommand = tracelistenersCollection.Commands.First().ChildCommands.Where(x => x.Title == "Add Database Trace Listener").First();
            addDBtracelinerCommand.Execute(null);

        }

        [TestMethod]
        public void then_dependend_block_is_added()
        {
            Assert.IsTrue(configurationSourceModel.HasSection(DataAccessDesignTime.ConnectionStringSettingsSectionName));
        }

        [TestMethod]
        public void then_provider_reference_is_set_to_default()
        {
            var elementLookup = Container.Resolve<ElementLookup>();
            var sateliteProvider = elementLookup.FindInstancesOfConfigurationType(typeof(FormattedDatabaseTraceListenerData)).FirstOrDefault();
            Assert.IsFalse(String.IsNullOrEmpty((string)sateliteProvider.Property("DatabaseInstanceName").Value));

        }
    }
}

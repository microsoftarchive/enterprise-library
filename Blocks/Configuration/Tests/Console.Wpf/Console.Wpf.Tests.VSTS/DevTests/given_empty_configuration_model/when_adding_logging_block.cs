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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Console.Wpf.Tests.VSTS.DevTests
{
    [TestClass]
    public class when_adding_logging_block : given_empty_configuration_model
    {
        ConfigurationSourceModel sourceModel;

        protected override void Arrange()
        {
            base.Arrange();
            sourceModel = Container.Resolve<ConfigurationSourceModel>();
        }

        protected override void Act()
        {
            var menuService = Container.Resolve<MenuCommandService>();
            var blockCommands = menuService.GetCommands(CommandPlacement.BlocksMenu);
            blockCommands.OfType<AddLoggingBlockCommand>().First().Execute(null);
        }

        [TestMethod]
        public void then_view_model_contains_logging_block()
        {
            Assert.IsTrue(sourceModel.Sections.OfType<LoggingSectionViewModel>().Any());
        }

        [TestMethod]
        public void then_logging_block_has_name()
        {
            LoggingSectionViewModel loggingModel = sourceModel.Sections.OfType<LoggingSectionViewModel>().First();
            Assert.IsFalse(string.IsNullOrEmpty(loggingModel.Name));
        }

        [TestMethod]
        public void then_logging_block_has_sources_tracelistener_and_formatter()
        {
            LoggingSectionViewModel loggingModel = sourceModel.Sections.OfType<LoggingSectionViewModel>().First();
            Assert.AreEqual(4, loggingModel.GetDescendentsOfType<TraceSourceData>().Count());
            Assert.AreEqual(1, loggingModel.GetDescendentsOfType<FormattedEventLogTraceListenerData>().Count());
            Assert.AreEqual(1, loggingModel.GetDescendentsOfType<TextFormatterData>().Count());
        }
    }
}

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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model
{
    [TestClass]
    public class when_querying_model_for_references : ExceptionHandlingSettingsContext
    {
        SectionViewModel viewModel;
        ElementLookup lookup;

        protected override void Act()
        {
            var configurationSourceModel = Container.Resolve<ConfigurationSourceModel>();
            viewModel = configurationSourceModel.AddSection(ExceptionHandlingSettings.SectionName, Section);
            lookup = Container.Resolve<ElementLookup>();
        }

        [TestMethod]
        public void then_querying_for_polymorphic_base_returns_all_realized_instances()
        {
            var handlers = lookup.FindInstancesOfConfigurationType(typeof(ExceptionHandlerData));
            Assert.IsTrue(handlers.Where(x => x.ConfigurationType == typeof(ReplaceHandlerData)).Any());
            Assert.IsTrue(handlers.Where(x => x.ConfigurationType == typeof(WrapHandlerData)).Any());
        }
    }

}

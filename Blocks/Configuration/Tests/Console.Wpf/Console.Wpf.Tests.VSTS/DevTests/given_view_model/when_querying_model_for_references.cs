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
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Console.Wpf.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.ViewModel.Services;

namespace Console.Wpf.Tests.VSTS.DevTests.given_view_model
{
    [TestClass]
    public class when_querying_model_for_references : ExceptionHandlingSettingsContext
    {
        SectionViewModel viewModel;
        ElementLookup lookup;

        protected override void Act()
        {
            viewModel = SectionViewModel.CreateSection(ServiceProvider, Section);
            lookup = (ElementLookup)ServiceProvider.GetService(typeof(ElementLookup));
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

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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Logging
{
    [TestClass]
    public class when_getting_related_elements_on_trace_listeners : LoggingConfigurationContext
    {
        SectionViewModel sectionViewModel;
        private IEnumerable<ElementViewModel> relatedElements;

        protected override void Arrange()
        {
            base.Arrange();

            var lookup = Container.Resolve<ElementLookup>();
            sectionViewModel = SectionViewModel.CreateSection(Container, LoggingSettings.SectionName, LoggingSection);
            sectionViewModel.Initialize(new InitializeContext());
            lookup.AddSection(sectionViewModel);
        }

        protected override void Act()
        {
            var tracelistener = sectionViewModel.GetDescendentsOfType<FormattedEventLogTraceListenerData>().First();
            relatedElements = sectionViewModel.GetRelatedElements(tracelistener);
        }

        [TestMethod]
        public void then_tracelistener_returns_categories()
        {
            Assert.IsTrue(relatedElements.Where(x => x.Name == "General").Any());
            Assert.IsTrue(relatedElements.Where(x => x.Name == "Critical").Any());
        }

        [TestMethod]
        public void then_should_not_return_unrelated_categories()
        {
            Assert.IsTrue(sectionViewModel.DescendentConfigurationsOfType<TraceSourceData>().Any(x => x.Name == "msmq"));
            Assert.IsFalse(relatedElements.Where(x => x.Name == "msmq").Any());
        }
    }
}

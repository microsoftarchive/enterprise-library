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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_model_for_disposal
{
    [TestClass]
    public class when_removing_actual_section : LoggingConfigurationContext
    {
        private WeakReference[] weakReferenceList;
        private ConfigurationSourceModel sourceModel;

        protected override void Arrange()
        {
            base.Arrange();

            sourceModel = Container.Resolve<ConfigurationSourceModel>();
            sourceModel.AddSection(LoggingSettings.SectionName, LoggingSection);

            var loggingSectionViewModel = sourceModel.Sections.Where(x => typeof(LoggingSettings) == x.ConfigurationType).First();
            weakReferenceList = loggingSectionViewModel.DescendentElements().Union(new[] { loggingSectionViewModel })
                .Select(x => new WeakReference(x)).ToArray();
        }

        protected override void Act()
        {
            sourceModel.RemoveSection(LoggingSettings.SectionName);
            GC.Collect();
        }

        [TestMethod]
        public void then_all_elements_collected()
        {
            Assert.IsTrue(weakReferenceList.All(r => r.IsAlive == false));
        }
    }
}

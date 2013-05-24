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
using Console.Wpf.Tests.VSTS.Contexts;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_reference
{
    [TestClass]
    public class when_updating_provider_name_in_reference_picker_list : LoggingConfigurationContext
    {
        ElementReferenceProperty defaultCategoryProperty;
        ElementViewModel traceSource;
        PropertyChangedListener defaultCacheManagerPropertyChangedListener;
        private SectionViewModel LoggingViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            LoggingViewModel = sourceModel.AddSection(LoggingSettings.SectionName, LoggingSection);

            defaultCategoryProperty = (ElementReferenceProperty)LoggingViewModel.Property("DefaultCategory");
            defaultCategoryProperty.Initialize(null);

            traceSource = LoggingViewModel.GetDescendentsOfType<TraceSourceData>().First();

            defaultCacheManagerPropertyChangedListener = new PropertyChangedListener(defaultCategoryProperty);
        }

        protected override void Act()
        {
            traceSource.Property("Name").Value = "new name";
        }

        [TestMethod]
        public void then_property_suggested_values_changed()
        {
            Assert.IsTrue(defaultCacheManagerPropertyChangedListener.ChangedProperties.Contains("SuggestedValues"));
        }
    }
}

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

using Console.Wpf.Tests.VSTS.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_reference
{
    [TestClass]
    public class when_assiging_value_to_reference : LoggingConfigurationContext
    {
        ElementReferenceProperty defaultCacheManagerProperty;
        bool elementReferencesChanged;
        private SectionViewModel LoggingViewModel;

        protected override void Arrange()
        {
            base.Arrange();

            var sourceModel = Container.Resolve<ConfigurationSourceModel>();
            LoggingViewModel = sourceModel.AddSection(LoggingSettings.SectionName, LoggingSection);

            elementReferencesChanged = false;
            LoggingViewModel.ElementReferencesChanged += (sender, args) => elementReferencesChanged = true;

            defaultCacheManagerProperty = (ElementReferenceProperty)LoggingViewModel.Property("DefaultCategory");
            defaultCacheManagerProperty.Initialize(null);
        }


        protected override void Act()
        {
            defaultCacheManagerProperty.Value = "new name";
        }


        [TestMethod]
        public void then_containg_element_references_changed()
        {
            Assert.IsTrue(elementReferencesChanged);
        }
    }
}

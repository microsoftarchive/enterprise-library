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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Console.Wpf.Tests.VSTS.DevTests.given_invalid_model
{
    [TestClass]
    public class when_navigating_validation_error : CachingElementModelContext
    {
        private ValidationModel validationModel;
        private ValidationResult result;
        private ElementViewModel cacheManager;

        protected override void Arrange()
        {
            base.Arrange();
            validationModel = Container.Resolve<ValidationModel>();
            cacheManager = CachingSettingsViewModel.DescendentElements(x => x.ConfigurationType == typeof(CacheManagerData)).First();

            var bindableProperty = cacheManager.Property("ExpirationPollFrequencyInSeconds").BindableProperty;
            bindableProperty.BindableValue = "Invalid";

            result = bindableProperty.Property.ValidationResults.First();
        }

        protected override void Act()
        {
            validationModel.Navigate(result);
        }

        [TestMethod]
        public void then_element_is_selected()
        {
            Assert.IsTrue(cacheManager.IsSelected);
        }

        [TestMethod]
        public void then_section_is_exanded()
        {
            Assert.IsTrue(cacheManager.ContainingSection.IsExpanded);
        }

        [TestMethod]
        public void then_element_properties_are_shown()
        {
            Assert.IsTrue(cacheManager.PropertiesShown);
        }
    }
}

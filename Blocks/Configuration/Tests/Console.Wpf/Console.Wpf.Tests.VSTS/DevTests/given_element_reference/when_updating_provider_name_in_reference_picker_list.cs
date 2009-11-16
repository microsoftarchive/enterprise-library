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
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration;
using Console.Wpf.Tests.VSTS.DevTests.given_caching_configuraton;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_reference
{
    [TestClass]
    public class when_updating_provider_name_in_reference_picker_list : given_caching_configuration
    {
        ElementReferenceProperty defaultCacheManagerProperty;
        ElementViewModel cacheManager;
        PropertyChangedListener defaultCacheManagerPropertyChangedListener;

        protected override void Arrange()
        {
            base.Arrange();
            defaultCacheManagerProperty = (ElementReferenceProperty)CachingViewModel.Property("DefaultCacheManager");
            defaultCacheManagerProperty.Initialize(null);

            cacheManager = CachingViewModel.GetDescendentsOfType<CacheManagerData>().First();

            defaultCacheManagerPropertyChangedListener = new PropertyChangedListener(defaultCacheManagerProperty);
        }


        protected override void Act()
        {
            cacheManager.Property("Name").Value = "new name";
        }

        [TestMethod]
        public void then_property_suggested_values_changed()
        {
            Assert.IsTrue(defaultCacheManagerPropertyChangedListener.ChangedProperties.Contains("SuggestedValues"));
        }

    }
}

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
using Console.Wpf.Tests.VSTS.DevTests.given_caching_configuraton;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_reference
{
    public class given_reference_property : given_caching_configuration
    {
        protected ElementReferenceProperty DefaultCacheManagerProperty;
        protected ElementViewModel CacheManager;

        protected override void Arrange()
        {
            base.Arrange();
            DefaultCacheManagerProperty = (ElementReferenceProperty)CachingViewModel.Property("DefaultCacheManager");
            DefaultCacheManagerProperty.Initialize(null);

            CacheManager = CachingViewModel.GetDescendentsOfType<CacheManagerData>().First();
        }
    }

    [TestClass]
    public class when_dropping_down_reference_picker : given_reference_property
    {
        [TestMethod]
        public void then_reference_property_has_none_option()
        {
            Assert.IsTrue(DefaultCacheManagerProperty.BindableSuggestedValues.Contains("<none>"));
        }
    }

    [TestClass]
    public class when_setting_reference_to_none : given_reference_property
    {
        protected override void Act()
        {
            DefaultCacheManagerProperty.BindableValue = "<none>";
        }

        [TestMethod]
        public void then_property_value_is_empty_string()
        {
            Assert.IsTrue(string.IsNullOrEmpty((string)DefaultCacheManagerProperty.Value));
        }
    }

    [TestClass]
    public class when_gettting_empty_reference_property : given_reference_property
    {
        protected override void Act()
        {
            CachingConfiguration.DefaultCacheManager = string.Empty;
        }

        [TestMethod]
        public void then_property_bindable_value_is_none()
        {
            Assert.AreEqual("<none>", DefaultCacheManagerProperty.BindableValue);
        }

        [TestMethod]
        public void then_property_value_is_empty()
        {
            Assert.IsTrue(string.IsNullOrEmpty((string)DefaultCacheManagerProperty.Value));
        }
    }
}

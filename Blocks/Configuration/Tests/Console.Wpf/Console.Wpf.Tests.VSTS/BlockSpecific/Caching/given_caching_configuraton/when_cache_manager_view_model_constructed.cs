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
using Console.Wpf.Tests.VSTS.Mocks;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;

namespace Console.Wpf.Tests.VSTS.BlockSpecific.Caching
{
    [TestClass]
    public class when_cache_manager_view_model_constructed : given_caching_configuraton.given_caching_configuration
    {
        [TestMethod]
        public void then_backing_store_custom_property_suggested_values_includes_empty_value()
        {
            Assert.IsTrue(CacheManager.Property("CacheStorage").SuggestedValues.Contains(string.Empty));
        }

        [TestMethod]
        public void then_backing_store_custom_property_bindable_includes_none_option()
        {
            Assert.IsTrue(((SuggestedValuesBindableProperty)CacheManager.Property("CacheStorage").BindableProperty).BindableSuggestedValues.Contains("<none>"));
        }
    }
}

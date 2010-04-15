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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Console.Wpf.Tests.VSTS.DevTests.given_element_reference
{
    [TestClass]
    public class when_assiging_value_to_reference : CachingConfigurationContext
    {
        ElementReferenceProperty defaultCacheManagerProperty;
        bool elementReferencesChanged;

        protected override void Arrange()
        {
            base.Arrange();

            elementReferencesChanged = false;
            CachingViewModel.ElementReferencesChanged += (sender, args) => elementReferencesChanged = true;

            defaultCacheManagerProperty = (ElementReferenceProperty)CachingViewModel.Property("DefaultCacheManager");
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

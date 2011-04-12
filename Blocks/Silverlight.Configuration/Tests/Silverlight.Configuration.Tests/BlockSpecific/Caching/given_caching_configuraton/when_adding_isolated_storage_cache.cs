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
using Console.Wpf.Tests.VSTS.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Configuration.Design.HostAdapterV5;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Caching.given_caching_configuraton
{
    [TestClass]
    public class when_adding_isolated_storage_cache : Context
    {
        protected override void Act()
        {
            Assert.AreEqual(0, TestCachingSettings.Caches.Count);
            AddNewIsolatedStorageCache.Execute(null);
        }

        [TestMethod]
        public void then_newly_isolated_storage_added()
        {
            Assert.AreEqual(1, TestCachingSettings.Caches.Count);
            Assert.IsFalse(string.IsNullOrEmpty((string)AddNewIsolatedStorageCache.AddedElementViewModel.Property("Name").Value));
        }

    }
}

//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Tests.VSTS.BlockSpecific.Caching.given_caching_configuraton
{
    [TestClass]
    public class when_adding_in_memory_cache : Context
    {
        protected override void Act()
        {
            Assert.AreEqual(0, TestCachingSettings.Caches.Count);
            AddNewInMemoryCache.Execute(null);
        }

        [TestMethod]
        public void then_newly_in_memory_cache_added()
        {
            Assert.AreEqual(1, TestCachingSettings.Caches.Count);
            Assert.IsFalse(string.IsNullOrEmpty((string)AddNewInMemoryCache.AddedElementViewModel.Property("Name").Value));
        }

    }
}

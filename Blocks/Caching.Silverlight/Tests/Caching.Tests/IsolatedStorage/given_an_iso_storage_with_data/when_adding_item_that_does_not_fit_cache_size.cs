//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_item_that_does_not_fit_cache_size : Context
    {
        protected override int MaxSize
        {
            get { return 8; }
        }

        private bool added;

        protected override void Act()
        {
            base.Act();

            added = Cache.Add("newIem", new byte[9000], new CacheItemPolicy());

            base.RefreshCache();
        }

        [TestMethod]
        public void then_returns_true_on_addition_bacause_it_fails_silently()
        {
            Assert.IsNull(base.initializeException);
            Assert.IsTrue(added);
        }

        [TestMethod]
        public void then_item_is_null()
        {
            Assert.IsNull(Cache["newItem"]);
        }
    }
}

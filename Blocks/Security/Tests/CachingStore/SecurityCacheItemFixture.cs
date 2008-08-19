//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Tests
{
    [TestClass]
    public class SecurityCacheItemFixture
    {
        [TestMethod]
        public void InitTest()
        {
            SecurityCacheItem item = new SecurityCacheItem();
            Assert.IsNull(item.Identity);
            Assert.IsNull(item.Principal);
            Assert.IsNull(item.Profile);

            Assert.IsTrue(item.IsRemoveable);
        }

        [TestMethod]
        public void PropertyTest()
        {
            SecurityCacheItem item = new SecurityCacheItem();

            item.Identity = new GenericIdentity("test");
            Assert.AreEqual("test", item.Identity.Name);

            item.Principal = new GenericPrincipal(new GenericIdentity("test"), new string[0]);
            Assert.AreEqual("test", item.Principal.Identity.Name);

            item.Profile = 0;
            Assert.AreEqual(0, (Int32)item.Profile);

            Assert.IsFalse(item.IsRemoveable);
        }
    }
}
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

using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_empty_iso_storage_cache
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_just_above_scavenging_limit : Context
    {
        private string largeValue;

        protected override void Act()
        {
            base.Act();
            largeValue = new string('a', (int)(MaxSize * (QuotaUsedAfterScavenging + 5) * 1024 / 100));
            
            Cache.Add("large", largeValue, new CacheItemPolicy());
        }

        [TestMethod]
        public void then_item_is_added()
        {
            Assert.IsNotNull(Cache["large"]);
        }

        [TestMethod]
        public void then_not_further_addition_does_not_trigger_scavenging()
        {
            Thread.Sleep(400);

            Assert.IsNotNull(Cache["large"]);
        }

        [TestMethod]
        public void then_adding_other_item_fails_silently()
        {
            Cache["new"] = largeValue;
            Assert.IsNull(Cache["new"]);
        }

        [TestMethod]
        public void then_adding_other_item_triggers_scavenging_for_successfull_add_next_time()
        {
            Assert.IsNotNull(Cache["large"]);
            Cache["new"] = largeValue;
            Assert.IsNull(Cache["new"]);

            for (int i = 0; i < 50 && Cache["large"] != null; i++)  // time-out at 5 seconds
            {
                Thread.Sleep(100);
            }

            Assert.IsNull(Cache["large"]);
            Cache["new"] = largeValue;
            Assert.IsNotNull(Cache["new"]);
        }
    }
}

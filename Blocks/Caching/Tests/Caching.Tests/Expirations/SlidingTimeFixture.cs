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

using System;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    [TestClass]
    public class SlidingTimeFixture
    {
        [TestMethod]
        public void WillUpdateLastTouchedTimeWhenNotified()
        {
            DateTime initialTimeStamp = DateTime.Now - TimeSpan.FromSeconds(5.0);
            TimeSpan expirationWindow = TimeSpan.FromSeconds(5.0);
            SlidingTime slidingTime = new SlidingTime(expirationWindow, initialTimeStamp);

            DateTime now = DateTime.Now;

            slidingTime.Notify();

            Assert.IsTrue(slidingTime.TimeLastUsed >= now);
        }

        [TestMethod]
        public void CanInitializeWithLastUpdatedTimeFromCacheItem()
        {
            CacheItem item = new CacheItem("key", "value", CacheItemPriority.Normal, null);
            DateTime timestampToSave = DateTime.Now + TimeSpan.FromDays(1.0);
            item.SetLastAccessedTime(timestampToSave);

            DateTime initialTimeStamp = DateTime.Now - TimeSpan.FromSeconds(5.0);
            TimeSpan expirationWindow = TimeSpan.FromSeconds(5.0);
            SlidingTime slidingTime = new SlidingTime(expirationWindow, initialTimeStamp);

            slidingTime.Initialize(item);

            Assert.AreEqual(timestampToSave, slidingTime.TimeLastUsed);
        }

        [TestMethod]
        public void WillExpireOnSchedule()
        {
            SlidingTime expiration = new SlidingTime(TimeSpan.FromSeconds(1.5));
            Thread.Sleep(2000);
            Assert.IsTrue(expiration.HasExpired(), "Should have expired after enough time elapsed");
        }

        [TestMethod]
        public void ClassCanSerializeCorrectly()
        {
            SlidingTime slidingTime = new SlidingTime(new TimeSpan(0, 0, 2));

            var formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
            var stream = new System.IO.MemoryStream();
            formatter.Serialize(stream, slidingTime);
            stream.Position = 0;
            SlidingTime slidingTime2 = (SlidingTime)formatter.Deserialize(stream);

            Assert.AreEqual(slidingTime.ItemSlidingExpiration, slidingTime2.ItemSlidingExpiration);
            Assert.AreEqual(slidingTime.TimeLastUsed, slidingTime2.TimeLastUsed);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ConstructingWithATimeSpaceLessThanASecondThrowsException()
        {
            TimeSpan t = new TimeSpan(1);
            //Assert.IsFalse(t.TotalSeconds >= 1);
            SlidingTime slidingTime = new SlidingTime(t);
        }
    }
}

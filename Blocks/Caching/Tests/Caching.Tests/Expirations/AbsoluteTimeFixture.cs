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
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    [TestClass]
    public class AbsoluteTimeFixture
    {
        [TestMethod]
        public void WillExpireAfterOneSecondFromNow()
        {
            AbsoluteTime expiration = new AbsoluteTime(TimeSpan.FromSeconds(0.2));
            Assert.IsFalse(expiration.HasExpired(), "Should not be expired immediately after creation");
            Thread.Sleep(400);
            Assert.IsTrue(expiration.HasExpired(), "Should have expired by now");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowsExceptionIfTimeEqualToNow()
        {
            new AbsoluteTime(TimeSpan.FromSeconds(0.0));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ThrowsExceptionIfTimeInPast()
        {
            new AbsoluteTime(TimeSpan.FromSeconds(-1.0));
        }

        [TestMethod]
        public void ClassCanSerializeCorrectly()
        {
            AbsoluteTime absoluteTime = new AbsoluteTime(DateTime.Now.AddDays(2));

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, absoluteTime);
            stream.Position = 0;
            AbsoluteTime absoluteTime2 = (AbsoluteTime)formatter.Deserialize(stream);

            Assert.AreEqual(absoluteTime.AbsoluteExpirationTime, absoluteTime2.AbsoluteExpirationTime);
        }
    }
}

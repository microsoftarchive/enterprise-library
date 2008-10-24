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
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Expirations.Tests
{
    [TestClass]
    public class ExtendedFormatTimeFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void NullTimeFormatThrowsException()
        {
            ExtendedFormatTime time = new ExtendedFormatTime(null);
        }

        [TestMethod]
        public void ExtendFormatTimeExpiresCorrectly()
        {
            ExtendedFormatTime format = new ExtendedFormatTime("5 * * * *");
            Assert.IsFalse(format.HasExpired());
        }

        [TestMethod]
        public void ClassCanSerializeCorrectly()
        {
            ExtendedFormatTime format = new ExtendedFormatTime("5 * * * *");

            BinaryFormatter formatter = new BinaryFormatter();
            MemoryStream stream = new MemoryStream();
            formatter.Serialize(stream, format);
            stream.Position = 0;
            ExtendedFormatTime format2 = (ExtendedFormatTime)formatter.Deserialize(stream);

            Assert.AreEqual(format.TimeFormat, format2.TimeFormat);
        }
    }
}

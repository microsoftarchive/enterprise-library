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
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
    [TestClass]
    public class SerializationUtilityFixture
    {
        [TestMethod]
        public void CanSerializeString()
        {
            Assert.AreEqual("this is a string", ToObject(ToBytes("this is a string")));
        }

        [TestMethod]
        [ExpectedException(typeof(SerializationException))]
        public void CanSerializeUnserializableObject()
        {
            ToBytes(new NonSerializableClass());
        }

        [TestMethod]
        public void CanSerializeSerializableObject()
        {
            SerializableClass serializedInstance = (SerializableClass)ToObject(ToBytes(new SerializableClass()));
            Assert.AreEqual(1, serializedInstance.Counter);
        }

        [TestMethod]
        public void CanSerializeMBRObject()
        {
            using (FileStream outputStream = new FileStream("test.out", FileMode.Create))
            {
                new BinaryFormatter().Serialize(outputStream, new MarshalByRefClass(13));
            }

            object deserializedObject = null;
            using (FileStream inputStream = new FileStream("test.out", FileMode.Open))
            {
                deserializedObject = new BinaryFormatter().Deserialize(inputStream);
            }

            File.Delete("test.out");

            MarshalByRefClass serializedInstance = (MarshalByRefClass)deserializedObject;
            Assert.AreEqual(13, serializedInstance.Counter);
        }

        [TestMethod]
        public void TryingToSerializeNullObjectReturnsNull()
        {
            Assert.IsNull(ToBytes(null));
        }

        [TestMethod]
        public void TryingToDeserializeNullArrayOfBytesReturnsNull()
        {
            Assert.IsNull(ToObject(null));
        }

        [TestMethod]
        public void CanSerializeAndDeserializeRefreshAction()
        {
            byte[] refreshBytes = ToBytes(new RefreshAction());
            object refreshObject = ToObject(refreshBytes);

            Assert.AreEqual(typeof(RefreshAction), refreshObject.GetType());
            Assert.IsTrue(refreshBytes.Length < 2048);
        }

        byte[] ToBytes(object objectToSerialize)
        {
            return SerializationUtility.ToBytes(objectToSerialize);
        }

        object ToObject(byte[] serializedObject)
        {
            return SerializationUtility.ToObject(serializedObject);
        }

        [Serializable]
        class RefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey,
                                object expiredValue,
                                CacheItemRemovedReason removalReason) {}
        }
    }

    [Serializable]
    class SerializableClass
    {
        int counter = 1;

        public int Counter
        {
            get { return counter; }
        }
    }

    class NonSerializableClass {}

    [Serializable]
    class MarshalByRefClass : MarshalByRefObject
    {
        int counter;

        public MarshalByRefClass(int counter)
        {
            this.counter = counter;
        }

        public int Counter
        {
            get { return counter; }
        }
    }
}

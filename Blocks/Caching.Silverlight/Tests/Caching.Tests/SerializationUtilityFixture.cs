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

using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
#if !SILVERLIGHT
using System.IO;
#endif

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
#if !SILVERLIGHT
        [ExpectedException(typeof(SerializationException))]
#else
        [ExpectedException(typeof(InvalidDataContractException))]
#endif
        public void CanSerializeUnserializableObject()
        {
            ToBytes(new NonSerializableClass());
        }

        [TestMethod]
        public void CanSerializeSerializableObject()
        {
            SerializableClass serializedInstance = (SerializableClass)ToObject(ToBytes(new SerializableClass { Counter = 42 }));
            Assert.AreEqual(42, serializedInstance.Counter);
        }

#if !SILVERLIGHT
        [TestMethod]
        public void CanSerializeMBRObject()
        {
            using (FileStream outputStream = new FileStream("test.out", FileMode.Create))
            {
                new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Serialize(outputStream, new MarshalByRefClass(13));
            }

            object deserializedObject = null;
            using (FileStream inputStream = new FileStream("test.out", FileMode.Open))
            {
                deserializedObject = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter().Deserialize(inputStream);
            }

            File.Delete("test.out");

            MarshalByRefClass serializedInstance = (MarshalByRefClass)deserializedObject;
            Assert.AreEqual(13, serializedInstance.Counter);
        }
#endif

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

        [TestMethod]
        public void CanSerializeAndDeserializeArrayOfObjects()
        {
            var deserialized = (object[])ToObject(ToBytes(new object[] { new RefreshAction(), new SerializableClass() }));

            Assert.IsNotNull(deserialized);
            Assert.AreEqual(2, deserialized.Length);
            Assert.IsInstanceOfType(deserialized[0], typeof(RefreshAction));
            Assert.IsInstanceOfType(deserialized[1], typeof(SerializableClass));
        }

        byte[] ToBytes(object objectToSerialize)
        {
            return SerializationUtility.ToBytes(objectToSerialize);
        }

        object ToObject(byte[] serializedObject)
        {
            return SerializationUtility.ToObject(serializedObject);
        }

#if !SILVERLIGHT
        [System.Serializable]
        class RefreshAction : ICacheItemRefreshAction
        {
            public void Refresh(string removedKey,
                                object expiredValue,
                                CacheItemRemovedReason removalReason) { }
        }
#else
        public class RefreshAction { }
#endif

    }

#if !SILVERLIGHT
    [System.Serializable]
    class SerializableClass
#else
    public class SerializableClass
#endif
    {
        private int counter;

        public int Counter
        {
            get { return counter; }
            set { counter = value; }
        }
    }

    class NonSerializableClass { }

#if !SILVERLIGHT
    [System.Serializable]
    class MarshalByRefClass : System.MarshalByRefObject
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
#endif
}

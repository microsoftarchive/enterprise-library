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

using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Utility class for serializing and deserializing objects to and from byte streams
    /// </summary>
    public static class SerializationUtility
    {
        /// <summary>
        /// Converts an object into an array of bytes. Object must be serializable.
        /// </summary>
        /// <param name="value">Object to serialize. May be null.</param>
        /// <returns>Serialized object, or null if input was null.</returns>
        public static byte[] ToBytes(object value)
        {
            if (value == null)
            {
                return null;
            }

            byte[] inMemoryBytes;
            using (MemoryStream inMemoryData = new MemoryStream())
            {
                new BinaryFormatter().Serialize(inMemoryData, value);
                inMemoryBytes = inMemoryData.ToArray();
            }

            return inMemoryBytes;
        }

        /// <summary>
        /// Converts a byte array into an object. 
        /// </summary>
        /// <param name="serializedObject">Object to deserialize. May be null.</param>
        /// <returns>Deserialized object, or null if input was null.</returns>
        public static object ToObject(byte[] serializedObject)
        {
            if (serializedObject == null)
            {
                return null;
            }

            using (MemoryStream dataInMemory = new MemoryStream(serializedObject))
            {
                return new BinaryFormatter().Deserialize(dataInMemory);
            }
        }
    }
}
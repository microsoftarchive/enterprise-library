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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
    /// <summary>
    /// Utility class for serializing and deserializing objects to and from byte streams
    /// </summary>
    public static class SerializationUtility
    {
        private static readonly Encoding encoding = new UTF8Encoding(false);
        private static readonly char[] separator = new[] { '|' };

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

            using (var inMemoryData = new MemoryStream())
            {
                var writer = new StreamWriter(inMemoryData, encoding);
                var typesBuilder = new StringBuilder();
                typesBuilder.Append(value.GetType().AssemblyQualifiedName);
                typesBuilder.Append(separator);

                var knownTypes = new HashSet<Type>();

                if (!(value is string))
                {
                    var enumerableValue = value as IEnumerable;
                    if (enumerableValue != null)
                    {
                        foreach (var element in enumerableValue)
                        {
                            var type = element != null ? element.GetType() : null;
                            if (type != null && !type.IsPrimitive)
                            {
                                typesBuilder.Append(type.AssemblyQualifiedName);
                                typesBuilder.Append(separator);

                                knownTypes.Add(type);
                            }
                        }
                    }
                }
                writer.WriteLine(typesBuilder.ToString());
                writer.Flush();

                var serializer = new DataContractJsonSerializer(value.GetType(), knownTypes);
                serializer.WriteObject(inMemoryData, value);

                return inMemoryData.ToArray();
            }
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

            Type valueType;
            IEnumerable<Type> knownTypes;
            int position;

            using (var inMemoryData = new MemoryStream(serializedObject))
            {
                var reader = new StreamReader(inMemoryData, encoding);
                var knownTypeNames = reader.ReadLine();
                var splitTypeNames = knownTypeNames.Split(separator, StringSplitOptions.RemoveEmptyEntries);

                valueType = Type.GetType(splitTypeNames[0]);
                knownTypes = splitTypeNames.Skip(1).Select(tn => Type.GetType(tn));

                position = encoding.GetByteCount(knownTypeNames + Environment.NewLine);
            }

            // creating a new offset stream as DataContractJsonSerializer.ReadObject(Stream) always sets stream.Position = 0L
            using (var inMemoryData = new MemoryStream(serializedObject, position, serializedObject.Length - position))
            {
                var serializer = new DataContractJsonSerializer(valueType, knownTypes);
                return serializer.ReadObject(inMemoryData);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Serializes and deserializes log entries into byte arrays.
    /// </summary>
    public class LogEntrySerializer
    {
        /// <summary>
        /// Serializes a log entry into an array of bytes.
        /// </summary>
        /// <param name="entry">The entry to serialize.</param>
        /// <returns>A array of bytes representing the <paramref name="entry"/>.</returns>
        public byte[] Serialize(LogEntry entry)
        {
            Func<object, IEnumerable<Type>> getTypes =
                o =>
                {
                    var logEntry = o as LogEntry;

                    if (logEntry == null) return null;
                    if (logEntry.ExtendedProperties == null) return null;

                    return
                        logEntry.ExtendedProperties.Values
                            .Select(v => v != null ? v.GetType() : null)
                            .Where(t => t != null)
                            .Distinct();
                };

            return DoSerialize(entry, getTypes);
        }

        /// <summary>
        /// Deserializes an array of bytes into a log entry.
        /// </summary>
        /// <param name="logEntryBytes">The array of bytes.</param>
        /// <returns>The <see cref="LogEntry"/> represented by <paramref name="logEntryBytes"/>.</returns>
        public LogEntry Deserialize(byte[] logEntryBytes)
        {
            return (LogEntry)DoDeserialize(logEntryBytes);
        }

        private static readonly Encoding encoding = new UTF8Encoding(false);
        private static readonly char[] separator = new[] { '|' };

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "This stream can be disposed several times. The alternative complicates the code to avoid CA2000")]
        private static byte[] DoSerialize(object value, Func<object, IEnumerable<Type>> getTypes)
        {
            if (value == null)
            {
                return null;
            }

            using (var inMemoryData = new MemoryStream())
            using (var writer = new StreamWriter(inMemoryData, encoding))
            {
                var typesBuilder = new StringBuilder();

                var knownTypes =
                    (getTypes(value) ?? new Type[0])
                        .Where(t => t != null && !t.IsPrimitive && t.Assembly != typeof(object).Assembly).ToArray();

                typesBuilder.Append(value.GetType().AssemblyQualifiedName);
                foreach (var type in knownTypes)
                {
                    typesBuilder.Append(separator);
                    typesBuilder.Append(type.AssemblyQualifiedName);
                }

                writer.WriteLine(typesBuilder.ToString());
                writer.Flush();

                var serializer = new DataContractJsonSerializer(value.GetType(), knownTypes);
                serializer.WriteObject(inMemoryData, value);

                return inMemoryData.ToArray();
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times", Justification = "This stream can be disposed several times. The alternative complicates the code to avoid CA2000")]
        private static LogEntry DoDeserialize(byte[] serializedBytes)
        {
            if (serializedBytes == null)
            {
                return null;
            }

            Type valueType;
            IEnumerable<Type> knownTypes;
            int position;

            using (var inMemoryData = new MemoryStream(serializedBytes))
            using (var reader = new StreamReader(inMemoryData, encoding))
            {
                var knownTypeNames = reader.ReadLine();
                var splitTypeNames = knownTypeNames.Split(separator);

                valueType = Type.GetType(splitTypeNames[0]);
                knownTypes = splitTypeNames.Skip(1).Select(tn => Type.GetType(tn));

                position = encoding.GetByteCount(knownTypeNames + Environment.NewLine);
            }

            // creating a new offset stream as DataContractJsonSerializer.ReadObject(Stream) always sets stream.Position = 0L
            using (var inMemoryData = new MemoryStream(serializedBytes, position, serializedBytes.Length - position))
            {
                var serializer = new DataContractJsonSerializer(valueType, knownTypes);
                return (LogEntry)serializer.ReadObject(inMemoryData);
            }
        }
    }
}

//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Newtonsoft.Json;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Formatters
{
    /// <summary>
    /// Represents a log formatter that will format a <see cref="LogEntry"/> in JSON-compliant format.
    /// </summary>
    [ConfigurationElementType(typeof(JsonLogFormatterData))]
    public class JsonLogFormatter : LogFormatter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLogFormatter" /> class.
        /// </summary>
        public JsonLogFormatter()
            : this(JsonFormatting.None)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JsonLogFormatter" /> class with the specified formatting options.
        /// </summary>
        /// <param name="formatting">The formatting options.</param>
        public JsonLogFormatter(JsonFormatting formatting)
        {
            this.Formatting = formatting;
        }

        /// <summary>
        /// Gets or sets the formatting of the written event.
        /// </summary>
        /// <value>
        /// The <see cref="JsonFormatting"/> value.
        /// </value>
        public JsonFormatting Formatting { get; set; }

        /// <summary>
        /// Formats a log entry and returns a string to be output.
        /// </summary>
        /// <param name="log">The log entry to format.</param>
        /// <returns>
        /// A string that represents the log entry.
        /// </returns>
        public override string Format(LogEntry log)
        {
            return JsonConvert.SerializeObject(log, (Newtonsoft.Json.Formatting)Formatting);
        }

        /// <summary>
        /// Deserializes the string representation of a <see cref="LogEntry"/> into a <see cref="LogEntry"/> instance.
        /// </summary>
        /// <param name="serializedLogEntry">The serialized <see cref="LogEntry"/> representation.</param>
        /// <returns>The <see cref="LogEntry"/>.</returns>
        public static T Deserialize<T>(string serializedLogEntry) where T : LogEntry
        {
            return JsonConvert.DeserializeObject<T>(serializedLogEntry);
        }
    }
}

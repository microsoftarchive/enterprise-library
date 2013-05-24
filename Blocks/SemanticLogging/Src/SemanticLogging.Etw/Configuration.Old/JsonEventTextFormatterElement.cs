#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Formatters;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Configuration
{
    /// <summary>
    /// Represents a JSON event text formatter configuration element.
    /// </summary>
    [XmlRoot("jsonEventTextFormatter", Namespace = Constants.Namespace)]
    public class JsonEventTextFormatterElement : EventTextFormatterElement
    {
        /// <summary>
        /// Gets or sets the formatting.
        /// </summary>
        /// <value>The formatting.</value>
        [XmlAttribute("formatting")]
        public EventTextFormatting Formatting { get; set; }

        /// <summary>
        /// Gets or sets the date time format.
        /// </summary>
        /// <value>The date time format.</value>
        [XmlAttribute("dateTimeFormat")]
        public string DateTimeFormat { get; set; }

        /// <summary>
        /// Creates the <see cref="IEventTextFormatter" /> instance.
        /// </summary>
        /// <returns>
        /// The event text formatter instance.
        /// </returns>
        public override IEventTextFormatter CreateEventTextFormatter()
        {
            var formatter = new JsonEventTextFormatter(this.Formatting)
            {
                DateTimeFormat = this.DateTimeFormat,
            };
            return formatter;
        }
    }
}

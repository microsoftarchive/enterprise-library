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
    /// Represents a configuration element for an event text formatter.
    /// </summary>
    public abstract class EventTextFormatterElement
    {
        /// <summary>
        /// Gets or sets the event text formatter name.
        /// </summary>
        /// <value>The event text formatter name.</value>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Creates the <see cref="IEventTextFormatter" /> instance.
        /// </summary>
        /// <returns>The event text formatter instance.</returns>
        public abstract IEventTextFormatter CreateEventTextFormatter();
    }
}

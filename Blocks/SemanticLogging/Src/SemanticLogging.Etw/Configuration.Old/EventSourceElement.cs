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

using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw.Configuration
{
    /// <summary>
    /// Represents an event source configuration element.
    /// </summary>
    [XmlRoot("eventSource", Namespace = Constants.Namespace)]
    public class EventSourceElement
    {
        /// <summary>
        /// Gets or sets the event source name.
        /// </summary>
        /// <value>The event source name.</value>
        [XmlAttribute("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the event source id.
        /// </summary>
        /// <value>The event source id.</value>
        [XmlAttribute("id")]
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets a collection of <see cref="EventSourceEventListenerElement"/>.
        /// </summary>
        /// <value>A collection of <see cref="EventSourceEventListenerElement"/>.</value>
        [XmlArray("eventListeners")]
        [XmlArrayItem("eventListener")]
        public Collection<EventSourceEventListenerElement> EventListeners { get; set; }
    }
}

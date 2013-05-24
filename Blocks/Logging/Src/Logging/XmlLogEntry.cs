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

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Extends the <see cref="LogEntry"/> to add XML support.
    /// </summary>
    [Serializable]
    public class XmlLogEntry : LogEntry
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="XmlLogEntry"/> class.
        /// </summary>
        public XmlLogEntry() : base() { }

        /// <summary>
        /// Initialize a new instance of the <see cref="XmlLogEntry" /> class with the specified options.
        /// </summary>
        /// <param name="message">Message body to log.  Value from ToString() method from message object.</param>
        /// <param name="category">Collection of category names used to route the log entry to a one or more sinks.</param>
        /// <param name="priority">Only messages must be above the minimum priority are processed.</param>
        /// <param name="eventId">Event number or identifier.</param>
        /// <param name="severity">Log entry severity as a <see cref="TraceEventType"/> enumeration.</param>
        /// <param name="title">Additional description of the log entry message.</param>
        /// <param name="properties">Dictionary of key/value pairs to record.</param>
        public XmlLogEntry(object message, ICollection<string> category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties)
            : base(message, category, priority, eventId, severity, title, properties) { }

        /// <summary>
        /// Field to be able to serialize the XPathNavigator. This a tradeoff.
        /// </summary>
        private string xmlString = null;

        [NonSerialized]
        private XPathNavigator xml;
        /// <summary>
        /// Gets or sets the XML to log.
        /// </summary>
        public XPathNavigator Xml
        {
            get
            {
                if (xml == null && !string.IsNullOrEmpty(xmlString))
                {
                    using (var reader = XmlReader.Create(new StringReader(xmlString), new XmlReaderSettings { CloseInput = true }))
                    {
                        xml = new XPathDocument(reader).CreateNavigator();
                    }
                }
                return xml;
            }
            set
            {
                if (xmlString == null && value != null)
                {
                    xmlString = value.InnerXml;
                }
                else
                {
                    xmlString = null;
                }
                xml = value;
            }
        }
    }
}

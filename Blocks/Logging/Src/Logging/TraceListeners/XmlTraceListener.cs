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

using System.Diagnostics;
using System.IO;
using System.Xml;
using System.Xml.XPath;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Represents a trace listener that writes entries as XML-encoded data to a file.
    /// </summary>
    [ConfigurationElementType(typeof(XmlTraceListenerData))]
    public class XmlTraceListener : XmlWriterTraceListener
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XmlTraceListener"/> class.
        /// </summary>
        public XmlTraceListener(string filename)
            : base(filename)
        { }

        /// <summary>
        /// Delivers the trace data as an XML message.
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The ID of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            object actualData = data;

            if (data is XmlLogEntry)
            {
                XmlLogEntry logEntryXml = data as XmlLogEntry;
                if (logEntryXml.Xml != null)
                {
                    actualData = logEntryXml.Xml;
                }
                else
                {
                    actualData = GetXml(logEntryXml);
                }
            }
            else if (data is LogEntry)
            {
                actualData = GetXml(data as LogEntry);
            }

            base.TraceData(eventCache, source, eventType, id, actualData);
        }

        internal virtual XPathNavigator GetXml(LogEntry logEntry)
        {
            using (var reader = XmlReader.Create(new StringReader(new XmlLogFormatter().Format(logEntry)), new XmlReaderSettings { CloseInput = true }))
            {
                return new XPathDocument(reader).CreateNavigator();
            }
        }
    }
}

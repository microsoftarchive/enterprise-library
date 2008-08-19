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
using System.Text;
using System.Diagnostics;
using System.Xml.XPath;
using System.Xml.Serialization;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// A <see cref="TraceListener"/> that writes an XML.
	/// </summary>
	public class XmlTraceListener : XmlWriterTraceListener
	{
		/// <summary>
		/// Initializes a new instance of <see cref="XmlTraceListener"/>.
		/// </summary>
		public XmlTraceListener(string filename) : base(filename) { }

		/// <summary>
		/// Delivers the trace data as an XML message.
		/// </summary>
		/// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
		/// <param name="source">The name of the trace source that delivered the trace data.</param>
		/// <param name="eventType">The type of event.</param>
		/// <param name="id">The id of the event.</param>
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
			return new XPathDocument(new StringReader(new XmlLogFormatter().Format(logEntry))).CreateNavigator();
		}
	}
}

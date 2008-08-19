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
using System.Xml.XPath;
using System.Diagnostics;
using System.IO;
using System.Management.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// Entension of <see cref="LogEntry"/> for adding XMl Support.
	/// </summary>
	[Serializable]
	public class XmlLogEntry : LogEntry
	{
		/// <summary>
		/// Initialize a new instance of a <see cref="XmlLogEntry"/> class.
		/// </summary>
		public XmlLogEntry() : base() {}

		/// <summary>
		/// Initialize a new instance of a <see cref="XmlLogEntry"/> class.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="category"></param>
		/// <param name="priority"></param>
		/// <param name="eventId"></param>
		/// <param name="severity"></param>
		/// <param name="title"></param>
		/// <param name="properties"></param>
		public XmlLogEntry(object message, ICollection<string> category, int priority, int eventId, TraceEventType severity, string title, IDictionary<string, object> properties) : base(message, category, priority, eventId, severity, title, properties) { }

		/// <summary>
		/// Field to be able to serialize the XPathNavigator. This a tradeoff.
		/// </summary>
		private string xmlString = null;
		
		[NonSerialized]
		private XPathNavigator xml;
		/// <summary>
		/// XML to Log.
		/// </summary>
		[IgnoreMember]
		public XPathNavigator Xml
		{
			get 
			{
				if (xml == null && !string.IsNullOrEmpty(xmlString))
				{
					TextReader reader = new StringReader(xmlString);
					xml = new XPathDocument(reader).CreateNavigator();
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

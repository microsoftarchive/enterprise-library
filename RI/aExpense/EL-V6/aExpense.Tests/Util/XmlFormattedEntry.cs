#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// aExpense Reference Implementation
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace AExpense.FunctionalTests.Util
{
    internal class XmlFormattedEntry
    {
        private const string EventNS = "{http://schemas.microsoft.com/win/2004/08/events/event}";

        internal static XElement Provider { get; set; }
        internal static XElement EventId { get; set; }
        internal static XElement Version { get; set; }
        internal static XElement Level { get; set; }
        internal static XElement Task { get; set; }
        internal static XElement Opcode { get; set; }
        internal static XElement Keywords { get; set; }
        internal static XElement TimeCreated { get; set; }
        internal static XElement Payload { get; set; }
        internal static XElement Message { get; set; }

        internal static void Fill(XElement entry)
        {
            Provider = entry.Descendants(EventNS + "Provider").Single();
            EventId = entry.Descendants(EventNS + "EventID").Single();
            Version = entry.Descendants(EventNS + "Version").Single();
            Level = entry.Descendants(EventNS + "Level").Single();
            Task = entry.Descendants(EventNS + "Task").Single();
            Opcode = entry.Descendants(EventNS + "Opcode").Single();
            Keywords = entry.Descendants(EventNS + "Keywords").Single();
            TimeCreated = entry.Descendants(EventNS + "TimeCreated").Single();
            Payload = entry.Descendants(EventNS + "EventData").Single();
            Message = entry.Descendants(EventNS + "RenderingInfo").Single();
        }
    }
}

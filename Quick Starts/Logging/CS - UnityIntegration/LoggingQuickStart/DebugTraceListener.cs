//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace LoggingQuickStart
{
	/// <summary>
	/// Trace listener that writes formatted messages to the Visual Studio debugger output.
	/// </summary>
	[ConfigurationElementType(typeof(CustomTraceListenerData))]
	public class DebugTraceListener : CustomTraceListener
    {
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (data is LogEntry && this.Formatter != null) 
            {
                this.WriteLine(this.Formatter.Format(data as LogEntry));
            }
            else
            {
                this.WriteLine(data.ToString());
            }
        }
               

        /// <summary>
        /// Writes a message to the debug window 
        /// </summary>
        /// <param name="message">The string to write to the debug window</param>
        public override void Write(string message)
        {
            Debug.Write(message);
        }

        /// <summary>
        /// Writes a message to the debug window 
        /// </summary>
        /// <param name="message">The string to write to the debug window</param>
        public override void WriteLine(string message)
        {
            Debug.WriteLine(message);
        }
 
	}
}

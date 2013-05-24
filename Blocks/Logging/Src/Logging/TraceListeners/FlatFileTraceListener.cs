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
using System.Diagnostics;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// A <see cref="TraceListener"/> that writes to a flat file, formatting the output with an <see cref="ILogFormatter"/>.
    /// </summary>
    [ConfigurationElementType(typeof(FlatFileTraceListenerData))]
    public class FlatFileTraceListener : FormattedTextWriterTraceListener
    {
        private string header = string.Empty;
        private string footer = string.Empty;

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name, a header, a footer and 
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="fileName">The file stream.</param>
        /// <param name="header">The header.</param>
        /// <param name="footer">The footer.</param>
        /// <param name="formatter">The formatter.</param>
        public FlatFileTraceListener(string fileName, string header = null, string footer = null, ILogFormatter formatter = null)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), formatter)
        {
            this.header = header ?? string.Empty;
            this.footer = footer ?? string.Empty;
        }

        /// <summary>
        /// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/> and 
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="stream">The file stream.</param>
        /// <param name="name">The name.</param>
        /// <param name="formatter">The formatter.</param>
        public FlatFileTraceListener(FileStream stream, string name = null, ILogFormatter formatter = null)
            : base(stream, name, formatter)
        { }

        /// <summary>
        /// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/> and 
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="writer">The stream writer.</param>
        /// <param name="name">The name.</param>
        /// <param name="formatter">The formatter.</param>
        public FlatFileTraceListener(StreamWriter writer, string name = null, ILogFormatter formatter = null)
            : base(writer, name, formatter)
        { }

        /// <summary>
        /// Delivers the trace data to the underlying file.
        /// </summary>
        /// <param name="eventCache">The context information provided by <see cref="System.Diagnostics"/>.</param>
        /// <param name="source">The name of the trace source that delivered the trace data.</param>
        /// <param name="eventType">The type of event.</param>
        /// <param name="id">The id of the event.</param>
        /// <param name="data">The data to trace.</param>
        public override void TraceData(TraceEventCache eventCache, string source, TraceEventType eventType, int id, object data)
        {
            if (this.Filter == null || this.Filter.ShouldTrace(eventCache, source, eventType, id, null, null, data, null))
            {
                if (header.Length > 0)
                    WriteLine(header);

                if (data is LogEntry)
                {
                    if (this.Formatter != null)
                    {
                        base.WriteLine(this.Formatter.Format(data as LogEntry));
                    }
                    else
                    {
                        base.TraceData(eventCache, source, eventType, id, data);
                    }
                }
                else
                {
                    base.TraceData(eventCache, source, eventType, id, data);
                }

                if (footer.Length > 0)
                    WriteLine(footer);
            }
        }

        /// <summary>
        /// Declare the supported attributes for <see cref="FlatFileTraceListener"/>
        /// </summary>
        protected override string[] GetSupportedAttributes()
        {
            return new string[4] { "formatter", "FileName", "header", "footer" };
        }
    }
}

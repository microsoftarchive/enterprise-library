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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
	/// <summary>
	/// A <see cref="TraceListener"/> that writes to a flat file, formatting the output with an <see cref="ILogFormatter"/>.
	/// </summary>
	[ConfigurationElementType(typeof(FlatFileTraceListenerData))]
	public class FlatFileTraceListener : FormattedTextWriterTraceListener
	{
		string header = String.Empty;
		string footer = String.Empty;

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/>.
		/// </summary>
		public FlatFileTraceListener()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(ILogFormatter formatter)
			: base(formatter)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/> and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="stream">The file stream writen to.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(FileStream stream, ILogFormatter formatter)
			: base(stream, formatter)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/>.
		/// </summary>
		/// <param name="stream">The file stream.</param>
		public FlatFileTraceListener(FileStream stream)
			: base(stream)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/> and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="writer">The stream writer.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(StreamWriter writer, ILogFormatter formatter)
			: base(writer, formatter)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/>.
		/// </summary>
		/// <param name="writer">The stream writer.</param>
		public FlatFileTraceListener(StreamWriter writer)
			: base(writer)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="fileName">The file name.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(string fileName, ILogFormatter formatter)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), formatter)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name.
		/// </summary>
		/// <param name="fileName">The file name.</param>
        public FlatFileTraceListener(string fileName)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName))
        {
        }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name, a header, a footer and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="fileName">The file stream.</param>
		/// <param name="header">The header.</param>
		/// <param name="footer">The footer.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(string fileName, string header, string footer, ILogFormatter formatter)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), formatter)
		{
			this.header = header;
			this.footer = footer;
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name, a header, and a footer.
		/// </summary>
		/// <param name="fileName">The file stream.</param>
		/// <param name="header">The header.</param>
		/// <param name="footer">The footer.</param>
		public FlatFileTraceListener(string fileName, string header, string footer)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName))
		{
			this.header = header;
			this.footer = footer;
		}

		/// <summary>
		/// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/> and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="stream">The file stream.</param>
		/// <param name="name">The name.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(FileStream stream, string name, ILogFormatter formatter)
			: base(stream, name, formatter)
		{
		}

		/// <summary>
		/// Initializes a new name instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/>.
		/// </summary>
		/// <param name="stream">The file stream.</param>
		/// <param name="name">The name.</param>
		public FlatFileTraceListener(FileStream stream, string name)
			: base(stream, name)
		{
		}

		/// <summary>
		/// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/> and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="writer">The stream writer.</param>
		/// <param name="name">The name.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(StreamWriter writer, string name, ILogFormatter formatter)
			: base(writer, name, formatter)
		{
		}

		/// <summary>
		/// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/>.
		/// </summary>
		/// <param name="writer">The stream writer.</param>
		/// <param name="name">The name.</param>
		public FlatFileTraceListener(StreamWriter writer, string name)
			: base(writer, name)
		{
		}

		/// <summary>
		/// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a file name and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="fileName">The file name.</param>
		/// <param name="name">The name.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(string fileName, string name, ILogFormatter formatter)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), name, formatter)
		{
		}

		/// <summary>
		/// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a file name.
		/// </summary>
		/// <param name="fileName">The file name.</param>
		/// <param name="name">The name.</param>
		public FlatFileTraceListener(string fileName, string name)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), name)
		{
		}

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
                    InstrumentationProvider.FireTraceListenerEntryWrittenEvent();
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

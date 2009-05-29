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
using Microsoft.Practices.EnterpriseLibrary.Logging.Instrumentation;

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
			: this(new NullLoggingInstrumentationProvider())
		{
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/>.
        /// </summary>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(ILoggingInstrumentationProvider instrumentationProvider)
            : base(instrumentationProvider)
        {
        }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(ILogFormatter formatter)
			: this(formatter, new NullLoggingInstrumentationProvider())
		{
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="formatter">The formatter.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(ILogFormatter formatter, ILoggingInstrumentationProvider instrumentationProvider)
            : base(formatter, instrumentationProvider)
        {
        }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/> and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="stream">The file stream writen to.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(FileStream stream, ILogFormatter formatter)
			: this(stream, formatter, new NullLoggingInstrumentationProvider())
		{
		}

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/> and 
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="stream">The file stream writen to.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(FileStream stream, ILogFormatter formatter, ILoggingInstrumentationProvider instrumentationProvider)
            : base(stream, formatter, instrumentationProvider)
        {
        }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/>.
		/// </summary>
		/// <param name="stream">The file stream.</param>
		public FlatFileTraceListener(FileStream stream)
			: this(stream, new NullLoggingInstrumentationProvider())
		{
		}

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/>.
        /// </summary>
        /// <param name="stream">The file stream.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(FileStream stream, ILoggingInstrumentationProvider instrumentationProvider)
            : base(stream, instrumentationProvider)
        {
        }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/> and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="writer">The stream writer.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(StreamWriter writer, ILogFormatter formatter)
			: this(writer, formatter, new NullLoggingInstrumentationProvider())
		{
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/> and 
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="writer">The stream writer.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(StreamWriter writer, ILogFormatter formatter, ILoggingInstrumentationProvider instrumentationProvider)
            : base(writer, formatter, instrumentationProvider)
        {
        }


		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/>.
		/// </summary>
		/// <param name="writer">The stream writer.</param>
		public FlatFileTraceListener(StreamWriter writer)
			: this(writer, new NullLoggingInstrumentationProvider())
		{
		}

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="writer">The stream writer.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(StreamWriter writer, ILoggingInstrumentationProvider instrumentationProvider)
            : base(writer, instrumentationProvider)
        {
        }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name and 
		/// a <see cref="ILogFormatter"/>.
		/// </summary>
		/// <param name="fileName">The file name.</param>
		/// <param name="formatter">The formatter.</param>
		public FlatFileTraceListener(string fileName, ILogFormatter formatter)
            :this(fileName, formatter, new NullLoggingInstrumentationProvider())
		{
		}

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name and 
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(string fileName, ILogFormatter formatter, ILoggingInstrumentationProvider instrumentationProvider)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), formatter, instrumentationProvider)
        {
        }

		/// <summary>
		/// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name.
		/// </summary>
		/// <param name="fileName">The file name.</param>
        public FlatFileTraceListener(string fileName)
            :this(fileName, new NullLoggingInstrumentationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(string fileName, ILoggingInstrumentationProvider instrumentationProvider)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), instrumentationProvider)
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
            : this(fileName, header, footer, formatter, new NullLoggingInstrumentationProvider())
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
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(string fileName, string header, string footer, ILogFormatter formatter, ILoggingInstrumentationProvider instrumentationProvider)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), formatter, instrumentationProvider)
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
            : this(fileName, header, footer, new NullLoggingInstrumentationProvider())
		{
		}

        /// <summary>
        /// Initializes a new instance of <see cref="FlatFileTraceListener"/> with a file name, a header, and a footer.
        /// </summary>
        /// <param name="fileName">The file stream.</param>
        /// <param name="header">The header.</param>
        /// <param name="footer">The footer.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(string fileName, string header, string footer, ILoggingInstrumentationProvider instrumentationProvider)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), instrumentationProvider)
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
			: this(stream, name, formatter, new NullLoggingInstrumentationProvider())
		{
		}

        /// <summary>
        /// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/> and 
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="stream">The file stream.</param>
        /// <param name="name">The name.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(FileStream stream, string name, ILogFormatter formatter, ILoggingInstrumentationProvider instrumentationProvider)
            : base(stream, name, formatter, instrumentationProvider)
        {
        }

		/// <summary>
		/// Initializes a new name instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/>.
		/// </summary>
		/// <param name="stream">The file stream.</param>
		/// <param name="name">The name.</param>
		public FlatFileTraceListener(FileStream stream, string name)
			: this(stream, name, new NullLoggingInstrumentationProvider())
		{
        }

        /// <summary>
        /// Initializes a new name instance of <see cref="FlatFileTraceListener"/> with a <see cref="FileStream"/>.
        /// </summary>
        /// <param name="stream">The file stream.</param>
        /// <param name="name">The name.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(FileStream stream, string name, ILoggingInstrumentationProvider instrumentationProvider)
            : base(stream, name, instrumentationProvider)
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
			: this(writer, name, formatter, new NullLoggingInstrumentationProvider())
		{
        }

        /// <summary>
        /// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/> and 
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="writer">The stream writer.</param>
        /// <param name="name">The name.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(StreamWriter writer, string name, ILogFormatter formatter, ILoggingInstrumentationProvider instrumentationProvider)
            : base(writer, name, formatter, instrumentationProvider)
        {
        }

		/// <summary>
		/// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/>.
		/// </summary>
		/// <param name="writer">The stream writer.</param>
		/// <param name="name">The name.</param>
		public FlatFileTraceListener(StreamWriter writer, string name)
			: this(writer, name, new NullLoggingInstrumentationProvider())
		{
		}

        /// <summary>
        /// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a <see cref="StreamWriter"/>.
        /// </summary>
        /// <param name="writer">The stream writer.</param>
        /// <param name="name">The name.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(StreamWriter writer, string name, ILoggingInstrumentationProvider instrumentationProvider)
            : base(writer, name, instrumentationProvider)
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
            : this(fileName, name, formatter, new NullLoggingInstrumentationProvider())
		{
		}

        /// <summary>
        /// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a file name and 
        /// a <see cref="ILogFormatter"/>.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="name">The name.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(string fileName, string name, ILogFormatter formatter, ILoggingInstrumentationProvider instrumentationProvider)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), name, formatter, instrumentationProvider)
        {
        }

		/// <summary>
		/// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a file name.
		/// </summary>
		/// <param name="fileName">The file name.</param>
		/// <param name="name">The name.</param>
		public FlatFileTraceListener(string fileName, string name)
            : this(fileName, name, new NullLoggingInstrumentationProvider())
		{
        }

        /// <summary>
        /// Initializes a new named instance of <see cref="FlatFileTraceListener"/> with a file name.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="name">The name.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public FlatFileTraceListener(string fileName, string name, ILoggingInstrumentationProvider instrumentationProvider)
            : base(EnvironmentHelper.ReplaceEnvironmentVariables(fileName), name, instrumentationProvider)
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

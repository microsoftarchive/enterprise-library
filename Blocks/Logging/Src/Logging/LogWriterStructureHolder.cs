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
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Holds the collaborators of a <see cref="LogWriter"/> to allow for an easy replacement should configuration change.
	/// </summary>
	[CustomFactory(typeof(LogWriterStructureHolderCustomFactory))]
	public class LogWriterStructureHolder : IDisposable
	{
		private ICollection<ILogFilter> filters;
		private IDictionary<string, LogSource> traceSources;
		private LogSource allEventsTraceSource;
		private LogSource notProcessedTraceSource;
		private LogSource errorsTraceSource;
		private string defaultCategory;
		private bool tracingEnabled;
		private bool logWarningsWhenNoCategoriesMatch;

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Initializes a new instance of the <see cref="LogWriterStructureHolder"/> class.
		/// </summary>
		/// <param name="filters">The collection of filters to use when processing an entry.</param>
		/// <param name="traceSources">The trace sources to dispatch entries to.</param>
		/// <param name="allEventsTraceSource">The special <see cref="LogSource"/> to which all log entries should be logged.</param>
		/// <param name="notProcessedTraceSource">The special <see cref="LogSource"/> to which log entries with at least one non-matching category should be logged.</param>
		/// <param name="errorsTraceSource">The special <see cref="LogSource"/> to which internal errors must be logged.</param>
		/// <param name="defaultCategory">The default category to set when entry categories list of a log entry is empty.</param>
		/// <param name="tracingEnabled">The tracing status.</param>
		/// <param name="logWarningsWhenNoCategoriesMatch">true if warnings should be logged when a non-matching category is found.</param>
		public LogWriterStructureHolder(
			ICollection<ILogFilter> filters,
			IDictionary<string, LogSource> traceSources,
			LogSource allEventsTraceSource,
			LogSource notProcessedTraceSource,
			LogSource errorsTraceSource,
			string defaultCategory,
			bool tracingEnabled,
			bool logWarningsWhenNoCategoriesMatch)
		{
			if (filters == null)
				throw new ArgumentNullException("filters");
			if (traceSources == null)
				throw new ArgumentNullException("traceSources");
			if (errorsTraceSource == null)
				throw new ArgumentNullException("errorsTraceSource");

			this.filters = filters;
			this.traceSources = traceSources;
			this.allEventsTraceSource = allEventsTraceSource;
			this.notProcessedTraceSource = notProcessedTraceSource;
			this.errorsTraceSource = errorsTraceSource;
			this.defaultCategory = defaultCategory;
			this.tracingEnabled = tracingEnabled;
			this.logWarningsWhenNoCategoriesMatch = logWarningsWhenNoCategoriesMatch;
		}

		/// <summary>
		/// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public ICollection<ILogFilter> Filters
		{
			get { return filters; }
		}

		/// <summary>
		/// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public IDictionary<string, LogSource> TraceSources
		{
			get { return traceSources; }
		}

		/// <summary>
		/// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public LogSource AllEventsTraceSource
		{
			get { return allEventsTraceSource; }
		}

		/// <summary>
		/// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public LogSource NotProcessedTraceSource
		{
			get { return notProcessedTraceSource; }
		}

		/// <summary>
		/// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public LogSource ErrorsTraceSource
		{
			get { return errorsTraceSource; }
		}

		/// <summary>
		/// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public string DefaultCategory
		{
			get { return defaultCategory; }
		}

		/// <summary>
		/// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public bool TracingEnabled
		{
			get { return tracingEnabled; }
		}

		/// <summary>
		/// This property supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// </summary>
		public bool LogWarningsWhenNoCategoriesMatch
		{
			get { return logWarningsWhenNoCategoriesMatch; }
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Releases the resources used by the <see cref="LogWriterStructureHolder"/>.
		/// </summary>
		public void Dispose()
		{
			foreach (LogSource source in traceSources.Values)
			{
				source.Dispose();
			}
			DisposeSpecialLogSource(errorsTraceSource);
			DisposeSpecialLogSource(notProcessedTraceSource);
			DisposeSpecialLogSource(allEventsTraceSource);
		}

		private void DisposeSpecialLogSource(LogSource specialLogSource)
		{
			if (specialLogSource != null)
			{
				specialLogSource.Dispose();
			}
		}
	}
}

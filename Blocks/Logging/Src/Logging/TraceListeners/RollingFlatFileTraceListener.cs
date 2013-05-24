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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Performs logging to a file and rolls the output file when either time or size thresholds are 
    /// exceeded.
    /// </summary>
    /// <remarks>
    /// Logging always occurs to the configured file name, and when roll occurs a new rolled file name is calculated
    /// by adding the timestamp pattern to the configured file name.
    /// <para/>
    /// The need of rolling is calculated before performing a logging operation, so even if the thresholds are exceeded
    /// roll will not occur until a new entry is logged.
    /// <para/>
    /// Both time and size thresholds can be configured, and when the first threshold is reached, both will be reset.
    /// <para/>
    /// The elapsed time is calculated from the creation date of the logging file.
    /// </remarks>
    [ConfigurationElementType(typeof(RollingFlatFileTraceListenerData))]
    public partial class RollingFlatFileTraceListener : FlatFileTraceListener
    {
        private readonly StreamWriterRollingHelper rollingHelper;

        private readonly RollFileExistsBehavior rollFileExistsBehavior;
        private readonly RollInterval rollInterval;
        private readonly int rollSizeInBytes;
        private readonly string timeStampPattern;
        private readonly int maxArchivedFiles;

        private readonly Timer timer;
        private bool disposed;

        /// <summary>
        /// Represents the default separator used for headers and footers.
        /// </summary>
        public const string DefaultSeparator = "----------------------------------------";

        /// <summary>
        /// Initializes a new instance of the <see cref="RollingFlatFileTraceListener"/> class.
        /// </summary>
        /// <param name="fileName">The filename where the entries will be logged.</param>
        /// <param name="header">The header to add before logging an entry.</param>
        /// <param name="footer">The footer to add after logging an entry.</param>
        /// <param name="formatter">The formatter.</param>
        /// <param name="rollSizeKB">The maxium file size (KB) before rolling.</param>
        /// <param name="timeStampPattern">The date format that will be appended to the new roll file.</param>
        /// <param name="rollFileExistsBehavior">Expected behavior that will be used when the roll file has to be created.</param>
        /// <param name="rollInterval">The time interval that makes the file rolles.</param>
        /// <param name="maxArchivedFiles">The maximum number of archived files to keep.</param>
        public RollingFlatFileTraceListener(string fileName,
                                            string header = DefaultSeparator,
                                            string footer = DefaultSeparator,
                                            ILogFormatter formatter = null,
                                            int rollSizeKB = 0,
                                            string timeStampPattern = "yyyy-MM-dd",
                                            RollFileExistsBehavior rollFileExistsBehavior = RollFileExistsBehavior.Overwrite,
                                            RollInterval rollInterval = RollInterval.None,
                                            int maxArchivedFiles = 0)
            : base(fileName, header, footer, formatter)
        {
            Guard.ArgumentNotNullOrEmpty(fileName, "fileName");

            this.rollSizeInBytes = rollSizeKB * 1024;
            this.timeStampPattern = timeStampPattern;
            this.rollFileExistsBehavior = rollFileExistsBehavior;
            this.rollInterval = rollInterval;
            this.maxArchivedFiles = maxArchivedFiles;

            this.rollingHelper = new StreamWriterRollingHelper(this);

            if (rollInterval == RollInterval.Midnight)
            {
                var now = this.rollingHelper.DateTimeProvider.CurrentDateTime;
                var midnight = now.AddDays(1).Date;

                this.timer = new Timer((o) => this.rollingHelper.RollIfNecessary(), null, midnight.Subtract(now), TimeSpan.FromDays(1));
            }
        }

        /// <summary>
        /// Gets the <see cref="StreamWriterRollingHelper"/> for the flat file.
        /// </summary>
        /// <value>
        /// The <see cref="StreamWriterRollingHelper"/> for the flat file.
        /// </value>
        public StreamWriterRollingHelper RollingHelper
        {
            get { return rollingHelper; }
        }

        /// <summary>
        /// Writes trace information, a data object and event information to the file, performing a roll if necessary.
        /// </summary>
        /// <param name="eventCache">A <see cref="TraceEventCache"/> object that contains the current process ID, thread ID, and stack trace information.</param>
        /// <param name="source">A name used to identify the output, typically the name of the application that generated the trace event.</param>
        /// <param name="eventType">One of the <see cref="TraceEventType"/> values specifying the type of event that has caused the trace.</param>
        /// <param name="id">A numeric identifier for the event.</param>
        /// <param name="data">The trace data to emit.</param>
        public override void TraceData(TraceEventCache eventCache,
                                       string source,
                                       TraceEventType eventType,
                                       int id,
                                       object data)
        {
            rollingHelper.RollIfNecessary();

            base.TraceData(eventCache, source, eventType, id, data);
        }

        /// <summary>
        /// Releases managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; 
        /// <see langword="false"/> to release only unmanaged resources. </param>
        protected override void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    base.Dispose(disposing);

                    if (this.timer != null)
                        this.timer.Dispose();
                }

                this.disposed = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="RollingFlatFileTraceListener"/> class.
        /// </summary>
        ~RollingFlatFileTraceListener()
        {
            this.Dispose(false);
        }
    }
}

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

using System.Linq;
using System.ServiceModel.Activation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Allows clients to submit log entries into the server log.
    /// </summary>
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LoggingService : ILoggingService
    {
        private readonly LogWriter logWriter;

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        public LoggingService()
            : this(Logger.Writer)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingService"/> class.
        /// </summary>
        /// <param name="logWriter">The log sink where to store incoming entries.</param>
        public LoggingService(LogWriter logWriter)
        {
            this.logWriter = logWriter;
        }

        /// <summary>
        /// Adds log entries into to the server log.
        /// </summary>
        /// <param name="entries">The client log entries to log in the server.</param>
        public void Add(LogEntryMessage[] entries)
        {
            if (entries != null)
            {
                foreach (var message in entries)
                {
                    if (message != null)
                    {
                        var entry = Translate(message);
                        if (entry != null)
                        {
                            this.CollectInformation(entry);
                            this.logWriter.Write(entry);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Used to collect more information or customize the incoming log entry before logging it.
        /// </summary>
        /// <param name="entry">The log entry coming from the client.</param>
        protected virtual void CollectInformation(LogEntry entry)
        {
        }

        /// <summary>
        /// Translates the incoming <see cref="LogEntryMessage"/> into a <see cref="LogEntry"/>.
        /// </summary>
        /// <param name="entry">The log entry coming from the client.</param>
        /// <returns>A <see cref="LogEntry"/> instance that can be stored in the log.</returns>
        protected virtual LogEntry Translate(LogEntryMessage entry)
        {
            return entry.ToLogEntry();
        }
    }
}

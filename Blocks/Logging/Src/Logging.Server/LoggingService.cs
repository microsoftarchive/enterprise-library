using System.Linq;
using System.ServiceModel.Activation;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class LoggingService : ILoggingService
    {
        private readonly LogWriter logWriter;

        public LoggingService()
            : this(Logger.Writer)
        {
        }

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
                    var entry = Translate(message);
                    this.CollectInformation(entry);
                    this.logWriter.Write(entry);
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

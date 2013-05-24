using System;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents the configuration for special log sources
    /// </summary>
    public class SpecialSourcesConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialSourcesConfiguration"/> class.
        /// </summary>
        public SpecialSourcesConfiguration()
        {
            this.LoggingErrorsAndWarnings = new SpecialLogSourceData("Logging Errors & Warnings");
            this.Unprocessed = new SpecialLogSourceData("Unprocessed Category");
            this.AllEvents = new SpecialLogSourceData("All Events");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialSourcesConfiguration"/> class.
        /// </summary>
        /// <param name="loggingErrorsAndWarnings">The special log source to which internal errors must be logged.</param>
        /// <param name="unprocessed">The special log source to which log entries with at least one non-matching category should be logged.</param>
        /// <param name="allEvents">The special log source to which all log entries should be logged.</param>
        public SpecialSourcesConfiguration(SpecialLogSourceData loggingErrorsAndWarnings, SpecialLogSourceData unprocessed, SpecialLogSourceData allEvents)
        {
            this.LoggingErrorsAndWarnings = loggingErrorsAndWarnings;
            this.Unprocessed = unprocessed;
            this.AllEvents = allEvents;
        }

        /// <summary>
        /// Gets the special log source used for errors and warnings.
        /// </summary>
        public SpecialLogSourceData LoggingErrorsAndWarnings { get; internal set; }

        /// <summary>
        /// Gets the special log source used for unprocessed entries.
        /// </summary>
        public SpecialLogSourceData Unprocessed { get; internal set; }

        /// <summary>
        /// Gets the special log source used for all events.
        /// </summary>
        public SpecialLogSourceData AllEvents { get; internal set; }
    }
}

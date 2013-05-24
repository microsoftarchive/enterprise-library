using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents configuration data for the <see cref="LogSource"/> class.
    /// </summary>
    public class LogSourceData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSourceData"/> class.
        /// </summary>
        public LogSourceData()
        {
            this.AutoFlush = true;
            this.Listeners = new List<TraceListener>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogSourceData"/> class with the specified parameters.
        /// </summary>
        /// <param name="name">The name for the log source.</param>
        /// <param name="level">The level for the log source.</param>
        /// <param name="autoFlush"><see langword="true"/> to enable auto-flush; otherwise, <see langword="false"/>.</param>
        /// <param name="traceListeners">One or more <see cref="TraceListener"/> objects.</param>
        public LogSourceData(string name, SourceLevels level, bool autoFlush, params TraceListener[] traceListeners)
        {
            this.Name = name;
            this.Level = level;
            this.AutoFlush = autoFlush;
            this.Listeners = traceListeners.ToList();
        }

        /// <summary>
        /// Gets or sets the name of the log source.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether auto-flush is enabled.
        /// </summary>
        public bool AutoFlush { get; set; }

        /// <summary>
        /// Gets or sets the event type filtering level.
        /// </summary>
        public SourceLevels Level { get; set; }

        /// <summary>
        /// Gets a collection of <see cref="TraceListener"/> objects.
        /// </summary>
        public ICollection<TraceListener> Listeners { get; private set; }

        internal LogSource ToLogSource()
        {
            return new LogSource(this.Name, this.Listeners, this.Level, this.AutoFlush);
        }
    }
}

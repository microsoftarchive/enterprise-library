using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents configuration settings for a special <see cref="LogSource"/> class.
    /// </summary>
    public class SpecialLogSourceData
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialLogSourceData"/> class.
        /// </summary>
        public SpecialLogSourceData()
        {
            this.AutoFlush = true;
            this.Level = SourceLevels.All;
            this.Listeners = new List<TraceListener>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialLogSourceData"/> class with a name.
        /// </summary>
        /// <param name="name">The name of the special log source.</param>
        public SpecialLogSourceData(string name)
            : this()
        {
            this.Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SpecialLogSourceData"/> class with the specified name, level, and options.
        /// </summary>
        /// <param name="name">The name of the special log source.</param>
        /// <param name="level">The filtering level of the special log source.</param>
        /// <param name="autoFlush"><see langword="true"/> to enable auto-flush; otherwise, <see langword="false"/>.</param>
        /// <param name="traceListeners">One or more <see cref="TraceListener"/> objects.</param>
        public SpecialLogSourceData(string name, SourceLevels level, bool autoFlush, params TraceListener[] traceListeners)
        {
            this.Name = name;
            this.Level = level;
            this.AutoFlush = autoFlush;
            this.Listeners = traceListeners.ToList();
        }

        /// <summary>
        /// Gets or sets the name of the special log source.
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

        internal static SpecialLogSourceData FromLogSource(LogSource logSource)
        {
            return new SpecialLogSourceData(logSource.Name, logSource.Level, logSource.AutoFlush, logSource.Listeners.ToArray());
        }
    }
}

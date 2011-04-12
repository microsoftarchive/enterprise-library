using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    partial class LogSource
    {
        /// <summary>
        /// Provides an update context for changing the <see cref="LogSource"/> settings.
        /// </summary>
        protected internal class LogSourceUpdateContext : ILogSourceUpdateContext, ICommitable
        {
            private readonly LogSource logSource;

            public LogSourceUpdateContext(LogSource logSource)
            {
                this.logSource = logSource;
                this.Level = logSource.Level;
                this.Listeners = logSource.Listeners.Select(x => x.Name).ToList();
            }

            public string Name
            {
                get { return this.logSource.Name; }
            }

            public SourceLevels Level { get; set; }

            public bool AutoFlush { get; set; }

            public ICollection<string> Listeners { get; private set; }

            protected internal virtual void ApplyChanges()
            {
                this.logSource.Level = this.Level;
                this.logSource.AutoFlush = this.AutoFlush;

                var newListeners = this.logSource.Listeners.Where(x => this.Listeners.Contains(x.Name)).ToList();
                this.logSource.Listeners = newListeners;
            }

            void ICommitable.Commit()
            {
                this.ApplyChanges();
            }
        }

        /// <summary>
        /// Provides an update context to batch change requests to the <see cref="LogSource"/> configuration.
        /// </summary>
        /// <returns>Returns an <see cref="ILogWriterUpdateContext"/> instance that can be used to apply the configuration changes.</returns>
        protected internal virtual ILogSourceUpdateContext GetUpdateContext()
        {
            return new LogSourceUpdateContext(this);
        }
    }
}

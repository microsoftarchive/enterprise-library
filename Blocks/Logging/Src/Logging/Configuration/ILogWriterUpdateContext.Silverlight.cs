using System.Collections.Generic;

using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Provides an update context for changing the <see cref="LogWriter"/> settings.
    /// </summary>
    public interface ILogWriterUpdateContext
    {
        /// <summary>
        /// Gets the update contexts for all the configured <see cref="LogSource"/>s.
        /// </summary>
        ICollection<ILogSourceUpdateContext> TraceSources { get; }

        /// <summary>
        /// Gets or sets if logging is enabled.
        /// </summary>
        /// <remarks>In some <see cref="LogWriter"/> implementations, you might require to add the <see cref="LogEnabledFilter"/>
        /// to be able to change this value.</remarks>
        /// <returns><see langword="true"/> if logging is enabled.</returns>
        bool IsLoggingEnabled { get; set; }

        /// <summary>
        /// Commits the changes.
        /// </summary>
        void ApplyChanges();
    }
}
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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Provides an update context for changing the <see cref="LogWriter"/> settings.
    /// </summary>
    public interface ILogWriterUpdateContext
    {
        /// <summary>
        /// Gets the update contexts for all the configured categories.
        /// </summary>
        /// <seealso cref="LogSource"/>
        ICollection<ILogSourceUpdateContext> Categories { get; }

        /// <summary>
        /// Gets or sets if logging is enabled.
        /// </summary>
        /// <returns><see langword="true"/> if logging is enabled.</returns>
        bool IsLoggingEnabled { get; set; }

        /// <summary>
        /// Gets the update contexts for all the configured <see cref="TraceListener"/>s.
        /// </summary>
        ICollection<ITraceListenerUpdateContext> Listeners { get; }

        /// <summary>
        /// Commits the changes.
        /// </summary>
        void ApplyChanges();

        /// <summary>
        /// Gets the update context for configured 'All Events' special category.
        /// </summary>
        /// <seealso cref="LogSource"/>
        ILogSourceUpdateContext AllEventsCategory { get; }

        /// <summary>
        /// Gets the update context for configured 'Not Processed' special category.
        /// </summary>
        /// <seealso cref="LogSource"/>
        ILogSourceUpdateContext NotProcessedCategory { get; }

        /// <summary>
        /// Gets the update context for configured 'Errors' special category.
        /// </summary>
        /// <seealso cref="LogSource"/>
        ILogSourceUpdateContext ErrorsCategory { get; }
    }
}

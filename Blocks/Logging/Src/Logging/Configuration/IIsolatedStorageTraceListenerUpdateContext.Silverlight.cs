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

using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration
{
    /// <summary>
    /// Provides an update context for changing the <see cref="IsolatedStorageTraceListener"/> settings.
    /// </summary>
    public interface IIsolatedStorageTraceListenerUpdateContext : ITraceListenerUpdateContext
    {
        /// <summary>
        /// Gets or sets the maximum size in kilobytes to be used in the isolated storage by the log entry repository.
        /// </summary>
        /// <remarks>When the repository is resized, it will try to allocate the specified space, but a smaller size than the 
        /// specified might be allocated if not as much space is available.</remarks>
        /// <returns>The maximum size in kilobytes as available when the storage was initialized.</returns>
        int MaxSizeInKilobytes { get; set; }

        /// <summary>
        /// Gets a value indicating if the underlying repository is available for the running instance.
        /// </summary>
        bool IsRepositoryAvailable { get; }
    }
}

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
    /// Provides an update context for changing the <see cref="RemoteServiceTraceListener"/> settings.
    /// </summary>
    public interface IRemoteServiceTraceListenerUpdateContext : ITraceListenerUpdateContext
    {
        /// <summary>
        /// Gets or sets a value indicating if the log entries should be sent shortly after they have been logged, or else 
        /// wait until the submit interval value has elapsed.
        /// </summary>
        /// <remarks>
        /// Set this value to <see langword="false"/> in order to buffer as many entries as possible during the submit interval time,
        /// and have potentially fewer and larger requests to the server.
        /// Set this value to <see langword="true"/> if you prefer to try to submit the entries as soon as possible, potentially sending only one or few log 
        /// entries per server call.
        /// </remarks>
        bool SendImmediately { get; set; }

        /// <summary>
        /// Gets or sets the maximum size in kilobytes to be used when storing entries into the isolated storage as a backup strategy.
        /// </summary>
        int IsolatedStorageBufferMaxSizeInKilobytes { get; set; }
    }
}

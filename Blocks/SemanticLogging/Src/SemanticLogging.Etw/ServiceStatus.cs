#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Semantic Logging Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

namespace Microsoft.Practices.EnterpriseLibrary.SemanticLogging.Etw
{
    /// <summary>
    /// The status of this service instance. 
    /// </summary>
    public enum ServiceStatus
    {
        /// <summary>
        /// The service was not started yet.
        /// </summary>
        NotStarted,

        /// <summary>
        /// The service has started.
        /// </summary>
        Started,

        /// <summary>
        /// The service is stopping.
        /// </summary>
        Stopping,

        /// <summary>
        /// The service was stopped.
        /// </summary>
        Stopped,

        /// <summary>
        /// The service was disposed.
        /// </summary>
        Disposed,

        /// <summary>
        /// The service has faulted.
        /// </summary>
        Faulted,
    }
}

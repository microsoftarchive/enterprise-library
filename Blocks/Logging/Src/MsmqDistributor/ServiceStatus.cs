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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.MsmqDistributor
{
    /// <summary>
    /// The Service status enum (as byte)
    /// This status is used to flag the service it should be shut down or not.
    /// </summary>
    public enum ServiceStatus : int
    {
        /// <summary>
        /// The service is running fine and should not be shut down.
        /// </summary>
        OK = 0,
        /// <summary>
        /// The service has encountered a problem or has been directed to shut down.
        /// </summary>
        Shutdown = 1,
        /// <summary>
        /// The service has encountered a problem or has been directed to shut down and cannot shut down due to errors.
        /// </summary>
        PendingShutdown = 2
    }
}

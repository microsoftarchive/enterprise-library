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

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Factory that creates a transient proxy for accessing the logging service.
    /// </summary>
    public interface ILoggingServiceFactory
    {
        /// <summary>
        /// Creates an <see cref="ILoggingService"/> channel that is used to send messages to the service.
        /// </summary>
        /// <returns>The transient <see cref="ILoggingService"/> created by the factory.</returns>
        ILoggingService CreateChannel();
    }
}
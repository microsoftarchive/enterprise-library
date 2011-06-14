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

using System.ServiceModel;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Service
{
    /// <summary>
    /// Allows clients to submit log entries into the server log.
    /// </summary>
    [ServiceContract(Namespace = "")]
    public interface ILoggingService
    {
        /// <summary>
        /// Adds log entries into to the server log.
        /// </summary>
        /// <param name="entries">The client log entries to log in the server.</param>
        [OperationContract(IsOneWay = true)]
        void Add(LogEntryMessage[] entries);
    }
}

#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

using System;
using System.Net.Sockets;
using System.ServiceModel;

using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling
{
    /// <summary>
    /// Provides the transient error detection logic that can recognize transient faults when dealing with Windows Azure Caching.
    /// </summary>
    public class CacheTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        /// <summary>
        /// Checks whether or not the specified exception belongs to a category of transient failures that can be compensated by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>true if the specified exception belongs to the category of transient errors; otherwise, false.</returns>
        public bool IsTransient(Exception ex)
        {
            if (ex == null)
            {
                return false;
            }

            if (ex is ServerTooBusyException)
            {
                return true;
            }

            var socketFault = ex as SocketException;
            if (socketFault != null)
            {
                return socketFault.SocketErrorCode == SocketError.TimedOut;
            }

            var dataCacheExceptionResult = DataCacheExceptionChecker.IsTransientDataCacheException(ex);
            if (dataCacheExceptionResult.HasValue)
            {
                return dataCacheExceptionResult.Value;
            }

            // Some transient exceptions may be wrapped into an outer exception, hence we should also inspect any inner exceptions.
            if (ex != null && ex.InnerException != null)
            {
                return this.IsTransient(ex.InnerException);
            }

            return false;
        }
    }
}

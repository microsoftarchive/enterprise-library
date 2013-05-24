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
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Services.Client;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text.RegularExpressions;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling
{
    /// <summary>
    /// Provides the transient error detection logic that can recognize transient faults when dealing with Windows Azure storage services.
    /// </summary>
    public class StorageTransientErrorDetectionStrategy : ITransientErrorDetectionStrategy
    {
        /// <summary>
        /// The error code strings that will be used to check for transient errors.
        /// </summary>
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes", Justification = "It's a read-only collection.")]
        public static readonly ReadOnlyCollection<string> TransientStorageErrorCodeStrings = new List<string> { "InternalError", "ServerBusy", "OperationTimedOut", "TableServerOutOfMemory", "TableBeingDeleted" }.AsReadOnly();
        private static readonly Regex errorCodeRegex = new Regex(@"<code>(\w+)</code>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        private static readonly int[] httpStatusCodes = new[] { (int)HttpStatusCode.InternalServerError, (int)HttpStatusCode.GatewayTimeout, (int)HttpStatusCode.ServiceUnavailable, (int)HttpStatusCode.RequestTimeout };
        private static readonly WebExceptionStatus[] webExceptionStatus = new[] 
        {
            WebExceptionStatus.ConnectionClosed, 
            WebExceptionStatus.Timeout,
            WebExceptionStatus.RequestCanceled, 
            WebExceptionStatus.KeepAliveFailure, 
            WebExceptionStatus.PipelineFailure, 
            WebExceptionStatus.ReceiveFailure, 
            WebExceptionStatus.ConnectFailure, 
            WebExceptionStatus.SendFailure
        };
        private static readonly SocketError[] socketErrorCodes = new[] { SocketError.ConnectionRefused, SocketError.TimedOut };

        /// <summary>
        /// Determines whether the specified exception represents a transient failure that can be compensated by a retry.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>true if the specified exception is considered transient; otherwise, false.</returns>
        public bool IsTransient(Exception ex)
        {
            return ex != null && (this.CheckIsTransient(ex) || (ex.InnerException != null && this.CheckIsTransient(ex.InnerException)));
        }

        /// <summary>
        /// Checks whether the specified exception is transient.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>true if the specified exception is considered transient; otherwise, false.</returns>
        protected virtual bool CheckIsTransient(Exception ex)
        {
            var webException = ex as WebException;
            if (webException != null)
            {
                if (webExceptionStatus.Contains(webException.Status)) return true;

                if (webException.Status == WebExceptionStatus.ProtocolError)
                {
                    var response = webException.Response as HttpWebResponse;
                    if (response != null && httpStatusCodes.Contains((int)response.StatusCode)) return true;
                }

                return false;
            }

            var socketException = ex as SocketException;
            if (socketException != null)
            {
                // This section handles the following transient faults:
                //
                // Exception Type: System.Net.Sockets.SocketException
                // 		Error Code: 10061
                // 		Message: No connection could be made because the target machine actively refused it XXX.XXX.XXX.XXX:443
                // 		Socket Error Code: ConnectionRefused
                // Exception Type: System.Net.Sockets.SocketException
                //      Error Code: 10060
                //      Message: A connection attempt failed because the connected party did not properly respond after a period of time, or established connection failed because connected host has failed to respond 168.62.128.143:443
                //      Socket Error Code: TimedOut
                if (socketErrorCodes.Contains(socketException.SocketErrorCode)) return true;
            }

            var dataServiceException = ex as DataServiceRequestException;
            if (dataServiceException != null)
            {
                if (TransientStorageErrorCodeStrings.Contains(GetErrorCode(dataServiceException))) return true;

                var response = dataServiceException.Response;
                if (response != null && response.Any(x => httpStatusCodes.Contains(x.StatusCode))) return true;
            }

            var dataServiceClientException = ex as DataServiceClientException;
            if (dataServiceClientException != null)
            {
                // It was found that sometimes a connection to the storage infrastructure can be subject to unexpected termination with a SocketException. 
                // The WCF Data Services client may not include actual exception but report it inside the message text, for example, the error message can say:
                // "System.Net.WebException: The underlying connection was closed: A connection that was expected to be kept alive was closed by the server. ---> 
                // System.IO.IOException: Unable to read data from the transport connection: A connection attempt failed because the connected party did not properly respond 
                // after a period of time, or established connection failed because connected host has failed to respond. ---> 
                // System.Net.Sockets.SocketException: A connection attempt failed because the connected party did not properly respond after a period of time, or 
                // established connection failed because connected host has failed to respond".
                // It was also found that the above exception may have a status code of 500 (Internal Server Error).

                if (httpStatusCodes.Contains(dataServiceClientException.StatusCode)) return true;
            }

            if (StorageV1ExceptionChecker.IsTransient(ex)) return true;

            if (StorageV2ExceptionChecker.IsTransient(ex)) return true;

            if (ex is TimeoutException) return true;

            // This may be System.IO.IOException: "Unable to read data from the transport connection: The connection was closed" which could manifest itself under extremely high load.
            // Do not return true if ex is a subtype of IOException (such as FileLoadException, when it cannot load a required assembly)
            if (ex.GetType() == typeof(IOException) && ex.InnerException == null) return true;

            return false;
        }

        /// <summary>
        /// Gets the error code string from the exception.
        /// </summary>
        /// <param name="ex">The exception that contains the error code as a string inside the message.</param>
        /// <returns>The error code string.</returns>
        [SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters", Justification = "As designed.")]
        protected static string GetErrorCode(DataServiceRequestException ex)
        {
            if (ex != null && ex.InnerException != null)
            {
                var match = errorCodeRegex.Match(ex.InnerException.Message);

                return match.Groups[1].Value;
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// Provides support for checking transient faults when using the Windows Azure Storage version 1 managed API.
        /// </summary>
        /// <remarks>
        /// This type avoids externalizing the usage of the Windows Azure Storage assembly, so if the application
        /// is using another version of the assembly, this type does not throw exceptions when the JIT
        /// compiler tries to load this version of the assembly.
        /// </remarks>
        private static class StorageV1ExceptionChecker
        {
            public static bool IsTransient(Exception ex)
            {
                if (ex.GetType().FullName == "Microsoft.WindowsAzure.StorageClient.StorageException")
                {
                    return IsTransientInternal(ex);
                }

                return false;
            }

            // avoid inlining so the JIT compiler does not try to load the type and potentially fail
            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            private static bool IsTransientInternal(Exception ex)
            {
                var storageErrorCodes = new[] 
                    { 
                        (int)Microsoft.WindowsAzure.StorageClient.StorageErrorCode.ServiceInternalError,
                        (int)Microsoft.WindowsAzure.StorageClient.StorageErrorCode.ServiceTimeout
                    };

                var storageException = ex as Microsoft.WindowsAzure.StorageClient.StorageException;
                if (storageException != null)
                {
                    if (storageErrorCodes.Contains((int)storageException.ErrorCode)) return true;

                    if (storageException.ExtendedErrorInformation != null)
                    {
                        if (TransientStorageErrorCodeStrings.Contains(storageException.ExtendedErrorInformation.ErrorCode)) return true;
                    }
                }

                return false;
            }
        }

        /// <summary>
        /// Provides support for checking transient faults when using the Windows Azure Storage version 2 managed API.
        /// </summary>
        /// <remarks>
        /// This type avoids externalizing the usage of the Windows Azure Storage assembly, so if the application
        /// is using another version of the assembly, this type does not throw exceptions when the JIT
        /// compiler tries to load this version of the assembly.
        /// </remarks>
        private static class StorageV2ExceptionChecker
        {
            public static bool IsTransient(Exception ex)
            {
                if (ex.GetType().FullName == "Microsoft.WindowsAzure.Storage.StorageException")
                {
                    return IsTransientInternal(ex);
                }

                return false;
            }

            // avoid inlining so the JIT compiler does not try to load the type and potentially fail
            [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
            [SecuritySafeCritical]
            private static bool IsTransientInternal(Exception ex)
            {
                var storageException = ex as Microsoft.WindowsAzure.Storage.StorageException;
                if (storageException != null)
                {
                    var requestInfo = storageException.RequestInformation;
                    if (requestInfo != null)
                    {
                        if (httpStatusCodes.Contains(storageException.RequestInformation.HttpStatusCode)) return true;

                        if (requestInfo.ExtendedErrorInformation != null
                            && TransientStorageErrorCodeStrings.Contains(requestInfo.ExtendedErrorInformation.ErrorCode)) return true;
                    }
                }

                return false;
            }
        }
    }
}

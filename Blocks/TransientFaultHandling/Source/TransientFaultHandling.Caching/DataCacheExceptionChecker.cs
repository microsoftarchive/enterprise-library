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
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Caching
{
    /// <summary>
    /// Checks whether an <see cref="Exception"/> is of the Microsoft.ApplicationServer.Caching.DataCacheException type,
    /// and if so, checks the error code and status code to determine if it is transient.
    /// This method dynamically discovers the type, to avoid a hard reference to the 
    /// Microsoft.ApplicationServer.Caching.Core assembly (either version 1.0.0.0 or 101.0.0.0).
    /// </summary>
    internal static class DataCacheExceptionChecker
    {
        // PublicKeyToken = 31bf3856ad364e35
        private static readonly byte[] CachePublicKeyToken = new byte[] { 0x31, 0xbf, 0x38, 0x56, 0xad, 0x36, 0x4e, 0x35 };

        private static Type DataCacheExceptionType;
        private static Func<Exception, int> GetErrorCode;

        private static int[] errorCodes = new int[]
            {
                16, //DataCacheErrorCode.ConnectionTerminated,
                17, //DataCacheErrorCode.RetryLater,
                18, //DataCacheErrorCode.Timeout,
                17016, //DataCacheErrorCode.ServiceAccessError,
            };

        /// <summary>
        /// Checks whether <paramref name="ex"/> is a transient DataCacheException.
        /// </summary>
        /// <param name="ex">The exception object to be verified.</param>
        /// <returns>
        /// <see langword="true"/> if the exception is of type DataCacheException and is transient;
        /// <see langword="false"/> if the exception is of type DataCacheException and is not transient;
        /// <see langword="null"/> if the exception is not of type DataCacheException.
        /// </returns>
        public static bool? IsTransientDataCacheException(Exception ex)
        {
            if (ex == null)
            {
                return null;
            }

            if (DataCacheExceptionType == null)
            {
                var exceptionType = ex.GetType();
                if (exceptionType.FullName == "Microsoft.ApplicationServer.Caching.DataCacheException")
                {
                    InitializeTypeAccesors(exceptionType);
                }
            }

            if (DataCacheExceptionType != null && DataCacheExceptionType.IsInstanceOfType(ex))
            {
                return errorCodes.Contains(GetErrorCode(ex));
            }

            return null;
        }

        private static void InitializeTypeAccesors(Type type)
        {
            // this creates the following function from the reflected type (DataCacheException):
            //GetErrorCode = i => ((DataCacheException)i).ErrorCode;

            if (DataCacheExceptionType == null)
            {
                CheckIsCacheAssembly(type.Assembly);

                var errorCodeProperty = type.GetProperty("ErrorCode");
                if (errorCodeProperty == null || errorCodeProperty.PropertyType != typeof(int))
                    throw new InvalidOperationException(Resources.TypeMismatchException);

                var parameter = Expression.Parameter(typeof(Exception), "i");

                // (DataCacheException)i
                var cast = Expression.TypeAs(parameter, type);

                // ((DataCacheException)i).ErrorCode
                GetErrorCode = Expression.Lambda<Func<Exception, int>>(
                    Expression.Property(
                        cast,
                        errorCodeProperty),
                    parameter).Compile();

                DataCacheExceptionType = type;
            }
        }

        private static void CheckIsCacheAssembly(Assembly assembly)
        {
            // should we also filter by version to support only 1.0.0.0 and 101.0.0.0?
            var assemblyName = assembly.GetName();
            if (assemblyName != null && assemblyName.Name == "Microsoft.ApplicationServer.Caching.Core")
            {
                var token = assemblyName.GetPublicKeyToken();
                if (token != null && CachePublicKeyToken.SequenceEqual(token))
                {
                    return;
                }
            }

            throw new InvalidOperationException(
                string.Format(CultureInfo.CurrentCulture, Resources.AssemblyMismatchException, assemblyName));
        }
    }
}

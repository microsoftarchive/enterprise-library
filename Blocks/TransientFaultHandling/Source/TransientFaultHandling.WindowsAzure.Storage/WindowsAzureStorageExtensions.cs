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

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling
{
    /// <summary>
    /// Extends the RetryStrategy class to allow using the retry strategies from the Transient Fault Handling Application Block with Windows Azure Store.
    /// </summary>
    public static class WindowsAzureStorageExtensions
    {
        /// <summary>
        /// Wraps a Transient Fault Handling Application Block retry strategy into a Microsoft.WindowsAzure.StorageClient.RetryPolicy.
        /// </summary>
        /// <param name="retryStrategy">The Transient Fault Handling Application Block retry strategy to wrap.</param>
        /// <returns>Returns a wrapped Transient Fault Handling Application Block retry strategy into a Microsoft.WindowsAzure.StorageClient.RetryPolicy.</returns>
        public static Microsoft.WindowsAzure.StorageClient.RetryPolicy AsAzureStorageClientRetryPolicy(this RetryStrategy retryStrategy)
        {
            if (retryStrategy == null) throw new ArgumentNullException("retryStrategy");

            return () => new ShouldRetryWrapper(retryStrategy.GetShouldRetry()).ShouldRetry;
        }

        private class ShouldRetryWrapper
        {
            private readonly ShouldRetry shouldRetry;

            public ShouldRetryWrapper(ShouldRetry shouldRetry)
            {
                this.shouldRetry = shouldRetry;
            }

            public bool ShouldRetry(int retryCount, Exception lastException, out TimeSpan delay)
            {
                return this.shouldRetry(retryCount, lastException, out delay);
            }
        }
    }
}

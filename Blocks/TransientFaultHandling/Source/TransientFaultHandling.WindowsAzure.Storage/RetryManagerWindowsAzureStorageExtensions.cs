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
    /// Provides extensions to the <see cref="RetryManager"/> class for using the Windows Azure Storage retry strategy.
    /// </summary>
    public static class RetryManagerWindowsAzureStorageExtensions
    {
        /// <summary>
        /// The technology name that can be used to get the default Windows Azure Storage retry strategy name.
        /// </summary>
        public const string DefaultStrategyTechnologyName = "WindowsAzure.Storage";

        /// <summary>
        /// Returns the default retry strategy for Windows Azure Storage.
        /// </summary>
        /// <returns>The default Windows Azure Storage retry strategy (or the default strategy if no default for Windows Azure Storage could be found).</returns>
        public static RetryStrategy GetDefaultAzureStorageRetryStrategy(this RetryManager retryManager)
        {
            if (retryManager == null) throw new ArgumentNullException("retryManager");

            return retryManager.GetDefaultRetryStrategy(DefaultStrategyTechnologyName);
        }

        /// <summary>
        /// Returns the default retry policy dedicated to handling transient conditions with Windows Azure Storage.
        /// </summary>
        /// <returns>The retry policy for Windows Azure Storage with the corresponding default strategy (or the default strategy if no retry strategy definition for Windows Azure Storage was found).</returns>
         public static RetryPolicy GetDefaultAzureStorageRetryPolicy(this RetryManager retryManager)
        {
            if (retryManager == null) throw new ArgumentNullException("retryManager");

            return new RetryPolicy(new StorageTransientErrorDetectionStrategy(), retryManager.GetDefaultAzureStorageRetryStrategy());
        }
    }
}

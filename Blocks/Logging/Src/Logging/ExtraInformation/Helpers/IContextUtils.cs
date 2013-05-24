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

using System.Security;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.ExtraInformation.Helpers
{
    /// <summary>
    /// Contract for accessing context information.
    /// </summary>
    [SecurityCritical]
    public interface IContextUtils
    {
        /// <summary>
        /// Returns the ActivityId.
        /// </summary>
        /// <returns>The ActivityId</returns>
        string GetActivityId();

        /// <summary>
        /// Returns the ApplicationId.
        /// </summary>
        /// <returns>The ApplicationId.</returns>
        string GetApplicationId();

        /// <summary>
        /// Returns the TransactionId.
        /// </summary>
        /// <returns>The TransactionId.</returns>
        string GetTransactionId();

        /// <summary>
        /// Returns the direct caller account name.
        /// </summary>
        /// <returns>The direct caller account name.</returns>
        string GetDirectCallerAccountName();

        /// <summary>
        /// Returns the original caller account name.
        /// </summary>
        /// <returns>The original caller account name.</returns>
        string GetOriginalCallerAccountName();
    }
}

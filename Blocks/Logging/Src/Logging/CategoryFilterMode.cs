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

namespace Microsoft.Practices.EnterpriseLibrary.Logging
{
    /// <summary>
    /// Represents the possible modes for client-side category filtering.
    /// </summary>
    public enum CategoryFilterMode
    {
        /// <summary>
        /// Allow all categories except those explicitly denied
        /// </summary>
        AllowAllExceptDenied,

        /// <summary>
        /// Deny all categories except those explicitly allowed
        /// </summary>
        DenyAllExceptAllowed
    }
}
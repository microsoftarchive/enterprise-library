//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile
{
    /// <summary>
    /// Provides attributes for the filter of types.
    /// </summary>
    public enum MatchKind
    {
        /// <summary>
        /// Type or instance is denied.
        /// </summary>
        Deny,

        /// <summary>
        /// Type or instance is allowed.
        /// </summary>
        Allow
    }
}

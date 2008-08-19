//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Security.Principal;

namespace Microsoft.Practices.EnterpriseLibrary.Security
{
    /// <summary>
    /// The types of entities supported by Security.
    /// </summary>
    public enum SecurityEntityType
    {
        /// <summary>
        /// See <see cref="IIdentity"/>.
        /// </summary>
        Identity,
        /// <summary>
        /// Any object representing the profile of a user.
        /// </summary>
        Profile,
        /// <summary>
        /// See <see cref="IPrincipal"/>.
        /// </summary>
        Principal
    }
}
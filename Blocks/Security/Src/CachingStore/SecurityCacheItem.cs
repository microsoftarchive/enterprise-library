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

using System;
using System.Security.Principal;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore
{
    /// <summary>
    /// Represents an item stored in SecurityCache
    /// </summary>
    [Serializable]
    public class SecurityCacheItem
    {
        private IIdentity identity = null;
        private object profile = null;
        private IPrincipal principal = null;

        /// <summary>
        /// The IIdentity to cache.
        /// </summary>
        public IIdentity Identity
        {
            get { return identity; }
            set { identity = value; }
        }

        /// <summary>
        /// The profile to cache.
        /// </summary>
        public object Profile
        {
            get { return profile; }
            set { profile = value; }
        }

        /// <summary>
        ///  The IPrincipal to cache.
        /// </summary>
        public IPrincipal Principal
        {
            get { return principal; }
            set { principal = value; }
        }

        /// <devDoc>
        /// A cache item is deemed removeable if all properties are null
        /// </devDoc>
        public bool IsRemoveable
        {
            get { return (identity == null && profile == null && principal == null); }
        }
    }
}
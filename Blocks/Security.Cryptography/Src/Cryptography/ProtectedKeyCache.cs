//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography
{
    /// <summary>
    /// Represents the protected key cache.
    /// </summary>
    public class ProtectedKeyCache
    {
        readonly Dictionary<string, ProtectedKey> cache = new Dictionary<string, ProtectedKey>();

        /// <summary>
        /// Gets the protected ke for the key file name.
        /// </summary>
        /// <param name="keyFileName">The key file name to get the key.</param>
        /// <returns>A <see cref="ProtectedKey"/> for the key file name.</returns>
        public ProtectedKey this[string keyFileName]
        {
            get
            {
                if (String.IsNullOrEmpty(keyFileName)) throw new ArgumentException("keyFileName");

                lock (cache)
                {
                    return cache.ContainsKey(keyFileName) ? cache[keyFileName] : null;
                }
            }
            set
            {
                if (value == null) throw new ArgumentNullException("value");
                if (String.IsNullOrEmpty(keyFileName)) throw new ArgumentException("keyFileName");

                lock (cache)
                {
                    cache[keyFileName] = value;
                }
            }
        }

        ///<summary>
        /// Clear the cache.
        ///</summary>
        public void Clear()
        {
            lock (cache)
            {
                cache.Clear();
            }
        }
    }
}

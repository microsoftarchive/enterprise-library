//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.IO.IsolatedStorage;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    /// <summary>
    /// Provides information about the isolated storage.
    /// </summary>
    public class IsolatedStorageInfo : IIsolatedStorageInfo
    {
        /// <summary>
        /// Gets the available free space.
        /// </summary>
        public long AvailableFreeSpace
        {
            get
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    return store.AvailableFreeSpace;
                }
            }
        }
    }
}

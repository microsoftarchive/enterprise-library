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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a custom <see cref="IBackingStore"/> instance.
    /// </summary>
    /// <seealso cref="CustomCacheStorageData"/>
    public interface IStoreInCustomStore : ICachingConfiguration, IFluentInterface
    {
        /// <summary>
        /// Returns a fluent interface that can be used to set up encryption for the current custom <see cref="IBackingStore"/> instance.
        /// </summary>
        IBackingStoreEncryptItemsUsing EncryptUsing { get; }
    }
}

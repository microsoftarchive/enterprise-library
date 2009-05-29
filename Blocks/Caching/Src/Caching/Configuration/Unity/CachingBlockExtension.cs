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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity
{
    /// <summary>
    /// A <see cref="UnityContainerExtension"/> that registers the policies necessary
    /// to create <see cref="ICacheManager"/> instances described in the standard
    /// configuration file.
    /// </summary>
    [Obsolete]
    public class CachingBlockExtension : EnterpriseLibraryBlockExtension
    {
    }
}

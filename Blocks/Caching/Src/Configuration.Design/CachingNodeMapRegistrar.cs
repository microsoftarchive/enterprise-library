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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design
{
	class CachingNodeMapRegistrar : NodeMapRegistrar
	{
		public CachingNodeMapRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override void Register()
		{
			AddMultipleNodeMap(Resources.CacheManagerMenuText,
				typeof(CacheManagerNode),
				typeof(CacheManagerData));

			AddMultipleNodeMap(Resources.CustomCacheManagerUICommandText,
				typeof(CustomCacheManagerNode),
				typeof(CustomCacheManagerData));

			AddSingleNodeMap(Resources.IsolatedStorageUICommandText,
				typeof(IsolatedStorageCacheStorageNode),
				typeof(IsolatedStorageCacheStorageData));

			AddSingleNodeMap(Resources.CustomStorageUICommandText,
				typeof(CustomCacheStorageNode),
				typeof(CustomCacheStorageData));
		}        
	}
}

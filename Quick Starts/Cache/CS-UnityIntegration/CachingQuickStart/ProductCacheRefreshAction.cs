//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Caching;

namespace CachingQuickStart
{
	/// <summary>
	/// Summary description for ProductCacheRefreshAction.
	/// </summary>
  [Serializable]
  public class ProductCacheRefreshAction : ICacheItemRefreshAction
  {
    public void Refresh(string key, object expiredValue, CacheItemRemovedReason removalReason)
    {
      // Item has been removed from cache. Perform desired actions here, based upon
      // the removal reason (e.g. refresh the cache with the item).
    }
  }
}

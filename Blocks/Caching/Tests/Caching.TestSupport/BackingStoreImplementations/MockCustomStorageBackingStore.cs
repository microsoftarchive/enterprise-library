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
using System.Collections;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport.BackingStoreImplementations
{
	[ConfigurationElementType(typeof(CustomCacheStorageData))]
	public class MockCustomStorageBackingStore : MockCustomProviderBase, IBackingStore
	{
		public MockCustomStorageBackingStore(NameValueCollection attributes)
			: base(attributes)
		{
		}

		public int Count
		{
			get { throw new Exception("The method or operation is not implemented."); }
		}

		public void Add(CacheItem newCacheItem)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Remove(string key)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void UpdateLastAccessedTime(string key, DateTime timestamp)
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Flush()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public Hashtable Load()
		{
			throw new Exception("The method or operation is not implemented.");
		}

		public void Dispose()
		{
			throw new Exception("The method or operation is not implemented.");
		}
	}
}

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
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests
{
	[ConfigurationElementType(typeof(CustomCacheManagerData))]
	public class CustomCacheManager : MockCustomProviderBase, ICacheManager, IDisposable
	{
        public bool wasDisposed = false;
		public CustomCacheManager(NameValueCollection attributes) 
			: base(attributes)
		{
		}


		#region ICacheManager Members

		public void Add(string key, object value)
		{
			throw new NotImplementedException();
		}

		public void Add(string key, object value, CacheItemPriority scavengingPriority, ICacheItemRefreshAction refreshAction, params ICacheItemExpiration[] expirations)
		{
			throw new NotImplementedException();
		}

		public bool Contains(string key)
		{
			throw new NotImplementedException();
		}

		public int Count
		{
			get { throw new NotImplementedException(); }
		}

		public void Flush()
		{
			throw new NotImplementedException();
		}

		public object GetData(string key)
		{
			throw new NotImplementedException();
		}

		public void Remove(string key)
		{
			throw new NotImplementedException();
		}

		public object this[string key]
		{
			get { throw new NotImplementedException(); }
		}

		#endregion

        #region IDisposable Members

        public void Dispose()
        {
            wasDisposed = true;
        }

        #endregion
    }
}

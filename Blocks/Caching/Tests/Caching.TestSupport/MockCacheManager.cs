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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Linq.Expressions;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport
{
	public class MockCacheManagerData : CacheManagerDataBase
	{
		public MockCacheManagerData()
		{
		}

		public MockCacheManagerData(string name, string foo)
			: base(name, typeof(MockCacheManager))
		{
			Foo = foo;
		}

		[ConfigurationProperty("foo", IsRequired = true)]
		public string Foo
		{
			get { return (string)base["foo"]; }
			set { base["foo"] = value; }
		}

        protected override Expression<Func<ICacheManager>> GetCacheManagerCreationExpression()
        {
            return () => new MockCacheManager(Foo);
        }
	}

	[ConfigurationElementType(typeof(MockCacheManagerData))]
	public class MockCacheManager : ICacheManager
	{
		public MockCacheManager(string foo)
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
	}
}

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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Caching.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
	public sealed class MockCacheManagerDataNode : CacheManagerBaseNode
	{
		private string foo;

		public MockCacheManagerDataNode()
			: this(new MockCacheManagerData("MockCacheManager", "Foo"))
		{
		}

		public MockCacheManagerDataNode(MockCacheManagerData cacheManagerDataBase)
		{
			if (null == cacheManagerDataBase) throw new ArgumentNullException("cacheManagerDataBase");
			Rename(cacheManagerDataBase.Name);

			this.foo = cacheManagerDataBase.Foo;
		}

		[Required]
		[Category("General")]
		[Description("Foo")]
		public string Foo
		{
			get { return foo; }
			set { foo = value; }
		}

		public override CacheManagerDataBase CacheManagerData
		{
			get { return new MockCacheManagerData(Name, foo); }
		}
	}

	class MockCacheManagerDataNodeMapRegistrar : NodeMapRegistrar
	{
		public MockCacheManagerDataNodeMapRegistrar(IServiceProvider serviceProvider)
			: base(serviceProvider)
		{
		}

		public override void Register()
		{
			AddMultipleNodeMap("MockCacheManager",
				typeof(MockCacheManagerDataNode),
				typeof(MockCacheManagerData));
		}
	}

	public sealed class MockCacheManagerDataConfigurationDesignManager : ConfigurationDesignManager
	{
		public override void Register(IServiceProvider serviceProvider)
		{
			NodeMapRegistrar nodeMapRegistrar = new MockCacheManagerDataNodeMapRegistrar(serviceProvider);
			nodeMapRegistrar.Register();
		}
	}
}

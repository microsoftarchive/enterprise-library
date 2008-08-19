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
using System.Threading;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching
{
	/// <summary>
    /// Factory for <see cref="CacheManager"/>s. This class is responsible for creating all the internal
	/// classes needed to implement a CacheManager.
	/// </summary>
	public class CacheManagerFactory : LocatorNameTypeFactoryBase<ICacheManager>
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="CacheManagerFactory"/> class 
		/// with the default configuration source.</para>
		/// </summary>
		protected CacheManagerFactory()
			: base()
		{
		}
 
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="CacheManagerFactory"/> class 
		/// with the given configuration source.</para>
		/// </summary>
        /// <param name="configurationSource">The configuration source that contains information on how to build the <see cref="CacheManager"/> instances</param>
		public CacheManagerFactory(IConfigurationSource configurationSource)
			: base(configurationSource)
		{ }
	}
}
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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
	/// <summary>
	/// Base class for configuration data defining CacheManagerDataBase. Defines the information needed to properly configure
	/// a ICacheManager instance.
	/// </summary>    	
	[Assembler(typeof(TypeInstantiationAssembler<ICacheManager, CacheManagerDataBase>))]
	public class CacheManagerDataBase : NameTypeConfigurationElement
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerDataBase"/> class.
		/// </summary>
		public CacheManagerDataBase()
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="CacheManagerDataBase"/> class.
		/// </summary>
		/// <param name="name">
		/// The name of the <see cref="CacheManagerDataBase"/>.
		/// </param>
		/// <param name="type">The type of <see cref="ICacheManager"/>.</param>
		public CacheManagerDataBase(string name, Type type)
			: base(name, type)
		{
		}
	}
}
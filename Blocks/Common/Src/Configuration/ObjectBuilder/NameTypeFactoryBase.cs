//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Base class for instance factories.
	/// </summary>
	/// <remarks>
	/// This class is used to create instances of types compatible with <typeparamref name="T"/> described 
	/// by a configuration source.
	/// </remarks>
	public class NameTypeFactoryBase<T>
	{
		private IConfigurationSource configurationSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="NameTypeFactoryBase{T}"/> class with the default configuration source.
		/// </summary>
		protected NameTypeFactoryBase()
			: this(ConfigurationSourceFactory.Create())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="NameTypeFactoryBase{T}"/> class with a configuration source.
		/// </summary>
		/// <param name="configurationSource">The configuration source to use.</param>
		protected NameTypeFactoryBase(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;
		}

		/// <summary>
		/// Returns a new instance of <typeparamref name="T"/> based on the default instance configuration.
		/// </summary>
		/// <returns>
		/// A new instance of <typeparamref name="T"/>.
		/// </returns>
		public T CreateDefault()
		{
			return EnterpriseLibraryFactory.BuildUp<T>(configurationSource);
		}

		/// <summary>
		/// Returns an new instance of <typeparamref name="T"/> based on the configuration for <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of the required instance.</param>
		/// <returns>
		/// A new instance of <typeparamref name="T"/>.
		/// </returns>
		public T Create(string name)
		{
			return EnterpriseLibraryFactory.BuildUp<T>(name, configurationSource);
		}
	}
}

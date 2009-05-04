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

using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Base class for instance factories that require a locator to provide singletons.
	/// </summary>
	/// <remarks>
	/// This class is used to create instances of types compatible with <typeparamref name="T"/> described 
	/// by a configuration source.
	/// The use of a <see cref="IReadWriteLocator"/> enables singletons for the types that support them.
	/// </remarks>
	public class LocatorNameTypeFactoryBase<T>
	{
		private readonly IReadWriteLocator locator;
		private readonly ILifetimeContainer lifetimeContainer;
		private readonly IConfigurationSource configurationSource;

		/// <summary>
		/// Initializes a new instance of the <see cref="LocatorNameTypeFactoryBase{T}"/> class with the default configuration source
		/// and a locator.
		/// </summary>
		protected LocatorNameTypeFactoryBase()
			: this(ConfigurationSourceFactory.Create())
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="LocatorNameTypeFactoryBase{T}"/> class with a configuration source 
		/// and a locator.
		/// </summary>
		/// <param name="configurationSource">The configuration source to use.</param>
		protected LocatorNameTypeFactoryBase(IConfigurationSource configurationSource)
		{
			this.configurationSource = configurationSource;

			locator = new Locator();
			lifetimeContainer = new LifetimeContainer();
		}

		/// <summary>
		/// Returns an instance of <typeparamref name="T"/> based on the default instance configuration.
		/// </summary>
		/// <returns>
		/// A new instance of <typeparamref name="T"/>, or the singleton instance if <typeparamref name="T"/> allows singletons.
		/// </returns>
		public T CreateDefault()
		{
			return EnterpriseLibraryFactory.BuildUp<T>(locator, lifetimeContainer, configurationSource);
		}

		/// <summary>
		/// Returns an new instance of <typeparamref name="T"/> based on the configuration for <paramref name="name"/>.
		/// </summary>
		/// <param name="name">The name of the required instance.</param>
		/// <returns>
		/// A new instance of <typeparamref name="T"/>, or the singleton instance if <typeparamref name="T"/> allows singletons.
		/// </returns>
		public T Create(string name)
		{
			return EnterpriseLibraryFactory.BuildUp<T>(locator, lifetimeContainer, name, configurationSource);
		}
	}
}

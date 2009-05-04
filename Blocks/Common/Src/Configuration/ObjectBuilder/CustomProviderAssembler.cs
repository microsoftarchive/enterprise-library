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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Represents the process to build a custom provider for type <typeparamref name="TObject"/> described by an instance of <typeparamref name="TConcreteConfiguration"/> configuration object.
	/// </summary>
	/// <typeparam name="TObject">The abstract provider type.</typeparam>
	/// <typeparam name="TConfiguration">The base configuration type for providers of type <typeparamref name="TObject"/>.</typeparam>
	/// <typeparam name="TConcreteConfiguration">The concrete configuration type for custom providers of type <typeparamref name="TObject"/>.</typeparam>
	public class CustomProviderAssembler<TObject, TConfiguration, TConcreteConfiguration> : IAssembler<TObject, TConfiguration>
		where TObject : class
		where TConfiguration : class, IObjectWithNameAndType
		where TConcreteConfiguration : class, TConfiguration, ICustomProviderData
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Builds a custom provider for type <typeparamref name="TObject"/> based on an instance of <typeparamref name="TConcreteConfiguration"/>.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <typeparamref name="TConcreteConfiguration"/>.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized custom provider for type <typeparamref name="TObject"/>.</returns>
		public TObject Assemble(IBuilderContext context, TConfiguration objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			TConcreteConfiguration castedObjectConfiguration = (TConcreteConfiguration)objectConfiguration;

			TObject provider
				= (TObject)Activator.CreateInstance(objectConfiguration.Type, castedObjectConfiguration.Attributes);
			return provider;
		}
	}
}

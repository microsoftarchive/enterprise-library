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
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Generic assembler that will just instantiate the type in a configuration element for polymorphic hierarchies.
	/// </summary>
	/// <typeparam name="TObject">The interface or the base type to build.</typeparam>
	/// <typeparam name="TConfiguration">The base configuration object type.</typeparam>
	public class TypeInstantiationAssembler<TObject, TConfiguration> : IAssembler<TObject, TConfiguration>
		where TObject : class
		where TConfiguration : NameTypeConfigurationElement
	{
		/// <summary>
		/// Builds an instance of the subtype of <typeparamref name="TObject"/> type the receiver knows how to build by
		/// invoking the default constructor on the type specified by the configuration object.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A new instance of the <typeparamref name="TObject"/> subtype.</returns>
		public TObject Assemble(IBuilderContext context, TConfiguration objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return (TObject)Activator.CreateInstance(objectConfiguration.Type);
		}
	}
}

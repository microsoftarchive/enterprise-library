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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Represents a generic process to build objects of a polymorphic hierarchy based on a single configuration object.
	/// </summary>
	/// <remarks>
	/// This custom factory will rely assemblers to do the actual building out for a concrete type. Assemblers are implementations of the 
	/// <see cref="IAssembler{TObject, TConfiguration}"/> interface. Concrete configuration objects must have an 
	/// <see cref="AssemblerAttribute">Assembler</see> attribute to allow the factory to determine how the configuration should be interpreted.
	/// </remarks>
	/// <typeparam name="TObject">The interface or the base type to build.</typeparam>
	/// <typeparam name="TConfiguration">The base configuration object type.</typeparam>
	public abstract class AssemblerBasedCustomFactory<TObject, TConfiguration> : AssemblerBasedObjectFactory<TObject, TConfiguration>, ICustomFactory
		where TObject : class
		where TConfiguration : class
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Returns a new instance of a concrete subtype of <typeparamref name="TObject"/>, described by the matching configuration object 
		/// of a concrete subtype of <typeparamref name="TConfiguration"/> found in the <paramref name="configurationSource"/> under 
		/// the name <paramref name="name"/>.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="name">The name of the instance to build, or null.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A new instance of the appropriate subtype of <typeparamref name="Tobject"/>.</returns>
		/// <exception cref="ConfigurationErrorsException">when the configuration is invalid or <paramref name="name"/> cannot be found.</exception>
		public TObject Create(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			if (string.IsNullOrEmpty(name))
			{
				throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "name");
			}

			TConfiguration objectConfiguration = GetConfiguration(name, configurationSource);
			if (objectConfiguration == null)
			{
				throw new ConfigurationErrorsException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionNamedConfigurationNotFound,
						name,
						GetType().FullName));
			}

			TObject createdObject = Create(context, objectConfiguration, configurationSource, reflectionCache);

			return createdObject;
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Returns a new instance of a concrete subtype of <typeparamref name="TObject"/>, described by the matching configuration object 
		/// of a concrete subtype of <typeparamref name="TConfiguration"/> found in the <paramref name="configurationSource"/> under 
		/// the name <paramref name="name"/>.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="name">The name of the instance to build, or null.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A new instance of the appropriate subtype of <typeparamref name="Tobject"/>.</returns>
		/// <exception cref="ConfigurationErrorsException">when the configuration is invalid or <paramref name="name"/> cannot be found.</exception>
		public object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			return Create(context, name, configurationSource, reflectionCache);
		}

		/// <summary>
		/// Returns the configuration object that represents the named <typeparamref name="TObject"/> instance in the configuration source.
		/// </summary>
		/// <param name="name">The name of the required instance.</param>
		/// <param name="configurationSource">The configuration source where to look for the configuration object.</param>
		/// <returns>The configuration object that represents the instance.</returns>
		/// <exception cref="ConfigurationErrorsException">when the configuration is invalid or <paramref name="name"/> cannot be found.</exception>
		protected abstract TConfiguration GetConfiguration(string name, IConfigurationSource configurationSource);
	}
}

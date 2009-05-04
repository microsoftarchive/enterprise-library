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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
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
	public abstract class AssemblerBasedObjectFactory<TObject, TConfiguration>
		where TObject : class
		where TConfiguration : class
	{
		private IDictionary<Type, IAssembler<TObject, TConfiguration>> assemblersMapping 
			= new Dictionary<Type, IAssembler<TObject, TConfiguration>>();
		private object assemblersMappingLock = new object();
		ConfigurationReflectionCache reflectionCache = new ConfigurationReflectionCache();
		
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Returns a new instance of a concrete subtype of <typeparamref name="TObject"/>, described by the matching configuration object 
		/// of a concrete subtype of <typeparamref name="TConfiguration"/> in <paramref name="objectConfiguration"/>.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A new instance of the appropriate subtype of <typeparamref name="Tobject"/>.</returns>
		public virtual TObject Create(IBuilderContext context, TConfiguration objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
		{
			IAssembler<TObject, TConfiguration> assembler = GetAssembler(objectConfiguration);
			TObject createdObject = assembler.Assemble(context, objectConfiguration, configurationSource, reflectionCache);

			return createdObject;
		}

		private IAssembler<TObject, TConfiguration> GetAssembler(TConfiguration objectConfiguration)
		{
			bool exists = false;
			Type type = objectConfiguration.GetType();
			IAssembler<TObject, TConfiguration> assembler;
			lock (assemblersMappingLock)
			{
				exists = assemblersMapping.TryGetValue(type, out assembler);
			}
			if (exists)
			{
				return assembler;
			}

			AssemblerAttribute assemblerAttribute = GetAssemblerAttribute(type);

			if (!typeof(IAssembler<TObject, TConfiguration>).IsAssignableFrom(assemblerAttribute.AssemblerType))
			{
				throw new InvalidOperationException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionAssemblerTypeNotCompatible,
						objectConfiguration.GetType().FullName,
						typeof(IAssembler<TObject, TConfiguration>),
						assemblerAttribute.AssemblerType.FullName));
			}

			assembler = (IAssembler<TObject, TConfiguration>)Activator.CreateInstance(assemblerAttribute.AssemblerType);
			lock (assemblersMappingLock)
			{
				assemblersMapping[type] = assembler;
			}

			return assembler;
		}

		private AssemblerAttribute GetAssemblerAttribute(Type type)
		{
			AssemblerAttribute assemblerAttribute 
				= Attribute.GetCustomAttribute(type, typeof(AssemblerAttribute)) as AssemblerAttribute;
			if (assemblerAttribute == null)
			{ 
				throw new InvalidOperationException(
					string.Format(
						Resources.Culture,
						Resources.ExceptionAssemblerAttributeNotSet,
						type.FullName));
			}

			return assemblerAttribute;
		}
	}
}

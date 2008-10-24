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
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Represents the process to build an instance of a specific subtype of <typeparamref name="TObject"/> described by a 
	/// matching specific subtype of <typeparamref name="TConfiguration"/>.
	/// </summary>
	/// <typeparam name="TObject">The interface or the base type to build.</typeparam>
	/// <typeparam name="TConfiguration">The base configuration object type.</typeparam>
	/// <seealso cref="AssemblerBasedCustomFactory{TObject, TConfiguration}"/>.
	public interface IAssembler<TObject, TConfiguration>
		where TObject : class
		where TConfiguration : class
	{
		/// <summary>
		/// Builds an instance of the subtype of <typeparamref name="TObject"/> type the receiver knows how to build,  based on 
		/// an a configuration object.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>A fully initialized instance of the <typeparamref name="TObject"/> subtype.</returns>
		TObject Assemble(IBuilderContext context, TConfiguration objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache);
	}
}

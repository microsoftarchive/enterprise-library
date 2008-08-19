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
	/// Represents the process to build instances of a type.
	/// </summary>
	/// <remarks>
	/// Custom factories are used by the <see cref="ConfiguredObjectStrategy"/> strategy to create Enterprise Library objects.
	/// Usually factories will query a configuration source for the configuration objects that describe the requested object, 
	/// and will perform the necessary conversions on the configuration information to create an instance of the type.
	/// The objects the factory can build can be part of a hierarchy. Type <see cref="AssemblerBasedCustomFactory{TObject, TConfiguration}"/> provides 
	/// a generic implementation of a factory that builds polymorphic hierarchies.
	/// </remarks>
	/// <seealso cref="ConfiguredObjectStrategy"/>
	/// <seealso cref="CustomFactoryAttribute"/>
	/// <seealso cref="AssemblerBasedCustomFactory{TObject, TConfiguration}"/>
	public interface ICustomFactory
	{
		/// <summary>
		/// Returns an new instance of the type the receiver knows how to build.
		/// </summary>
		/// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
		/// <param name="name">The name of the instance to build, or null.</param>
		/// <param name="configurationSource">The source for configuration objects.</param>
		/// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
		/// <returns>The new instance.</returns>
		object CreateObject(IBuilderContext context, string name, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache);
	}
}

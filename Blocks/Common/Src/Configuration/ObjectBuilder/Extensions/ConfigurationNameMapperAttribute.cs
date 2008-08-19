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
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder
{
	/// <summary>
	/// Specifies what type to use to map instance names for the type this attribute is bound to. This class cannot be inherited.
	/// </summary>
	/// <remarks>
	/// This attribute is used when building objects through ObjectBuilder. The <see cref="ConfigurationNameMappingStrategy"/> will query 
	/// the requested type for this attribute, and will use an instance of the specified mapper type to map the instance name when necessary.
	/// The specified type must implement the <see cref="IConfigurationNameMapper"/> interface.
	/// The attribute needs to be bound only to the types that will be requested to ObjectBuilder that might need instance name mappings; 
	/// it is not necessary to bind the attribute to subclasses or implementations of interfaces.
	/// </remarks>
	/// <seealso cref="IConfigurationNameMapper"/>
	/// <seealso cref="ConfigurationNameMappingStrategy"/>
	/// <seealso cref="EnterpriseLibraryFactory"/>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public sealed class ConfigurationNameMapperAttribute : Attribute
	{
		/// <summary>
		/// The mapper type.
		/// </summary>
		public Type NameMappingObjectType;

		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationNameMapperAttribute"/> class with a mapper type.
		/// </summary>
		/// <param name="nameMappingObjectType">The mapper type. Must implement <see cref="IConfigurationNameMapper"/>.</param>
		public ConfigurationNameMapperAttribute(Type nameMappingObjectType)
		{
			if (nameMappingObjectType == null) 
				throw new ArgumentNullException("nameMappingObjectType");
			if (!typeof(IConfigurationNameMapper).IsAssignableFrom(nameMappingObjectType))
				throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionTypeNotNameMapper, nameMappingObjectType), "nameMappingObjectType");

			NameMappingObjectType = nameMappingObjectType;
		}
	}
}

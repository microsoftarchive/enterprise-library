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
	/// Specifies what type to use to build instances of the type this attribute is bound to. This class cannot be inherited.
	/// </summary>
	/// <remarks>
	/// This attribute is used when building objects through ObjectBuilder. The <see cref="ConfiguredObjectStrategy"/> will query 
	/// the requested type for this attribute, and will use an instance of the specified factory type to build the requested instance.
	/// The specified type must implement the <see cref="ICustomFactory"/> interface.
	/// The attribute needs to be bound only to the types that will be requested to ObjectBuilder; 
	/// it is not necessary to bind the attribute to subclasses or implementations of interfaces.
	/// </remarks>
	/// <seealso cref="ICustomFactory"/>
	/// <seealso cref="ConfiguredObjectStrategy"/>
	/// <seealso cref="EnterpriseLibraryFactory"/>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = false, Inherited = false)]
	public sealed class CustomFactoryAttribute : Attribute
	{
		private Type factoryType;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomFactoryAttribute"/> class with a factory type.
		/// </summary>
		/// <param name="factoryType">The factory type. Must implement <see cref="ICustomFactory"/>.</param>
		public CustomFactoryAttribute(Type factoryType)
		{
			if (factoryType == null)
				throw new ArgumentNullException("factoryType");
			if (!typeof(ICustomFactory).IsAssignableFrom(factoryType))
				throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionTypeNotCustomFactory, factoryType), "factoryType");
			this.factoryType = factoryType;
		}

		/// <summary>
		/// Gets the factory type.
		/// </summary>
		public Type FactoryType
		{
			get { return factoryType; }
		}
	}
}

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
	/// Specifies what assembler type to use to build a concrete type in a polymorphic provider hierarchy for the type described by 
	/// the configuration object this attribute is bound to. This class cannot be inherited.
	/// The specified type must implement the <see cref="IAssembler{TObject, TConfiguration}"/> interface.
	/// </summary>
	/// <remarks>
	/// The attribute is used on configuration objects to specify the build process create a provider based on the information 
	/// in the configuration object.
	/// The attribute is used by the <see cref="AssemblerBasedCustomFactory{TObject, TConfiguration}"/> implementation of the <see cref="ICustomFactory"/> interface,
	/// that provides a generic way to build polymorphic providers.
	/// </remarks>
	[AttributeUsage(AttributeTargets.Class, AllowMultiple=false, Inherited=false)]
	public sealed class AssemblerAttribute : Attribute
	{
		private Type assemblerType;

		/// <summary>
		/// Initializes a new instance of the <see cref="AssemblerAttribute"/> class.
		/// </summary>
		/// <param name="assemblerType">The type that implements the <see cref="IAssembler{TObject, TConfiguration}"/> interface.</param>
		public AssemblerAttribute(Type assemblerType)
		{
			if (assemblerType == null)
				throw new ArgumentNullException("assemblerType");
			// can't test for compatibility since IAssembler is generic

			this.assemblerType = assemblerType;
		}

		/// <summary>
		/// Returns the assembler type.
		/// </summary>
		public Type AssemblerType
		{
			get { return assemblerType; }
		}
	}
}

//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="EnumConversionValidatorData"/>.
	/// </summary>
	public class EnumConversionValidatorData : ValueValidatorData
	{
		private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

		private const string EnumTypePropertyName = "enumType";
		
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="EnumConversionValidatorData"/> class.</para>
		/// </summary>
		public EnumConversionValidatorData()
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="EnumConversionValidatorData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public EnumConversionValidatorData(string name)
			: base(name, typeof(EnumConversionValidator))
		{ }

		/// <summary>
		/// Gets or sets the enum element type.
		/// </summary>
		public Type EnumType
		{
			get { return (Type)typeConverter.ConvertFrom(EnumTypeName); }
			set { EnumTypeName = typeConverter.ConvertToString(value); }
		}

		private const string EnumTypeNamePropertyName = "enumType";
		/// <summary>
		/// Gets or sets the name of the target element type for the represented validator.
		/// </summary>
		[ConfigurationProperty(EnumTypeNamePropertyName, IsRequired=true)]
		public string EnumTypeName
		{
			get { return (string)this[EnumTypeNamePropertyName]; }
			set { this[EnumTypeNamePropertyName] = value; }
		}

		/// <summary>
		/// Creates the <see cref="EnumConversionValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="EnumConversionValidator"/>.</returns>	
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new EnumConversionValidator(EnumType, Negated);
		}
	}
}

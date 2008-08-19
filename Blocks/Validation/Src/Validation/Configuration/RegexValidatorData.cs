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
using System.Collections.Generic;
using System.Configuration;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="RegexValidator"/>.
	/// </summary>
	/// <seealso cref="AndCompositeValidator"/>
	/// <seealso cref="RegexValidator"/>
	public class RegexValidatorData : ValueValidatorData
	{
		private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RegexValidatorData"/> class.</para>
		/// </summary>
		public RegexValidatorData()
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="RegexValidatorData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public RegexValidatorData(string name)
			: base(name, typeof(RegexValidator))
		{ }

		private const string PatternPropertyName = "pattern";
		/// <summary>
		/// Gets or sets the pattern for the represented validator.
		/// </summary>
		/// <seealso cref="RegexValidator.Pattern"/>
		[ConfigurationProperty(PatternPropertyName)]
		public string Pattern
		{
			get { return (string)this[PatternPropertyName]; }
			set { this[PatternPropertyName] = value; }
		}

		private const string OptionsPropertyName = "options";
		/// <summary>
		/// Gets or sets the regex options for the represented validator.
		/// </summary>
		/// <seealso cref="RegexOptions"/>
		/// <seealso cref="RegexValidator.Options"/>
		[ConfigurationProperty(OptionsPropertyName)]
		public RegexOptions Options
		{
			get { return (RegexOptions)this[OptionsPropertyName]; }
			set { this[OptionsPropertyName] = value; }
		}

		private const string PatternResourceNamePropertyName = "patternResourceName";
		/// <summary>
		/// Gets or sets the name of the resource holding the regex pattern.
		/// </summary>
		[ConfigurationProperty(PatternResourceNamePropertyName)]
		public string PatternResourceName
		{
			get { return (string)this[PatternResourceNamePropertyName]; }
			set { this[PatternResourceNamePropertyName] = value; }
		}

		private const string PatternResourceTypePropertyName = "patternResourceType";
		/// <summary>
		/// Gets or sets the name of the resource type holding the regex pattern.
		/// </summary>
		[ConfigurationProperty(PatternResourceTypePropertyName)]
		public string PatternResourceTypeName
		{
			get { return (string)this[PatternResourceTypePropertyName]; }
			set { this[PatternResourceTypePropertyName] = value; }
		}

		/// <summary>
		/// Gets or sets the enum element type.
		/// </summary>
		public Type PatternResourceType
		{
			get { return (Type)typeConverter.ConvertFrom(PatternResourceTypeName); }
			set { PatternResourceTypeName = typeConverter.ConvertToString(value); }
		}

		/// <summary>
		/// Creates the <see cref="RegexValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="RegexValidator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new RegexValidator(this.Pattern, this.PatternResourceName, this.PatternResourceType, this.Options, MessageTemplate, Negated);
		}
	}
}
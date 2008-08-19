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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Represents a ruleset for a validated type.
	/// </summary>
	/// <seealso cref="ValidatedTypeReference"/>
	/// <remarks>
	/// Self validation is not supported thorugh configuration.
	/// </remarks>
	/// <seealso cref="ValidatedTypeReference"/>
	public class ValidationRulesetData : NamedConfigurationElement
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ValidationRulesetData"/> class.</para>
		/// </summary>
		public ValidationRulesetData()
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ValidationRulesetData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public ValidationRulesetData(string name)
			: base(name)
		{ }

		private const string validatorsPropertyName = "";
		/// <summary>
		/// Gets the collection of validators configured for the type owning the ruleset.
		/// </summary>
		[ConfigurationProperty(validatorsPropertyName, IsDefaultCollection = true)]
		public ValidatorDataCollection Validators
		{
			get { return (ValidatorDataCollection)this[validatorsPropertyName]; }
		}

		private const string FieldsPropertyName = "fields";
		/// <summary>
		/// Gets the collection of validated fields for the type owning the ruleset.
		/// </summary>
		[ConfigurationProperty(FieldsPropertyName)]
		public ValidatedFieldReferenceCollection Fields
		{
			get { return (ValidatedFieldReferenceCollection)this[FieldsPropertyName]; }
		}

		private const string MethodsPropertyName = "methods";
		/// <summary>
		/// Gets the collection of validated methods for the type owning the ruleset.
		/// </summary>
		[ConfigurationProperty(MethodsPropertyName)]
		public ValidatedMethodReferenceCollection Methods
		{
			get { return (ValidatedMethodReferenceCollection)this[MethodsPropertyName]; }
		}

		private const string PropertiesPropertyName = "properties";
		/// <summary>
		/// Gets the collection of validated properties for the type owning the ruleset.
		/// </summary>
		[ConfigurationProperty(PropertiesPropertyName)]
		public new ValidatedPropertyReferenceCollection Properties
		{
			get { return (ValidatedPropertyReferenceCollection)this[PropertiesPropertyName]; }
		}
	}
}
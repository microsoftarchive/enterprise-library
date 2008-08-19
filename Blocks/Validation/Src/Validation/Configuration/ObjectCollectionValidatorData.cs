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
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="ObjectCollectionValidator"/>.
	/// </summary>
	public class ObjectCollectionValidatorData : ValidatorData
	{
		private static readonly AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectCollectionValidatorData"/> class.</para>
		/// </summary>
		public ObjectCollectionValidatorData()
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectCollectionValidatorData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public ObjectCollectionValidatorData(string name)
			: base(name, typeof(ObjectCollectionValidator))
		{ }

		/// <summary>
		/// Creates the <see cref="ObjectCollectionValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="ObjectCollectionValidator"/>.</returns>
		/// <exception cref="ConfigurationErrorsException">when the receiver's target type is <see langword="null"/>.</exception>
		protected override Validator DoCreateValidator(Type targetType)
		{
			if (this.TargetType == null)
			{
				throw new ConfigurationErrorsException(Resources.ExceptionObjectCollectionValidatorDataTargetTypeNotSet);
			}

			return new ObjectCollectionValidator(this.TargetType, this.TargetRuleset);
		}

		/// <summary>
		/// Gets or sets the target element type.
		/// </summary>
		/// <value>
		/// The target element type.
		/// </value>
		public Type TargetType
		{
			get { return (Type)typeConverter.ConvertFrom(TargetTypeName); }
			set { TargetTypeName = typeConverter.ConvertToString(value); }
		}

		private const string TargetTypePropertyName = "targetType";
		/// <summary>
		/// Gets or sets the name of the target element type for the represented validator.
		/// </summary>
		/// <seealso cref="ObjectCollectionValidatorData.TargetTypeName"/>
		[ConfigurationProperty(TargetTypePropertyName)]
		public string TargetTypeName
		{
			get { return (string)this[TargetTypePropertyName]; }
			set { this[TargetTypePropertyName] = value; }
		}

		private const string TargetRulesetPropertyName = "targetRuleset";
		/// <summary>
		/// Gets or sets the name for the target ruleset for the represented validator.
		/// </summary>
		[ConfigurationProperty(TargetRulesetPropertyName)]
		public string TargetRuleset
		{
			get { return (string)this[TargetRulesetPropertyName]; }
			set { this[TargetRulesetPropertyName] = value; }
		}
	}
}
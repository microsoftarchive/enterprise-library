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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="ObjectValidator"/>.
	/// </summary>
	public class ObjectValidatorData : ValidatorData
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectValidatorData"/> class.</para>
		/// </summary>
		public ObjectValidatorData()	
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ObjectValidatorData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public ObjectValidatorData(string name)
			: base(name, typeof(ObjectValidator))
		{ }

		/// <summary>
		/// Creates the <see cref="ObjectValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <returns>The created <see cref="ObjectValidator"/>.</returns>
		protected override Validator DoCreateValidator(Type targetType)
		{
			return new ObjectValidator(targetType, this.TargetRuleset);
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
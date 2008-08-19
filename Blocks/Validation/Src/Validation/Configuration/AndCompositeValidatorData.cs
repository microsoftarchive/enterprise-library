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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Configuration object to describe an instance of class <see cref="AndCompositeValidator"/>.
	/// </summary>
	/// <seealso cref="AndCompositeValidator"/>
	/// <seealso cref="ValidatorData"/>
	public class AndCompositeValidatorData : ValidatorData
	{
		/// <summary>
		/// <para>Initializes a new instance of the <see cref="AndCompositeValidatorData"/> class.</para>
		/// </summary>
		public AndCompositeValidatorData()
		{ }

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="AndCompositeValidatorData"/> class with a name.</para>
		/// </summary>
		/// <param name="name">The name for the instance.</param>
		public AndCompositeValidatorData(string name)
			: base(name, typeof(AndCompositeValidator))
		{ }

		private const string ValidatorsPropertyName = "";
		/// <summary>
		/// Gets the collection with the definitions for the validators composed by 
		/// the represented <see cref="AndCompositeValidator"/>.
		/// </summary>
		[ConfigurationProperty(ValidatorsPropertyName, IsDefaultCollection = true)]
		public ValidatorDataCollection Validators
		{
			get { return (ValidatorDataCollection)this[ValidatorsPropertyName]; }
		}

		/// <summary>
		/// Creates the <see cref="AndCompositeValidator"/> described by the configuration object.
		/// </summary>
		/// <param name="targetType">The type of object that will be validated by the validator.</param>
		/// <param name="ownerType">The type of the object from which the value to validate is extracted.</param>
		/// <param name="memberValueAccessBuilder">The <see cref="MemberValueAccessBuilder"/> to use for validators that
		/// require access to properties.</param>
		/// <returns>The created <see cref="AndCompositeValidator"/>.</returns>
		/// <seealso cref="AndCompositeValidator"/>
		protected override Validator DoCreateValidator(Type targetType, Type ownerType, MemberValueAccessBuilder memberValueAccessBuilder)
		{
			List<Validator> childValidators = new List<Validator>(this.Validators.Count);
			foreach (IValidatorDescriptor validatorData in this.Validators)
			{
				childValidators.Add(validatorData.CreateValidator(targetType, ownerType, memberValueAccessBuilder));
			}

			return new AndCompositeValidator(childValidators.ToArray());
		}
	}
}
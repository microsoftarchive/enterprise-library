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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Windows.Markup;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration
{
	/// <summary>
	/// Represents the validation information for a member of a type.
	/// </summary>
	/// <seealso cref="ValidationRulesetData"/>
	/// <seealso cref="ValidatedMemberReference"/>
    [ContentProperty("Validators")]
    public abstract class ValidatedMemberReference : NamedConfigurationElement
	{
        private NamedElementCollection<ValidatorData> validators = new NamedElementCollection<ValidatorData>();
		/// <summary>
		/// Gets the collection of validators configured for the member.
		/// </summary>
		public NamedElementCollection<ValidatorData> Validators
		{
            get { return this.validators; }
		}
	}
}

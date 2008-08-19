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
using System.ComponentModel;
using System.Drawing.Design;
using System.ComponentModel.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Represents the designtime configuration node for any value validator.
	/// </summary>
	public abstract class ValueValidatorNode : SingleValidatorNodeBase
	{
		private bool negated;

		/// <summary>
		/// Initializes a new instance of the <see cref="ValueValidatorNode"/> representing <paramref name="validatorData"/>.
		/// </summary>
		/// <param name="validatorData">The represented <see cref="ValueValidatorData"/>.</param>
		protected ValueValidatorNode(ValueValidatorData validatorData)
			: base(validatorData)
		{
			this.negated = validatorData.Negated;
		}

		/// <summary>
		/// Gets or sets the flag indicating the represented validator is negated.
		/// </summary>
		[SRDescription("NegatedDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[Required]
		public bool Negated
		{
			get { return negated; }
			set { negated = value; }
		}

		/// <summary>
		/// Copies properties declared on this node to a runtime configuration data.
		/// </summary>
		/// <param name="validatorData">The runtime configuration data which should be updated with settings in this node.</param>
		protected void SetValueValidatorBaseProperties(ValueValidatorData validatorData)
		{
			validatorData.Negated = negated;
		}
	}
}

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
	/// Respresents the designtime configuration node for an <see cref="TypeConversionValidatorData"/>.
	/// </summary>
	public class TypeConversionValidatorNode : ValueValidatorNode
	{
		private string targetTypeName;

		/// <summary>
		/// Creates an instance of <see cref="TypeConversionValidatorNode"/> based on default values.
		/// </summary>
		public TypeConversionValidatorNode()
			: this(new TypeConversionValidatorData(Resources.TypeConversionValidatorNodeName))
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="TypeConversionValidatorNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="validatorData">The corresponding runtime configuration data.</param>
		public TypeConversionValidatorNode(TypeConversionValidatorData validatorData)
			: base(validatorData)
		{
			this.targetTypeName = validatorData.TargetTypeName;
		}

		/// <summary>
		/// Gets or sets the fully qualified assembly name of the target element type.
		/// </summary>
		/// <value>
		/// The fully qualified assembly name of the target element type.
		/// </value>
		[Required]
		[Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
		[BaseType(typeof(object))]
		[SRDescription("TargetTypeConversionDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string TargetType
		{
			get { return targetTypeName; }
			set { targetTypeName = value; }
		}
	
		/// <summary>
		/// Returns the runtime configuration data that is represented by this node.
		/// </summary>
		/// <returns>An instance of <see cref="TypeConversionValidatorData"/> that can be persisted to a configuration file.</returns>
		public override ValidatorData CreateValidatorData()
		{
			TypeConversionValidatorData validatorData = new TypeConversionValidatorData(Name);
			SetValidatorBaseProperties(validatorData);
			SetValueValidatorBaseProperties(validatorData);

			validatorData.TargetTypeName = targetTypeName;

			return validatorData;
		}
	}
}

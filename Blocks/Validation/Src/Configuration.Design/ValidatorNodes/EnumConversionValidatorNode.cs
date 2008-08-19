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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Respresents the designtime configuration node for an <see cref="EnumConversionValidatorData"/>.
	/// </summary>
	public class EnumConversionValidatorNode : ValueValidatorNode
	{
		private string enumTypeName;
		
		/// <summary>
		/// Creates an instance of <see cref="EnumConversionValidatorNode"/> based on default values.
		/// </summary>
		public EnumConversionValidatorNode()
			: this(new EnumConversionValidatorData(Resources.EnumConversionValidatorNodeName))
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="EnumConversionValidatorNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="validatorData">The corresponding runtime configuration data.</param>
		public EnumConversionValidatorNode(EnumConversionValidatorData validatorData)
			: base(validatorData)
		{
			enumTypeName = validatorData.EnumTypeName;
		}

		/// <summary>
		/// Gets or sets the fully qualified assembly name of the target element type.
		/// </summary>
		/// <value>
		/// The fully qualified assembly name of the target element type.
		/// </value>
		[Required]
		[Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
		[BaseType(typeof(Enum))]
		[SRDescription("EnumTypeDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string EnumType
		{
			get { return enumTypeName; }
			set { enumTypeName = value; }
		}
		
		/// <summary>
		/// Returns the runtime configuration data that is represented by this node.
		/// </summary>
		/// <returns>An instance of <see cref="EnumConversionValidatorData"/> that can be persisted to a configuration file.</returns>
		public override ValidatorData CreateValidatorData()
		{
			EnumConversionValidatorData validatorData = new EnumConversionValidatorData(Name);
			SetValidatorBaseProperties(validatorData);
			SetValueValidatorBaseProperties(validatorData);

			validatorData.EnumTypeName = enumTypeName;

			return validatorData;
		}
	}
}

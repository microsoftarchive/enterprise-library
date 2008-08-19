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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Represents a <see cref="PropertyComparisonValidatorData"/>.
	/// </summary>
	public class PropertyComparisonValidatorNode : ValueValidatorNode
	{
		private string propertyToCompare;
		private ComparisonOperator comparisonOperator;

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyComparisonValidatorNode"/> class with default values.
		/// </summary>
		public PropertyComparisonValidatorNode()
			: this(new PropertyComparisonValidatorData(Resources.PropertyComparisonValidatorNodeName))
		{ }

		/// <summary>
		/// Initializes a new instance of the <see cref="PropertyComparisonValidatorNode"/> class representing <paramref name="validatorData"/>.
		/// </summary>
		/// <param name="validatorData">The represented <see cref="PropertyComparisonValidatorData"/>.</param>
		public PropertyComparisonValidatorNode(PropertyComparisonValidatorData validatorData)
			: base(validatorData)
		{
			this.comparisonOperator = validatorData.ComparisonOperator;
			this.propertyToCompare = validatorData.PropertyToCompare;
		}

		/// <summary>
		/// Creates the <see cref="PropertyComparisonValidatorData"/> represented by the node.
		/// </summary>
		/// <returns>A <see cref="PropertyComparisonValidatorData"/> instance.</returns>
		public override ValidatorData CreateValidatorData()
		{
			PropertyComparisonValidatorData validatorData = new PropertyComparisonValidatorData(this.Name);
            SetValidatorBaseProperties(validatorData);

			validatorData.Negated = this.Negated;
			validatorData.ComparisonOperator = this.comparisonOperator;
			validatorData.PropertyToCompare = this.propertyToCompare;

			return validatorData;
		}

		/// <summary>
		/// Gets or sets the name of the property to compare.
		/// </summary>
		[SRDescription("PropertyComparisonPropertyToCompareDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[Required]
		public string PropertyToCompare
		{
			get { return propertyToCompare; }
			set { propertyToCompare = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="ComparisonOperator"/>.
		/// </summary>
		[SRDescription("PropertyComparisonComparisonOperatorDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public ComparisonOperator ComparisonOperator
		{
			get { return comparisonOperator; }
			set { comparisonOperator = value; }
		}
	}
}
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
	/// Respresents the designtime configuration node for an <see cref="RelativeDateTimeValidatorData"/>.
	/// </summary>
	public class RelativeDateTimeValidatorNode : RangeValidatorNodeBase<int>
	{
		private DateTimeUnit lowerUnit;
		private DateTimeUnit upperUnit;

		/// <summary>
		/// Creates an instance of <see cref="RelativeDateTimeValidatorNode"/> based on default values.
		/// </summary>
		public RelativeDateTimeValidatorNode()
			: this(new RelativeDateTimeValidatorData(Resources.RelativeDateTimeValidatorNodeName))
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="RelativeDateTimeValidatorNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="validatorData">The corresponding runtime configuration data.</param>
		public RelativeDateTimeValidatorNode(RelativeDateTimeValidatorData validatorData)
			: base(validatorData)
		{
			this.lowerUnit = validatorData.LowerUnit;
			this.upperUnit = validatorData.UpperUnit;
		}

		/// <summary>
		/// Gets or sets the <see cref="DateTimeUnit"/> for the lower unit.
		/// </summary>
		[Required]
		[SRDescription("DataTimeLowerUnitDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public DateTimeUnit LowerUnit
		{
			get { return lowerUnit; }
			set { lowerUnit = value; }
		}

		/// <summary>
		/// Gets or sets the <see cref="DateTimeUnit"/> for the upper unit.
		/// </summary>
		[Required]
		[SRDescription("DataTimeUpperUnitDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public DateTimeUnit UpperUnit
		{
			get { return upperUnit; }
			set { upperUnit = value; }
		}

		/// <summary>
		/// Returns the runtime configuration data that is represented by this node.
		/// </summary>
		/// <returns>An instance of <see cref="RelativeDateTimeValidatorData"/> that can be persisted to a configuration file.</returns>
		public override ValidatorData CreateValidatorData()
		{
			RelativeDateTimeValidatorData validatorData = new RelativeDateTimeValidatorData(Name);
			SetRangeValidatorBaseProperties(validatorData);
			SetValueValidatorBaseProperties(validatorData);

			validatorData.LowerUnit = this.lowerUnit;
			validatorData.UpperUnit = this.upperUnit;

			return validatorData;
		}
	}
}

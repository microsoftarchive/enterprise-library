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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Respresents the designtime configuration node for a <see cref="ObjectValidatorData"/>.
	/// </summary>
	public class ObjectValidatorNode : SingleValidatorNodeBase
	{
		private string targetRuleset;

		/// <summary>
		/// Creates an instance of <see cref="ObjectValidatorNode"/> based on default values.
		/// </summary>
		public ObjectValidatorNode()
			: this(new ObjectValidatorData(Resources.ObjectValidatorNodeName))
		{ }

		/// <summary>
		/// Creates an instance of <see cref="ObjectValidatorNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="validatorData">The corresponding runtime configuration data.</param>
		public ObjectValidatorNode(ObjectValidatorData validatorData)
			: base(validatorData)
		{
			this.targetRuleset = validatorData.TargetRuleset;
		}

		/// <summary>
		/// Returns the runtime configuration data that is represented by this node.
		/// </summary>
		/// <returns>An instance of <see cref="ObjectValidatorData"/> that can be persisted to a configuration file.</returns>
		public override ValidatorData CreateValidatorData()
		{
			ObjectValidatorData validatorData = new ObjectValidatorData(this.Name);
			validatorData.TargetRuleset = this.targetRuleset;

			return validatorData;
		}

		/// <summary>
		/// Gets or sets the target ruleset for the node.
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("TargetRulesetDescription", typeof(Resources))]
		public string TargetRuleset
		{
			get { return targetRuleset; }
			set { targetRuleset = value; }
		}
	}
}
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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
	/// <summary>
	/// Respresents the designtime configuration node for a <see cref="ObjectCollectionValidatorData"/>.
	/// </summary>
	public class ObjectCollectionValidatorNode : SingleValidatorNodeBase
	{
		private string targetTypeName;
		private string targetRuleset;

		/// <summary>
		/// Creates an instance of <see cref="ObjectCollectionValidatorNode"/> based on default values.
		/// </summary>
		public ObjectCollectionValidatorNode()
			: this(new ObjectCollectionValidatorData(Resources.ObjectCollectionValidatorNodeName))
		{ }

		/// <summary>
		/// Creates an instance of <see cref="ObjectCollectionValidatorNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="validatorData">The corresponding runtime configuration data.</param>
		public ObjectCollectionValidatorNode(ObjectCollectionValidatorData validatorData)
			: base(validatorData)
		{
			this.targetTypeName = validatorData.TargetTypeName;
			this.targetRuleset = validatorData.TargetRuleset;
		}

		/// <summary>
		/// Returns the runtime configuration data that is represented by this node.
		/// </summary>
		/// <returns>An instance of <see cref="ObjectCollectionValidatorData"/> that can be persisted to a configuration file.</returns>
		public override ValidatorData CreateValidatorData()
		{
			ObjectCollectionValidatorData validatorData = new ObjectCollectionValidatorData(this.Name);
			validatorData.TargetTypeName = this.targetTypeName;
			validatorData.TargetRuleset = this.targetRuleset;

			return validatorData;
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
		[SRDescription("TargetTypeDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string TargetType
		{
			get { return targetTypeName; }
			set { targetTypeName = value; }
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
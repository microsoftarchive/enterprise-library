//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Drawing.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Represents a <see cref="CustomMatchingRuleData"/>.
	/// </summary>
	public class CustomMatchingRuleNode : MatchingRuleNode
	{
		string typeName;
		private List<EditableKeyValue> editableAttributes = new List<EditableKeyValue>();

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomMatchingRuleNode"/> class with default values.
		/// </summary>
		public CustomMatchingRuleNode()
			: this(new CustomMatchingRuleData(Resources.CustomMatchingRuleNodeName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomMatchingRuleNode"/> class for representing a <see cref="CustomMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="ruleData">The <see cref="CustomMatchingRuleData"/> to represent.</param>
		public CustomMatchingRuleNode(CustomMatchingRuleData ruleData)
			: base(ruleData)
		{
			typeName = ruleData.TypeName;
			foreach (string key in ruleData.Attributes)
			{
				editableAttributes.Add(new EditableKeyValue(key, ruleData.Attributes[key]));
			}
		}

		/// <summary>
		/// Gets or sets the name of the type of the custom rule for the represented configuration object.
		/// </summary>
		[Required]
		[Editor(typeof(TypeSelectorEditor), typeof(UITypeEditor))]
		[BaseType(typeof(IMatchingRule), typeof(CustomMatchingRuleData))]
		[SRDescription("RuleProviderTypeNameDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Type
		{
			get { return typeName; }
			set { typeName = value; }
		}

		/// <summary>
		/// Gets the list of custom attributes for the represented configuration object.
		/// </summary>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("RuleProviderExtensionsDescription", typeof(Resources))]
		[CustomAttributesValidation]
		public List<EditableKeyValue> Attributes
		{
			get
			{
				return editableAttributes;
			}
		}

		/// <summary>
		/// Returs the represented <see cref="CustomMatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="CustomMatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			CustomMatchingRuleData ruleData = new CustomMatchingRuleData(Name, typeName);
			foreach (EditableKeyValue kv in editableAttributes)
			{
				ruleData.Attributes.Add(kv.Key, kv.Value);
			}
			return ruleData;

		}
	}
}

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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.ComponentModel;
using System.Drawing.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Represents a <see cref="CustomAttributeMatchingRuleData"/> instance.
	/// </summary>
	public class CustomAttributeMatchingRuleNode : MatchingRuleNode
	{
		private bool searchInheritanceChain;
		private string attributeType;

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomAttributeMatchingRuleNode"/> class with default values.
		/// </summary>
		public CustomAttributeMatchingRuleNode()
			: this(new CustomAttributeMatchingRuleData(Resources.CustomAttributeMatchingRuleNodeName))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="CustomAttributeMatchingRuleNode"/> class for representing a <see cref="CustomAttributeMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="ruleData">The <see cref="CustomAttributeMatchingRuleData"/> to represent.</param>
		public CustomAttributeMatchingRuleNode(CustomAttributeMatchingRuleData ruleData)
			: base(ruleData)
		{
			searchInheritanceChain = ruleData.SearchInheritanceChain;
			attributeType = ruleData.AttributeTypeName;
		}

		/// <summary>
		/// Gets or sets the indication of inheritance chain search for the represented configuration object.
		/// </summary>
		[SRDescription("SearchInheritanceChainDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public bool SearchInheritanceChain
		{
			get { return searchInheritanceChain; }
			set { searchInheritanceChain = value; }
		}

		/// <summary>
		/// Gets or sets the attribute type for the represented configuration object.
		/// </summary>
		[Required]
		[SRDescription("AttributeTypeDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string AttributeType
		{
			get { return attributeType; }
			set { attributeType = value; }
		}

		/// <summary>
		/// Returs the represented <see cref="MatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="MatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			return new CustomAttributeMatchingRuleData(Name, attributeType, searchInheritanceChain);
		}
	}
}

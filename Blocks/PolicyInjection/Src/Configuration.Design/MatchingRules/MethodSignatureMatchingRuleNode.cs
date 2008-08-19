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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Represents a <see cref="MethodSignatureMatchingRuleData"/> instance.
	/// </summary>
	public class MethodSignatureMatchingRuleNode : MatchingRuleNode
	{
		List<ParameterType> parameterTypes;
		bool ignoreCase;
		string match;

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodSignatureMatchingRuleNode"/> class with default values.
		/// </summary>
		public MethodSignatureMatchingRuleNode()
			: this(new MethodSignatureMatchingRuleData(Resources.MethodSignatureMatchingRuleNodeName, Resources.DefaultMethodSignatureMatchValue))
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="MethodSignatureMatchingRuleNode"/> class for representing a <see cref="MethodSignatureMatchingRuleData"/> instance.
		/// </summary>
		/// <param name="ruleData">The <see cref="MethodSignatureMatchingRuleData"/> to represent.</param>
		public MethodSignatureMatchingRuleNode(MethodSignatureMatchingRuleData ruleData)
			: base(ruleData)
		{
			ignoreCase = ruleData.IgnoreCase;
			match = ruleData.Match;

			parameterTypes = new List<ParameterType>();
			foreach (ParameterTypeElement parameterType in ruleData.Parameters)
			{
				parameterTypes.Add(new ParameterType(parameterType.Name, parameterType.ParameterTypeName));
			}
		}

		/// <summary>
		/// Gets or sets the indication of whether case should be ignored when matching for the represented configuration object.
		/// </summary>
		[SRDescription("MethodSignatureIgnoreCaseDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public bool IgnoreCase
		{
			get { return ignoreCase; }
			set { ignoreCase = value; }
		}

		/// <summary>
		/// Gets or sets the match for the represented configuration object.
		/// </summary>
		[Required]
		[SRDescription("MethodSignatureMatchDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public string Match
		{
			get { return match; }
			set { match = value; }
		}

		/// <summary>
		/// Gets the list of parameter types for the represented configuration object.
		/// </summary>
		[ParameterTypesValidation]
		[SRDescription("ParameterTypesDescription", typeof(Resources))]
		[SRCategory("CategoryGeneral", typeof(Resources))]
		public List<ParameterType> ParameterTypes
		{
			get { return parameterTypes; }
		}

		/// <summary>
		/// Returs the represented <see cref="MethodSignatureMatchingRuleData"/> instance.
		/// </summary>
		/// <returns>A newly created <see cref="MethodSignatureMatchingRuleData"/> instance.</returns>
		public override MatchingRuleData GetConfigurationData()
		{
			MethodSignatureMatchingRuleData ruleData = new MethodSignatureMatchingRuleData(Name, Match);
			ruleData.IgnoreCase = IgnoreCase;

			foreach (ParameterType parameterType in ParameterTypes)
			{
				ruleData.Parameters.Add(new ParameterTypeElement(parameterType.Name, parameterType.Type));
			}
			return ruleData;
		}
	}
}

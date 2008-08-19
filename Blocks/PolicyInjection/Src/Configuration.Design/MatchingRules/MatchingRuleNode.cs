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

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Base class for nodes representing concrete <see cref="MatchingRuleData"/> instances.
	/// </summary>
	[Image(typeof(MatchingRuleNode))]
	[SelectedImage(typeof(MatchingRuleNode))]
	public abstract class MatchingRuleNode : ConfigurationNode
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="MatchingRuleNode"/> class representing a <see cref="MatchingRuleData"/>.
		/// </summary>
		/// <param name="matchingRuleData">The <see cref="MatchingRuleData"/> to represent.</param>
		protected MatchingRuleNode(MatchingRuleData matchingRuleData)
			: base(matchingRuleData.Name)
		{
		}

		/// <summary>
		/// Returns a <see cref="MatchingRuleData"/> configuration object from the nodes data.
		/// </summary>
		/// <returns>
		/// A newly created <see cref="MatchingRuleData"/> configuration object from the nodes data.
		/// </returns>
		/// <remarks>
		/// Subclasses implement this method to return an instance of a concrete <see cref="MatchingRuleData"/> subclass.
		/// </remarks>
		public abstract MatchingRuleData GetConfigurationData();
	}
}
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

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
	/// <summary>
	/// Represents a <see cref="PolicyData"/> configuration object.
	/// </summary>
	[Image(typeof(PolicyNode))]
	[SelectedImage(typeof(PolicyNode))]
	public class PolicyNode : ConfigurationNode
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="PolicyNode"/> class representing a <see cref="PolicyData"/> instance.
		/// </summary>
		/// <param name="policy">The <see cref="PolicyData"/> to represent.</param>
		public PolicyNode(PolicyData policy)
			: base(policy.Name)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="PolicyNode"/> class with default data.
		/// </summary>
		public PolicyNode()
			: base(Resources.PolicyNodeName)
		{
		}

		/// <summary>
		/// Gets if children added to the node are sorted.
		/// </summary>
		/// <value><see langword="false"/> as nodes in the policy should not be sorted.</value>
		public override bool SortChildren
		{
			get
			{
				return false;
			}
		}
	}
}
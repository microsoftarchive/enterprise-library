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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	/// <summary>
	/// Represents a <see cref="ValidatedMethodReference"/>.
	/// </summary>
	[Image(typeof(MethodNode))]
	[SelectedImage(typeof(MethodNode))]
	public class MethodNode : ConfigurationNode
	{
		/// <summary>
		/// Creates an instance of <see cref="MethodNode"/> based on default values.
		/// </summary>
		public MethodNode()
			: this(Resources.MethodNodeName)
		{ }

		/// <summary>
		/// Creates an instance of <see cref="MethodNode"/> based on a method name.
		/// </summary>
		/// <param name="methodName">The name of the method represented by this node.</param>
		public MethodNode(string methodName)
			: base(methodName)
		{ }

		/// <summary>
		/// Creates an instance of <see cref="MethodNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="methodData">The corresponding runtime configuration data.</param>
		public MethodNode(ValidatedMethodReference methodData)
			: this(methodData.Name)
		{ }

		/// <summary>
		/// Determines if the the child nodes are sorted by name.
		/// </summary>
		/// <value>
		/// Returns <see langword="false"/>.
		/// </value>
		/// <remarks>
		/// Child nodes must be in order that they are added because they are handled in a chain.
		/// </remarks>
		public override bool SortChildren
		{
			get
			{
				return false;
			}
		}
	}
}

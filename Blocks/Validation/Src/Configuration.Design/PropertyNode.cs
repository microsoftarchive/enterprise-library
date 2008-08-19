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
	/// Represents a <see cref="ValidatedPropertyReference"/>.
	/// </summary>
	[Image(typeof(PropertyNode))]
	[SelectedImage(typeof(PropertyNode))]
	public class PropertyNode : ConfigurationNode
	{
		/// <summary>
		/// Creates an instance of <see cref="PropertyNode"/> based on default values.
		/// </summary>
		public PropertyNode()
			: this(Resources.PropertyNodeName)
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="PropertyNode"/> based on a property name.
		/// </summary>
		/// <param name="propertyName">The name of the property represented by this node.</param>
		public PropertyNode(string propertyName)
			: base(propertyName)
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="PropertyNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="propertyData">The corresponding runtime configuration data.</param>
		public PropertyNode(ValidatedPropertyReference propertyData)
			: this(propertyData.Name)
		{
		}

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

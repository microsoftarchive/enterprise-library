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
	/// Represents a <see cref="ValidatedFieldReference"/>.
	/// </summary>
	[Image(typeof(FieldNode))]
	[SelectedImage(typeof(FieldNode))]
	public class FieldNode : ConfigurationNode
	{
		/// <summary>
		/// Creates an instance of <see cref="FieldNode"/> based on default values.
		/// </summary>
		public FieldNode()
			: this(Resources.FieldNodeName)
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="FieldNode"/> based on a field name.
		/// </summary>
		/// <param name="fieldName">The name of the field represented by this node.</param>
		public FieldNode(string fieldName)
			: base(fieldName)
		{
		}

		/// <summary>
		/// Creates an instance of <see cref="FieldNode"/> based on runtime configuration data.
		/// </summary>
		/// <param name="fieldData">The corresponding runtime configuration data.</param>
		public FieldNode(ValidatedFieldReference fieldData)
			: this(fieldData.Name)
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

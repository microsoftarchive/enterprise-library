//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents a design time node for a <see cref="ConfigurationSourceElement"/>. The class is abstract.
	/// </summary>
	[Image(typeof(ConfigurationSourceElementNode))]
	[SelectedImage(typeof(ConfigurationSourceElementNode))]
	public abstract class ConfigurationSourceElementNode : ConfigurationNode
	{		

		/// <summary>
		/// Initialize a new instance of the <see cref="ConfigurationSourceElementNode"/> class with a name.
		/// </summary>
		/// <param name="name">The name of the node.</param>
		protected ConfigurationSourceElementNode(string name) : base(name)
		{
		}		

		/// <summary>
		/// When overridden by a class, gets the <see cref="ConfigurationSourceElement"/> this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="ConfigurationSourceElement"/> this node represents.
		/// </value>
		[Browsable(false)]
		public abstract ConfigurationSourceElement ConfigurationSourceElement { get; }

		/// <summary>
		/// When overridden by a class, gets the <see cref="IConfigurationSource"/> that this node represents.
		/// </summary>
		/// <value>
		/// The <see cref="IConfigurationSource"/> that this node represents.
		/// </value>
		[Browsable(false)]
		public abstract IConfigurationSource ConfigurationSource { get; }
	}
}

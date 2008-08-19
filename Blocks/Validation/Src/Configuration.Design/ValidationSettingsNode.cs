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
using System.ComponentModel;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{
	/// <summary>
    /// Represents the <see cref="ValidationSettingsNode"/> configuration section.
	/// </summary>
    [Image(typeof(ValidationSettingsNode))]
    [SelectedImage(typeof(ValidationSettingsNode))]
    public sealed class ValidationSettingsNode : ConfigurationSectionNode
	{
		/// <summary>
        /// Initialize a new instance of the <see cref="ValidationSettingsNode"/> class.
		/// </summary>
        public ValidationSettingsNode()
            :base(Resources.ValidationSettingsNodeName)
		{
		}


		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		[ReadOnly(true)]
		public override string Name
		{
			get { return base.Name; }
		}
	}
}

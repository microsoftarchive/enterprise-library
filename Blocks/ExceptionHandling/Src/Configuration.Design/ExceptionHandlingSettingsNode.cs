//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Design
{
	/// <summary>
	/// Represents a design time representation of a <see cref="ExceptionHandlingSettings"/> configuration section.
	/// </summary>
	[Image(typeof(ExceptionHandlingSettingsNode), "SettingsNode")]
    [SelectedImage(typeof(ExceptionHandlingSettingsNode), "SettingsNode")]
    public sealed class ExceptionHandlingSettingsNode : ConfigurationSectionNode
	{
		/// <summary>
		/// Initialize a new instance of the <see cref="ExceptionHandlingSettingsNode"/> class.
		/// </summary>
		public ExceptionHandlingSettingsNode()
			: base(Resources.DefaultExceptionHandlingSettingsNodeName)
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
			get
			{
				return base.Name;
			}
		}
	}
}

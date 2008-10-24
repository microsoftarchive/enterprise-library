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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design.Properties;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.AppSettings.Configuration.Design
{
	/// <summary>
	/// Represents the design time node for the <see cref="AppSettingsSection"/>.
	/// </summary>
	[Image(typeof(AppSettingsNode))]
	[SelectedImage(typeof(AppSettingsNode))]
	public class AppSettingsNode : ConfigurationSectionNode
	{
		string file;

		/// <summary>
		/// Initialize a new instance of the <see cref="AppSettingsNode"/> class.
		/// </summary>
		public AppSettingsNode()
			: this(new AppSettingsSection())
		{
		}

		/// <summary>
		/// Initialize a new instance of the <see cref="AppSettingsNode"/> class with a <see cref="AppSettingsSection"/> instance.
		/// </summary>
		/// <param name="appSettings">A <see cref="AppSettingsSection"/> instance.</param>
		public AppSettingsNode(AppSettingsSection appSettings)
			: base(Resources.AppSettingsNodeName)
		{
			file = appSettings.File;
		}

		/// <summary>
		/// Gets the name of the node.
		/// </summary>
		/// <value>
		/// The name of the node.
		/// </value>
		/// <remarks>
		/// Overriden so it is readonly in the designer.
		/// </remarks>
		[ReadOnly(true)]
		public override string Name
		{
			get { return base.Name; }
		}

		/// <summary>
		/// Gets the external file for the <see cref="AppSettingsSection"/>.
		/// </summary>
		[SRCategory("CategoryGeneral", typeof(Resources))]
		[SRDescription("AppSettingsNodeFileDescription", typeof(Resources))]
		public string File
		{
			get { return file; }
			set { file = value; }
		}

		/// <summary>
		/// Gets or sets the flag indicating the section requires permission to be retrieved.
		/// </summary>
		/// <remarks>
		/// The section represented by this node is defined elsewhere, so the property does not need to be exposed.
		/// </remarks>
		[Browsable(false)]
		public override bool RequirePermission
		{
			get { return base.RequirePermission; }
			set { base.RequirePermission = value; }
		}
	}
}

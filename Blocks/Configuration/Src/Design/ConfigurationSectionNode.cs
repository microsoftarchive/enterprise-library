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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Base node for configuration sections.
	/// </summary>
	public abstract class ConfigurationSectionNode : ConfigurationNode
	{
		string protectProvider = Resources.NoProtectionProvider;
		private bool requirePermission = true;

		/// <summary>
		/// Initializes a new instance of class <see cref="ConfigurationSectionNode"/>.
		/// </summary>
		public ConfigurationSectionNode()
		{
		}

		/// <summary>
		/// Initializes a new instance of class <see cref="ConfigurationSectionNode"/> with the supplied name.
		/// </summary>
		public ConfigurationSectionNode(string name)
			: base(name)
		{
		}

		/// <summary>
		/// Gets or sets the name of the protection provider to use for the represented section.
		/// </summary>
		[SRCategory("CategoryProtection", typeof(Resources))]
		[SRDescription("SectionNodeProtectionProviderDescription", typeof(Resources))]
		[TypeConverter(typeof(ProtectionProviderTypeConverter))]
		[ProtectionProviderValidation]
		public string ProtectionProvider
		{
			get { return protectProvider; }
			set { protectProvider = value; }
		}

		/// <summary>
		/// Gets or sets the flag indicating the section requires permission to be retrieved.
		/// </summary>
		[SRCategory("CategorySection", typeof(Resources))]
		[SRDescription("SectionNodeRequirePermissionDescription", typeof(Resources))]
		[DefaultValue(true)]
		public virtual bool RequirePermission
		{
			get { return requirePermission; }
			set { requirePermission = value; }
		}
	}
}

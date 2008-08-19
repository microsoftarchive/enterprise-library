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
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design
{

	/// <summary>
	/// Represents the design manager for the Validation settings configuration section.
	/// </summary>
	public sealed class ValidationConfigurationDesignManager : Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ConfigurationDesignManager
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ConfigurationDesignManager"/> class.
		/// </summary>
		public ValidationConfigurationDesignManager()
		{
		}

		/// <summary>
		/// Register the commands and node maps needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		public override void Register(IServiceProvider serviceProvider)
		{
            ValidationCommandRegistrar cmdRegistrar = new ValidationCommandRegistrar(serviceProvider);
			cmdRegistrar.Register();
            ValidationNodeMapRegistrar nodeRegistrar = new ValidationNodeMapRegistrar(serviceProvider);
			nodeRegistrar.Register();
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the security settings configuration section.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the validation settings configuration section.</returns>
		protected override Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			ValidationSettingsNode node = null;
            if (null != rootNode) node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(ValidationSettingsNode)) as ValidationSettingsNode;
			ValidationSettings blockSettings = null;
			if (node != null)
			{
                ValidationSettingsBuilder builder = new ValidationSettingsBuilder(serviceProvider, node);
				blockSettings = builder.Build();
			}

            string protectionProviderName = GetProtectionProviderName(node);
            return new ConfigurationSectionInfo(node, blockSettings, ValidationSettings.SectionName, protectionProviderName);
		}

		/// <summary>
		/// Sets up the design time objects to represent the configuration for the validation block.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
        /// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource"/>.</param>
		protected override void OpenCore(IServiceProvider serviceProvider, 
			ConfigurationApplicationNode rootNode, 
			ConfigurationSection section)
		{
			if (null != section)
			{
                ValidationSettingsNodeBuilder builder = new ValidationSettingsNodeBuilder(serviceProvider, (ValidationSettings)section);
                ValidationSettingsNode node = builder.Build();

                SetProtectionProvider(section, node);
				rootNode.AddNode(node);
			}
		}
	}
}

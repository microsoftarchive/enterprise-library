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
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
	/// <summary>
	/// Represents the design manager that will register the design time information for the Policy Injection Application Block.
	/// </summary>
	public class PolicyInjectionConfigurationDesignManager : ConfigurationDesignManager
	{
		/// <summary>
		/// Registers the policy injection design manager into the environment.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		public override void Register(IServiceProvider serviceProvider)
		{
			PolicyInjectionNodeMapRegistrar nodeMapRegistrar = new PolicyInjectionNodeMapRegistrar(serviceProvider);
			nodeMapRegistrar.Register();

			PolicyInjectionCommandRegistrar commandRegistrar = new PolicyInjectionCommandRegistrar(serviceProvider);
			commandRegistrar.Register();
		}

		/// <summary>
		/// Opens the policy injection configuration from an application configuration file.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The <see cref="ConfigurationApplicationNode"/> of the hierarchy.</param>
		/// <param name="section">The policy injection configuration section or <see langword="null"/> if no section was found.</param>
		protected override void OpenCore(IServiceProvider serviceProvider,
			ConfigurationApplicationNode rootNode,
			ConfigurationSection section)
		{
			if (null != section)
			{
				PolicyInjectionSettingsNodeBuilder builder = new PolicyInjectionSettingsNodeBuilder(serviceProvider, (PolicyInjectionSettings)section);
				PolicyInjectionSettingsNode node = builder.Build();

				SetProtectionProvider(section, node);
				rootNode.AddNode(node);
			}
		}

		/// <summary>
		/// Gets a <see cref="ConfigurationSectionInfo"/> object containing the Policy Injection Block's configuration information.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> object containing the Policy Injection Block's configuration information.</returns>
		protected override ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			ConfigurationNode rootNode = ServiceHelper.GetCurrentRootNode(serviceProvider);
			PolicyInjectionSettingsNode node = null;
			if (null != rootNode) node = rootNode.Hierarchy.FindNodeByType(rootNode, typeof(PolicyInjectionSettingsNode)) as PolicyInjectionSettingsNode;
			PolicyInjectionSettings settings = null;
			if (node != null)
			{
				PolicyInjectionSettingsBuilder builder = new PolicyInjectionSettingsBuilder(serviceProvider, node);
				settings = builder.Build();
			}

			string protectionProviderName = GetProtectionProviderName(node);
			return new ConfigurationSectionInfo(node, settings, PolicyInjectionSettings.SectionName, protectionProviderName);
		}
	}
}
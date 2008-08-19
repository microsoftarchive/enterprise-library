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
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.Configuration;
using System.Drawing;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
	/// <summary>
	/// Represents the mechanism to register a configuration section(s) in the design time environment.
	/// </summary>
	public abstract class ConfigurationDesignManager : IConfigurationDesignManager
	{
		
		/// <summary>
		/// When overridden by a class, Register any mechanisms needed for the design manager into the design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		public abstract void Register(IServiceProvider serviceProvider);				

		/// <summary>
		/// Save the current defined configuration to an external <see cref="IConfigurationSource"/>.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public virtual void Save(IServiceProvider serviceProvider)
		{
            ConfigurationSectionInfo info = null;
            ConfigurationNode node = ServiceHelper.GetCurrentRootNode(serviceProvider);
			
            try
			{
                IConfigurationSource configurationSource = GetConfigurationSource(serviceProvider);
                IConfigurationParameter parameter = GetConfigurationParameter(serviceProvider);

				info = GetConfigurationSectionInfo(serviceProvider);
				if (null != info && !string.IsNullOrEmpty(info.SectionName))
				{
					if (null != info.Section)
					{
                        if (!string.IsNullOrEmpty(info.ProtectionProviderName))
                        {
                            IProtectedConfigurationSource protectedConfigurationSource = configurationSource as IProtectedConfigurationSource;
                            if (protectedConfigurationSource != null)
                            {
                                protectedConfigurationSource.Add(parameter, info.SectionName, info.Section, info.ProtectionProviderName);
                            }
                        }
                        else
                        {
                            configurationSource.Add(parameter, info.SectionName, info.Section);
                        }
					}
					else
					{
						configurationSource.Remove(parameter, info.SectionName);
					}
				}
			}
			catch (Exception e)
			{
				ServiceHelper.LogError(serviceProvider, info != null ? info.Node : node, e);

			}			
		}

		/// <summary>
		/// Open the configuration for this design manager.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
		public void Open(IServiceProvider serviceProvider)
		{
			IConfigurationSource configurationSource = GetConfigurationSource(serviceProvider);
			ConfigurationApplicationNode appNode = ServiceHelper.GetCurrentRootNode(serviceProvider);						
			try
			{
				ConfigurationSectionInfo info = GetConfigurationSectionInfo(serviceProvider);
				if (null != info)
				{
					ConfigurationSection section = configurationSource.GetSection(info.SectionName);
					OpenCore(serviceProvider, appNode, section);
				}				
			}
			catch (Exception e)
			{
				ServiceHelper.LogError(serviceProvider, appNode, e);
			}
		}		

		/// <summary>
		/// Builds the currently defined configuration in design time.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="dictionaryConfigurationSource">The <see cref="DictionaryConfigurationSource"/> to save the configuration.</param>
		public void BuildConfigurationSource(IServiceProvider serviceProvider, DictionaryConfigurationSource dictionaryConfigurationSource)
		{
			if (null == dictionaryConfigurationSource) throw new ArgumentNullException("dictionaryConfigurationSource");

			ConfigurationSectionInfo info = GetConfigurationSectionInfo(serviceProvider);			
			if (null != info && null != info.Section)
			{
				dictionaryConfigurationSource.Add(info.SectionName, info.Section);
			}			
		}

		/// <summary>
		/// When overriden by a class, allows design managers to perform specific operations when opening configuration.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <param name="rootNode">The root node of the application.</param>
		/// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="IConfigurationSource"/>.</param>
		protected virtual void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, ConfigurationSection section)
		{
		}

		/// <summary>
		/// Gets the <see cref="IConfigurationSource"/> to used by the design manager to save and open configuration.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>The <see cref="IConfigurationSource"/> to used by the design manager to save and open configuration</returns>
		protected virtual IConfigurationSource GetConfigurationSource(IServiceProvider serviceProvider)
		{
			return ServiceHelper.GetCurrentConfigurationSource(serviceProvider);
		}

		/// <summary>
		/// Gets the <see cref="IConfigurationParameter"/> to used by the design manager to save and open configuration.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>The <see cref="IConfigurationParameter"/> to used by the design manager to save and open configuration</returns>
		protected virtual IConfigurationParameter GetConfigurationParameter(IServiceProvider serviceProvider)
		{
			return ServiceHelper.GetCurrentConfigurationParameter(serviceProvider); ;
		}

		/// <summary>
		/// Gets the a <see cref="ConfigurationSectionInfo"/> for the configuration for this design manager.
		/// </summary>
		/// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
		/// <returns>A <see cref="ConfigurationSectionInfo"/> for the configuration for this design manager.</returns>
		protected virtual ConfigurationSectionInfo GetConfigurationSectionInfo(IServiceProvider serviceProvider)
		{
			return new ConfigurationSectionInfo(null, null, string.Empty, string.Empty);
		}

        /// <summary>
        /// TODO
        /// </summary>
        /// <param name="section"></param>
        /// <param name="protectionProviderName"></param>
        protected void ProtectConfigurationSection(ConfigurationSection section, string protectionProviderName)
        {
            if (!string.IsNullOrEmpty(protectionProviderName))
            {
                section.SectionInformation.ProtectSection(protectionProviderName);
            }
            else if (section.SectionInformation.IsProtected)
            {
                section.SectionInformation.UnprotectSection();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sectionNode"></param>
        /// <returns></returns>
        protected string GetProtectionProviderName(ConfigurationSectionNode sectionNode)
        {
            if (sectionNode == null || string.Compare(Resources.NoProtectionProvider, sectionNode.ProtectionProvider) == 0)
            {
                return string.Empty;
            }
            return sectionNode.ProtectionProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationSection"></param>
        /// <param name="sectionNode"></param>
        protected void SetProtectionProvider(ConfigurationSection configurationSection, ConfigurationSectionNode sectionNode)
        {
            if (null != configurationSection && null != configurationSection.SectionInformation.ProtectionProvider)
            {
                sectionNode.ProtectionProvider = configurationSection.SectionInformation.ProtectionProvider.Name;
            }
        }
	}
}

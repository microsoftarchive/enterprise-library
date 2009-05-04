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
using System.Configuration;
using System.IO;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm
{
	/// <summary>
	/// Encapsulates the process to generate the ADM template contents to represent the configuration 
	/// information contained in a <see cref="IConfigurationSource"/>, delegating to registered
	/// <see cref="ConfigurationSectionManageabilityProvider"/> instances the generation of the
	/// specific contents for each section.
	/// </summary>
	/// <seealso cref="ConfigurationSectionManageabilityProvider"/>
	public static class AdministrativeTemplateGenerator
	{
		/// <summary>
		/// Generates the ADM template contents that represent the configuration information in 
		/// <paramref name="configurationSource"/> 
		/// </summary>
		/// <param name="configurationSource">The configuration source holding the configuration sections.</param>
		/// <param name="applicationName">The ApplicationName to be used when generating the ADM policy keys.</param>
		/// <param name="manageabilityProviders">The mapping from section names to the
		/// <returns>The generated content.</returns>
		/// <see cref="ConfigurationSectionManageabilityProvider"/> instances that generate the ADM contents.</param>
		/// <remarks>Both MACHINE and USER policies are generated.</remarks>
		public static AdmContent GenerateAdministrativeTemplateContent(
			IConfigurationSource configurationSource,
			String applicationName,
			IDictionary<string, ConfigurationSectionManageabilityProvider> manageabilityProviders)
		{
			AdmContentBuilder contentBuilder = new AdmContentBuilder();

			contentBuilder.StartCategory(applicationName);
			foreach (KeyValuePair<string, ConfigurationSectionManageabilityProvider> kvp in manageabilityProviders)
			{
				ConfigurationSection configurationSection = configurationSource.GetSection(kvp.Key);
				if (configurationSection != null)
				{
					kvp.Value.AddAdministrativeTemplateDirectives(contentBuilder,
						configurationSection,
						configurationSource,
						applicationName);
				}
			}
			contentBuilder.EndCategory();

			return contentBuilder.GetContent();
		}
	}
}

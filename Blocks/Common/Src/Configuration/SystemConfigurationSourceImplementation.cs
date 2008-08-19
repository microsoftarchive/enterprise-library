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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the implementation details for the <see cref="SystemConfigurationSource"/>.
	/// </summary>
	public class SystemConfigurationSourceImplementation : BaseFileConfigurationSourceImplementation
	{
		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Initializes a new instance of the <see cref="FileConfigurationSourceImplementation"/> class.
		/// </summary>
		/// <remarks>
		/// The <see cref="SystemConfigurationSourceImplementation"/> will use the current application configuration file as
		/// the main configuration file.
		/// </remarks>
		public SystemConfigurationSourceImplementation()
			: base(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)
		{
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Initializes a new instance of the <see cref="FileConfigurationSourceImplementation"/> class.
		/// </summary>
		/// <remarks>
		/// The <see cref="SystemConfigurationSourceImplementation"/> will use the current application's configuration file as
		/// the main configuration file.
		/// </remarks>
		/// <param name="refresh"><b>true</b>if runtime changes should be refreshed, <b>false</b> otherwise.</param>
		public SystemConfigurationSourceImplementation(bool refresh)
			: base(AppDomain.CurrentDomain.SetupInformation.ConfigurationFile, refresh)
		{
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Retrieves the specified <see cref="ConfigurationSection"/> from the current application's configuration file, 
		/// and starts watching for its changes if not watching already.
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		/// <returns>The section, or <see langword="null"/> if it doesn't exist.</returns>
		public override ConfigurationSection GetSection(string sectionName)
		{
			ConfigurationSection configurationSection = ConfigurationManager.GetSection(sectionName) as ConfigurationSection;

			SetConfigurationWatchers(sectionName, configurationSection);

			return configurationSection;
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Refreshes the configuration sections from the main configuration file and determines which sections have suffered notifications
		/// and should be notified to registered handlers.
		/// </summary>
		/// <param name="localSectionsToRefresh">A dictionary with the configuration sections residing in the main configuration file that must be refreshed.</param>
		/// <param name="externalSectionsToRefresh">A dictionary with the configuration sections residing in external files that must be refreshed.</param>
		/// <param name="sectionsToNotify">A new collection with the names of the sections that suffered changes and should be notified.</param>
		/// <param name="sectionsWithChangedConfigSource">A new dictionary with the names and file names of the sections that have changed their location.</param>
		protected override void RefreshAndValidateSections(IDictionary<string, string> localSectionsToRefresh, IDictionary<string, string> externalSectionsToRefresh, out ICollection<string> sectionsToNotify, out IDictionary<string, string> sectionsWithChangedConfigSource)
		{
			sectionsToNotify = new List<string>();
			sectionsWithChangedConfigSource = new Dictionary<string, string>();

			// refresh local sections and determine what to do.
			foreach (KeyValuePair<string, string> sectionMapping in localSectionsToRefresh)
			{
				ConfigurationManager.RefreshSection(sectionMapping.Key);
				ConfigurationSection section = ConfigurationManager.GetSection(sectionMapping.Key) as ConfigurationSection;
				string refreshedConfigSource = section != null ? section.SectionInformation.ConfigSource : NullConfigSource;
				if (!sectionMapping.Value.Equals(refreshedConfigSource))
				{
					sectionsWithChangedConfigSource.Add(sectionMapping.Key, refreshedConfigSource);
				}

				// notify anyway, since it might have been updated.
				sectionsToNotify.Add(sectionMapping.Key);
			}

			// refresh external sections and determine what to do.
			foreach (KeyValuePair<string, string> sectionMapping in externalSectionsToRefresh)
			{
				ConfigurationManager.RefreshSection(sectionMapping.Key);
				ConfigurationSection section = ConfigurationManager.GetSection(sectionMapping.Key) as ConfigurationSection;
				string refreshedConfigSource = section != null ? section.SectionInformation.ConfigSource : NullConfigSource;
				if (!sectionMapping.Value.Equals(refreshedConfigSource))
				{
					sectionsWithChangedConfigSource.Add(sectionMapping.Key, refreshedConfigSource);

					// notify only if che config source changed
					sectionsToNotify.Add(sectionMapping.Key);
				}
			}
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Refreshes the configuration sections from an external configuration file.
		/// </summary>
		/// <param name="sectionsToRefresh">A collection with the names of the sections that suffered changes and should be refreshed.</param>
		protected override void RefreshExternalSections(string[] sectionsToRefresh)
		{
			foreach (string sectionName in sectionsToRefresh)
			{
				ConfigurationManager.RefreshSection(sectionName);
			}
		}
	}
}

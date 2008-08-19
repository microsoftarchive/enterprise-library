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
using System.ComponentModel;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	/// <summary>
	/// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
	/// Represents the implementation details for configuration sources over arbitrary files.
	/// </summary>
	public class FileConfigurationSourceImplementation : BaseFileConfigurationSourceImplementation
	{
		private string configurationFilepath;
		private ExeConfigurationFileMap fileMap;
		private System.Configuration.Configuration cachedConfiguration;
		private object cachedConfigurationLock = new object();

		/// <summary>
		/// Initializes a new instance of <see cref="FileConfigurationSourceImplementation"/>.
		/// </summary>
		/// <param name="configurationFilepath">The path for the main configuration file.</param>
		public FileConfigurationSourceImplementation(string configurationFilepath)
			: this(configurationFilepath, true)
		{
		}

		/// <summary>
		/// Initializes a new instance of <see cref="FileConfigurationSourceImplementation"/>.
		/// </summary>
		/// <param name="refresh">A bool indicating if runtime changes should be refreshed or not.</param>
		/// <param name="configurationFilepath">The path for the main configuration file.</param>
		public FileConfigurationSourceImplementation(string configurationFilepath, bool refresh)
			: base(configurationFilepath, refresh)
		{
			this.configurationFilepath = configurationFilepath;
			this.fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = configurationFilepath;
		}

		/// <summary>
		/// Retrieves a section from System.Configuration, and starts watching for 
		/// its changes if not watching already.
		/// </summary>
		/// <param name="sectionName">The section name.</param>
		/// <returns>The section, or null if it doesn't exist.</returns>
		public override ConfigurationSection GetSection(string sectionName)
		{
			System.Configuration.Configuration configuration = GetConfiguration();

			ConfigurationSection configurationSection = configuration.GetSection(sectionName) as ConfigurationSection;

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
			UpdateCache();

			sectionsToNotify = new List<string>();
			sectionsWithChangedConfigSource = new Dictionary<string, string>();

			// refresh local sections and determine what to do.
			foreach (KeyValuePair<string, string> sectionMapping in localSectionsToRefresh)
			{
				ConfigurationSection section = cachedConfiguration.GetSection(sectionMapping.Key) as ConfigurationSection;
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
				ConfigurationSection section = cachedConfiguration.GetSection(sectionMapping.Key) as ConfigurationSection;
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
			UpdateCache();
		}

		private System.Configuration.Configuration GetConfiguration()
		{
			if (cachedConfiguration == null)
			{
				lock (cachedConfigurationLock)
				{
					if (cachedConfiguration == null)
					{
						cachedConfiguration = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
					}
				}
			}

			return cachedConfiguration;
		}

		internal void UpdateCache()
		{
			System.Configuration.Configuration newConfiguration
				= ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			lock (cachedConfigurationLock)
			{
				cachedConfiguration = newConfiguration;
			}
		}
	}
}

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

using System.Collections.Generic;
using System.Configuration;
using System.IO;
namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
	public static class ConfigurationTestHelper
	{
		public static IConfigurationSource SaveSectionsAndReturnConfigurationSource(IDictionary<string, ConfigurationSection> sections)
		{
			System.Configuration.Configuration configuration 
				= ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

			SaveSections(configuration, sections);

			return new SystemConfigurationSource();
		}

		public static IConfigurationSource SaveSectionsInFileAndReturnConfigurationSource(IDictionary<string, ConfigurationSection> sections)
		{
			System.Configuration.Configuration configuration
				= GetConfigurationForCustomFile("test.exe.config");

			SaveSections(configuration, sections);

			return GetConfigurationSourceForCustomFile("test.exe.config");
		}

		public static IConfigurationSource GetConfigurationSourceForCustomFile(string fileName)
		{
			FileConfigurationSource.ResetImplementation(fileName, false);
			return new FileConfigurationSource((fileName));
		}

		public static System.Configuration.Configuration GetConfigurationForCustomFile(string fileName)
		{
			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = fileName;
			File.SetAttributes(fileMap.ExeConfigFilename, FileAttributes.Normal);
			return ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
		}

		private static void SaveSections(System.Configuration.Configuration configuration, 
									IDictionary<string, ConfigurationSection> sections)
		{
			foreach (string sectionName in sections.Keys)
			{
				configuration.Sections.Remove(sectionName);
				configuration.Sections.Add(sectionName, sections[sectionName]);
			}

			configuration.Save();
		}
	}
}

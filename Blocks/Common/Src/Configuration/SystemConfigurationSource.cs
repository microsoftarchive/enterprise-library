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
using System.Configuration;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	/// <summary>
	/// Represents an <see cref="IConfigurationSource"/> that retrieves the configuration information from the 
	/// application's default configuration file using the <see cref="ConfigurationManager"/> API.
	/// </summary>
	/// <remarks>
	/// <para>
	/// The <see cref="SystemConfigurationSource"/> is a wrapper over the static configuration access API provided by 
	/// <see cref="ConfigurationManager"/> and watches for changes in the configuration files to refresh the 
	/// configuration when a change is detected.
	/// </para>
	/// <para>
	/// It uses a shared instance of <see cref="SystemConfigurationSourceImplementation"/> to manage the access to the 
	/// configuration subsystem and to manage the file change watchers.
	/// </para>
	/// </remarks>
	/// <seealso cref="ConfigurationManager"/>
	/// <seealso cref="SystemConfigurationSourceImplementation"/>
	[ConfigurationElementType(typeof(SystemConfigurationSourceElement))]
	public class SystemConfigurationSource : IConfigurationSource
	{
		private static SystemConfigurationSourceImplementation implementation = new SystemConfigurationSourceImplementation(true);

		/// <summary>
		/// Initializes a new instance of the <see cref="SystemConfigurationSource"/> class.
		/// </summary>
		public SystemConfigurationSource()
		{
		}

		/// <summary>
		/// Retrieves the specified <see cref="ConfigurationSection"/>.
		/// </summary>
		/// <param name="sectionName">The section name to retrieve.</param>
		/// <returns>The specified <see cref="ConfigurationSection"/>, or <see langword="null"/> (<b>Nothing</b> in Visual Basic)
		/// if a section by that name is not found.</returns>
		public ConfigurationSection GetSection(string sectionName)
		{
			return implementation.GetSection(sectionName);
		}

		/// <summary>
		/// Adds a handler to be called when changes to the section named <paramref name="sectionName"/> are detected.
		/// </summary>
		/// <param name="sectionName">The name of the section to watch for.</param>
		/// <param name="handler">The handler for the change event to add.</param>
		public void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
			implementation.AddSectionChangeHandler(sectionName, handler);
		}

		/// <summary>
		/// Removes a handler to be called when changes to section <paramref name="sectionName"/> are detected.
		/// </summary>
		/// <param name="sectionName">The name of the watched section.</param>
		/// <param name="handler">The handler for the change event to remove.</param>
		public void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler)
		{
			implementation.RemoveSectionChangeHandler(sectionName, handler);
		}

        /// <summary>
        /// Reset the current configuration implementation.
        /// </summary>
        /// <param name="refreshing">
        /// true to have the implementation to do refreshing; owtherwise, false.
        /// </param>
		public static void ResetImplementation(bool refreshing)
		{
			SystemConfigurationSourceImplementation currentImplementation = implementation;
			implementation = new SystemConfigurationSourceImplementation(refreshing);
			currentImplementation.Dispose();
		}

        /// <summary>
        /// Gets the current configuration source implementation.
        /// </summary>
        /// <value>
        /// The current configuration source implementation.
        /// </value>
		public static BaseFileConfigurationSourceImplementation Implementation
		{
			get { return implementation; }
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Adds or replaces <paramref name="configurationSection"/> under name <paramref name="section"/> in the configuration 
		/// file named <paramref name="fileName" /> and saves the configuration file.
		/// </summary>
		/// <param name="fileName">The name of the configuration file.</param>
		/// <param name="section">The name for the section.</param>
		/// <param name="configurationSection">The configuration section to add or replace.</param>
		public void Save(string fileName, string section, ConfigurationSection configurationSection)
		{
			ValidateArgumentsAndFileExists(fileName, section, configurationSection);

			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = fileName;
			System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			config.Sections.Remove(section);
			config.Sections.Add(section, configurationSection);
			config.Save();
		}

		/// <summary>
		/// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
		/// Removes the configuration section named <paramref name="section"/> from the configuration file named
		/// <paramref name="fileName"/> and saves the configuration file.
		/// </summary>
		/// <param name="fileName">The name of the configuration file.</param>
		/// <param name="section">The name for the section.</param>
		public void Remove(string fileName, string section)
		{
			if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "fileName");
			if (string.IsNullOrEmpty(section)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "section");

			ExeConfigurationFileMap fileMap = new ExeConfigurationFileMap();
			fileMap.ExeConfigFilename = fileName;
			System.Configuration.Configuration config = ConfigurationManager.OpenMappedExeConfiguration(fileMap, ConfigurationUserLevel.None);
			config.Sections.Remove(section);
			config.Save();
		}

		/// <summary>
		/// Adds a <see cref="ConfigurationSection"/> to the configuration source location specified by 
		/// <paramref name="saveParameter"/> and saves the configuration source.
		/// </summary>
		/// <remarks>
		/// If a configuration section with the specified name already exists in the location specified by 
		/// <paramref name="saveParameter"/> it will be replaced.
		/// </remarks>
		/// <param name="addParameter">The <see cref="IConfigurationParameter"/> that represents the location where 
		/// to save the updated configuration. Must be an instance of <see cref="FileConfigurationParameter"/>.</param>
		/// <param name="sectionName">The name by which the <paramref name="configurationSection"/> should be added.</param>
		/// <param name="configurationSection">The configuration section to add.</param>
		public void Add(IConfigurationParameter addParameter, string sectionName, ConfigurationSection configurationSection)
		{
			FileConfigurationParameter parameter = addParameter as FileConfigurationParameter;
			if (null == parameter) throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(FileConfigurationParameter).Name), "saveParameter");

			Save(parameter.FileName, sectionName, configurationSection);
		}

		/// <summary>
		/// Removes a <see cref="ConfigurationSection"/> from the configuration source location specified by 
		/// <paramref name="removeParameter"/> and saves the configuration source.
		/// </summary>
		/// <param name="removeParameter">The <see cref="IConfigurationParameter"/> that represents the location where 
		/// to save the updated configuration. Must be an instance of <see cref="FileConfigurationParameter"/>.</param>
		/// <param name="sectionName">The name of the section to remove.</param>
		public void Remove(IConfigurationParameter removeParameter, string sectionName)
		{
			FileConfigurationParameter parameter = removeParameter as FileConfigurationParameter;
			if (null == parameter) throw new ArgumentException(string.Format(Resources.Culture, Resources.ExceptionUnexpectedType, typeof(FileConfigurationParameter).Name), "saveParameter");

			Remove(parameter.FileName, sectionName);
		}

		private static void ValidateArgumentsAndFileExists(string fileName, string section, ConfigurationSection configurationSection)
		{
			if (string.IsNullOrEmpty(fileName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "fileName");
			if (string.IsNullOrEmpty(section)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "section");
			if (null == configurationSection) throw new ArgumentNullException("configurationSection");

			if (!File.Exists(fileName)) throw new FileNotFoundException(string.Format(Resources.ExceptionConfigurationFileNotFound, section), fileName);
		}
	}
}

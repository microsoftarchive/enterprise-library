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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
	/// <summary>
	/// Represents a source for getting configuration information.
	/// </summary>
	public interface IConfigurationSource
	{
		/// <summary>
		/// Retrieves the specified <see cref="ConfigurationSection"/>.
		/// </summary>
		/// <param name="sectionName">The name of the section to be retrieved.</param>
		/// <returns>The specified <see cref="ConfigurationSection"/>, or <see langword="null"/> (<b>Nothing</b> in Visual Basic)
		/// if a section by that name is not found.</returns>
		ConfigurationSection GetSection(string sectionName);

		/// <summary>
		/// Adds a <see cref="ConfigurationSection"/> to the configuration source location specified by 
		/// <paramref name="saveParameter"/> and saves the configuration source.
		/// </summary>
		/// <remarks>
		/// If a configuration section with the specified name already exists in the location specified by 
		/// <paramref name="saveParameter"/> it will be replaced.
		/// </remarks>
		/// <param name="saveParameter">The <see cref="IConfigurationParameter"/> that represents the location where 
		/// to save the updated configuration.</param>
		/// <param name="sectionName">The name by which the <paramref name="configurationSection"/> should be added.</param>
		/// <param name="configurationSection">The configuration section to add.</param>
		void Add(IConfigurationParameter saveParameter, string sectionName, ConfigurationSection configurationSection);

		/// <summary>
		/// Removes a <see cref="ConfigurationSection"/> from the configuration source location specified by 
		/// <paramref name="removeParameter"/> and saves the configuration source.
		/// </summary>
		/// <param name="removeParameter">The <see cref="IConfigurationParameter"/> that represents the location where 
		/// to save the updated configuration.</param>
		/// <param name="sectionName">The name of the section to remove.</param>
		void Remove(IConfigurationParameter removeParameter, string sectionName);

		/// <summary>
		/// Adds a handler to be called when changes to the section named <paramref name="sectionName"/> are detected.
		/// </summary>
		/// <param name="sectionName">The name of the section to watch for.</param>
		/// <param name="handler">The handler for the change event to add.</param>
		void AddSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler);

		/// <summary>
		/// Removes a handler to be called when changes to section <code>sectionName</code> are detected.
		/// </summary>
		/// <param name="sectionName">The name of the watched section.</param>
		/// <param name="handler">The handler for the change event to remove.</param>
		void RemoveSectionChangeHandler(string sectionName, ConfigurationChangedEventHandler handler);
	}

	/// <summary>
	/// Represents a configuration parameter for an <see cref="IConfigurationSource"/>.
	/// </summary>
	/// <remarks>
	/// Configuration parameters represent the configuration of a configuration source.
	/// </remarks>
	public interface IConfigurationParameter
	{
	}
}

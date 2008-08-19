//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using System.Management.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Formatters
{
	/// <summary>
	/// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.BinaryLogFormatterData"/>
	/// as an instrumentation class.
	/// </summary>
	[ManagementEntity]
	public partial class BinaryFormatterSetting : FormatterSetting
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="BinaryFormatterSetting"/> class with a configuraiton source element,
        /// and the name of the formatter.
        /// </summary>
        /// <param name="sourceElement">The configuration source element.</param>
        /// <param name="name">The name of the formatter.</param>
		public BinaryFormatterSetting(BinaryLogFormatterData sourceElement, string name)
			: base(sourceElement, name)
		{ }

        /// <summary>
        /// Returns an enumeration of the published <see cref="BinaryFormatterSetting"/> instances.
        /// </summary>
		[ManagementEnumerator]
		public static IEnumerable<BinaryFormatterSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<BinaryFormatterSetting>();
		}

        /// <summary>
        /// Returns the <see cref="BinaryFormatterSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="BinaryFormatterSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static BinaryFormatterSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<BinaryFormatterSetting>(ApplicationName, SectionName, Name);
		}

        /// <summary>
        /// Saves the changes on the <see cref="BinaryFormatterSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return false; // no writable properties for this formatter
		}
	}
}
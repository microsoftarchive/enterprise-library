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
	/// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.CustomFormatterData"/>
	/// as an instrumentation class.
	/// </summary>
	[ManagementEntity]
	public partial class CustomFormatterSetting : FormatterSetting
	{
		private string formatterType;
		private string[] attributes;

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomFormatterSetting"/> class with the formatter configuration,
        /// the name of the formatter, the formatter type and the attributes for the formatter.
        /// </summary>
        /// <param name="sourceElement">The formatter configuration.</param>
        /// <param name="name">The name of the formatter.</param>
        /// <param name="formatterType">The formatter type.</param>
        /// <param name="attributes">The attributes for the formatter.</param>
		public CustomFormatterSetting(CustomFormatterData sourceElement, string name, string formatterType, string[] attributes)
			: base(sourceElement, name)
		{
			this.formatterType = formatterType;
			this.attributes = attributes;
		}

		/// <summary>
		/// Gets the assembly qualified name of the formatter type for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string FormatterType
		{
			get { return formatterType; }
			set { formatterType = value; }
		}

		/// <summary>
		/// Gets the attributes for the represented configuration element.
		/// </summary>
		/// <remarks>
		/// The attributes are encoded as an string array of name/value pairs.
		/// </remarks>
		[ManagementConfiguration]
		public string[] Attributes
		{
			get { return attributes; }
			set { attributes = value; }
		}

        /// <summary>
        /// Returns an enumeration of the published <see cref="CustomFormatterSetting"/> instances.
        /// </summary>
		[ManagementEnumerator]
		public static IEnumerable<CustomFormatterSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<CustomFormatterSetting>();
		}

        /// <summary>
        /// Returns the <see cref="CustomFormatterSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="CustomFormatterSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static CustomFormatterSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<CustomFormatterSetting>(ApplicationName, SectionName, Name);
		}

        /// <summary>
        /// Saves the changes on the <see cref="CustomFormatterSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return CustomFormatterDataWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
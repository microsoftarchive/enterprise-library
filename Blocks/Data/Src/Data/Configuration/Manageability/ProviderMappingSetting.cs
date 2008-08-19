//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Data Access Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
	/// <summary>
	/// Represents the configuration information for a provider mapping defined by the Database Application Block
	/// configuration section.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings"/>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DbProviderMapping"/>
	[ManagementEntity]
	public partial class ProviderMappingSetting : NamedConfigurationSetting
	{
		private string databaseTypeName;

        /// <summary>
        /// Initialize a new instance of the <see cref="ProviderMappingSetting"/> class with a source element the name and the database type name.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="name">The name of the mapping.</param>
        /// <param name="databaseTypeName">The database type.</param>
		public ProviderMappingSetting(DbProviderMapping sourceElement, string name, string databaseTypeName)
			: base(sourceElement, name)
		{
			this.databaseTypeName = databaseTypeName;
		}

		/// <summary>
		/// Gets the type of the database to which the represented database mapping 
		/// maps its provider name.
		/// </summary>
		[ManagementConfiguration]
		public string DatabaseType
		{
			get { return databaseTypeName; }
			set { databaseTypeName = value; }
		}

        /// <summary>
        /// Returns an enumeration of the published <see cref="ProviderMappingSetting"/> instances.
        /// </summary>
        /// <returns>A sequence of <see cref="ProviderMappingSetting"/> objects.</returns>
        [ManagementEnumerator]
        public static IEnumerable<ProviderMappingSetting> GetInstances()
        {
            return NamedConfigurationSetting.GetInstances<ProviderMappingSetting>();
        }

        /// <summary>
        /// Returns the <see cref="ProviderMappingSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="ProviderMappingSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static ProviderMappingSetting BindInstance(string ApplicationName, string SectionName, string Name)
        {
            return NamedConfigurationSetting.BindInstance<ProviderMappingSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Saves the changes on the <see cref="ProviderMappingSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return DatabaseSettingsWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}

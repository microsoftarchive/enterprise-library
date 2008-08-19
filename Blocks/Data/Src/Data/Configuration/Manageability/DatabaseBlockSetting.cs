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
	/// Represents the configuration information for the Database Application Block.
	/// </summary>
	/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings"/>
	[ManagementEntity]
	public partial class DatabaseBlockSetting : ConfigurationSectionSetting
	{
		private string defaultDatabase;

        /// <summary>
        /// Initialize a new instance of the <see cref="DatabaseBlockSetting"/> class with a source element and the default database..
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="defaultDatabase">The default database.</param>
		public DatabaseBlockSetting(DatabaseSettings sourceElement, string defaultDatabase)
			: base(sourceElement)
		{
			this.defaultDatabase = defaultDatabase;
		}

		/// <summary>
		/// Gets the name of the default database on the represented database configuration section.
		/// </summary>
		/// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSettings.DefaultDatabase">DatabaseSettings.DefaultDatabase</seealso>
		[ManagementConfiguration]
		public string DefaultDatabase
		{
			get { return defaultDatabase; }
			set { defaultDatabase = value; }
		}

        /// <summary>
        /// Returns an enumeration of the published <see cref="DatabaseBlockSetting"/> instances.
        /// </summary>
        /// <returns></returns>
        [ManagementEnumerator]
        public static IEnumerable<DatabaseBlockSetting> GetInstances()
        {
            return ConfigurationSectionSetting.GetInstances<DatabaseBlockSetting>();
        }

        /// <summary>
        /// Returns the <see cref="DatabaseBlockSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="applicationName">The value for the ApplicationName key property.</param>
        /// <param name="sectionName">The value for the SectionName key property.</param>
        /// <returns>The published <see cref="DatabaseBlockSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static DatabaseBlockSetting BindInstance(string applicationName, string sectionName)
        {
            return ConfigurationSectionSetting.BindInstance<DatabaseBlockSetting>(applicationName, sectionName);
        }

        /// <summary>
        /// Saves the changes on the <see cref="DatabaseBlockSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return DatabaseSettingsWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}
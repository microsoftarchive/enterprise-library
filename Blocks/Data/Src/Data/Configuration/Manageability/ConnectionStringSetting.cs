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
	/// Represents the configuration information for an database connection defined by the connection 
	/// strings configuration section.
	/// </summary>
	[ManagementEntity]
	public partial class ConnectionStringSetting : NamedConfigurationSetting
	{
		private string connectionString;
		private string providerName;

        /// <summary>
        /// Initialize a new instance of the <see cref="ConnectionStringSetting"/> class with a source element, 
        /// the name, connection string and provider name.
        /// </summary>
        /// <param name="sourceElement">The configuration source element.</param>
        /// <param name="name">The name of the connection string.</param>
        /// <param name="connectionString">The connection string.</param>
        /// <param name="providerName">The provider name.</param>
		public ConnectionStringSetting(ConnectionStringSettings sourceElement,
			string name,
			string connectionString,
			string providerName
		)
			: base(sourceElement, name)
		{
			this.connectionString = connectionString;
			this.providerName = providerName;
		}

		/// <summary>
		/// Gets the connection string for the represented <see cref="System.Configuration.ConnectionStringSettings"/> instance.
		/// </summary>
		[ManagementConfiguration]
		public string ConnectionString
		{
			get { return connectionString; }
			set { connectionString = value; }
		}

		/// <summary>
		/// Gets the provider name for the represented <see cref="System.Configuration.ConnectionStringSettings"/> instance.
		/// </summary>
		[ManagementConfiguration]
		public string ProviderName
		{
			get { return providerName; }
			set { providerName = value; }
		}

		/// <summary>
        /// Returns an enumeration of the published <see cref="ConnectionStringSetting"/> instances.
		/// </summary>
		/// <returns>Sequence of <see cref="ConnectionStringSetting"/> objects.</returns>
		[ManagementEnumerator]
		public static IEnumerable<ConnectionStringSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<ConnectionStringSetting>();
		}

        /// <summary>
        /// Returns the <see cref="ConnectionStringSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="ConnectionStringSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static ConnectionStringSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<ConnectionStringSetting>(ApplicationName, SectionName, Name);
		}

        /// <summary>
        /// Saves the changes on the <see cref="ConnectionStringSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
		protected override bool SaveChanges(ConfigurationElement sourceElement)
		{
			return ConnectionStringsWmiMapper.SaveChanges(this, sourceElement);
		}
	}
}

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
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Manageability
{
    /// <summary>
    /// Represents the configuration information for an Oracle database connection defined by the Oracle
    /// specific connection information configuration section.
    /// </summary>
    /// <remarks>
    /// The collection of <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration.OraclePackageData"/> instances
    /// defined by an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration.OracleConnectionData"/>
    /// is represented as a <see cref="string"/> array contaning key/value pairs.
    /// </remarks>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration.OracleConnectionSettings"/>
    /// <seealso cref="Microsoft.Practices.EnterpriseLibrary.Data.Oracle.Configuration.OracleConnectionData"/>
    [ManagementEntity]
    public partial class OracleConnectionSetting : NamedConfigurationSetting
    {
        string[] packages;

        /// <summary>
        /// Initialize a new instance of the <see cref="OracleConnectionSetting"/> class with the source element, the name 
        /// and the list of package.
        /// </summary>
        /// <param name="sourceElement">The source element.</param>
        /// <param name="name">The name of the setting.</param>
        /// <param name="packages">The packages to manage.</param>
        public OracleConnectionSetting(OracleConnectionData sourceElement,
                                       string name,
                                       string[] packages)
            : base(sourceElement, name)
        {
            this.packages = packages;
        }

        /// <summary>
        /// Gets the package mapping information specified by the represented Oracle connection data object as a
        /// <see cref="string"/> array of key/value pairs.
        /// </summary>
        [ManagementConfiguration]
        public string[] Packages
        {
            get { return packages; }
            set { packages = value; }
        }

        /// <summary>
        /// Returns the <see cref="OracleConnectionSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="OracleConnectionSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
        [ManagementBind]
        public static OracleConnectionSetting BindInstance(string ApplicationName,
                                                           string SectionName,
                                                           string Name)
        {
            return BindInstance<OracleConnectionSetting>(ApplicationName, SectionName, Name);
        }

        /// <summary>
        /// Returns an enumeration of the published <see cref="OracleConnectionSetting"/> instances.
        /// </summary>
        /// <returns></returns>
        [ManagementEnumerator]
        public static IEnumerable<OracleConnectionSetting> GetInstances()
        {
            return GetInstances<OracleConnectionSetting>();
        }

        /// <summary>
        /// Saves the changes on the <see cref="OracleConnectionSetting"/> to its corresponding configuration object.
        /// </summary>
        /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
        /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
        protected override bool SaveChanges(ConfigurationElement sourceElement)
        {
            return OracleConnectionSettingsWmiMapper.SaveChanges(this, sourceElement);
        }
    }
}

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

using System;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.ComponentModel;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration
{
    /// <summary>
    /// Represents the mapping from an ADO.NET provider to an Enterprise Library <see cref="Database"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Enterprise Library Data Access Application Block leverages the ADO.NET 2.0 provider factories. To determine what type of <see cref="Database"/> matches a given provider factory type, the optional 
    /// <see cref="DbProviderMapping"/> configuration objects can be defined in the block's configuration section.
    /// </para>
    /// <para>
    /// If a mapping is not present for a given provider type, sensible defaults will be used:
    /// <list type="bullet">
    /// <item>For provider name "System.Data.SqlClient", or for a provider of type <see cref="System.Data.SqlClient.SqlClientFactory"/>, the 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Sql.SqlDatabase"/> will be used.</item>
    /// <item>For provider name "System.Data.OracleClient", or for a provider of type <see cref="System.Data.OracleClient.OracleClientFactory"/>, the 
    /// <see cref="Microsoft.Practices.EnterpriseLibrary.Data.Oracle.OracleDatabase"/> will be used.</item>
    /// <item>In any other case, the <see cref="GenericDatabase"/> will be used.</item>
    /// </list>
    /// </para>
    /// </remarks>
    /// <seealso cref="DatabaseSyntheticConfigSettings.GetProviderMapping(string)"/>
    /// <seealso cref="System.Data.Common.DbProviderFactory"/>
    [ResourceDescription(typeof(DesignResources), "DbProviderMappingDescription")]
    [ResourceDisplayName(typeof(DesignResources), "DbProviderMappingDisplayName")]
    public class DbProviderMapping : NamedConfigurationElement
    {
        private static AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        /// <summary>
        /// Default name for the Sql managed provider.
        /// </summary>
        public const string DefaultSqlProviderName = "System.Data.SqlClient";
        /// <summary>
        /// Default name for the Oracle managed provider.
        /// </summary>
        public const string DefaultOracleProviderName = "System.Data.OracleClient";

        internal const string DefaultGenericProviderName = "generic";
        private const string databaseTypeProperty = "databaseType";

        /// <summary>
        /// Initializes a new instance of the <see cref="DbProviderMapping"/> class.
        /// </summary>
        public DbProviderMapping()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbProviderMapping"/> class with name and <see cref="Database"/> type.
        /// </summary>
        public DbProviderMapping(string dbProviderName, Type databaseType)
            : this(dbProviderName, (string)typeConverter.ConvertTo(databaseType, typeof(string)))
        {

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DbProviderMapping"/> class with name and fully qualified type name of the <see cref="Database"/> type.
        /// </summary>
        public DbProviderMapping(string dbProviderName, string databaseTypeName)
            : base(dbProviderName)
        {
            this.DatabaseTypeName = databaseTypeName;
        }


        /// <summary>
        /// Gets or sets the type of database to use for the mapped ADO.NET provider.
        /// </summary>
        public Type DatabaseType
        {
            get { return (Type)typeConverter.ConvertFrom(DatabaseTypeName); }
            set { DatabaseTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified type name of the database to use for the mapped ADO.NET provider.
        /// </summary>
        /// <value>
        /// The fully qualified type name of the database to use for the mapped ADO.NET provider.
        /// </value>
        [ConfigurationProperty(databaseTypeProperty)]
        [ResourceDescription(typeof(DesignResources), "DbProviderMappingDatabaseTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DbProviderMappingDatabaseTypeNameDisplayName")]
        [Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(Database))]
        public string DatabaseTypeName
        {
            get { return (string)this[databaseTypeProperty]; }
            set { this[databaseTypeProperty] = value; }
        }

        /// <summary>
        ///  Gets the logical name of the ADO.NET provider.
        /// </summary>
        public string DbProviderName
        {
            get { return Name; }
        }

        /// <summary/>
        // TODO : make this a designtime converter. normal converter gets in the wat of system.configuration
        //[TypeConverter("Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Converters.SystemDataProviderConverter,  Microsoft.Practices.EnterpriseLibrary.Configuration.Design")]
        public override string Name
        {
            get{ return base.Name; }
            set{ base.Name = value; }
        }
    }
}

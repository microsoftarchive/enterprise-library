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

using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration
{
    /// <summary>
    /// Configuration object for a <see cref="FormattedDatabaseTraceListener"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "FormattedDatabaseTraceListenerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "FormattedDatabaseTraceListenerDataDisplayName")]
    [AddSateliteProviderCommand("connectionStrings", typeof(DatabaseSettings), "DefaultDatabase", "DatabaseInstanceName")]
    public class FormattedDatabaseTraceListenerData : TraceListenerData
    {
        private const string addCategoryStoredProcNameProperty = "addCategoryStoredProcName";
        private const string databaseInstanceNameProperty = "databaseInstanceName";
        private const string formatterNameProperty = "formatter";
        private const string writeLogStoredProcNameProperty = "writeLogStoredProcName";

        /// <summary>
        /// Initializes a <see cref="FormattedDatabaseTraceListenerData"/>.
        /// </summary>
        public FormattedDatabaseTraceListenerData()
            : base(typeof(FormattedDatabaseTraceListener))
        {
            this.ListenerDataType = typeof(FormattedDatabaseTraceListenerData);
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FormattedDatabaseTraceListenerData"/> with 
        /// name, stored procedure name, databse instance name, and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="writeLogStoredProcName">The stored procedure name for writing the log.</param>
        /// <param name="addCategoryStoredProcName">The stored procedure name for adding a category for this log.</param>
        /// <param name="databaseInstanceName">The database instance name.</param>
        /// <param name="formatterName">The formatter name.</param>        
        public FormattedDatabaseTraceListenerData(string name,
                                                  string writeLogStoredProcName,
                                                  string addCategoryStoredProcName,
                                                  string databaseInstanceName,
                                                  string formatterName)
            : this(
                name,
                writeLogStoredProcName,
                addCategoryStoredProcName,
                databaseInstanceName,
                formatterName,
                TraceOptions.None,
                SourceLevels.All)
        {
        }

        /// <summary>
        /// Initializes a named instance of <see cref="FormattedDatabaseTraceListenerData"/> with 
        /// name, stored procedure name, databse instance name, and formatter name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="writeLogStoredProcName">The stored procedure name for writing the log.</param>
        /// <param name="addCategoryStoredProcName">The stored procedure name for adding a category for this log.</param>
        /// <param name="databaseInstanceName">The database instance name.</param>
        /// <param name="formatterName">The formatter name.</param>
        /// <param name="traceOutputOptions">The trace options.</param>
        /// <param name="filter">The filter to be applied</param>
        public FormattedDatabaseTraceListenerData(string name,
                                                  string writeLogStoredProcName,
                                                  string addCategoryStoredProcName,
                                                  string databaseInstanceName,
                                                  string formatterName,
                                                  TraceOptions traceOutputOptions,
                                                  SourceLevels filter)
            : base(name, typeof(FormattedDatabaseTraceListener), traceOutputOptions, filter)
        {
            DatabaseInstanceName = databaseInstanceName;
            WriteLogStoredProcName = writeLogStoredProcName;
            AddCategoryStoredProcName = addCategoryStoredProcName;
            Formatter = formatterName;
        }

        /// <summary>
        /// Gets and sets the database instance name.
        /// </summary>
        [ConfigurationProperty(databaseInstanceNameProperty, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "FormattedDatabaseTraceListenerDataDatabaseInstanceNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FormattedDatabaseTraceListenerDataDatabaseInstanceNameDisplayName")]
        [Reference(typeof(ConnectionStringSettingsCollection), typeof(ConnectionStringSettings))]
        public string DatabaseInstanceName
        {
            get { return (string)base[databaseInstanceNameProperty]; }
            set { base[databaseInstanceNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the stored procedure name for writing the log.
        /// </summary>
        [ConfigurationProperty(writeLogStoredProcNameProperty, IsRequired = true, DefaultValue = "WriteLog")]
        [ResourceDescription(typeof(DesignResources), "FormattedDatabaseTraceListenerDataWriteLogStoredProcNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FormattedDatabaseTraceListenerDataWriteLogStoredProcNameDisplayName")]
        public string WriteLogStoredProcName
        {
            get { return (string)base[writeLogStoredProcNameProperty]; }
            set { base[writeLogStoredProcNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the stored procedure name for adding a category for this log.
        /// </summary>
        [ConfigurationProperty(addCategoryStoredProcNameProperty, IsRequired = true, DefaultValue = "AddCategory")]
        [ResourceDescription(typeof(DesignResources), "FormattedDatabaseTraceListenerDataAddCategoryStoredProcNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FormattedDatabaseTraceListenerDataAddCategoryStoredProcNameDisplayName")]
        public string AddCategoryStoredProcName
        {
            get { return (string)base[addCategoryStoredProcNameProperty]; }
            set { base[addCategoryStoredProcNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the formatter name.
        /// </summary>
        [ConfigurationProperty(formatterNameProperty, IsRequired = false)]
        [ResourceDescription(typeof(DesignResources), "FormattedDatabaseTraceListenerDataFormatterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FormattedDatabaseTraceListenerDataFormatterDisplayName")]
        [Reference(typeof(NameTypeConfigurationElementCollection<FormatterData, CustomFormatterData>), typeof(FormatterData))]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// Builds the <see cref="TraceListener" /> object represented by this configuration object.
        /// </summary>
        /// <param name="settings">The configuration settings for logging.</param>
        /// <returns>
        /// A trace listener.
        /// </returns>
        protected override TraceListener CoreBuildTraceListener(LoggingSettings settings)
        {
            var database = DatabaseFactory.CreateDatabase(this.DatabaseInstanceName);
            var formatter = this.BuildFormatterSafe(settings, this.Formatter);

            return new FormattedDatabaseTraceListener(database, WriteLogStoredProcName, AddCategoryStoredProcName, formatter);
        }
    }
}

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

using System;
using System.Configuration;
using System.Diagnostics;
using System.Linq.Expressions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Logging.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration
{
    /// <summary>
    /// Configuration object for a <see cref="FormattedDatabaseTraceListener"/>.
    /// </summary>
    [Assembler(typeof(FormattedDatabaseTraceListenerAssembler))]
    [ContainerPolicyCreator(typeof(FormattedDatabaseTraceListenerPolicyCreator))]
    public class FormattedDatabaseTraceListenerData : TraceListenerData
    {
        private const string writeLogStoredProcNameProperty = "writeLogStoredProcName";
        private const string addCategoryStoredProcNameProperty = "addCategoryStoredProcName";
        private const string databaseInstanceNameProperty = "databaseInstanceName";
        private const string formatterNameProperty = "formatter";

        /// <summary>
        /// Initializes a <see cref="FormattedDatabaseTraceListenerData"/>.
        /// </summary>
        public FormattedDatabaseTraceListenerData()
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
            this.DatabaseInstanceName = databaseInstanceName;
            this.WriteLogStoredProcName = writeLogStoredProcName;
            this.AddCategoryStoredProcName = addCategoryStoredProcName;
            this.Formatter = formatterName;
        }

        /// <summary>
        /// Gets and sets the database instance name.
        /// </summary>
        [ConfigurationProperty(databaseInstanceNameProperty, IsRequired = false)]
        public string DatabaseInstanceName
        {
            get { return (string)base[databaseInstanceNameProperty]; }
            set { base[databaseInstanceNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the stored procedure name for writing the log.
        /// </summary>
        [ConfigurationProperty(writeLogStoredProcNameProperty, IsRequired = true)]
        public string WriteLogStoredProcName
        {
            get { return (string)base[writeLogStoredProcNameProperty]; }
            set { base[writeLogStoredProcNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the stored procedure name for adding a category for this log.
        /// </summary>
        [ConfigurationProperty(addCategoryStoredProcNameProperty, IsRequired = true)]
        public string AddCategoryStoredProcName
        {
            get { return (string)base[addCategoryStoredProcNameProperty]; }
            set { base[addCategoryStoredProcNameProperty] = value; }
        }

        /// <summary>
        /// Gets and sets the formatter name.
        /// </summary>
        [ConfigurationProperty(formatterNameProperty, IsRequired = false)]
        public string Formatter
        {
            get { return (string)base[formatterNameProperty]; }
            set { base[formatterNameProperty] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected override Expression<Func<TraceListener>> GetCreationExpression()
        {
            return () =>
                new FormattedDatabaseTraceListener(
                    Container.Resolved<Data.Database>(this.DatabaseInstanceName),
                    this.WriteLogStoredProcName,
                    this.AddCategoryStoredProcName,
                    Container.ResolvedIfNotNull<ILogFormatter>(this.Formatter));
        }
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="FormattedDatabaseTraceListener"/> described by a <see cref="FormattedDatabaseTraceListenerData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="FormattedDatabaseTraceListenerData"/> type and it is used by the <see cref="TraceListenerCustomFactory"/> 
    /// to build the specific <see cref="TraceListener"/> object represented by the configuration object.
    /// </remarks>
    public class FormattedDatabaseTraceListenerAssembler : TraceListenerAsssembler
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="FormattedDatabaseTraceListener"/> based on an instance of <see cref="FormattedDatabaseTraceListenerData"/>.
        /// </summary>
        /// <seealso cref="TraceListenerCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="FormattedDatabaseTraceListenerData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="FormattedDatabaseTraceListener"/>.</returns>
        public override TraceListener Assemble(IBuilderContext context,
                                               TraceListenerData objectConfiguration,
                                               IConfigurationSource configurationSource,
                                               ConfigurationReflectionCache reflectionCache)
        {
            FormattedDatabaseTraceListenerData castedObjectConfiguration
                = (FormattedDatabaseTraceListenerData)objectConfiguration;

            IBuilderContext databaseContext
                = context.CloneForNewBuild(
                    NamedTypeBuildKey.Make<Data.Database>(castedObjectConfiguration.DatabaseInstanceName), null);

            Data.Database database
                = (Data.Database)databaseContext.Strategies.ExecuteBuildUp(databaseContext);

            ILogFormatter formatter
                = GetFormatter(context, castedObjectConfiguration.Formatter, configurationSource, reflectionCache);

            TraceListener createdObject
                = new FormattedDatabaseTraceListener(
                    database,
                    castedObjectConfiguration.WriteLogStoredProcName,
                    castedObjectConfiguration.AddCategoryStoredProcName,
                    formatter);

            return createdObject;
        }
    }
}

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
using System.ComponentModel;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Formatters;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Design
{
    /// <summary>
    /// Represents a <see cref="FormattedDatabaseTraceListenerData"/> configuraiton element.
    /// </summary>
    public sealed class LoggingDatabaseNode : TraceListenerNode
    {
        const string addCategoryStoredProcedureDefault = "AddCategory";
        const string writeLogStoredProcedureDefault = "WriteLog";

        string addCategoryStoredProcedure;

        ConnectionStringSettingsNode connectionStringNode;
        string databaseName;
        string formatterName;
        FormatterNode formatterNode;
        EventHandler<ConfigurationNodeChangedEventArgs> onConnectionStringNodeRemoved;
        EventHandler<ConfigurationNodeChangedEventArgs> onConnectionStringNodeRenamed;
        string writeLogStoredProcedureName;

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingDatabaseNode"/> class.
        /// </summary>
        public LoggingDatabaseNode()
            : this(new FormattedDatabaseTraceListenerData(Resources.DatabaseTraceListenerName, writeLogStoredProcedureDefault, addCategoryStoredProcedureDefault, string.Empty, string.Empty)) {}

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingDatabaseNode"/> class with a <see cref="FormattedDatabaseTraceListenerData"/> instance.
        /// </summary>
        /// <param name="traceListenerData">A <see cref="FormattedDatabaseTraceListenerData"/> instance</param>
        public LoggingDatabaseNode(FormattedDatabaseTraceListenerData traceListenerData)
        {
            if (null == traceListenerData) throw new ArgumentNullException("traceListenerData");

            Rename(traceListenerData.Name);
            TraceOutputOptions = traceListenerData.TraceOutputOptions;
            databaseName = traceListenerData.DatabaseInstanceName;
            addCategoryStoredProcedure = traceListenerData.AddCategoryStoredProcName;
            writeLogStoredProcedureName = traceListenerData.WriteLogStoredProcName;
            formatterName = traceListenerData.Formatter;
            onConnectionStringNodeRemoved = new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringNodeRemoved);
            onConnectionStringNodeRenamed = new EventHandler<ConfigurationNodeChangedEventArgs>(OnConnectionStringNodeRenamed);
        }

        /// <summary>
        /// Gets or sets the stored procedure to add a category.
        /// </summary>
        /// <value>
        /// The stored procedure to add a category.
        /// </value>
        [Required]
        [SRDescription("AddCategoryStoredProcNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string AddCategoryStoredProcedure
        {
            get { return addCategoryStoredProcedure; }
            set { addCategoryStoredProcedure = value; }
        }

        /// <summary>
        /// Gets or sets the database instance to use.
        /// </summary>
        /// <value>
        /// The database instance to use.
        /// </value>
        [Required]
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(ConnectionStringSettingsNode))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        [SRDescription("DatabaseInstanceDescription", typeof(Resources))]
        public ConnectionStringSettingsNode DatabaseInstance
        {
            get { return connectionStringNode; }
            set
            {
                connectionStringNode = LinkNodeHelper.CreateReference<ConnectionStringSettingsNode>(connectionStringNode,
                                                                                                    value,
                                                                                                    onConnectionStringNodeRemoved,
                                                                                                    onConnectionStringNodeRenamed);

                databaseName = (connectionStringNode == null) ? String.Empty : connectionStringNode.Name;
            }
        }

        /// <summary>
        /// Gets or sets the formatter to use.
        /// </summary>
        /// <value>
        /// The formatter to use.
        /// </value>
        [Editor(typeof(ReferenceEditor), typeof(UITypeEditor))]
        [ReferenceType(typeof(FormatterNode))]
        [SRDescription("FormatDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public FormatterNode Formatter
        {
            get { return formatterNode; }
            set
            {
                formatterNode = LinkNodeHelper.CreateReference<FormatterNode>(formatterNode,
                                                                              value,
                                                                              OnFormatterNodeRemoved,
                                                                              OnFormatterNodeRenamed);

                formatterName = formatterNode == null ? string.Empty : formatterNode.Name;
            }
        }

        /// <summary>
        /// Gets the <see cref="FormattedDatabaseTraceListenerData"/> this node represents.
        /// </summary>
        /// <value>
        /// The <see cref="FormattedDatabaseTraceListenerData"/> this node represents.
        /// </value>
        public override TraceListenerData TraceListenerData
        {
            get
            {
                return new FormattedDatabaseTraceListenerData(Name, writeLogStoredProcedureName, addCategoryStoredProcedure, databaseName,
                                                              formatterName);
            }
        }

        /// <summary>
        /// Gets or sets the write stored procedure.
        /// </summary>
        /// <value>
        /// The write stored procedure.
        /// </value>
        [Required]
        [SRDescription("WriteStoredProcNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string WriteLogStoredProcedureName
        {
            get { return writeLogStoredProcedureName; }
            set { writeLogStoredProcedureName = value; }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="LoggingDatabaseNode"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources.
        /// </param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connectionStringNode != null)
                {
                    connectionStringNode.Removed -= onConnectionStringNodeRemoved;
                    connectionStringNode.Renamed -= onConnectionStringNodeRenamed;
                }
            }
            base.Dispose(disposing);
        }

        void OnConnectionStringNodeRemoved(object sender,
                                           ConfigurationNodeChangedEventArgs e)
        {
            connectionStringNode = null;
        }

        void OnConnectionStringNodeRenamed(object sender,
                                           ConfigurationNodeChangedEventArgs e)
        {
            databaseName = e.Node.Name;
        }

        void OnFormatterNodeRemoved(object sender,
                                    ConfigurationNodeChangedEventArgs e)
        {
            formatterName = null;
        }

        void OnFormatterNodeRenamed(object sender,
                                    ConfigurationNodeChangedEventArgs e)
        {
            formatterName = e.Node.Name;
        }

        /// <summary>
        /// Sets the formatter to use for this listener.
        /// </summary>
        /// <param name="formatterNodeReference">
        /// A <see cref="FormatterNode"/> reference or <see langword="null"/> if no formatter is defined.
        /// </param>
        protected override void SetFormatterReference(ConfigurationNode formatterNodeReference)
        {
            if (formatterName == formatterNodeReference.Name) Formatter = (FormatterNode)formatterNodeReference;
        }
    }
}

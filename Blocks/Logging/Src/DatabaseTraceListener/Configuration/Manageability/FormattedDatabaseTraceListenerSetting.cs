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
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability
{
	/// <summary>
	/// Represents an instance of <see cref="Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.FormattedDatabaseTraceListenerData"/>
	/// as an instrumentation class.
	/// </summary>
	[ManagementEntity]
	public partial class FormattedDatabaseTraceListenerSetting : TraceListenerSetting
	{
		private string addCategoryStoredProcName;
		private string databaseInstanceName;
		private string formatter;
		private string writeLogStoredProcName;

        /// <summary>
        /// Initialize a new instance of the <see cref="FormattedDatabaseTraceListenerData"/> class.
        /// </summary>
        /// <param name="sourceElement">The configuration source element.</param>
        /// <param name="name">The name of the trace listener.</param>
        /// <param name="databaseInstanceName">The database instance name.</param>
        /// <param name="writeLogStoredProcName">The write log stored procedure.</param>
        /// <param name="addCategoryStoredProcName">The stored procedure to add categories.</param>
        /// <param name="formatter">The formatter to use.</param>
        /// <param name="traceOutputOptions">The trace output options.</param>
		/// <param name="filter">The filter value.</param>
		public FormattedDatabaseTraceListenerSetting(FormattedDatabaseTraceListenerData sourceElement,
			string name,
			string databaseInstanceName,
			string writeLogStoredProcName,
			string addCategoryStoredProcName,
			string formatter,
			string traceOutputOptions,
			string filter)
			: base(sourceElement, name, traceOutputOptions, filter)
		{
			this.addCategoryStoredProcName = addCategoryStoredProcName;
			this.databaseInstanceName = databaseInstanceName;
			this.formatter = formatter;
			this.writeLogStoredProcName = writeLogStoredProcName;
		}

		/// <summary>
		/// Gets the name of the stored procedure to add a logging category for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string AddCategoryStoredProcName
		{
			get { return addCategoryStoredProcName; }
			set { addCategoryStoredProcName = value; }
		}

		/// <summary>
		/// Gets the name of the database instance for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string DatabaseInstanceName
		{
			get { return databaseInstanceName; }
			set { databaseInstanceName = value; }
		}

		/// <summary>
		/// Gets the name of the formatter for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string Formatter
		{
			get { return formatter; }
			set { formatter = value; }
		}

		/// <summary>
		/// Gets the name of the stored procedure to write a log entry for the represented configuration element.
		/// </summary>
		[ManagementConfiguration]
		public string WriteLogStoredProcName
		{
			get { return writeLogStoredProcName; }
			set { writeLogStoredProcName = value; }
		}

        /// <summary>
        /// Returns an enumeration of the published <see cref="FormattedDatabaseTraceListenerSetting"/> instances.
        /// </summary>
		[ManagementEnumerator]
		public static IEnumerable<FormattedDatabaseTraceListenerSetting> GetInstances()
		{
			return NamedConfigurationSetting.GetInstances<FormattedDatabaseTraceListenerSetting>();
		}

        /// <summary>
        /// Returns the <see cref="FormattedDatabaseTraceListenerSetting"/> instance corresponding to the provided values for the key properties.
        /// </summary>
        /// <param name="ApplicationName">The value for the ApplicationName key property.</param>
        /// <param name="SectionName">The value for the SectionName key property.</param>
        /// <param name="Name">The value for the Name key property.</param>
        /// <returns>The published <see cref="FormattedDatabaseTraceListenerSetting"/> instance specified by the values for the key properties,
        /// or <see langword="null"/> if no such an instance is currently published.</returns>
		[ManagementBind]
		public static FormattedDatabaseTraceListenerSetting BindInstance(string ApplicationName, string SectionName, string Name)
		{
			return NamedConfigurationSetting.BindInstance<FormattedDatabaseTraceListenerSetting>(ApplicationName, SectionName, Name);
		}

	    /// <summary>
	    /// Pushes the current property values to the <see cref="ConfigurationSetting.SourceElement"/>
	    /// </summary>
	    /// <remarks>Must be overridden by subclasses to perform the actual save.</remarks>
	    /// <param name="sourceElement">The configuration object on which the changes must be saved.</param>
	    /// <returns><see langword="true"/> if changes have been saved, <see langword="false"/> otherwise.</returns>
	    protected override bool SaveChanges(ConfigurationElement sourceElement)
	    {
	        return FormattedDatabaseTraceListenerDataWmiMapper.SaveChanges(this, sourceElement);
	    }
	}
}

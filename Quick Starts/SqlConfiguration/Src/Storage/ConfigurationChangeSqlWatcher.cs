//===============================================================================
// Microsoft patterns & practices Enterprise Library
// SQL Configuration Source QuickStart
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Properties;
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Data;
using System.Data.SqlClient;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Storage
{
	/// <summary>
	/// <para>Represents an <see cref="IConfigurationChangeWatcher"/> that watches a Microsoft SQL Server Database.</para>
	/// </summary>
	public class ConfigurationChangeSqlWatcher : ConfigurationChangeWatcher, IConfigurationChangeWatcher
	{
        private static readonly string eventSourceName = Resources.SqlWatcherEventSource;
		private string configurationSectionName;
		private string connectString;
        private string getStoredProcedure;

        /// <summary/>
        /// 
        /// <param name="connectString"><para>The connection string to the SQL Server database.</para></param>
        /// <param name="getStoredProcedure"><para>The stored procedure name to get the data.</para></param>
        /// <param name="configurationSectionName"><para>The section name of the changes.</para></param>
        public ConfigurationChangeSqlWatcher(string connectString, string getStoredProcedure, string configurationSectionName)
		{
            if (connectString == null) throw new ArgumentNullException("connectString");
            if (getStoredProcedure == null) throw new ArgumentNullException("getStoredProcedure");

			this.configurationSectionName = configurationSectionName;
            this.connectString = connectString;
            this.getStoredProcedure = getStoredProcedure;
        }

		/// <summary>
		/// <para>Allows an <see cref="ConfigurationChangeFileWatcher"/> to attempt to free resources and perform other cleanup operations before the <see cref="ConfigurationChangeFileWatcher"/> is reclaimed by garbage collection.</para>
		/// </summary>
		~ConfigurationChangeSqlWatcher()
		{
			Disposing(false);
		}

		/// <summary>
		/// <para>Gets the name of the configuration section being watched.</para>
		/// </summary>
		/// <value>
		/// <para>The name of the configuration section being watched.</para>
		/// </value>
		public override string SectionName
		{
			get { return configurationSectionName; }
		}

		/// <summary>
		/// <para>Returns the <see cref="DateTime"/> of the last change of the information watched</para>
		/// <para>The information is retrieved using the watched file modification timestamp</para>
		/// </summary>
		/// <returns>The <see cref="DateTime"/> of the last modificaiton, or <code>DateTime.MinValue</code> if the information can't be retrieved</returns>
		protected override DateTime GetCurrentLastWriteTime()
		{
            using (SqlConnection myConnection = new SqlConnection(this.connectString))
            {
                DateTime currentLastWriteTime = DateTime.MinValue;

                SqlCommand myCommand = new SqlCommand(this.getStoredProcedure, myConnection);
                myCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter parameterSectionName = new SqlParameter(@"@SectionName", SqlDbType.NVarChar);
                parameterSectionName.Value = this.SectionName;
                myCommand.Parameters.Add(parameterSectionName);

                // Execute the command
                myConnection.Open();

                using (SqlDataReader reader = myCommand.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        currentLastWriteTime = reader.IsDBNull(2) ? DateTime.MinValue : reader.GetDateTime(2);
                    }
                }
                return currentLastWriteTime;
            }
		}
    
		/// <summary>
		/// Builds the change event data, including the full path of the watched file
		/// </summary>
		/// <returns>The change event information</returns>
		protected override ConfigurationChangedEventArgs BuildEventData()
		{
            return new ConfigurationSqlChangedEventArgs(this.connectString, this.getStoredProcedure, this.configurationSectionName);
		}

		/// <summary>
		/// Returns the source name to use when logging events
		/// </summary>
		/// <returns>The event source name</returns>
		protected override string GetEventSourceName()
		{
			return eventSourceName;
		}
	}
}

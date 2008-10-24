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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource
{
    /// <summary>
    /// </summary>
    [Serializable]
    public class ConfigurationSqlChangingEventArgs : ConfigurationChangingEventArgs
    {
        private string connectString;
        private string getStoredProcedure;
        private string setStoredProcedure;
        
        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationChangingEventArgs"/> class with the configuration file, the section name, the old value, and the new value of the changes.</para>
        /// </summary>
        /// <param name="connectString"><para>The connection string to the SQL Server database.</para></param>
        /// <param name="getStoredProcedure"><para>The stored procedure name to get the data.</para></param>
        /// <param name="setStoredProcedure"><para>The stored procedure name to set the data.</para></param>
        /// <param name="sectionName"><para>The section name of the changes.</para></param>
        /// <param name="oldValue"><para>The old value.</para></param>
        /// <param name="newValue"><para>The new value.</para></param>
        public ConfigurationSqlChangingEventArgs(string connectString, string getStoredProcedure, string setStoredProcedure, string sectionName, object oldValue, object newValue)
            : base(sectionName, oldValue, newValue)
        {
            this.connectString = connectString;
            this.getStoredProcedure = getStoredProcedure;
            this.setStoredProcedure = setStoredProcedure;            
        }


        /// <summary>
        /// <para>Gets the connection string to the SQL Server database.</para>
        /// </summary>
        /// <value><para>The connection string to the SQL Server database.</para></value>        
        public string ConnectionString
        {
            get { return this.connectString; }
        }

        /// <summary>
        /// <para>Gets the stored procedure name to get the data.</para>
        /// </summary>
        /// <value><para>The stored procedure name to get the data.</para></value>
        public string GetStoredProcedure
        {
            get { return this.getStoredProcedure; }
        }

        /// <summary>
        /// <para>Gets or sets the stored procedure name to set the data.</para>
        /// </summary>
        /// <value><para>The stored procedure name to set the data.</para></value>
        public string SetStoredProcedure
        {
            get { return this.setStoredProcedure; }
        }

    }
}

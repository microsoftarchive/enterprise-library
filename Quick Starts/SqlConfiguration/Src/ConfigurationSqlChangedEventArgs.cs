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
    public class ConfigurationSqlChangedEventArgs : ConfigurationChangedEventArgs
    {
        private string connectString;
        private string getStoredProcedure;

        /// <summary>
        /// <para>Initialize a new instance of the <see cref="ConfigurationSqlChangedEventArgs"/> class with the connection string, 
        /// get stored proc, set stored proc, and the section name.</para>
        /// </summary>
        /// <param name="connectString"><para>The connection string to the SQL Server database.</para></param>
        /// <param name="getStoredProcedure"><para>The stored procedure name to get the data.</para></param>
        /// <param name="sectionName"><para>The section name of the changes.</para></param>
        public ConfigurationSqlChangedEventArgs(string connectString, string getStoredProcedure, string sectionName)
            : base(sectionName)
        {
            this.connectString = connectString;
            this.getStoredProcedure = getStoredProcedure;
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
    }
}

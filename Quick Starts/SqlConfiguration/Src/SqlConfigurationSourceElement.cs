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

using System.Configuration;

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlConfigurationSourceElement : ConfigurationSourceElement
    {
        private const string connectionStringProperty = "connectionString";
        private const string getStoredProcProperty = "getStoredProcedure";
        private const string setStoredProcProperty = "setStoredProcedure";
        private const string refreshStoredProcProperty = "refreshStoredProcedure";
        private const string removeStoredProcProperty = "removeStoredProcedure";

        /// <summary>
        /// 
        /// </summary>
        public SqlConfigurationSourceElement()
            : this(Resources.SqlConfigurationSourceName, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectionString"></param>
        /// <param name="getStoredProcedure"></param>
        /// <param name="setStoredProcedure"></param>
        /// <param name="refreshStoredProcedure"></param>
        /// <param name="removeStoredProcedure"></param>
        public SqlConfigurationSourceElement(string name, string connectionString, string getStoredProcedure, string setStoredProcedure, string refreshStoredProcedure, string removeStoredProcedure)
            : base(name, typeof(SqlConfigurationSource))
        {
            this.ConnectionString = connectionString;
            this.GetStoredProcedure = getStoredProcedure;
            this.SetStoredProcedure = setStoredProcedure;
            this.RefreshStoredProcedure = refreshStoredProcedure;
            this.RemoveStoredProcedure = removeStoredProcedure;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override IConfigurationSource CreateSource()
        {
            IConfigurationSource createdObject = new SqlConfigurationSource(ConnectionString, GetStoredProcedure, SetStoredProcedure, RefreshStoredProcedure, RemoveStoredProcedure);

            return createdObject;
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(connectionStringProperty, IsRequired = true)]
        public string ConnectionString
        {
            get { return (string)this[connectionStringProperty]; }
            set { this[connectionStringProperty] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(getStoredProcProperty, IsRequired = true)]
        public string GetStoredProcedure
        {
            get { return (string)this[getStoredProcProperty]; }
            set { this[getStoredProcProperty] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(setStoredProcProperty, IsRequired = true)]
        public string SetStoredProcedure
        {
            get { return (string)this[setStoredProcProperty]; }
            set { this[setStoredProcProperty] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(refreshStoredProcProperty, IsRequired = true)]
        public string RefreshStoredProcedure
        {
            get { return (string)this[refreshStoredProcProperty]; }
            set { this[refreshStoredProcProperty] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(removeStoredProcProperty, IsRequired = true)]
        public string RemoveStoredProcedure
        {
            get { return (string)this[removeStoredProcProperty]; }
            set { this[removeStoredProcProperty] = value; }
        }
    }
}

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
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Configuration;
using System.Configuration.Internal;
using System.Collections.Generic;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.SqlConfigurationSource.Configuration
{
    /// <summary>
    /// 
    /// </summary>
    public class SqlConfigurationData : NamedConfigurationElement
    {
        private const string connectStringProperty = "connectionString";
        private const string getStoredProcedureProperty = "getStoredProcedure";
        private const string setStoredProcedureProperty = "setStoredProcedure";
        private const string refreshStoredProcedureProperty = "refreshStoredProcedure";
        private const string removeStoredProcedureProperty = "removeStoredProcedure";

        /// <summary>
        /// 
        /// </summary>
        public SqlConfigurationData()
        {
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        public SqlConfigurationData(SqlConfigurationData data)
            : this(data.ConnectionString, data.GetStoredProcedure, data.SetStoredProcedure, data.RefreshStoredProcedure, data.RemoveStoredProcedure)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="data"></param>
        public SqlConfigurationData(string name, SqlConfigurationData data) 
            :  this(name, data.ConnectionString, data.GetStoredProcedure, data.SetStoredProcedure, data.RefreshStoredProcedure, data.RemoveStoredProcedure)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="connectString"></param>
        /// <param name="getStoredProcedure"></param>
        /// <param name="setStoredProcedure"></param>
        /// <param name="refreshStoredProcedure"></param>
        /// <param name="removeStoredProcedure"></param>
        public SqlConfigurationData(
                        string connectString, 
                        string getStoredProcedure, 
                        string setStoredProcedure, 
                        string refreshStoredProcedure, 
                        string removeStoredProcedure)
            : base()
        {
            this.ConnectionString = connectString;
            this.GetStoredProcedure = getStoredProcedure;
            this.SetStoredProcedure = setStoredProcedure;
            this.RefreshStoredProcedure = refreshStoredProcedure;
            this.RemoveStoredProcedure = removeStoredProcedure;
        }
 
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="connectString"></param>
        /// <param name="getStoredProcedure"></param>
        /// <param name="setStoredProcedure"></param>
        /// <param name="refreshStoredProcedure"></param>
        /// <param name="removeStoredProcedure"></param>
        public SqlConfigurationData(
                        string name, 
                        string connectString, 
                        string getStoredProcedure, 
                        string setStoredProcedure, 
                        string refreshStoredProcedure, 
                        string removeStoredProcedure)
            : this(connectString, getStoredProcedure, setStoredProcedure, refreshStoredProcedure, removeStoredProcedure)
        {
            this.Name = name;
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(connectStringProperty, IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return (string)this[connectStringProperty];
            }
            set
            {
                this[connectStringProperty] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(getStoredProcedureProperty, IsRequired = true)]
        public string GetStoredProcedure
        {
            get
            {
                return (string)this[getStoredProcedureProperty];
            }
            set
            {
                this[getStoredProcedureProperty] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(setStoredProcedureProperty, IsRequired = true)]
        public string SetStoredProcedure
        {
            get
            {
                return (string)this[setStoredProcedureProperty];
            }
            set
            {
                this[setStoredProcedureProperty] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(refreshStoredProcedureProperty, IsRequired = true)]
        public string RefreshStoredProcedure
        {
            get
            {
                return (string)this[refreshStoredProcedureProperty];
            }
            set
            {
                this[refreshStoredProcedureProperty] = value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(removeStoredProcedureProperty, IsRequired = true)]
        public string RemoveStoredProcedure
        {
            get
            {
                return (string)this[removeStoredProcedureProperty];
            }
            set
            {
                this[removeStoredProcedureProperty] = value;
            }
        }
    
    }
}

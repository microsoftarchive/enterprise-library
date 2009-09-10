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
using System.Data.Common;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    ///<summary>
    /// Base class to help build database-specific configurations extensions for <see cref="DataConfigurationSourceBuilderExtensions"/>.
    ///</summary>
    public abstract class DatabaseConfigurationExtension : IDatabaseConfigurationProperties
    {
        private readonly IDatabaseConfigurationProperties context;

        ///<summary>
        /// Initializes a new DatabaseConfigurationExtension with a <see cref="IDatabaseConfigurationProviders"/> context.
        ///</summary>
        ///<remarks>
        /// This class supports extending the data configuration section's fluent-style API.  New database providers
        /// can inherit from this class to gain access to the current <see cref="ConnectionString"/> and underlying <see cref="Builder"/>
        /// properties.  
        /// 
        /// This class also implements the <see cref="IDatabaseConfigurationProperties"/> to enable continuation of the data
        /// fluent interface.
        /// </remarks>
        ///<param name="context"></param>
        protected DatabaseConfigurationExtension(IDatabaseConfigurationProviders context)
        {
            if (context == null) throw new ArgumentNullException("context");

            this.context = (IDatabaseConfigurationProperties)context;
            Debug.Assert(typeof (IDatabaseProviderExtensionContext).IsAssignableFrom(context.GetType()));
        }

        ///<summary>
        /// Specify the type of database.
        ///</summary>
        public IDatabaseConfigurationProviders ThatIs
        {
            get { return context.ThatIs; }
        }

        ///<summary>
        /// Configure a named database.
        ///</summary>
        ///<param name="databaseName">Name of database to configure</param>
        ///<returns></returns>
        public IDatabaseConfigurationProperties ForDatabaseNamed(string databaseName)
        {
            return context.ForDatabaseNamed(databaseName);
        }

        ///<summary>
        /// Set this database as the default one in the configuration.
        ///</summary>
        ///<returns></returns>
        public IDatabaseConfigurationProperties AsDefault()
        {
            return context.AsDefault();
        }

        ///<summary>
        /// The connection string in progress.
        ///</summary>
        public ConnectionStringSettings ConnectionString
        {
            get { return ((IDatabaseProviderExtensionContext)context).ConnectionString; }
        }

        ///<summary>
        /// Context of the current builder for the extension.
        ///</summary>
        public IConfigurationSourceBuilder Builder
        {
            get { return ((IDatabaseProviderExtensionContext)context).Builder; }
        }

        /// <summary>
        /// Connection string to use for this data source.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        /// <seealso cref="ConnectionStringSettings"/>
        /// <seealso cref="DbConnectionStringBuilder"/>
        public IDatabaseConfigurationProperties WithConnectionString(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "connectionString");
            ConnectionString.ConnectionString = connectionString;
            return this;
        }

        ///<summary>
        /// Connection string to use for this data source
        ///</summary>
        ///<param name="builder"></param>
        ///<returns></returns>
        public IDatabaseConfigurationProperties WithConnectionString(DbConnectionStringBuilder builder)
        {
            if (builder == null) throw new ArgumentNullException("builder");

            return WithConnectionString(builder.ConnectionString);
        }

    }
}

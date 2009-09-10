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
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Data.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    ///<summary>
    /// Data configuration fluent interface extions to <see cref="IConfigurationSourceBuilder"/>
    ///</summary>
    /// <seealso cref="ConfigurationSourceBuilder"/>
    public static class DataConfigurationSourceBuilderExtensions
    {
        ///<summary>
        /// Configure database connections for Enterprise Library.
        ///</summary>
        ///<param name="configurationSourceBuilderRoot">Source builder root that is extended.</param>
        ///<returns></returns>
        public static IDataConfiguration ConfigureData(this IConfigurationSourceBuilder configurationSourceBuilderRoot)
        {
            return new DataConfigurationBuilder(configurationSourceBuilderRoot);
        }

        private class DataConfigurationBuilder : IDatabaseConfigurationProviders,
                                                 IDatabaseProviderExtensionContext,
                                                 IDatabaseProviderConfiguration,
                                                 IDatabaseConfigurationProperties
        {
            private IConfigurationSourceBuilder Builder { get; set; }
            private readonly ConnectionStringsSection connectionStringSection = new ConnectionStringsSection();
            private ConnectionStringSettings currentDatabaseConnectionInfo;
            private DatabaseSettings currentDatabaseSection;
            private DbProviderMapping currentProviderMapping;

            public DataConfigurationBuilder(IConfigurationSourceBuilder builder)
            {
                Builder = builder;
                builder.AddSection("connectionStrings", connectionStringSection);
            }

            ///<summary>
            /// Configure a named database.
            ///</summary>
            ///<param name="databaseName">Name of database to configure</param>
            ///<returns></returns>
            public IDatabaseConfigurationProperties ForDatabaseNamed(string databaseName)
            {
                if (string.IsNullOrEmpty(databaseName)) throw new ArgumentException(Properties.Resources.ExceptionStringNullOrEmpty, "databaseName");

                ResetForNewDatabase(databaseName);
                connectionStringSection.ConnectionStrings.Add(currentDatabaseConnectionInfo);

                return this;
            }

            ///<summary>
            /// Set this database as the default one in the configuration.
            ///</summary>
            ///<returns></returns>
            public IDatabaseConfigurationProperties AsDefault()
            {
                EnsureDatabaseSettings();
                currentDatabaseSection.DefaultDatabase = currentDatabaseConnectionInfo.Name;
                return this;
            }

            ///<summary>
            /// Specify the type of database.
            ///</summary>
            public IDatabaseConfigurationProviders ThatIs
            {
                get { return this; }
            }

            /// <summary />
            public IDatabaseProviderConfiguration WithProviderNamed(string providerName)
            {
                if (string.IsNullOrEmpty(providerName)) throw new ArgumentException(Properties.Resources.ExceptionStringNullOrEmpty, "providerName");

                EnsureDatabaseSettings();
                currentProviderMapping = new DbProviderMapping();
                currentProviderMapping.Name = providerName;
                currentProviderMapping.DatabaseType = typeof(GenericDatabase);
                currentDatabaseSection.ProviderMappings.Add(currentProviderMapping);
                return this;
            }

            /// <summary>
            /// Map the provider alias to the specified database type.
            /// </summary>
            /// <param name="databaseType">Maps the provider to a type that derives from <see cref="Database"/></param>
            /// <returns></returns>
            public IDataConfiguration MappedToDatabase(Type databaseType)
            {
                if (!typeof(Database).IsAssignableFrom(databaseType)) 
                {
                    throw new ArgumentException(Resources.ExceptionArgumentMustInheritFromDatabase, "databaseType");
                }
                
                currentProviderMapping.DatabaseType = databaseType;
                return this;
            }

            /// <summary />
            public IDataConfiguration MappedToDatabase<T>() where T : Database
            {
                return MappedToDatabase(typeof(T));
            }


            ConnectionStringSettings IDatabaseProviderExtensionContext.ConnectionString
            {
                get { return currentDatabaseConnectionInfo; }
            }

            IConfigurationSourceBuilder IDatabaseProviderExtensionContext.Builder
            {
                get { return Builder; }
            }

            private void ResetForNewDatabase(string databaseName)
            {
                currentDatabaseConnectionInfo = new ConnectionStringSettings
                {
                    Name = databaseName,
                    ProviderName = DbProviderMapping.DefaultSqlProviderName,
                    ConnectionString = Data.Properties.Resources.DefaultSqlConnctionString
                };
            }

            private void EnsureDatabaseSettings()
            {
                if (currentDatabaseSection != null) return;
                currentDatabaseSection = new DatabaseSettings();
                Builder.AddSection(DatabaseSettings.SectionName, currentDatabaseSection);
            }

        }
    }
}

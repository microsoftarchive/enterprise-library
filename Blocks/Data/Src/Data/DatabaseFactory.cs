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
using Microsoft.Practices.EnterpriseLibrary.Common.Utility;
using Microsoft.Practices.EnterpriseLibrary.Data.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Data
{
    /// <summary>
    /// Contains factory methods for creating <see cref="Database"/> objects.
    /// </summary>
    public static class DatabaseFactory
    {
        private static volatile Func<string, Database> createNamedDatabase;
        private static volatile Func<Database> createDefaultDatabase;

        /// <summary>
        /// Creates the default <see cref="Database"/> object
        /// </summary>
        /// <example>
        /// <code>
        /// Database dbSvc = DatabaseFactory.CreateDatabase();
        /// </code>
        /// </example>
        /// <returns>The default database</returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">The configuration information cannot be read.</exception>
        /// <exception cref="System.InvalidOperationException">The database factory has not been intialized or some configuration information is missing.</exception>
        public static Database CreateDatabase()
        {
            return GetCreateDefaultDatabase().Invoke();
        }

        /// <summary>
        /// Creates the <see cref="Database"/> object with the specified name.
        /// </summary>
        /// <example>
        /// <code>
        /// Database dbSvc = DatabaseFactory.CreateDatabase("SQL_Customers");
        /// </code>
        /// </example>
        /// <param name="name">The configuration key for database service</param>
        /// <returns>The database with the specified name</returns>
        /// <exception cref="System.Configuration.ConfigurationErrorsException">The configuration information cannot be read.</exception>
        /// <exception cref="System.InvalidOperationException">The database factory has not been intialized or some configuration information is missing.</exception>
        public static Database CreateDatabase(string name)
        {
            return GetCreateDatabase().Invoke(name);
        }

        /// <summary>
        /// Sets the provider factory for the static <see cref="DatabaseFactory"/>.
        /// </summary>
        /// <param name="factory">The provider factory.</param>
        /// <param name="throwIfSet"><see langword="true"/> to thrown an exception if the factory is already set; otherwise, <see langword="false"/>. Defaults to <see langword="true"/>.</param>
        /// <exception cref="InvalidOperationException">The factory is already set and <paramref name="throwIfSet"/> is <see langword="true"/>.</exception>
        public static void SetDatabaseProviderFactory(DatabaseProviderFactory factory, bool throwIfSet = true)
        {
            Guard.ArgumentNotNull(factory, "factory");

            SetDatabases(factory.CreateDefault, factory.Create, throwIfSet);
        }

        /// <summary>
        /// Sets the database mappings.
        /// </summary>
        /// <param name="createDefaultDatabase">A method that returns the default database.</param>
        /// <param name="createNamedDatabase">A method that returns a database for the specified name.</param>
        /// <param name="throwIfSet"><see langword="true"/> to thrown an exception if the factory is already set; otherwise, <see langword="false"/>. Defaults to <see langword="true"/>.</param>
        /// <exception cref="InvalidOperationException">The factory is already set and <paramref name="throwIfSet"/> is <see langword="true"/>.</exception>
        public static void SetDatabases(Func<Database> createDefaultDatabase, Func<string, Database> createNamedDatabase, bool throwIfSet = true)
        {
            Guard.ArgumentNotNull(createDefaultDatabase, "createDefaultDatabase");
            Guard.ArgumentNotNull(createNamedDatabase, "createNamedDatabase");

            var currentCreateDb = DatabaseFactory.createNamedDatabase;
            var currentCreateDefaultDb = DatabaseFactory.createDefaultDatabase;

            if ((currentCreateDb != null && currentCreateDefaultDb != null) && throwIfSet)
            {
                throw new InvalidOperationException(Resources.ExceptionDatabaseProviderFactoryAlreadySet);
            }

            DatabaseFactory.createDefaultDatabase = createDefaultDatabase;
            DatabaseFactory.createNamedDatabase = createNamedDatabase;
        }

        /// <summary>
        /// Clears the provider factory for the static <see cref="DatabaseFactory"/>.
        /// </summary>
        public static void ClearDatabaseProviderFactory()
        {
            createNamedDatabase = null;
            createDefaultDatabase = null;
        }

        private static Func<string, Database> GetCreateDatabase()
        {
            var func = createNamedDatabase;

            if (func == null)
            {
                throw new InvalidOperationException(Resources.ExceptionDatabaseProviderFactoryNotSet);
            }

            return func;
        }

        private static Func<Database> GetCreateDefaultDatabase()
        {
            var func = createDefaultDatabase;

            if (func == null)
            {
                throw new InvalidOperationException(Resources.ExceptionDatabaseProviderFactoryNotSet);
            }

            return func;
        }
    }
}

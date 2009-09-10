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
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    /// <summary>
    /// Configuration extensions for database types specified via the <see cref="DatabaseProviderExtensions.AnotherDatabaseType" />.
    /// </summary>
    public interface IDatabaseAnotherDatabaseConfiguration : IDatabaseDefaultConnectionString, IDatabaseConfigurationProperties
    {
        /// <summary />
        IDatabaseAnotherDatabaseConfiguration WithConnectionString(System.Data.Common.DbConnectionStringBuilder builder);
    }

    internal class AnotherDatabaseConfigurationExtensions : DatabaseConfigurationExtension, IDatabaseAnotherDatabaseConfiguration
    {
        public AnotherDatabaseConfigurationExtensions(IDatabaseConfigurationProviders context, string providerName) : base(context)
        {
            if (String.IsNullOrEmpty(providerName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "providerName");
            ConnectionString.ProviderName = providerName;
        }

        IDatabaseAnotherDatabaseConfiguration IDatabaseAnotherDatabaseConfiguration.WithConnectionString(DbConnectionStringBuilder builder)
        {
            base.WithConnectionString(builder);
            return this;
        }
    }
}

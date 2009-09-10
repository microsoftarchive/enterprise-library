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

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{

    /// <summary>
    /// SqlCe database configuration options.
    /// </summary>
    public interface IDatabaseSqlCeDatabaseConfiguration : IDatabaseDefaultConnectionString, IDatabaseConfigurationProperties
    { }


    internal class SqlCeDatabaseConfigurationExtension : DatabaseConfigurationExtension, IDatabaseSqlCeDatabaseConfiguration
    {
        public SqlCeDatabaseConfigurationExtension(IDatabaseConfigurationProviders context)
            : base(context)
        {
            base.ConnectionString.ProviderName = "System.Data.SqlServerCe";
        }
    }
}

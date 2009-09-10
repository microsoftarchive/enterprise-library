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

using System.Data.OleDb;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    /// <summary>
    /// OleDb database configuration options.
    /// </summary>
    public interface IOleDbDatabaseConfiguration : IDatabaseDefaultConnectionString, IDatabaseConfigurationProperties
    {
        /// <summary>
        /// Define an OleDb connection with the <see cref="OleDbConnectionStringBuilder"/>
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        IDatabaseConfigurationProperties WithConnectionString(OleDbConnectionStringBuilder builder);
    }

    internal class OleDbConfigurationExtension : DatabaseConfigurationExtension, IOleDbDatabaseConfiguration
    {
        public OleDbConfigurationExtension(IDatabaseConfigurationProviders context)
            : base(context)
        {
            base.ConnectionString.ProviderName = "System.Data.OleDb";
        }

        public IDatabaseConfigurationProperties WithConnectionString(OleDbConnectionStringBuilder builder)
        {
            return base.WithConnectionString(builder);
        }
    }
}

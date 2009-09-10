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

using System.Data.Odbc;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    /// <summary>   
    /// Odbc database configuration options. 
    /// </summary>
    public interface IOdbcDatabaseConfiguration : IDatabaseDefaultConnectionString, IDatabaseConfigurationProperties
    {
        /// <summary>
        /// Define a connection string with the <see cref="OdbcConnectionStringBuilder"/>.
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        IDatabaseConfigurationProperties WithConnectionString(OdbcConnectionStringBuilder builder);
    }


    internal class OdbcConfigurationExtension : DatabaseConfigurationExtension, IOdbcDatabaseConfiguration
    {
        public OdbcConfigurationExtension(IDatabaseConfigurationProviders context) : base(context)
        {
            base.ConnectionString.ProviderName = "System.Data.Odbc";
        }

        public IDatabaseConfigurationProperties WithConnectionString(OdbcConnectionStringBuilder builder)
        {
            return base.WithConnectionString(builder);
        }
    }
}

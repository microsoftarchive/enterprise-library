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

using System.Configuration;
using System.Data.Common;
using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    /// <summary>
    /// Defines default connection string settings for fluent-style interface.
    /// </summary>
    public interface IDatabaseDefaultConnectionString : IFluentInterface
    {
        /// <summary>
        /// Connection string to use for this data source.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        /// <returns></returns>
        /// <seealso cref="ConnectionStringSettings"/>
        /// <seealso cref="DbConnectionStringBuilder"/>
        IDatabaseConfigurationProperties WithConnectionString(string connectionString);
    }
}

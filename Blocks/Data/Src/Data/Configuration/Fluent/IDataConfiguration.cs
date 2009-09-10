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
using System.Data.Common;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data.Oracle;
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    ///<summary>
    /// Starting point for data configuration.
    ///</summary>
    /// <seealso cref="DataConfigurationSourceBuilderExtensions"/>
    public interface IDataConfiguration : IDatabaseConfiguration
    {
        /// <summary>
        /// Specify a custom provider name or alias to use.  This must
        /// map to the name of the invarient name specified by <see cref="DbProviderFactories"/>
        /// </summary>
        /// <remarks>If the provider is not mapped to a specific Enterprise Library <see cref="Database"/> class, then the <see cref="GenericDatabase"/> will be used.</remarks>
        /// <param name="providerName">The name of the database provider's invarient.</param>
        /// <returns></returns>
        IDatabaseProviderConfiguration WithProviderNamed(string providerName);
    }

    /// <summary>
    /// Defines the mapping options for providers.
    /// </summary>
    public interface IDatabaseProviderConfiguration : IDataConfiguration
    {
        /// <summary>
        /// The <see cref="Database"/> to map the provider to.
        /// </summary>
        /// <param name="databaseType">The <see cref="Database"/> type.</param>
        /// <returns></returns>
        /// <seealso cref="GenericDatabase"/>
        /// <seealso cref="SqlDatabase" />
        /// <seealso cref="OracleDatabase" />
        IDataConfiguration MappedToDatabase(Type databaseType);

        /// <summary>
        /// The <see cref="Database"/> to map the provider to.
        /// </summary>
        /// <typeparam name="T">Database type to map to</typeparam>
        /// <returns></returns>
        /// <seealso cref="GenericDatabase"/>
        /// <seealso cref="SqlDatabase" />
        /// <seealso cref="OracleDatabase" />
        IDataConfiguration MappedToDatabase<T>() where T : Data.Database;
    }
}

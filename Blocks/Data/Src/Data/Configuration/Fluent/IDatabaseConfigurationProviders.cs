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

using System.Data.OracleClient;
using System.Data.OleDb;
using System.Data.Odbc;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    ///<summary>
    /// Extension point for database providers to connect to the data configuration fluent-api.
    ///</summary>
    /// <seealso cref="DataConfigurationSourceBuilderExtensions"/>
    /// <seealso cref="DatabaseConfigurationExtension"/>
    public interface IDatabaseConfigurationProviders : IFluentInterface
    {
    }
}

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
    /// Database configuration properties that apply to all databases.
    /// </summary>
    /// <remarks>This interface is intended to support a fluent-style configuration interface.</remarks>
    public interface IDatabaseConfigurationProperties : IDatabaseConfigurationProviderEntry, IDatabaseConfiguration
    {
        ///<summary>
        /// Set this database as the default one in the configuration.
        ///</summary>
        ///<returns></returns>
        IDatabaseConfigurationProperties AsDefault();
    }
}

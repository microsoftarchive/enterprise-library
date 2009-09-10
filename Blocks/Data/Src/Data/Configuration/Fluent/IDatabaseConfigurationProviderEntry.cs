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

using Microsoft.Practices.EnterpriseLibrary.Common;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    ///<summary>
    /// This interface support the database configuration fluent interface.
    ///</summary>
    public interface IDatabaseConfigurationProviderEntry : IFluentInterface
    {
        ///<summary>
        /// Specify the type of database.
        ///</summary>
        IDatabaseConfigurationProviders ThatIs { get; }
    }
}

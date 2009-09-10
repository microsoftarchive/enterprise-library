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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Data.Configuration.Fluent
{
    /// <summary>
    /// Provides extenson context for database provider extensions.
    /// </summary>
    public interface IDatabaseProviderExtensionContext
    {
        ///<summary>
        /// The current connetion string under construction in the fluent interface.
        ///</summary>
        ConnectionStringSettings ConnectionString { get; }

        ///<summary>
        /// Context of the current builder for the extension
        ///</summary>
        IConfigurationSourceBuilder Builder { get;  }
    }
}

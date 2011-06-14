//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// Contains factory methods to create configuration sources.
    /// </summary>
    public class ConfigurationSourceFactory
    {
        /// <summary>
        /// Creates a default configuration source.
        /// </summary>
        /// <returns>The default configuration source.</returns>
        /// <seealso cref="DictionaryConfigurationSource.CreateDefault"/>
        public static IConfigurationSource Create()
        {
            return DictionaryConfigurationSource.CreateDefault();
        }
    }
}

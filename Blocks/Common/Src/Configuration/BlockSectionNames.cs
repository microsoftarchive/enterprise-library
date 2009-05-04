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
    /// A set of string constants listing the names of the configuration
    /// sections used by the standard set of Entlib blocks.
    /// </summary>
    public static class BlockSectionNames
    {
        /// <summary>
        /// Crypto block section name
        /// </summary>
        public const string Cryptography = "securityCryptographyConfiguration";

        /// <summary>
        /// Data block custom settings
        /// </summary>
        public const string Data = "dataConfiguration";

        /// <summary>
        /// Exception handling block section name
        /// </summary>
        public const string ExceptionHandling = "exceptionHandling";

        /// <summary>
        /// Not actually a section name, this is the type name used to get the
        /// TypeRegistrationProviderLocatorStrategy used to retrieve information
        /// for the Data block.
        /// </summary>
        public const string DataRegistrationProviderLocatorType =
            "Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSyntheticConfigSettings, Microsoft.Practices.EnterpriseLibrary.Data";

    }
}

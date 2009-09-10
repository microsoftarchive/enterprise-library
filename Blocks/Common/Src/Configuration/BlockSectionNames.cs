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
        /// Data Access Application Block custom settings
        /// </summary>
        public const string Data = "dataConfiguration";

        /// <summary>
        /// Exception Handling Application Block section name
        /// </summary>
        public const string ExceptionHandling = "exceptionHandling";

        /// <summary>
        /// Caching Application Block section name
        /// </summary>
        public const string Caching = "cachingConfiguration";

        /// <summary>
        /// Security Application Block section name
        /// </summary>
        public const string Security = "securityConfiguration";

        /// <summary>
        /// Logging Application Block section name
        /// </summary>
        public const string Logging = "loggingConfiguration";

        /// <summary>
        /// Instrumentation section name
        /// </summary>
        public const string Instrumentation = "instrumentationConfiguration";

        /// <summary>
        /// Policy injection section name
        /// </summary>
        public const string PolicyInjection = "policyInjection";


        ///<summary>
        /// Validation section name
        ///</summary>
        public const string Validation = "validation";

        /// <summary>
        /// Not actually a section name, this is the type name used to get the
        /// TypeRegistrationProviderLocatorStrategy used to retrieve information
        /// for the Data Access Application Block.
        /// </summary>
        public const string DataRegistrationProviderLocatorType =
            "Microsoft.Practices.EnterpriseLibrary.Data.Configuration.DatabaseSyntheticConfigSettings, Microsoft.Practices.EnterpriseLibrary.Data";

        /// <summary>
        /// Not actually a section name, this is the type name used to get the
        /// TypeRegistrationProviderLocatorStrategy used to retrieve information
        /// for the Validation Application Block.
        /// </summary>
        public const string ValidationRegistrationProviderLocatorType =
            "Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.ValidationTypeRegistrationProvider, Microsoft.Practices.EnterpriseLibrary.Validation";

    }
}

//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{

    /// <summary>
    /// Fluent interface that allows log categories to be set up.
    /// </summary>
    public interface ILoggingConfigurationContd : IFluentInterface
    {
        /// <summary>
        /// Creates a Category Source in the configuration schema with the specified name.
        /// </summary>
        /// <param name="categoryName">The name of the Category Source.</param>
        /// <returns>Fluent interface that allows for this Category Source to be configured further.</returns>
        ILoggingConfigurationCustomCategoryStart LogToCategoryNamed(string categoryName);

        /// <summary>
        /// Returns an interface that can be used to configure special logging categories.
        /// </summary>
        ILoggingConfigurationSpecialSources SpecialSources { get; }
    }

}

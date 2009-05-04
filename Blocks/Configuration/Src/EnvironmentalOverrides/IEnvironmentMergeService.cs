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

using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.EnvironmentalOverrides
{
    /// <summary>
    /// Provides access to contextual information on the merging of environments.
    /// </summary>
    /// <remarks>
    /// This service can usually be retrieved through an <see cref="IServiceProvider"/> instance.
    /// </remarks>
    public interface IEnvironmentMergeService
    {
        /// <summary>
        /// Gets a boolean value indicating wether merging of an environment currently is in progress.
        /// </summary>
        /// <value>
        /// <see langword="true"/> if merging of an environmental configuration currently is in progress. Otherwise <see langword="false"/>.
        /// </value>
        bool EnvironmentMergeInProgress { get; }
    }
}

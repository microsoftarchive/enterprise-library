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
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents a key for manageability configuration implementation.
    /// </summary>
    public struct ImplementationKey
    {
        /// <summary>
        /// Gets or sets the application name.
        /// </summary>
        /// <value>
        /// The application name.
        /// </value>
        public String ApplicationName;

        /// <summary>
        /// Gets or sets weather to enable Group Policies.
        /// </summary>
        /// <value>
        /// true to enable Group Policies; otherwise false.
        /// </value>
        public Boolean EnableGroupPolicies;

        /// <summary>
        /// Gets or sets the configuration file name.
        /// </summary>
        /// <value>
        /// The configuration file name.
        /// </value>
        public String FileName;

        /// <summary>
        /// Initialize a new instance of the <see cref="ImplementationKey"/> struct.
        /// </summary>
        /// <param name="fileName">The configuration file name.</param>
        /// <param name="applicationName">The application name.</param>
        /// <param name="enableGroupPolicies">true to enable Group Policy; otherwise, false.</param>
        public ImplementationKey(String fileName,
                                 String applicationName,
                                 Boolean enableGroupPolicies)
        {
            FileName = fileName != null ? fileName.ToLowerInvariant() : null;
            ApplicationName = applicationName != null ? applicationName.ToLowerInvariant() : null;
            EnableGroupPolicies = enableGroupPolicies;
        }
    }
}

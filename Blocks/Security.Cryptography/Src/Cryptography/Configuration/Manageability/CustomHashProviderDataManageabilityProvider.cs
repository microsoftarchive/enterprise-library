//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="CustomHashProviderData"/> that
    /// processes policy overrides, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class CustomHashProviderDataManageabilityProvider
        : CustomProviderDataManageabilityProvider<CustomHashProviderData>
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="CustomHashProviderDataManageabilityProvider"/> class.
        /// </summary>
        public CustomHashProviderDataManageabilityProvider()
            : base(Resources.HashProviderPolicyNameTemplate)
        {
        }
    }
}

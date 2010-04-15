//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="CustomAuthorizationProviderData"/> that
    /// processes policy overrides, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class CustomAuthorizationProviderDataManageabilityProvider
        : CustomProviderDataManageabilityProvider<CustomAuthorizationProviderData>
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="CustomAuthorizationProviderDataManageabilityProvider"/> class.
        /// </summary>
        public CustomAuthorizationProviderDataManageabilityProvider()
            : base(Resources.AuthorizationProviderPolicyNameTemplate)
        { }
    }
}

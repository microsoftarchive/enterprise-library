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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Security;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a custom authorization provider.
    /// </summary>
    /// <seealso cref="CustomAuthorizationProviderData"/>
    public interface IAuthorizeUsingCustomProvider : IConfigureSecuritySettings, IFluentInterface
    {
        /// <summary>
        /// Specifies the current custom authorization provider as the default authorization provider instance.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure security settings.</returns>
        /// <seealso cref="CustomAuthorizationProviderData"/>
        IConfigureSecuritySettings SetAsDefault();
    }
}

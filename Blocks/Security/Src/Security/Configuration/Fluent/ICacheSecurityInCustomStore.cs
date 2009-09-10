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
    /// Fluent interface used to configure a custom security cache provider.
    /// </summary>
    /// <seealso cref="CustomSecurityCacheProviderData"/>
    public interface ICacheSecurityInCustomStore : IConfigureSecuritySettings
    {        
        /// <summary>
        /// Specifies the current custom security cache provider as the default security cache provider instance.
        /// </summary>
        /// <returns>Fluent interface that can be used to further configure security settings.</returns>
        /// <seealso cref="CustomSecurityCacheProviderData"/>
        IConfigureSecuritySettings SetAsDefault();
    }
}

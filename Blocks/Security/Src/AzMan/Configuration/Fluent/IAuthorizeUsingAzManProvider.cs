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
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent
{
    /// <summary>
    /// Fluent interface used to configure a <see cref="AzManAuthorizationProvider"/> instance.
    /// </summary>
    /// <seealso cref="AzManAuthorizationProvider"/>
    /// <seealso cref="AzManAuthorizationProviderData"/>
    public interface IAuthorizeUsingAzManProvider : IConfigureSecuritySettings
    {
        /// <summary>
        /// Specifies this <see cref="AzManAuthorizationProvider"/> instance as the default <see cref="IAuthorizationProvider"/>.
        /// </summary>
        /// <seealso cref="AzManAuthorizationProvider"/>
        /// <seealso cref="AzManAuthorizationProviderData"/>
        IAuthorizeUsingAzManProvider SetAsDefault();

        /// <summary>
        /// Returns a fluent interface to further configure this <see cref="AzManAuthorizationProvider"/> instance. 
        /// </summary>
        IAuthorizeUsingAzManOptions WithOptions { get; }

    }

    /// <summary>
    /// Fluent interface used to further configure a <see cref="AzManAuthorizationProvider"/> instance.
    /// </summary>
    /// <seealso cref="AzManAuthorizationProvider"/>
    /// <seealso cref="AzManAuthorizationProviderData"/>
    public interface IAuthorizeUsingAzManOptions : IAuthorizeUsingAzManProvider
    {

        //TODOC : needs review
        /// <summary/>
        /// <seealso cref="AzManAuthorizationProvider"/>
        /// <seealso cref="AzManAuthorizationProviderData"/>
        IAuthorizeUsingAzManOptions Scoped(string scope);

        /// <summary/>
        /// <seealso cref="AzManAuthorizationProvider"/>
        /// <seealso cref="AzManAuthorizationProviderData"/>
        IAuthorizeUsingAzManOptions UseStoreFrom(string storeLocation);

        /// <summary/>
        /// <seealso cref="AzManAuthorizationProvider"/>
        /// <seealso cref="AzManAuthorizationProviderData"/>
        IAuthorizeUsingAzManOptions ForApplication(string applicationName);

        /// <summary/>
        /// <seealso cref="AzManAuthorizationProvider"/>
        /// <seealso cref="AzManAuthorizationProviderData"/>
        IAuthorizeUsingAzManOptions UsingAuditIdentifierPrefix(string auditIdentifierPrefix);
    }
}

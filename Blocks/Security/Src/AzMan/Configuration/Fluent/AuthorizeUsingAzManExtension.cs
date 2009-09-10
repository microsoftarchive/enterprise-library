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
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.Practices.EnterpriseLibrary.Common.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration
{
    /// <summary>
    /// <see cref="IConfigureSecuritySettings"/> extensions to support configuring <see cref="AzManAuthorizationProvider"/> instances.
    /// </summary>
    /// <seealso cref="AzManAuthorizationProvider"/>
    /// <seealso cref="AzManAuthorizationProviderData"/>
    public static class AuthorizeUsingAzManExtension
    {
        /// <summary>
        /// Adds a new <see cref="AzManAuthorizationProvider"/> to the security configuration.
        /// </summary>
        /// <param name="context">Fluent interface extension point.</param>
        /// <param name="azManAuthorizationProviderName">The name of the <see cref="AzManAuthorizationProvider"/> instance.</param>
        /// <returns>Fluent interface that can be used to further configure the created <see cref="AzManAuthorizationProviderData"/>. </returns>
        /// <seealso cref="AzManAuthorizationProvider"/>
        /// <seealso cref="AzManAuthorizationProviderData"/>
        public static IAuthorizeUsingAzManProvider AuthorizeUsingAzManProviderNamed(this IConfigureSecuritySettings context, string azManAuthorizationProviderName)
        {
            if (string.IsNullOrEmpty(azManAuthorizationProviderName)) 
                throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "azManAuthorizationProviderName");
            
            return new AuthorizeUsingAzManProviderBuilder(context, azManAuthorizationProviderName);
        }

        private class AuthorizeUsingAzManProviderBuilder : ConfigureSecuritySettingsExtension, IAuthorizeUsingAzManProvider, IAuthorizeUsingAzManOptions
        {
            AzManAuthorizationProviderData azManProviderData;

            public AuthorizeUsingAzManProviderBuilder(IConfigureSecuritySettings context, string azManAuthorizationProviderName)
                : base(context)
            {
                azManProviderData = new AzManAuthorizationProviderData
                {
                    Name = azManAuthorizationProviderName
                };

                base.SecuritySettings.AuthorizationProviders.Add(azManProviderData);
            }

            IAuthorizeUsingAzManProvider IAuthorizeUsingAzManProvider.SetAsDefault()
            {
                base.SecuritySettings.DefaultAuthorizationProviderName = azManProviderData.Name;

                return this;
            }
           
            public IAuthorizeUsingAzManOptions WithOptions
            {
                get { return this; ; }
            }

            public IAuthorizeUsingAzManOptions Scoped(string scope)
            {
                azManProviderData.Scope = scope;

                return this;
            }

            public IAuthorizeUsingAzManOptions UseStoreFrom(string storeLocation)
            {
                azManProviderData.StoreLocation = storeLocation;

                return this;
            }

            public IAuthorizeUsingAzManOptions ForApplication(string applicationName)
            {
                azManProviderData.Application = applicationName;

                return this;
            }

            public IAuthorizeUsingAzManOptions UsingAuditIdentifierPrefix(string auditIdentifierPrefix)
            {
                azManProviderData.AuditIdentifierPrefix = auditIdentifierPrefix;

                return this;
            }
        }
    }
}

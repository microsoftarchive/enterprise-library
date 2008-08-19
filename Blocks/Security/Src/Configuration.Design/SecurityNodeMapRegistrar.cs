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
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    sealed class SecurityNodeMapRegistrar:  NodeMapRegistrar
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        public SecurityNodeMapRegistrar(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Register()
        {

            AddMultipleNodeMap(Resources.CustomAuthorizationProviderCommandName,
                typeof(CustomAuthorizationProviderNode),
                typeof(CustomAuthorizationProviderData));

            AddMultipleNodeMap(Resources.CustomSecurityCacheNodeCommandName,
                typeof(CustomSecurityCacheProviderNode),
                typeof(CustomSecurityCacheProviderData));

            AddMultipleNodeMap(Resources.AuthorizationRuleProviderCommandName,
                typeof(AuthorizationRuleProviderNode),
                typeof(AuthorizationRuleProviderData));
        }
    }
}

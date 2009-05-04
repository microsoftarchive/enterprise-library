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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design
{
    sealed class SecurityAzManCommandRegistrar: CommandRegistrar
    {
        public SecurityAzManCommandRegistrar(IServiceProvider serviceProvider)
            :base(serviceProvider)
        {
        }

        public override void Register()
        {
            AddMultipleChildNodeCommand(Resources.AzManProvider,
                string.Format(Resources.Culture, Resources.GenericCreateStatusText, Resources.AzManProvider),
                typeof(AzManAuthorizationProviderNode), typeof(AuthorizationProviderCollectionNode));

            AddDefaultCommands(typeof(AzManAuthorizationProviderNode));

        }
    }
}

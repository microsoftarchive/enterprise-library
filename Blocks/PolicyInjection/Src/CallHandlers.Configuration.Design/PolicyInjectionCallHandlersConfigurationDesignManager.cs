//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
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

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// A <see cref="ConfigurationDesignManager"/> that registers the
    /// commands and node maps for the Policy Injection Block configuration.
    /// </summary>
    public class PolicyInjectionCallHandlersConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// Performs the registration of commands and node maps.
        /// </summary>
        /// <param name="serviceProvider"><see cref="IServiceProvider"/> to use to perform the registration.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
            new PolicyInjectionCallHandlerCommandRegistrar(serviceProvider).Register();

            new PolicyInjectionCallHandlerNodeMapRegistrar(serviceProvider).Register();

        }
    }
}

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
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.Properties;
using System.Windows.Forms;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design
{
    /// <summary>
    /// Configuration Design Manager for AzMan. <seealso cref="IConfigurationDesignManager"/>.
    /// </summary>
    public class SecurityAzManConfigurationDesignManager : ConfigurationDesignManager
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityAzManConfigurationDesignManager"/> class.
        /// </summary>
        public SecurityAzManConfigurationDesignManager()
        {
        }

        /// <summary>
        /// <para>Registers the <see cref="AzManAuthorizationProviderNode"/> in the application.</para>
        /// </summary>
        /// <param name="serviceProvider">
        /// <para>The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</para>
        /// </param>
        public override void Register(IServiceProvider serviceProvider)
        {
            SecurityAzManNodeMapRegisrar nodeMapRegistrar = new SecurityAzManNodeMapRegisrar(serviceProvider);
            nodeMapRegistrar.Register();

            SecurityAzManCommandRegistrar commandRegistrar = new SecurityAzManCommandRegistrar(serviceProvider);
            commandRegistrar.Register();
        }
    }
}
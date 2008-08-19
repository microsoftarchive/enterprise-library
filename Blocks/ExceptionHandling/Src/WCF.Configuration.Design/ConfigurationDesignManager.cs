//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Text;
using System.Configuration;
using System.Collections.Generic;
using System.Diagnostics;
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Design
{
    /// <summary>
    /// Represents the design manager for the WCF exception handler.
    /// </summary>
    public sealed class ConfigurationDesignManager : Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ConfigurationDesignManager
    {
        /// <summary>
        /// Register the commands and node maps needed for the design manager into the design time.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        public override void Register(IServiceProvider serviceProvider)
        {
            CommandRegistrar cmdRegistrar = new CommandRegistrar(serviceProvider);
            cmdRegistrar.Register();
            NodeMapRegistrar nodeRegistrar = new NodeMapRegistrar(serviceProvider);
            nodeRegistrar.Register();
        }

        /// <summary>
        /// Initializes the WCF.
        /// </summary>
        /// <param name="serviceProvider">The a mechanism for retrieving a service object; that is, an object that provides custom support to other objects.</param>
        /// <param name="rootNode">The root node of the application.</param>
        /// <param name="section">The <see cref="ConfigurationSection"/> that was opened from the <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.IConfigurationSource"/>.</param>
        protected override void OpenCore(IServiceProvider serviceProvider, ConfigurationApplicationNode rootNode, System.Configuration.ConfigurationSection section)
        {
            base.OpenCore(serviceProvider, rootNode, section);
        }
    }
}

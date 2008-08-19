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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design
{
    /// <summary>
    /// Represents an injection configuration node.
    /// </summary>
    [Image(typeof(PolicyInjectionSettingsNode))]
    public abstract class InjectorNode : ConfigurationNode
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="InjectorNode"/> class
        /// </summary>
        /// <param name="data">The <see cref="InjectorData"/></param> to use.
        protected InjectorNode(InjectorData data)
            : base(data.Name) {}

        /// <summary>
        /// Gets the configuration data for this node.
        /// </summary>
        /// <returns>The configuration data for this node.</returns>
        public abstract InjectorData GetConfigurationData();
    }
}
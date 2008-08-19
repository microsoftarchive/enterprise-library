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

using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection
{
    /// <summary>
    /// A factory class that can create <see cref="PolicyInjector"/>s based
    /// on configuration.
    /// </summary>
    public class PolicyInjectorFactory
    {
        private readonly IConfigurationSource configurationSource;

        /// <summary>
        /// Creates a new <see cref="PolicyInjectorFactory"/> that creates
        /// <see cref="PolicyInjector"/>s based on the default configuration.
        /// </summary>
        public PolicyInjectorFactory()
            : this(ConfigurationSourceFactory.Create())
        {

        }

        /// <summary>
        /// Creates a new <see cref="PolicyInjectorFactory"/> that creates
        /// <see cref="PolicyInjector"/>s based on the given configuration source.
        /// </summary>
        /// <param name="configurationSource"><see cref="IConfigurationSource"/>
        /// containing the information about the configured injectors.</param>
        public PolicyInjectorFactory(IConfigurationSource configurationSource)
        {
            this.configurationSource = configurationSource;
        }

        /// <summary>
        /// Creates a new <see cref="PolicyInjector"/> based on the configured
        /// injector and the policy set defined in the configuration source.
        /// </summary>
        /// <returns>The new injector object.</returns>
        public PolicyInjector Create()
        {
            return EnterpriseLibraryFactory.BuildUp<PolicyInjector>(configurationSource);
        }

        /// <summary>
        /// Creates a new <see cref="PolicyInjector"/> by the name given in the
        /// specified configuration source.
        /// </summary>
        /// <param name="name">Name of the injector to create as given in config file.</param>
        /// <returns>new injector.</returns>
        public PolicyInjector Create(string name)
        {
            return EnterpriseLibraryFactory.BuildUp<PolicyInjector>(name, configurationSource);
        }
    }
}

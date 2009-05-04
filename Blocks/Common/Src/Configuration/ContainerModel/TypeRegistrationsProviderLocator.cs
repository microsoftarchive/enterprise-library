//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// This class encapsulates the logic used to find the type registration providers
    /// in the current application.
    /// </summary>
    public class TypeRegistrationsProviderLocator
    {
        private readonly List<TypeRegistrationsProviderLocationStrategy> strategies;

        /// <summary>
        /// Create a new <see cref="TypeRegistrationsProviderLocator"/> that uses the default
        /// list of strategies.
        /// </summary>
        public TypeRegistrationsProviderLocator()
            : this(GetDefaultLocatorStrategies())
        {

        }

        /// <summary>
        /// Create a new <see cref="TypeRegistrationsProviderLocator"/> instance that searches
        /// a <see cref="IConfigurationSource"/> for configuration sections.
        /// </summary>
        /// <param name="strategies">The <see cref="TypeRegistrationsProviderLocationStrategy"/> objects to use
        /// when searching for specific providers.</param>
        public TypeRegistrationsProviderLocator(IEnumerable<TypeRegistrationsProviderLocationStrategy> strategies)
        {
            this.strategies = new List<TypeRegistrationsProviderLocationStrategy>(strategies);
        }

        /// <summary>
        /// Create a new <see cref="TypeRegistrationsProviderLocator"/> instance that searches
        /// a <see cref="IConfigurationSource"/> for configuration sections.
        /// </summary>
        /// <param name="strategies">The <see cref="TypeRegistrationsProviderLocationStrategy"/> objects to use
        /// when searching for specific providers.</param>
        public TypeRegistrationsProviderLocator(params TypeRegistrationsProviderLocationStrategy[] strategies)
            : this((IEnumerable<TypeRegistrationsProviderLocationStrategy>) strategies)
        {
        }

        /// <summary>
        /// Add a new <see cref="TypeRegistrationsProviderLocationStrategy"/> to the list this object
        /// uses to look for configuration information.
        /// </summary>
        /// <param name="newStrategy">Strategy object to add.</param>
        public void AddStrategy(TypeRegistrationsProviderLocationStrategy newStrategy)
        {
            strategies.Add(newStrategy);
        }

        /// <summary>
        /// Remove the named strategy from the list.
        /// </summary>
        /// <param name="name">Name of strategy to remove.</param>
        public void RemoveStrategy(string name)
        {
            strategies.RemoveAll(s => s.Name == name);
        }

        /// <summary>
        /// Returns the sequence of <see cref="ITypeRegistrationsProvider"/> object that need to be
        /// used to configure a container.
        /// </summary>
        /// <param name="configurationSource"><see cref="IConfigurationSource"/> implementation
        /// that contains the configuration information.</param>
        /// <returns>The sequence of <see cref="ITypeRegistrationsProvider"/> objects needed to completely
        /// configure a container according to the given configuration source.</returns>
        public IEnumerable<ITypeRegistrationsProvider> GetProviders(IConfigurationSource configurationSource)
        {
            return strategies
                .Select(strategy => strategy.GetProvider(configurationSource))
                .Where(provider => provider != null);
        }

        /// <summary>
        /// Returns the set of <see cref="TypeRegistrationsProviderLocationStrategy"/> objects used
        /// by default to configure entlib blocks.
        /// </summary>
        /// <returns>The set of <see cref="TypeRegistrationsProviderLocationStrategy"/> objects.</returns>
        public static ICollection<TypeRegistrationsProviderLocationStrategy> GetDefaultLocatorStrategies()
        {
            return new TypeRegistrationsProviderLocationStrategy[]
                       {
                           new ConfigSectionLocationStrategy(BlockSectionNames.Cryptography),
                           new ConfigSectionLocationStrategy(BlockSectionNames.ExceptionHandling),
                           new TypeLoadingLocationStrategy(
                               BlockSectionNames.DataRegistrationProviderLocatorType)
                       };
        }
    }
}

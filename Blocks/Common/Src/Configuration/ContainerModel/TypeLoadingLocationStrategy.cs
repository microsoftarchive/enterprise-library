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

using System;
using System.Reflection;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel
{
    /// <summary>
    /// A <see cref="TypeRegistrationsProviderLocationStrategy"/> implementation that
    /// loads a type by name, and returns an instance of that type as the provider.
    /// </summary>
    /// <remarks>
    /// This is primarily used to support the Data block's configuration provider, which
    /// has to pull stuff from several spots. Also, we load by name rather than
    /// using a type object directly to avoid a compile time dependency from Common on the
    /// Data assembly.
    /// </remarks>
    public class TypeLoadingLocationStrategy : TypeRegistrationsProviderLocationStrategy
    {
        private readonly string typeName;

        /// <summary>
        /// Construct a <see cref="TypeLoadingLocationStrategy"/> that will use the
        /// type named in <paramref name="typeName"/> as the provider.
        /// </summary>
        /// <param name="typeName">type to construct as a provider. This type must have a single argument
        /// constructor that takes an <see cref="IConfigurationSource"/> parameter.</param>
        public TypeLoadingLocationStrategy(string typeName)
        {
            this.typeName = typeName;
        }

        /// <summary>
        /// Name that identifies this particular strategy object.
        /// </summary>
        public override string Name
        {
            get { return typeName; }
        }

        /// <summary>
        /// Find a single <see cref="ITypeRegistrationsProvider"/> object.
        /// </summary>
        /// <param name="configurationSource">Configuration source to pull config information from.</param>
        /// <returns>The <see cref="ITypeRegistrationsProvider"/> object, or null if no such object exists.</returns>
        public override ITypeRegistrationsProvider GetProvider(IConfigurationSource configurationSource)
        {
            Type providerType = Type.GetType(typeName);
            if (providerType == null) return null;

            return (ITypeRegistrationsProvider)Activator.CreateInstance(providerType, configurationSource);
        }
    }
}

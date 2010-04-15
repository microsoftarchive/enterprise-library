//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Cryptography Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Represents the common configuration data for all symmetric cryptography providers.</para>
    /// </summary>
    public class SymmetricProviderData : NameTypeConfigurationElement
    {
        /// <summary>
        /// <para>Initializes a new instance of <see cref="SymmetricProviderData"/> class.</para>
        /// </summary>
        public SymmetricProviderData() 
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="SymmetricProviderData"/> class with a name and a <see cref="ISymmetricCryptoProvider"/>.
        /// </summary>
        /// <param name="type">The type of <see cref="ISymmetricCryptoProvider"/>.</param>
        public SymmetricProviderData(Type type) : this(null, type)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="SymmetricProviderData"/> class with a name and a <see cref="ISymmetricCryptoProvider"/>.
        /// </summary>
        /// <param name="name">The name of the configured <see cref="ISymmetricCryptoProvider"/>.</param>
        /// <param name="type">The type of <see cref="ISymmetricCryptoProvider"/>.</param>
        public SymmetricProviderData(string name, Type type)
            : base(name, type)
        {
        }

        /// <summary>
        /// Creates a <see cref="TypeRegistration"/> instance describing the provider represented by 
        /// this configuration object.
        /// </summary>
        /// <returns>A <see cref="TypeRegistration"/> instance describing a provider.</returns>
        public virtual IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            // Cannot make abstract for serialization reasons.
            throw new NotImplementedException("Must be implemented by subclasses.");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="configurationSource"></param>
        /// <returns></returns>
        protected TypeRegistration GetInstrumentationProviderRegistration(IConfigurationSource configurationSource)
        {
            var instrumentationSection = InstrumentationConfigurationSection.GetSection(configurationSource);

            return new TypeRegistration<ISymmetricAlgorithmInstrumentationProvider>(
                () => new SymmetricAlgorithmInstrumentationProvider(
                    Name,
                    instrumentationSection.PerformanceCountersEnabled,
                    instrumentationSection.EventLoggingEnabled,
                    instrumentationSection.ApplicationInstanceName))
            {
                Name = Name,
                Lifetime = TypeRegistrationLifetime.Transient
            };
        }
    }
}

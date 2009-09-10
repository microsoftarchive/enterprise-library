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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// Configuration settings for the <c>HashAlgorithm</c> hash provider.
    /// </summary>	
    public class HashAlgorithmProviderData : HashProviderData
    {
        private AssemblyQualifiedTypeNameConverter typeConverter = new AssemblyQualifiedTypeNameConverter();

        private const string algorithmTypeProperty = "algorithmType";
        private const string saltEnabledProperty = "saltEnabled";

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        public HashAlgorithmProviderData() : base(typeof(HashAlgorithmProvider))
        {
        }

        /// <summary>
        /// Initializes with default configuration.
        /// </summary>
        /// <param name="type">The type of the <see cref="IHashProvider"/>.</param>
        public HashAlgorithmProviderData(Type type)
            : base(type)
        {
        }

        /// <summary>
        /// Initialized a new HashAlgorithmProviderData with the given name
        /// </summary>
        /// <param name="name">The name for this object</param>
        /// <param name="algorithmType">The hash algorithm to use</param>
        /// <param name="saltEnabled">Should a salt be used?</param>
        public HashAlgorithmProviderData(string name, Type algorithmType, bool saltEnabled)
            : this(name, typeof(HashAlgorithmProvider), algorithmType, saltEnabled)
        {
        }

        /// <summary>
        /// Initialized a new HashAlgorithmProviderData with the given name
        /// </summary>
        /// <param name="name">The name for this object</param>
        /// <param name="providerType">The</param>
        /// <param name="algorithmType">The hash algorithm to use</param>
        /// <param name="saltEnabled">Should a salt be used?</param>
        protected HashAlgorithmProviderData(string name, Type providerType, Type algorithmType, bool saltEnabled)
            : base(name, providerType)
        {
            AlgorithmType = algorithmType;
            SaltEnabled = saltEnabled;
        }

        /// <summary>
        /// Gets or sets the type of <see cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </summary>
        public Type AlgorithmType
        {
            get { return (Type)typeConverter.ConvertFrom(AlgorithmTypeName); }
            set { AlgorithmTypeName = typeConverter.ConvertToString(value); }
        }

        /// <summary>
        /// Gets or sets the fully qualified name of the type of <see cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </summary>
        /// <value>
        /// The fully qualified type name of the type of <see cref="System.Security.Cryptography.HashAlgorithm"/>.
        /// </value>
        [ConfigurationProperty(algorithmTypeProperty, IsRequired = true)]
        public string AlgorithmTypeName
        {
            get { return (string)this[algorithmTypeProperty]; }
            set { this[algorithmTypeProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the salt enabled flag.
        /// </summary>
        [ConfigurationProperty(saltEnabledProperty, IsRequired = true, DefaultValue=true)]
        public bool SaltEnabled
        {
            get { return (bool)this[saltEnabledProperty]; }
            set { this[saltEnabledProperty] = value; }
        }

        /// <summary>
        /// Creates a <see cref="TypeRegistration"/> instance describing the provider represented by 
        /// this configuration object.
        /// </summary>
        /// <param name="configurationSource">TODO</param>
        /// <returns>A <see cref="TypeRegistration"/> instance describing a provider.</returns>
        public override IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
        {
            yield return base.GetInstrumentationProviderRegistration(configurationSource);

            yield return
                new TypeRegistration<IHashProvider>(
                    () => new HashAlgorithmProvider(AlgorithmType, SaltEnabled, Container.Resolved<IHashAlgorithmInstrumentationProvider>(Name)))
                        {
                            Name = Name,
                            Lifetime = TypeRegistrationLifetime.Transient
                        };


        }
    }
}

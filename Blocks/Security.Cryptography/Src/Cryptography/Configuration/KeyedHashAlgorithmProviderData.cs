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
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration settings for the <c>KeyedHashAlgorithm</c> hash provider.</para>
    /// </summary>
    [Assembler(typeof(KeyedHashAlgorithmProviderAssembler))]
    public class KeyedHashAlgorithmProviderData : HashAlgorithmProviderData
    {
        private const string protectedKeyFilename = "protectedKeyFilename";
        private const string protectedKeyProtectionScope = "protectedKeyProtectionScope";

        /// <summary>
        /// <para>Initializes a new instance of <see cref="KeyedHashAlgorithmProviderData"/> class.</para>
        /// </summary>
        public KeyedHashAlgorithmProviderData()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="name"></param>
        /// <param name="algorithmType"></param>
        /// <param name="saltEnabled"></param>
        /// <param name="protectedKeyFilename"></param>
        /// <param name="protectedKeyProtectionScope"></param>
        public KeyedHashAlgorithmProviderData(string name, Type algorithmType, bool saltEnabled, string protectedKeyFilename, DataProtectionScope protectedKeyProtectionScope)
            : base(name, typeof(KeyedHashAlgorithmProvider), algorithmType, saltEnabled)
        {
            ProtectedKeyProtectionScope = protectedKeyProtectionScope;
            ProtectedKeyFilename = protectedKeyFilename;
        }


        /// <summary>
        /// 
        /// </summary>	
        [ConfigurationProperty(protectedKeyFilename)]
        public string ProtectedKeyFilename
        {
            get { return (string)this[protectedKeyFilename]; }
            set { this[protectedKeyFilename] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(protectedKeyProtectionScope)]
        public DataProtectionScope ProtectedKeyProtectionScope
        {
            get { return (DataProtectionScope)this[protectedKeyProtectionScope]; }
            set { this[protectedKeyProtectionScope] = value; }
        }

        /// <summary>
        /// Creates a <see cref="TypeRegistration"/> instance describing the provider represented by 
        /// this configuration object.
        /// </summary>
        /// <returns>A <see cref="TypeRegistration"/> instance describing a provider.</returns>
        public override TypeRegistration GetContainerConfigurationModel()
        {
            return
                new TypeRegistration<IHashProvider>(
                    () =>
                        new KeyedHashAlgorithmProvider(
                            AlgorithmType,
                            SaltEnabled,
                            ProtectedKeyFilename,
                            ProtectedKeyProtectionScope))
                {
                    Name = Name
                };
        }
    }

    /// <summary>
    /// This type supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
    /// Represents the process to build a <see cref="KeyedHashAlgorithmProvider"/> described by a <see cref="KeyedHashAlgorithmProviderData"/> configuration object.
    /// </summary>
    /// <remarks>This type is linked to the <see cref="KeyedHashAlgorithmProviderData"/> type and it is used by the <see cref="HashProviderCustomFactory"/> 
    /// to build the specific <see cref="IHashProvider"/> object represented by the configuration object.
    /// </remarks>
    public class KeyedHashAlgorithmProviderAssembler : IAssembler<IHashProvider, HashProviderData>
    {
        /// <summary>
        /// This method supports the Enterprise Library infrastructure and is not intended to be used directly from your code.
        /// Builds a <see cref="KeyedHashAlgorithmProvider"/> based on an instance of <see cref="KeyedHashAlgorithmProviderData"/>.
        /// </summary>
        /// <seealso cref="HashProviderCustomFactory"/>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build. Must be an instance of <see cref="KeyedHashAlgorithmProviderData"/>.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of <see cref="KeyedHashAlgorithmProvider"/>.</returns>
        public IHashProvider Assemble(IBuilderContext context, HashProviderData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            KeyedHashAlgorithmProviderData castedObjectConfiguration
                = (KeyedHashAlgorithmProviderData)objectConfiguration;

            IHashProvider createdObject
                = new KeyedHashAlgorithmProvider(
                    castedObjectConfiguration.AlgorithmType,
                    castedObjectConfiguration.SaltEnabled,
                    castedObjectConfiguration.ProtectedKeyFilename,
                    castedObjectConfiguration.ProtectedKeyProtectionScope);

            return createdObject;
        }
    }
}

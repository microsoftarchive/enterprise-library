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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration
{
    /// <summary>
    /// <para>Configuration settings for the <c>KeyedHashAlgorithm</c> hash provider.</para>
    /// </summary>
    [Command(CommonDesignTime.CommandTypeNames.HiddenCommand, 
           CommandPlacement = CommandPlacement.ContextAdd,
            Replace = CommandReplacement.DefaultAddCommandReplacement)]
    [ViewModel(CryptographyDesignTime.ViewModelTypeNames.KeyedHashAlgorithmProviderDataViewModel)]
    public class KeyedHashAlgorithmProviderData : HashAlgorithmProviderData
    {
        private const string protectedKeyFilename = "protectedKeyFilename";
        private const string protectedKeyProtectionScope = "protectedKeyProtectionScope";

        /// <summary>
        /// <para>Initializes a new instance of <see cref="KeyedHashAlgorithmProviderData"/> class.</para>
        /// </summary>
        public KeyedHashAlgorithmProviderData() : base(typeof(KeyedHashAlgorithmProvider))
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
        [ResourceDescription(typeof(DesignResources), "KeyedHashAlgorithmProviderDataProtectedKeyFilenameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "KeyedHashAlgorithmProviderDataProtectedKeyFilenameDisplayName")]
        [System.ComponentModel.Browsable(false)]
        public string ProtectedKeyFilename
        {
            get { return (string)this[protectedKeyFilename]; }
            set { this[protectedKeyFilename] = value; }
        }

        /// <summary>
        /// 
        /// </summary>
        [ConfigurationProperty(protectedKeyProtectionScope)]
        [ResourceDescription(typeof(DesignResources), "KeyedHashAlgorithmProviderDataProtectedKeyProtectionScopeDescription")]
        [ResourceDisplayName(typeof(DesignResources), "KeyedHashAlgorithmProviderDataProtectedKeyProtectionScopeDisplayName")]
        [System.ComponentModel.Browsable(false)]
        public DataProtectionScope ProtectedKeyProtectionScope
        {
            get { return (DataProtectionScope)this[protectedKeyProtectionScope]; }
            set { this[protectedKeyProtectionScope] = value; }
        }


        /// <summary>
        /// 
        /// </summary>
        [System.ComponentModel.Editor(CommonDesignTime.EditorTypes.TypeSelector, CommonDesignTime.EditorTypes.UITypeEditor)]
        [BaseType(typeof(KeyedHashAlgorithm))]
        [ResourceDescription(typeof(DesignResources), "KeyedHashAlgorithmProviderDataAlgorithmTypeNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "KeyedHashAlgorithmProviderDataAlgorithmTypeNameDisplayName")]
        public override string AlgorithmTypeName
        {
            get { return base.AlgorithmTypeName; }
            set { base.AlgorithmTypeName = value; }
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
                    () =>
                        new KeyedHashAlgorithmProvider(
                            AlgorithmType,
                            SaltEnabled,
                            ProtectedKeyFilename,
                            ProtectedKeyProtectionScope,
                            Container.Resolved<IHashAlgorithmInstrumentationProvider>(Name)))
                {
                    Name = Name,
                    Lifetime = TypeRegistrationLifetime.Transient
                };
        }
    }
}

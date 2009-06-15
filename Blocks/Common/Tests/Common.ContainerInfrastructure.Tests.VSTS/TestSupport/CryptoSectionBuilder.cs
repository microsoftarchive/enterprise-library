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
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;

namespace Common.ContainerInfrastructure.Tests.VSTS.TestSupport
{
    /// <summary>
    /// A test helper class that makes it easier to set up and fill
    /// in a Crytpgraphy block configuration section.
    /// </summary>
    public class CryptoSectionBuilder : IAddHashProvider, IAddTo
    {
        private readonly List<HashProviderBuilder> hashProviderBuilders = new List<HashProviderBuilder>();

        public INamedHashProvider AddHashProvider(Type providerType)
        {
            var builder = new HashProviderBuilder(this, providerType);
            hashProviderBuilders.Add(builder);
            return builder;
        }

        public INamedHashProvider AddHashProvider<TProvider>()
        {
            return AddHashProvider(typeof(TProvider));
        }

        public void AddTo(IConfigurationSource configurationSource)
        {
            var section = new CryptographySettings();
            foreach (var builder in hashProviderBuilders)
            {
                builder.AddTo(section);
            }
            configurationSource.Add(null, BlockSectionNames.Cryptography, section);
        }

        private class HashProviderBuilder
            : INamedHashProvider, ISetDefaultHashProviderAddHashProviderAddTo
        {
            private readonly CryptoSectionBuilder outer;
            private readonly HashProviderData providerSettings = new HashProviderData();
            private bool isDefault;

            public HashProviderBuilder(CryptoSectionBuilder outer, Type providerType)
            {
                this.outer = outer;
                providerSettings.Type = providerType;
            }

            public void AddTo(CryptographySettings settings)
            {
                var providerSetting = new HashAlgorithmProviderData(providerSettings.Name, providerSettings.Type, true);
                settings.HashProviders.Add(providerSetting);
                if (isDefault)
                {
                    settings.DefaultHashProviderName = providerSetting.Name;
                }
            }

            ISetDefaultHashProviderAddHashProviderAddTo INamedHashProvider.Named(string name)
            {
                providerSettings.Name = name;
                return this;
            }

            CryptoSectionBuilder ISetDefaultHashProvider.AsDefault
            {
                get
                {
                    isDefault = true;
                    return outer;

                }
            }

            INamedHashProvider IAddHashProvider.AddHashProvider<TProvider>()
            {
                return outer.AddHashProvider<TProvider>();
            }

            INamedHashProvider IAddHashProvider.AddHashProvider(Type providerType)
            {
                return outer.AddHashProvider(providerType);
            }

            void IAddTo.AddTo(IConfigurationSource configurationSource)
            {
                outer.AddTo(configurationSource);
            }
        }
    }

    #region Fluent interface interfaces

    public interface IAddHashProvider
    {
        INamedHashProvider AddHashProvider<TProvider>();

        INamedHashProvider AddHashProvider(Type providerType);
    }

    public interface INamedHashProvider
    {
        ISetDefaultHashProviderAddHashProviderAddTo Named(string name);
    }

    public interface ISetDefaultHashProvider
    {
        CryptoSectionBuilder AsDefault { get; }
    }

    public interface IAddTo
    {
        void AddTo(IConfigurationSource configurationSource);
    }

    public interface ISetDefaultHashProviderAddHashProviderAddTo :
        ISetDefaultHashProvider, IAddHashProvider, IAddTo { }

    #endregion
}

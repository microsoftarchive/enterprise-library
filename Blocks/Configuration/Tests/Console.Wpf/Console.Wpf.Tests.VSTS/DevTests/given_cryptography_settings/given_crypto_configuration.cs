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
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.Tests.VSTS.TestSupport;

namespace Console.Wpf.Tests.VSTS.DevTests.given_cryptography_settings
{

    public abstract class given_crypto_configuration :ContainerContext
    {
        protected CryptographySettings CryptoConfiguration;
        protected SectionViewModel CryptographyModel;

        protected override void Arrange()
        {
            base.Arrange();

            var sourceBuilder = new ConfigurationSourceBuilder();

            sourceBuilder.ConfigureCryptograpy()
                .EncryptUsingDPAPIProviderNamed("DPapi Provider")
                .EncryptUsingHashAlgorithmProviderNamed("HashAlgoProvider")
                .EncryptUsingKeyedHashAlgorithmProviderNamed("keyed hash provider")
                .EncryptUsingSymmetricAlgorithmProviderNamed("Symm Instance Provider")
                .EncryptUsingHashAlgorithmProviderNamed("Hash Provider 2");


            DictionaryConfigurationSource source = new DictionaryConfigurationSource();
            sourceBuilder.UpdateConfigurationWithReplace(source);

            CryptoConfiguration = (CryptographySettings)source.GetSection(CryptographySettings.SectionName);
            CryptographyModel = SectionViewModel.CreateSection(Container, CryptographySettings.SectionName, CryptoConfiguration);
        }

    }
}

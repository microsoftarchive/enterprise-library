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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration.Fluent
{
    public abstract class Given_ConfigurationSourceBuilder : ArrangeActAssert
    {
        protected IConfigurationSourceBuilder ConfigurationSourceBuilder;

        protected override void Arrange()
        {
            ConfigurationSourceBuilder = new ConfigurationSourceBuilder();
        }


        protected IConfigurationSource GetConfigurationSource()
        {
            IConfigurationSource configurationSource = new DictionaryConfigurationSource();
            ConfigurationSourceBuilder.UpdateConfigurationWithReplace(configurationSource);

            return configurationSource;
        }
    }

    [TestClass]
    public class When_ConfiguringCryptographySettingsOnConfiguration : Given_ConfigurationSourceBuilder
    {
        IConfigureCryptography ConfigureCryptography;

        protected override void Act()
        {
            ConfigureCryptography = base.ConfigurationSourceBuilder.ConfigureCryptography();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsCryptographySettings()
        {
            var configurationSource = GetConfigurationSource();
            Assert.IsNotNull(configurationSource.GetSection(CryptographySettings.SectionName));
        }
    }

    public abstract class Given_CryptographySettingsInConfigurationSourceBuilder : Given_ConfigurationSourceBuilder
    {
        protected IConfigureCryptography ConfigureCryptography;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureCryptography = ConfigurationSourceBuilder.ConfigureCryptography();
        }


        protected CryptographySettings GetCryptographySettings()
        {
            var configurationSource = GetConfigurationSource();
            return (CryptographySettings)configurationSource.GetSection(CryptographySettings.SectionName);
        }


        protected class CustomHashProvider : IHashProvider
        {
            public byte[] CreateHash(byte[] plaintext)
            {
                throw new NotImplementedException();
            }

            public bool CompareHash(byte[] plaintext, byte[] hashedtext)
            {
                throw new NotImplementedException();
            }
        }

        protected class CustomSymmetricProvider : ISymmetricCryptoProvider
        {
            public byte[] Encrypt(byte[] plaintext)
            {
                throw new NotImplementedException();
            }

            public byte[] Decrypt(byte[] ciphertext)
            {
                throw new NotImplementedException();
            }
        }
    }
}

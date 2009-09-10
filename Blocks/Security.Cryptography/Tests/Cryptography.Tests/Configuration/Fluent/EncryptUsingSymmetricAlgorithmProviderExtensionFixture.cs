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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using System.Collections.Specialized;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration.Fluent
{
    [TestClass]
    public class When_AddingSymmetricAlgorithmProviderToConfigureCryptographyPassingNullName: Given_CryptographySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_EncryptUsingSymmetricAlgorithmProviderNamed_ThrowsArgumentException()
        {
            ConfigureCryptography
                .EncryptUsingSymmetricAlgorithmProviderNamed(null);
        }
    }
    [TestClass]
    public class When_AddingSymmetricAlgorithmProviderToConfigureCryptography : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureCryptography
                .EncryptUsingSymmetricAlgorithmProviderNamed("symm algo provider");
        }

        [TestMethod]
        public void Then_SymmetricAlgorithmProviderIsContainedInSymmetricCryptoProvider()
        {
            Assert.IsTrue(base.GetCryptographySettings().SymmetricCryptoProviders.OfType<SymmetricAlgorithmProviderData>().Any());
        }

        [TestMethod]
        public void Then_SymmetricAlgorithmProviderHasTypeSet()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<SymmetricAlgorithmProviderData>().First();
            Assert.AreEqual(typeof(SymmetricAlgorithmProvider), providerData.Type);
        }

        [TestMethod]
        public void Then_SymmetricAlgorithmProviderHasHasSpecifiedName()
        {
            var providerData = base.GetCryptographySettings().SymmetricCryptoProviders.OfType<SymmetricAlgorithmProviderData>().First();
            Assert.AreEqual("symm algo provider", providerData.Name);
        }
    }

    public abstract class Given_SymmetricAlgorithmProviderInConfiguration : Given_CryptographySettingsInConfigurationSourceBuilder
    {
        protected IEncryptUsingSymmetricProviderNamed ConfigureSymmetricAlgorithmProvider;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureSymmetricAlgorithmProvider = ConfigureCryptography
                .EncryptUsingSymmetricAlgorithmProviderNamed("symm provider provider");
        }

        protected SymmetricAlgorithmProviderData GetSymmetricAlgorithmProviderData()
        {
            return base.GetCryptographySettings().SymmetricCryptoProviders.OfType<SymmetricAlgorithmProviderData>().First();
        }
    }

    [TestClass]
    public class When_SettingSymmetricAlgorithmProviderAsDefault : Given_SymmetricAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureSymmetricAlgorithmProvider.SetAsDefault();
        }

        [TestMethod]
        public void Then_CryptographySettingsContainDefaultSymmetricProvider()
        {
            var cryptoSettings = GetCryptographySettings();
            var symmAlgoProvider = GetSymmetricAlgorithmProviderData();

            Assert.AreEqual(symmAlgoProvider.Name, cryptoSettings.DefaultSymmetricCryptoProviderName);
        }
    }

    [TestClass]
    public class When_SettingNullAlgorithmOnSymmetricAlgorithmProvider : Given_SymmetricAlgorithmProviderInConfiguration
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_UsingSymmetricAlgorithm_ThrowsArgumentNullException()
        {
            ConfigureSymmetricAlgorithmProvider.WithOptions.UsingSymmetricAlgorithm(null);
        }
    }

    [TestClass]
    public class When_SettingNonSymmetricAlgorithmOnSymmetricAlgorithmProvider : Given_SymmetricAlgorithmProviderInConfiguration
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_UsingSymmetricAlgorithm_ThrowsArgumentNullException()
        {
            ConfigureSymmetricAlgorithmProvider.WithOptions.UsingSymmetricAlgorithm(typeof(object));
        }
    }

    [TestClass]
    public class When_SettingAlgorithmOnSymmetricAlgorithmProvider : Given_SymmetricAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureSymmetricAlgorithmProvider.WithOptions.UsingSymmetricAlgorithm(typeof(RijndaelManaged));
        }

        [TestMethod]
        public void Then_SymmetricAlgorithmProviderHasAlgorithmSet()
        {
            var symmAlgoProvider = GetSymmetricAlgorithmProviderData();

            Assert.AreEqual(typeof(RijndaelManaged), symmAlgoProvider.AlgorithmType);
        }
    }

    [TestClass]
    public class When_SettingAlgorithmOnSymmetricAlgorithmProviderGeneric : Given_SymmetricAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureSymmetricAlgorithmProvider.WithOptions.UsingSymmetricAlgorithm<RijndaelManaged>();
        }

        [TestMethod]
        public void Then_SymmetricAlgorithmProviderHasAlgorithmSet()
        {
            var symmAlgoProvider = GetSymmetricAlgorithmProviderData();

            Assert.AreEqual(typeof(RijndaelManaged), symmAlgoProvider.AlgorithmType);
        }
    }

    [TestClass]
    public class When_SettingKeyOnSymmetricAlgorithmProvider : Given_SymmetricAlgorithmProviderInConfiguration
    {
        protected override void Act()
        {
            ConfigureSymmetricAlgorithmProvider.WithOptions.UseKeyFile("c:\\key.file", DataProtectionScope.CurrentUser);
        }

        [TestMethod]
        public void Then_SymmetricAlgorithmProviderHasKeyInfoSet()
        {
            var symmAlgoProvider = GetSymmetricAlgorithmProviderData();

            Assert.AreEqual("c:\\key.file", symmAlgoProvider.ProtectedKeyFilename);
            Assert.AreEqual(DataProtectionScope.CurrentUser, symmAlgoProvider.ProtectedKeyProtectionScope);
        }
    }

}

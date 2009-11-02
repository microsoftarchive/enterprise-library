using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Console.Wpf.Tests.VSTS.DevTests.Contexts;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Console.Wpf.ViewModel;
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


    [TestClass]
    public class when_loading_cryptography_configuration : given_crypto_configuration
    {
        protected override void Act()
        {
            CryptographyModel.UpdateLayout();
        }

        [TestMethod]
        public void then_hash_providers_are_in_first_column()
        {
            var allHashProviders = CryptographyModel.GetDescendentsOfType<HashProviderData>();

            Assert.IsTrue(allHashProviders.Any());
            Assert.IsFalse(allHashProviders.Where(x => x.Column != 0).Any());
        }

        [TestMethod]
        public void then_hash_providers_start_at_row_1()
        {
            var allHashProviders = CryptographyModel.GetDescendentsOfType<HashProviderData>();

            Assert.IsTrue(allHashProviders.Any());
            Assert.AreEqual(1, allHashProviders.Min(x => x.Row));
        }


        [TestMethod]
        public void then_symm_providers_are_in_second_column()
        {
            var allSymmProviders = CryptographyModel.GetDescendentsOfType<SymmetricProviderData>();

            Assert.IsTrue(allSymmProviders.Any());
            Assert.IsFalse(allSymmProviders.Where(x => x.Column != 1).Any());
        }

        [TestMethod]
        public void then_symm_providers_start_at_row_1()
        {
            var allSymmProviders = CryptographyModel.GetDescendentsOfType<SymmetricProviderData>();

            Assert.IsTrue(allSymmProviders.Any());
            Assert.AreEqual(1, allSymmProviders.Min(x => x.Row));
        }
    }
}

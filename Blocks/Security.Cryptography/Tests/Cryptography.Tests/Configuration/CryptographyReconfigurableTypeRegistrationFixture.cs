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

using System.Linq;
using System.Security.Cryptography;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Configuration;
using Microsoft.Practices.ServiceLocation;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cryptography.Tests.Configuration
{
    
    public abstract class UpdatedConfigurationSourceContext : ArrangeActAssert
    {
        protected UnityContainerConfigurator containerConfigurator;
        protected UnityContainer container;
        protected ConfigurationSourceUpdatable updatableConfigurationSource;
        protected CryptographySettings cryptoSettings;
        protected HashAlgorithmProviderData hashProvider;
        protected SymmetricProviderData symmetricAlgorithmProvider;

        protected override void Arrange()
        {
            updatableConfigurationSource = new ConfigurationSourceUpdatable();
            cryptoSettings = new CryptographySettings();

            hashProvider = new HashAlgorithmProviderData("hash provider", typeof(MD5), true);
            cryptoSettings.HashProviders.Add(hashProvider);
            cryptoSettings.DefaultHashProviderName = hashProvider.Name;

            symmetricAlgorithmProvider = new CustomSymmetricCryptoProviderData("symm provider", typeof(MockCustomSymmetricProvider));
            cryptoSettings.SymmetricCryptoProviders.Add(symmetricAlgorithmProvider);
            cryptoSettings.DefaultSymmetricCryptoProviderName = symmetricAlgorithmProvider.Name;
            updatableConfigurationSource.Add(CryptographySettings.SectionName, cryptoSettings);

            container = new UnityContainer();
            containerConfigurator = new UnityContainerConfigurator(container);
            EnterpriseLibraryContainer.ConfigureContainer(containerConfigurator, updatableConfigurationSource);
        }

        protected override void Act()
        {
            updatableConfigurationSource.DoSourceChanged(new string[] { CryptographySettings.SectionName });
        }

    }

    [TestClass]
    public class WhenHashProviderNameChanges : UpdatedConfigurationSourceContext
    {
        protected override void Act()
        {
            hashProvider.Name = "new name";

            base.Act();
        }

        [TestMethod]
        public void ThenContainerWillReturnHashProviderBasedOnNewName()
        {
            Assert.IsNotNull(container.Resolve<IHashProvider>("new name"));
        }
    }

    [TestClass]
    public class WhenSymmetricProviderNameChanges : UpdatedConfigurationSourceContext
    {
        protected override void Act()
        {
            symmetricAlgorithmProvider.Name = "new name";

            base.Act();
        }

        [TestMethod]
        public void ThenContainerWillReturnHashProviderBasedOnNewName()
        {
            Assert.IsNotNull(container.Resolve<ISymmetricCryptoProvider>("new name"));
        }
    }

}

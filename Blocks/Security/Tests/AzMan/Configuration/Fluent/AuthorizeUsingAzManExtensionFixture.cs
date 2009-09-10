//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.ContextBase;
using Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Tests.Configuration.Fluent
{

    public abstract class Given_SecuritySettingsInConfigurationSourceBuilder : ArrangeActAssert
    {
        IConfigurationSourceBuilder configurationSourceBuilder;
        protected IConfigureSecuritySettings ConfigureSecuritySettings;

        protected override void Arrange()
        {
            configurationSourceBuilder = new ConfigurationSourceBuilder();
            ConfigureSecuritySettings = configurationSourceBuilder.ConfigureSecurity();
        }

        protected SecuritySettings GetSecuritySettings()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            configurationSourceBuilder.UpdateConfigurationWithReplace(source);

            return (SecuritySettings)source.GetSection(SecuritySettings.SectionName);
        }
    }

    [TestClass]
    public class When_AddingAuthorizationManagerProviderToConfigurationSourceBuilder : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        IAuthorizeUsingAzManProvider authorizeWithAzMan;

        protected override void Act()
        {
            authorizeWithAzMan = ConfigureSecuritySettings.AuthorizeUsingAzManProviderNamed("az man provider");
        }

        [TestMethod]
        public void Then_SecurityConfigurationContainsAzManProvider()
        {
            Assert.IsTrue(GetSecuritySettings().AuthorizationProviders.OfType<AzManAuthorizationProviderData>().Any());
        }

        [TestMethod]
        public void Then_AuthorizationProviderHasAppropriateName()
        {
            var azManProvider = GetSecuritySettings().AuthorizationProviders.OfType<AzManAuthorizationProviderData>().First();
            Assert.AreEqual("az man provider", azManProvider.Name);
        }

        [TestMethod]
        public void Then_AuthorizationProviderHasCorrectType()
        {
            var azManProvider = GetSecuritySettings().AuthorizationProviders.OfType<AzManAuthorizationProviderData>().First();
            Assert.AreEqual(typeof(AzManAuthorizationProvider), azManProvider.Type);
        }

        [TestMethod]
        public void Then_CanAddAntoherAuthorizationProvider()
        {
            authorizeWithAzMan.AuthorizeUsingAzManProviderNamed("another provider");

            var azManProviders = GetSecuritySettings().AuthorizationProviders.OfType<AzManAuthorizationProviderData>();
            Assert.AreEqual(2, azManProviders.Count());
        }
    }
    
    [TestClass]
    public class When_AddingAuthorizationManagerProviderPassingNullName : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_AuthorizeUsingAzManProviderNamed_ThrowsArgumentException()
        {
            ConfigureSecuritySettings.AuthorizeUsingAzManProviderNamed(null);
        }
    }

    public abstract class Given_AzManAuthorizationProviderInConfigurationSourceBuilder : Given_SecuritySettingsInConfigurationSourceBuilder
    {
        protected IAuthorizeUsingAzManProvider ConfigureAzManProvider;

        protected override void Arrange()
        {
            base.Arrange();

            ConfigureAzManProvider = base.ConfigureSecuritySettings.AuthorizeUsingAzManProviderNamed("az man provider");
        }

        protected AzManAuthorizationProviderData GetAzManProviderData()
        {
            return GetSecuritySettings()
                    .AuthorizationProviders
                    .OfType<AzManAuthorizationProviderData>()
                    .Where(x => x.Name == "az man provider").First();
        }
    }

    [TestClass]
    public class When_CallingSetDefaultOnAzManProviderData : Given_AzManAuthorizationProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.ConfigureAzManProvider.SetAsDefault();
        }

        [TestMethod]
        public void Then_SecurityConfigurationHasDefaultAuthorizationProvider()
        {
            var securityConfiguration = base.GetSecuritySettings();
            var azManProvider = base.GetAzManProviderData();
            Assert.AreEqual(azManProvider.Name, securityConfiguration.DefaultAuthorizationProviderName);
        }
    }

    [TestClass]
    public class When_SpecifyingScopeOnAzManProviderData : Given_AzManAuthorizationProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.ConfigureAzManProvider.WithOptions.Scoped("scope");
        }

        [TestMethod]
        public void Then_AzManProviderHasScopeSet()
        {
            var azManProvider = base.GetAzManProviderData();
            Assert.AreEqual("scope", azManProvider.Scope);
        }
    }

    [TestClass]
    public class When_SpecifyingStoreLocationOnAzManProviderData : Given_AzManAuthorizationProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.ConfigureAzManProvider.WithOptions.UseStoreFrom("f:\\azman\\store.xml");
        }

        [TestMethod]
        public void Then_AzManProviderHasStoreSet()
        {
            var azManProvider = base.GetAzManProviderData();
            Assert.AreEqual("f:\\azman\\store.xml", azManProvider.StoreLocation);
        }
    }


    [TestClass]
    public class When_SpecifyingAppliationOnAzManProviderData : Given_AzManAuthorizationProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.ConfigureAzManProvider.WithOptions.ForApplication("my app");
        }

        [TestMethod]
        public void Then_AzManProviderHasApplicationSet()
        {
            var azManProvider = base.GetAzManProviderData();
            Assert.AreEqual("my app", azManProvider.Application);
        }
    }


    [TestClass]
    public class When_SpecifyingAuditIndentifierPrefixOnAzManProviderData : Given_AzManAuthorizationProviderInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.ConfigureAzManProvider.WithOptions.UsingAuditIdentifierPrefix("x");
        }

        [TestMethod]
        public void Then_AzManProviderHasAuditIdentifierPrefixSet()
        {
            var azManProvider = base.GetAzManProviderData();
            Assert.AreEqual("x", azManProvider.AuditIdentifierPrefix);
        }
    }
}

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

using System.Collections.Generic;
using System.Configuration;
using System.Security;
using System.Linq;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Storage;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    [DeploymentItem(@"typeRegistrationProviderSettings.config")]
    public class GivenAProvidersConfigurationSection
    {
        protected const int NumberOfDefaultRegistrations = 9;

        protected IConfigurationSource ConfigurationSource;
        protected TypeRegistrationProvidersConfigurationSection TypeRegistrationProvidersSection;

        [TestInitialize]
        public void Initialize()
        {
            ConfigurationSource = new FileConfigurationSource("typeRegistrationProviderSettings.config");

            TypeRegistrationProvidersSection = Given(ConfigurationSource);

        }

        protected virtual TypeRegistrationProvidersConfigurationSection Given(IConfigurationSource source)
        {
            return source.GetSection("unmodified") as TypeRegistrationProvidersConfigurationSection;
        }
    }

    [TestClass]
    public class GivenProvidersConfigurationSectionWithRemoveElement : GivenAProvidersConfigurationSection
    {
        protected override TypeRegistrationProvidersConfigurationSection Given(IConfigurationSource source)
        {
            return source.GetSection("removedPiab") as TypeRegistrationProvidersConfigurationSection;
        }

        [TestMethod]
        public void ThenConfigurationDoesntContainProviderForPiab()
        {
            Assert.AreEqual(NumberOfDefaultRegistrations - 1, TypeRegistrationProvidersSection.TypeRegistrationProviders.Count);
            Assert.IsNull(TypeRegistrationProvidersSection.TypeRegistrationProviders.Cast<TypeRegistrationProviderSettings>().Where(x => x.SectionName == BlockSectionNames.PolicyInjection).FirstOrDefault());
        }
    }

    [TestClass]
    public class GivenProvidersConfigurationSectionUnModified : GivenAProvidersConfigurationSection
    {
        [TestMethod]
        public void ThenConfigurationContainsProvidersForEachBlock()
        {
            Assert.AreEqual(NumberOfDefaultRegistrations, TypeRegistrationProvidersSection.TypeRegistrationProviders.Count);
        }
    }

    [TestClass]
    public class GivenProvidersConfigurationSectionCleared : GivenAProvidersConfigurationSection
    {
        protected override TypeRegistrationProvidersConfigurationSection Given(IConfigurationSource source)
        {
            return (TypeRegistrationProvidersConfigurationSection)source.GetSection("clearedAndRedeclared");
        }

        [TestMethod]
        public void ThenConfigurationContainsOnlyRedeclaredProvider()
        {
            Assert.AreEqual(1, TypeRegistrationProvidersSection.TypeRegistrationProviders.Count);
            Assert.AreEqual("policyInjection", TypeRegistrationProvidersSection.TypeRegistrationProviders.Cast<TypeRegistrationProviderSettings>().First().SectionName);
        }
    }

    [TestClass]
    public class GivenProvidersConfigurationSectionWithAddedSection : GivenAProvidersConfigurationSection
    {
        protected override TypeRegistrationProvidersConfigurationSection Given(IConfigurationSource source)
        {
            return (TypeRegistrationProvidersConfigurationSection)source.GetSection("addedSectionName");
        }

        [TestMethod]
        public void ThenConfigurationContainsOnlyRedeclaredProvider()
        {
            Assert.AreEqual(NumberOfDefaultRegistrations + 1, TypeRegistrationProvidersSection.TypeRegistrationProviders.Count);
            Assert.IsNotNull(TypeRegistrationProvidersSection.TypeRegistrationProviders.Cast<TypeRegistrationProviderSettings>()
                .Where(x => x.SectionName == "sectionname").FirstOrDefault());
        }
    }

    [TestClass]
    public class GivenProvidersConfigurationSectionWithAddedProviderType : GivenAProvidersConfigurationSection
    {
        protected override TypeRegistrationProvidersConfigurationSection Given(IConfigurationSource source)
        {
            return (TypeRegistrationProvidersConfigurationSection)source.GetSection("addedType");
        }

        [TestMethod]
        public void ThenConfigurationContainsAddedProviderType()
        {
            Assert.AreEqual(NumberOfDefaultRegistrations + 1, TypeRegistrationProvidersSection.TypeRegistrationProviders.Count);
            Assert.IsNotNull(TypeRegistrationProvidersSection.TypeRegistrationProviders.Cast<TypeRegistrationProviderSettings>()
                .Where(x => x.ProviderTypeName == "MockTypeRegistrationsProvider").FirstOrDefault());
        }
    }

    [TestClass]
    public class GivenProvidersConfigurationSettingsWithDupicateName : GivenAProvidersConfigurationSection
    {
        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void ThenThrowsConfigurationErrorsException()
        {
            ConfigurationSource.GetSection("duplicateName");
        }
    }

}

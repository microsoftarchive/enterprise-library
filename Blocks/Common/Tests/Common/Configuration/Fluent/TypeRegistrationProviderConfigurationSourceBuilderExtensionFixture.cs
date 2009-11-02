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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Fluent;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Tests.Configuration
{
    [TestClass]
    public class When_ConfiguringTypeRegistrationsForConfigurationSourceBuilder : Given_EmptyConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.ConfigurationSourceBuilder.ConfigureTypeRegistrations();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsTypeRegistrationProviderSettings()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            base.ConfigurationSourceBuilder.UpdateConfigurationWithReplace(source);

            Assert.IsNotNull(source.GetSection(TypeRegistrationProvidersConfigurationSection.SectionName));
        }

        [TestMethod]
        public void Then_TypeRegistrationsAreMadeForAllKnownBlocks()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            base.ConfigurationSourceBuilder.UpdateConfigurationWithReplace(source);

            TypeRegistrationProvidersConfigurationSection section = (TypeRegistrationProvidersConfigurationSection)source.GetSection(TypeRegistrationProvidersConfigurationSection.SectionName);

            Assert.AreEqual(9, section.TypeRegistrationProviders.Count);
        }
    }

    [TestClass]
    public class When_ConfiguringEmptyTypeRegistrationsForConfigurationSourceBuilder : Given_EmptyConfigurationSourceBuilder
    {
        protected override void Act()
        {
            base.ConfigurationSourceBuilder.ConfigureEmptyTypeRegistrations();
        }

        [TestMethod]
        public void Then_ConfigurationSourceContainsTypeRegistrationProviderSettings()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            base.ConfigurationSourceBuilder.UpdateConfigurationWithReplace(source);

            Assert.IsNotNull(source.GetSection(TypeRegistrationProvidersConfigurationSection.SectionName));
        }

        [TestMethod]
        public void Then_ThereAreNoTypeRegistrations()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            base.ConfigurationSourceBuilder.UpdateConfigurationWithReplace(source);

            TypeRegistrationProvidersConfigurationSection section = (TypeRegistrationProvidersConfigurationSection)source.GetSection(TypeRegistrationProvidersConfigurationSection.SectionName);

            Assert.AreEqual(0, section.TypeRegistrationProviders.Count);
        }
    }

    public abstract class Given_TypeRegistrationProvidersInConfigurationSourceBuilder : Given_EmptyConfigurationSourceBuilder
    {
        protected IConfigureTypeRegistrations ConfigureTypeRegistrations;
        protected override void Arrange()
        {
            base.Arrange();

            ConfigureTypeRegistrations = base.ConfigurationSourceBuilder.ConfigureTypeRegistrations();
        }

        protected TypeRegistrationProvidersConfigurationSection GetTypeRegistrationSection()
        {
            IConfigurationSource source = new DictionaryConfigurationSource();
            base.ConfigurationSourceBuilder.UpdateConfigurationWithReplace(source);

            TypeRegistrationProvidersConfigurationSection section = (TypeRegistrationProvidersConfigurationSection)source.GetSection(TypeRegistrationProvidersConfigurationSection.SectionName);

            return section;
        }

        protected class TestTypeRegistrationProvider : ITypeRegistrationsProvider
        {
            public IEnumerable<TypeRegistration> GetRegistrations(IConfigurationSource configurationSource)
            {
                throw new NotImplementedException();
            }

            public IEnumerable<TypeRegistration> GetUpdatedRegistrations(IConfigurationSource configurationSource)
            {
                throw new NotImplementedException();
            }
        }
    }

    [TestClass]
    public class When_AddingTypeRegistrationProviderPassingNullName : Given_TypeRegistrationProvidersInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_AddTypeRegistrationsProviderNamed_ThrowsArgumentException()
        {
            ConfigureTypeRegistrations.AddTypeRegistrationsProviderNamed(null);
        }
    }

    [TestClass]
    public class When_AddingTypeRegistrationProviderUsingNullType : Given_TypeRegistrationProvidersInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void Then_ForType_ThrowsArgumentNullException()
        {
            ConfigureTypeRegistrations.AddTypeRegistrationsProviderNamed("type reg").ForType(null);
        }
    }


    [TestClass]
    public class When_AddingTypeRegistrationProviderWithWrongType : Given_TypeRegistrationProvidersInConfigurationSourceBuilder
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void Then_ForType_ThrowsArgumentException()
        {
            ConfigureTypeRegistrations.AddTypeRegistrationsProviderNamed("type reg").ForType(typeof(object));
        }
    }

    [TestClass]
    public class When_AddingTypeRegistrationProviderUsingType : Given_TypeRegistrationProvidersInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureTypeRegistrations.AddTypeRegistrationsProviderNamed("type reg").ForType(typeof(TestTypeRegistrationProvider));
        }

        [TestMethod]
        public void Then_ConfigurationContainsAddedTypeRegistrationProvider()
        {
            var typeRegistrations = base.GetTypeRegistrationSection().TypeRegistrationProviders.OfType<TypeRegistrationProviderElement>();
            Assert.IsTrue(typeRegistrations.Where(x => x.Name == "type reg").Any());
        }

        [TestMethod]
        public void Then_TypeRegistrationHasAppropriateType()
        {
            var typeRegistrations = base.GetTypeRegistrationSection().TypeRegistrationProviders.OfType<TypeRegistrationProviderElement>();
            var addedRegistration = typeRegistrations.Where(x => x.Name == "type reg").First();
            Assert.AreEqual(typeof(TestTypeRegistrationProvider), Type.GetType(addedRegistration.ProviderTypeName));
        }
    }

    [TestClass]
    public class When_AddingTypeRegistrationProviderUsingTypeGeneric : Given_TypeRegistrationProvidersInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureTypeRegistrations.AddTypeRegistrationsProviderNamed("type reg").ForType<TestTypeRegistrationProvider>();
        }

        [TestMethod]
        public void Then_ConfigurationContainsAddedTypeRegistrationProvider()
        {
            var typeRegistrations = base.GetTypeRegistrationSection().TypeRegistrationProviders.OfType<TypeRegistrationProviderElement>();
            Assert.IsTrue(typeRegistrations.Where(x => x.Name == "type reg").Any());
        }

        [TestMethod]
        public void Then_TypeRegistrationHasAppropriateType()
        {
            var typeRegistrations = base.GetTypeRegistrationSection().TypeRegistrationProviders.OfType<TypeRegistrationProviderElement>();
            var addedRegistration = typeRegistrations.Where(x => x.Name == "type reg").First();
            Assert.AreEqual(typeof(TestTypeRegistrationProvider), Type.GetType(addedRegistration.ProviderTypeName));
        }
    }

    [TestClass]
    public class When_AddingTypeRegistrationProviderUsingSectionName : Given_TypeRegistrationProvidersInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureTypeRegistrations.AddTypeRegistrationsProviderNamed("type reg").ForSection("test section");
        }

        [TestMethod]
        public void Then_ConfigurationContainsAddedTypeRegistrationProvider()
        {
            var typeRegistrations = base.GetTypeRegistrationSection().TypeRegistrationProviders.OfType<TypeRegistrationProviderElement>();
            Assert.IsTrue(typeRegistrations.Where(x => x.Name == "type reg").Any());
        }

        [TestMethod]
        public void Then_TypeRegistrationHasAppropriateSection()
        {
            var typeRegistrations = base.GetTypeRegistrationSection().TypeRegistrationProviders.OfType<TypeRegistrationProviderElement>();
            var addedRegistration = typeRegistrations.Where(x => x.Name == "type reg").First();
            Assert.AreEqual("test section", addedRegistration.SectionName);
        }
    }

    [TestClass]
    public class When_AddingMultipleTypeRegistrationProviders : Given_TypeRegistrationProvidersInConfigurationSourceBuilder
    {
        protected override void Act()
        {
            ConfigureTypeRegistrations.AddTypeRegistrationsProviderNamed("type reg1").ForSection("test section")
                                      .AddTypeRegistrationsProviderNamed("type reg2").ForType(typeof(TestTypeRegistrationProvider))
                                      .AddTypeRegistrationsProviderNamed("type reg3").ForType<TestTypeRegistrationProvider>();
        }

        [TestMethod]
        public void Then_ConfigurationContainsMultipleTypeRegistrationProviders()
        {
            var typeRegistrations = base.GetTypeRegistrationSection().TypeRegistrationProviders.OfType<TypeRegistrationProviderElement>();
            Assert.AreEqual(3, typeRegistrations.Where(x => x.Name.StartsWith("type reg")).Count());
        }
    }

}

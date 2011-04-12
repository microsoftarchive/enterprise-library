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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Tests
{
    [TestClass]
    public class GivenATypeRegistrationProviderElementCollection
    {
        protected TypeRegistrationProviderElementCollection typeRegistrationProviderElementCollection;

        [TestInitialize]
        public void Initialize()
        {
            typeRegistrationProviderElementCollection = new TypeRegistrationProviderElementCollection();
        }

        [TestMethod]
        public void ThenPolicyInjectionTypeRegistrationIsIncluded()
        {
            Assert.IsTrue(typeRegistrationProviderElementCollection.Any(x => x.SectionName == BlockSectionNames.PolicyInjection && x.Name == TypeRegistrationProvidersConfigurationSection.PolicyInjectionTypeRegistrationProviderName));
        }

        [TestMethod]
        public void ThenLoggingTypeRegistrationIsIncluded()
        {
            Assert.IsTrue(typeRegistrationProviderElementCollection.Any(x => x.SectionName == BlockSectionNames.Logging && x.Name == TypeRegistrationProvidersConfigurationSection.LoggingTypeRegistrationProviderName));
        }

        [TestMethod]
        public void ThenExceptionHandlingTypeRegistrationIsIncluded()
        {
            Assert.IsTrue(typeRegistrationProviderElementCollection.Any(x => x.SectionName == BlockSectionNames.ExceptionHandling && x.Name == TypeRegistrationProvidersConfigurationSection.ExceptionHandlingTypeRegistrationProviderName));
        }

        [TestMethod]
        public void ThenValidationTypeRegistrationIsIncluded()
        {
            Assert.IsTrue(typeRegistrationProviderElementCollection.Any(x => x.ProviderTypeName == BlockSectionNames.ValidationRegistrationProviderLocatorType && x.Name == TypeRegistrationProvidersConfigurationSection.ValidationTypeRegistrationProviderName));
        }

        [TestMethod]
        public void ThenCachingTypeRegistrationIsIncluded()
        {
            Assert.IsTrue(typeRegistrationProviderElementCollection.Any(x => x.SectionName == BlockSectionNames.Caching && x.Name == TypeRegistrationProvidersConfigurationSection.CachingTypeRegistrationProviderName));
        }
    }
}

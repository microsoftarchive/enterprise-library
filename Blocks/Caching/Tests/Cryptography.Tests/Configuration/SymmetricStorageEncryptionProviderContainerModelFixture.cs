//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
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
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Cryptography.Tests.Configuration
{
    [TestClass]
    public class GivenRegistrationOfCacheManagerSettingsWithSymmetricStorageEncryption   
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CacheManagerSettings settings = new CacheManagerSettings();
            settings.EncryptionProviders.Add(new SymmetricStorageEncryptionProviderData("symm storage encryption", "default"));
            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenHasRegistrationForIStorageEncryptionProvider()
        {
            Assert.IsTrue(registrations.Where(r => r.ServiceType == typeof(IStorageEncryptionProvider)).Any());
        }

        [TestMethod]
        public void ThenImplementationTypeIsSymmetricStorageEncryptionProvider()
        {
            TypeRegistration storageEncryptionProvider = registrations.Where(r => r.ServiceType == typeof(IStorageEncryptionProvider)).First();
            
            Assert.AreEqual(typeof(SymmetricStorageEncryptionProvider), storageEncryptionProvider.ImplementationType);
        }
    }
}

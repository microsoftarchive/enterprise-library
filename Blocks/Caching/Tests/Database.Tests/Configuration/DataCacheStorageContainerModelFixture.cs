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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ContainerModel;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Database.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Database.Tests.Configuration
{
    [TestClass]
    public class GivenRegistrationOfCacheManagerSettingsWithDataCacheStorage
    {
        private IEnumerable<TypeRegistration> registrations;

        [TestInitialize]
        public void Setup()
        {
            CacheManagerSettings settings = new CacheManagerSettings();
            settings.BackingStores.Add(new DataCacheStorageData("Data Cache Storage", "db instance", "partition"));

            registrations = settings.GetRegistrations(null);
        }

        [TestMethod]
        public void ThenRegistrationsContainIBackingStore()
        {
            Assert.IsTrue(registrations.Where(r => r.ServiceType == typeof(IBackingStore)).Any());
        }

        [TestMethod]
        public void ThenRegisteredBackingSoreIsDataBackingStore()
        {
            TypeRegistration backingStoreRegistration = registrations.Where(r => r.ServiceType == typeof(IBackingStore)).First();

            Assert.AreEqual(typeof(DataBackingStore), backingStoreRegistration.ImplementationType);
        }
    }
}

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

using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations;
using Microsoft.Practices.EnterpriseLibrary.Caching.BackingStoreImplementations.Tests;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Unity;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Unity;
using Microsoft.Practices.Unity;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.Configuration.Unity
{
	[TestClass]
	public class CachingBlockExtensionFixture
	{
		private CacheManagerSettings settings;
		private DictionaryConfigurationSource configurationSource;
		private IUnityContainer container;

		[TestInitialize]
		public void SetUp()
		{
			container = new UnityContainer();

			settings = new CacheManagerSettings();
			configurationSource = new DictionaryConfigurationSource();
			configurationSource.Add(CacheManagerSettings.SectionName, settings);

			container.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
		}

		[TestCleanup]
		public void TearDown()
		{
			container.Dispose();
		}

		[TestMethod]
		public void CanCreateCustomCacheStorage()
		{
			CustomCacheStorageData data = new CustomCacheStorageData("name", typeof(MockCustomStorageBackingStore));
			data.Attributes[MockCustomStorageBackingStore.AttributeKey] = "value1";
			settings.BackingStores.Add(data);

			container.AddExtension(new CachingBlockExtension());

			MockCustomStorageBackingStore createdObject = (MockCustomStorageBackingStore)container.Resolve<IBackingStore>("name");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("value1", createdObject.customValue);
		}

		[TestMethod]
		public void CanCreateIsolatedCacheStorageWithoutEncryption()
		{
			IsolatedStorageCacheStorageData data = new IsolatedStorageCacheStorageData("name", "", "partition");
			settings.BackingStores.Add(data);

			container.AddExtension(new CachingBlockExtension());

			IsolatedStorageBackingStore createdObject = (IsolatedStorageBackingStore)container.Resolve<IBackingStore>("name");

			Assert.IsNotNull(createdObject);
		}

		//[TestMethod]
		//public void CanCreateIsolatedCacheStorageWithEncryption()
		//{
		//     encryptionData = new CustomCacheStorageData("encryption", typeof(MockCustomStorageBackingStore));
		//    encryptionData.Attributes[MockCustomStorageBackingStore.AttributeKey] = "value1";
		//    settings.BackingStores.Add(encryptionData);

		//    IsolatedStorageCacheStorageData data = new IsolatedStorageCacheStorageData("name", "encryption", "partition");
		//    settings.BackingStores.Add(data);

		//    container.AddExtension(new CachingBlockExtension());

		//    IsolatedStorageBackingStore createdObject = (IsolatedStorageBackingStore)container.Resolve<IBackingStore>("name");

		//    Assert.IsNotNull(createdObject);
		//}

		[TestMethod]
		public void CanCreateCustomCacheManager()
		{
			CustomCacheManagerData data = new CustomCacheManagerData("name", typeof(CustomCacheManager));
			data.Attributes[MockCustomStorageBackingStore.AttributeKey] = "value1";
			settings.CacheManagers.Add(data);

			container.AddExtension(new CachingBlockExtension());

			CustomCacheManager createdObject = (CustomCacheManager)container.Resolve<ICacheManager>("name");

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("value1", createdObject.customValue);
		}

		[TestMethod]
		public void CanCreateDefaultCacheManager()
		{
			CustomCacheManagerData data = new CustomCacheManagerData("name", typeof(CustomCacheManager));
			data.Attributes[MockCustomStorageBackingStore.AttributeKey] = "value1";
			settings.CacheManagers.Add(data);

			settings.DefaultCacheManager = "name";

			container.AddExtension(new CachingBlockExtension());

			CustomCacheManager createdObject = (CustomCacheManager)container.Resolve<ICacheManager>();

			Assert.IsNotNull(createdObject);
			Assert.AreEqual("value1", createdObject.customValue);
		}

		[TestMethod]
		public void CanCreateCacheManager()
		{
			CacheStorageData data = new CacheStorageData("storage", typeof(NullBackingStore));
			settings.BackingStores.Add(data);

			CacheManagerData managerData = new CacheManagerData("name", 300, 200, 100, "storage");
			settings.CacheManagers.Add(managerData);

			container.AddExtension(new CachingBlockExtension());

			CacheManager createdObject = (CacheManager)container.Resolve<ICacheManager>("name");

			Assert.IsNotNull(createdObject);

			// does it work?
			object value = new object();
			createdObject.Add("key", value);
			Assert.AreSame(value, createdObject.GetData("key"));
		}

		[TestMethod]
		public void CacheManagerIsSingleton()
		{
			CacheStorageData data = new CacheStorageData("storage", typeof(NullBackingStore));
			settings.BackingStores.Add(data);

			CacheManagerData managerData = new CacheManagerData("name", 300, 200, 100, "storage");
			settings.CacheManagers.Add(managerData);

			container.AddExtension(new CachingBlockExtension());

			CacheManager createdObject1 = (CacheManager)container.Resolve<ICacheManager>("name");
			CacheManager createdObject2 = (CacheManager)container.Resolve<ICacheManager>("name");

			Assert.IsNotNull(createdObject1);
			Assert.AreSame(createdObject1, createdObject2);
		}

        [TestMethod]
        public void CheckLifetime()
        {
            CustomCacheManagerData data = new CustomCacheManagerData("name", typeof(CustomCacheManager));
            settings.CacheManagers.Add(data);

            CustomCacheManager createdObject = null;

            using (UnityContainer mockContainer = new UnityContainer())
            {
                mockContainer.AddExtension(new EnterpriseLibraryCoreExtension(configurationSource));
                mockContainer.AddExtension(new CachingBlockExtension());

                createdObject = (CustomCacheManager)mockContainer.Resolve<ICacheManager>("name");

                Assert.IsNotNull(createdObject);
                Assert.IsFalse(createdObject.wasDisposed);
            }

            Assert.IsTrue(createdObject.wasDisposed);
        }
	}
}

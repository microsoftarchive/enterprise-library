using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_initialized_with_truncated_data : Context
    {
        protected override void Act()
        {
            base.Act();

            using (Cache as IDisposable) { }
            Cache = null;

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (store.OpenFile(Path.Combine("Cache_" + CacheName, "content.dat"), FileMode.Truncate, FileAccess.ReadWrite)) { }
            }

            base.RefreshCache();
        }

        [TestMethod]
        public void then_cache_resets_storage()
        {
            Assert.AreEqual(0, Cache.Count());
        }
    }

    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_initialized_with_missing_data_file : Context
    {
        protected override void Act()
        {
            base.Act();

            using (Cache as IDisposable) { }
            Cache = null;

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                store.DeleteFile(Path.Combine("Cache_" + CacheName, "content.dat"));
            }

            base.RefreshCache();
        }

        [TestMethod]
        public void then_cache_resets_storage()
        {
            Assert.AreEqual(0, Cache.Count());
        }
    }

    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_initialized_with_missing_fat_file : Context
    {
        protected override void Act()
        {
            base.Act();

            using (Cache as IDisposable) { }
            Cache = null;

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                store.DeleteFile(Path.Combine("Cache_" + CacheName, "fat.dat"));
            }

            base.RefreshCache();
        }

        [TestMethod]
        public void then_cache_resets_storage()
        {
            Assert.AreEqual(0, Cache.Count());
        }
    }

    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_initialized_with_truncated_fat_file : Context
    {
        protected override void Act()
        {
            base.Act();

            using (Cache as IDisposable) { }
            Cache = null;

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (store.OpenFile(Path.Combine("Cache_" + CacheName, "fat.dat"), FileMode.Truncate, FileAccess.ReadWrite)) { }
            }

            base.RefreshCache();
        }

        [TestMethod]
        public void then_cache_resets_storage()
        {
            Assert.AreEqual(0, Cache.Count());
        }
    }
}

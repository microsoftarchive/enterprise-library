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
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    public class AccessIssuesContext : Context
    {
        protected string itemFileName;

        protected override void Arrange()
        {
            base.Arrange();

            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                // find the file representation for "largeData" key
                foreach (var fileName in store.GetFileNames("__Cache_" + CacheName + Path.DirectorySeparatorChar + "*.dat"))
                {
                    var path = Path.Combine("__Cache_" + CacheName, fileName);
                    using (var file = store.OpenFile(path, FileMode.Open, FileAccess.Read))
                    {
                        if (file.Length > 5000)
                        {
                            itemFileName = path;
                            break;
                        }
                    }
                }
            }
        }

        [TestClass]
        [Tag("IsolatedStorage")]
        public class when_some_data_is_truncated : AccessIssuesContext
        {
            protected override void Act()
            {
                base.Act();

                using (Cache as IDisposable) { }
                Cache = null;

                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    using (var file = store.OpenFile(itemFileName, FileMode.Open, FileAccess.ReadWrite))
                    {
                        file.SetLength(file.Length - 1);
                    }
                }

                this.RefreshCache();
            }

            [TestMethod]
            public void then_corrupted_file_is_removed()
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    Assert.IsFalse(store.FileExists(itemFileName));
                }
            }

            [TestMethod]
            public void then_preserves_all_other_entries()
            {
                Assert.AreEqual(1, Cache.Count());
                Assert.AreEqual("value", Cache["key"]);
            }
        }

        [TestClass]
        [Tag("IsolatedStorage")]
        public class when_some_data_is_locked : AccessIssuesContext
        {
            IsolatedStorageFile store;
            IsolatedStorageFileStream file;

            protected override void Act()
            {
                base.Act();

                using (Cache as IDisposable) { }
                Cache = null;

                store = IsolatedStorageFile.GetUserStoreForApplication();
                file = store.OpenFile(itemFileName, FileMode.Open, FileAccess.ReadWrite);

                this.RefreshCache();
            }

            protected override void Teardown()
            {
                using (file) { }
                using (store) { }

                base.Teardown();
            }

            [TestMethod]
            public void then_corrupted_file_is_still_present()
            {
                using (var store = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    Assert.IsTrue(store.FileExists(itemFileName));
                }
            }

            [TestMethod]
            public void then_entry_was_skipped()
            {
                Assert.IsNull(Cache["largeData"]);
            }

            [TestMethod]
            public void then_preserves_all_other_entries()
            {
                Assert.AreEqual(1, Cache.Count());
                Assert.AreEqual("value", Cache["key"]);
            }
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO.IsolatedStorage;
using Microsoft.Practices.EnterpriseLibrary.Caching.Runtime.Caching;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage.given_an_iso_storage_with_data
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_item_that_does_not_fit_storage : Context
    {
        protected override void Arrange()
        {
            base.Arrange();

            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var freeSpace = isoStore.AvailableFreeSpace;
                if (freeSpace > 5000)
                {
                    byte[] fileContent = new byte[isoStore.AvailableFreeSpace - 5000];
                    using (var file = isoStore.CreateFile("largeBigFile.dat"))
                    {
                        file.Write(fileContent, 0, fileContent.Length);
                    }
                }
            }
        }

        protected override void Teardown()
        {
            base.Teardown();

            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (isoStore.FileExists("largeBigFile.dat"))
                {
                    isoStore.DeleteFile("largeBigFile.dat");
                }
            }
        }

        private bool added;

        protected override void Act()
        {
            base.Act();

            added = Cache.Add("newIem", new byte[6000], new CacheItemPolicy());

            base.RefreshCache();
        }

        [TestMethod]
        public void then_returns_true_on_addition_bacause_it_fails_silently()
        {
            Assert.IsTrue(added);
        }

        [TestMethod]
        public void then_item_is_null()
        {
            Assert.IsNull(Cache["newItem"]);
        }
    }
}

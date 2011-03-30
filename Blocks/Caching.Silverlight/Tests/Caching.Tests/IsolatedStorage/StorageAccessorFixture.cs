using System;
using System.IO.IsolatedStorage;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class StorageAccessorFixture
    {
        private const string TestStorageName = "TestStorage";

        [TestInitialize]
        [TestCleanup]
        public void Cleanup()
        {
            StorageAccessor.DeleteStorage(TestStorageName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenInitializingWithNullName_ThenThrows()
        {
            new StorageAccessor(null, 10240);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenInitializingWithEmptyName_ThenThrows()
        {
            new StorageAccessor(" ", 10240);
        }

        [TestMethod]
        public void WhenUsingNameWithInvalidFileChars_ThenCanCreateStorageAnyway()
        {
            string id;
            string name = @"Th1s&Is:Inv""alid\In>Windows";

            try
            {
                using (var savingBlockStorage = new StorageAccessor(name, 10240))
                {
                    id = savingBlockStorage.Save(new byte[2]);
                }

                using (var readingBlockStorage = new StorageAccessor(name, 10240))
                {
                    Assert.AreEqual(id, readingBlockStorage.ReadAll().Single().Key);
                }
            }
            finally
            {
                StorageAccessor.DeleteStorage(name);
            }
        }

        [TestMethod]
        public void WhenWritingContentLargerThanBlockSize_ThenCanReadContent()
        {
            string id;
            byte[] content = Enumerable.Range(0, 70).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var contents = readingBlockStorage.ReadAll();

                CollectionAssert.AreEqual(content, contents[id]);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenWritingNullContent_ThenThrows()
        {
            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                savingBlockStorage.Save(null);
            }
        }

        [TestMethod]
        public void WhenOverwritingContent_ThenCanReadUpdatedContent()
        {
            string id;
            byte[] content = new byte[] { 1, 2, 3, 4, 5 };

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                overwritingBlockStorage.Overwrite(id, new byte[] { 7, 8 }, 1);
            }

            using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var actualContent = readingBlockStorage.ReadAll()[id];

                CollectionAssert.AreEqual(new byte[] { 1, 7, 8, 4, 5 }, actualContent);
            }
        }

        [TestMethod]
        public void WhenOverwritingLargeLargeContent_ThenCanReadUpdatedContent()
        {
            string id;
            byte[] content = Enumerable.Range(0, 600).Select(n => (byte)n).ToArray();
            byte[] update = new byte[] { 1, 1, 1, 1, 1 };
            int offset = 29;

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                overwritingBlockStorage.Overwrite(id, update, offset);
            }

            using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var actualContent = readingBlockStorage.ReadAll()[id];

                Array.Copy(update, 0, content, offset, update.Length);
                CollectionAssert.AreEqual(content, actualContent);
            }
        }


        [TestMethod]
        public void WhenRemovingEntry_CannotReadItAfterwards()
        {
            string id = null;
            try
            {
                using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
                {
                    id = savingBlockStorage.Save(new byte[] { 1, 2, 3, 4, 5 });
                }

                using (var deletingBlockStorage = new StorageAccessor(TestStorageName, 10240))
                {
                    deletingBlockStorage.Remove(id);
                }
            }
            catch
            {
                Assert.Fail();
            }

            using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                Assert.IsFalse(readingBlockStorage.ReadAll().ContainsKey(id));
            }
        }

        [TestMethod]
        public void WhenWritingContent_ThenCanRetrieveIds()
        {
            string id1;
            string id2;

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                id1 = savingBlockStorage.Save(new byte[] { 1, 2, 3, 4, 5 });
                id2 = savingBlockStorage.Save(new byte[] { 7, 8, 9, 10, 11 });

                var ids = savingBlockStorage.ReadAll().Select(x => x.Key).ToArray();

                Assert.AreEqual(2, ids.Length);
                CollectionAssert.Contains(ids, id1);
                CollectionAssert.Contains(ids, id2);
            }

            using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var ids = readingBlockStorage.ReadAll().Select(x => x.Key).ToArray();

                Assert.AreEqual(2, ids.Length);
                CollectionAssert.Contains(ids, id1);
                CollectionAssert.Contains(ids, id2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenOverwritingContentWithInvalidId_ThenThrows()
        {
            string id;
            byte[] content = Enumerable.Range(0, 70).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                overwritingBlockStorage.Overwrite(id + "invalid", new byte[] { 7, 8 }, 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenOverwritingWithLargerContentSizeThanAllocatedSize_ThenThrows()
        {
            string id;
            var content = new byte[] { 1, 2, 3, 4, 5 };
            byte[] update = Enumerable.Range(0, 6).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                overwritingBlockStorage.Overwrite(id, update, 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenOverwritingWithNegativeOffset_ThenThrows()
        {
            string id;
            var content = new byte[] { 1, 2, 3, 4, 5 };

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                overwritingBlockStorage.Overwrite(id, new byte[] { 1 }, -1);
            }
        }

        [TestMethod]
        public void CanCreateStorageWithMaxSizeLargerThanAvailableIsoStorageSpace()
        {
            long quota;
            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                quota = isoStore.Quota;
            }

            using (var blockStorage = new StorageAccessor(TestStorageName, quota * 1000))
            {
            }
        }

        [TestMethod]
        public void WhenCreatingStorage_ThenOnlySmallSizeIsPreallocatedForInfrastructure()
        {
            long previousFreeSpace;
            long quota;
            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                quota = isoStore.Quota;
                previousFreeSpace = isoStore.AvailableFreeSpace;
            }

            using (var blockStorage = new StorageAccessor(TestStorageName, quota * 1000))
            {
            }

            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var newFreeSpace = isoStore.AvailableFreeSpace;

                Assert.IsTrue(previousFreeSpace - newFreeSpace < 10 * 1024, "Infrastructure is more than 10 KB");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(AllocationException))]
        public void WhenSavingContentLargerThanMaxSize_ThenThrows()
        {
            byte[] content = Enumerable.Range(0, 10241).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                savingBlockStorage.Save(content);
            }
        }

        [TestMethod]
        public void WhenSavingContentLargerThanMaxSize_ThenDoesNotAllocateSpace()
        {
            string id;
            byte[] largeContent = Enumerable.Range(0, 10241).Select(n => (byte)n).ToArray();
            byte[] smallContent = Enumerable.Range(0, 9000).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                try
                {
                    savingBlockStorage.Save(largeContent);
                    Assert.Fail("Exception not thrown.");
                }
                catch (AllocationException) { }

                id = savingBlockStorage.Save(smallContent);
            }

            using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var contents = readingBlockStorage.ReadAll();
                Assert.AreEqual(1, contents.Count);
                Assert.AreEqual(id, contents.Keys.Single());

                CollectionAssert.AreEqual(smallContent, contents[id]);
            }
        }

        [TestMethod]
        public void WhenWritingContentLargerThanAvailableIsoStorage_ThenThrowsExceptionButDoesNotCorruptState()
        {
            try
            {
                using (var blockStorage = new StorageAccessor(TestStorageName, 30000))
                {
                    blockStorage.ReadAll();
                }

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

                using (var blockStorage = new StorageAccessor(TestStorageName, 30000))
                {
                    blockStorage.ReadAll();
                    var content = new byte[10000];
                    try
                    {
                        blockStorage.Save(content);
                        Assert.Fail("Exception not thrown.");
                    }
                    catch (AllocationException) { }   // This is the expected exception
                }

                using (var readingBlockStorage = new StorageAccessor(TestStorageName, 30000))
                {
                    Assert.AreEqual(0, readingBlockStorage.ReadAll().Count);
                }
            }

            finally
            {
                using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
                {
                    if (isoStore.FileExists("largeBigFile.dat"))
                    {
                        isoStore.DeleteFile("largeBigFile.dat");
                    }
                }
            }
        }

        [TestMethod]
        public void WhenWritingContent_ThenTracksSize()
        {
            byte[] content1 = new byte[70];
            byte[] content2 = new byte[60];
            byte[] content3 = new byte[50];

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                savingBlockStorage.Save(content1);
                savingBlockStorage.Save(content2);
                savingBlockStorage.Save(content3);

                Assert.AreEqual(70 + 60 + 50, savingBlockStorage.UsedLogicalSize);
            }
        }

        [TestMethod]
        public void WhenReadingContentAfterRehydrating_ThenTracksSize()
        {
            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                savingBlockStorage.Save(new byte[70]);
                savingBlockStorage.Save(new byte[60]);
                savingBlockStorage.Save(new byte[50]);

                Assert.AreEqual(70 + 60 + 50, savingBlockStorage.UsedLogicalSize);
            }

            using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var contents = readingBlockStorage.ReadAll();

                Assert.AreEqual(70 + 60 + 50, readingBlockStorage.UsedLogicalSize);
            }
        }

        [TestMethod]
        public void WhenReadingPhysicalSize_ThenItIsCalculatedAccountingForAClusterSizeOf1024()
        {
            long originalPshysicalSize;

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                savingBlockStorage.ReadAll();

                originalPshysicalSize = savingBlockStorage.UsedPhysicalSize;

                savingBlockStorage.Save(new byte[1]);       // 1 cluster
                savingBlockStorage.Save(new byte[1]);       // 1 cluster
                savingBlockStorage.Save(new byte[1]);       // 1 cluster
                savingBlockStorage.Save(new byte[1024]);    // 1 cluster
                savingBlockStorage.Save(new byte[1025]);    // 2 clusters

                Assert.AreEqual(originalPshysicalSize + (1024 * 6), savingBlockStorage.UsedPhysicalSize);
            }

            using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var contents = readingBlockStorage.ReadAll();

                Assert.AreEqual(originalPshysicalSize + (1024 * 6), readingBlockStorage.UsedPhysicalSize);
            }
        }

        [TestMethod]
        public void WhenInitializingAfterRehydrating_ThenGetsAllContentInSingleRead()
        {
            byte[] content1 = Enumerable.Range(0, 70).Select(n => (byte)n).ToArray();
            byte[] content2 = Enumerable.Range(0, 60).Select(n => (byte)n).ToArray();
            byte[] content3 = Enumerable.Range(0, 50).Select(n => (byte)n).ToArray();

            string id1, id2, id3;

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                id1 = savingBlockStorage.Save(content1);
                id2 = savingBlockStorage.Save(content2);
                id3 = savingBlockStorage.Save(content3);
            }

            using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var contents = readingBlockStorage.ReadAll();

                Assert.AreEqual(3, contents.Count());
                CollectionAssert.AreEqual(content1, contents[id1]);
                CollectionAssert.AreEqual(content2, contents[id2]);
                CollectionAssert.AreEqual(content3, contents[id3]);
            }
        }

        [TestMethod]
        public void WhenOpeningSecondInstance_ThenCanReadContent()
        {
            string id;
            var content = new byte[] { 1, 2, 3, 4, 5 };

            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                id = savingBlockStorage.Save(content);

                using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
                {
                    var contents = readingBlockStorage.ReadAll();

                    CollectionAssert.AreEqual(content, contents[id]);
                }
            }
        }

        [TestMethod]
        public void WhenOpeningSecondInstance_ThenCannotWriteContent()
        {
            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                savingBlockStorage.Save(new byte[1]);
                Assert.IsFalse(savingBlockStorage.IsReadOnly);

                using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
                {
                    readingBlockStorage.ReadAll();

                    Assert.IsTrue(readingBlockStorage.IsReadOnly);

                    try
                    {
                        readingBlockStorage.Save(new byte[1]);
                        Assert.Fail("Exception not thrown");
                    }
                    catch (InvalidOperationException)
                    {
                        // expected exception thrown
                    }
                }
            }
        }

        [TestMethod]
        public void WhenOpeningSecondInstance_ThenCannotRemoveContent()
        {
            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var id = savingBlockStorage.Save(new byte[1]);

                using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
                {
                    readingBlockStorage.ReadAll();

                    try
                    {
                        readingBlockStorage.Remove(id);
                        Assert.Fail("Exception not thrown");
                    }
                    catch (InvalidOperationException)
                    {
                        // expected exception thrown
                    }
                }
            }
        }

        [TestMethod]
        public void WhenOpeningSecondInstance_ThenCannotOverwriteContent()
        {
            using (var savingBlockStorage = new StorageAccessor(TestStorageName, 10240))
            {
                var id = savingBlockStorage.Save(new byte[2]);

                using (var readingBlockStorage = new StorageAccessor(TestStorageName, 10240))
                {
                    readingBlockStorage.ReadAll();

                    try
                    {
                        readingBlockStorage.Overwrite(id, new byte[1], 0);
                        Assert.Fail("Exception not thrown");
                    }
                    catch (InvalidOperationException)
                    {
                        // expected exception thrown
                    }
                }
            }
        }
    }
}

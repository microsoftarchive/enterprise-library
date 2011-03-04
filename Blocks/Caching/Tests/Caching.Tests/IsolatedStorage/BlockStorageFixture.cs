using System;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class BlockStorageFixture
    {
        private const string TestStorageName = "TestStorage";

        [TestInitialize]
        [TestCleanup]
        public void Cleanup()
        {
            BlockStorage.DeleteStorage(TestStorageName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenInitializingWithNullName_ThenThrows()
        {
            new BlockStorage(null, 32, 1024);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenInitializingWithEmptyName_ThenThrows()
        {
            new BlockStorage(" ", 32, 1024);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenInitializingWithBlockSizeLessThan32_ThenThrows()
        {
            new BlockStorage(TestStorageName, 31, 1024);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenInitializingWithBlockSizeGreaterThanMaxSize_ThenThrows()
        {
            new BlockStorage(TestStorageName, 4096, 3072);
        }

        [TestMethod]
        public void WhenInitializingWithDifferentBlockSize_ThenReinitializesStorage()
        {
            using (var storage = new BlockStorage(TestStorageName, 32, 1024))
            {
                storage.Save(new byte[5]);
            }

            using (var newStorage = new BlockStorage(TestStorageName, 64, 1024))
            {
                Assert.AreEqual(0, newStorage.GetIds().Count());
            }
        }

        [TestMethod]
        public void WhenWritingContentShorterThanBlockSize_ThenCanReadContent()
        {
            int id;
            var content = new byte[] { 1, 2, 3, 4, 5 };

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                var actualContent = readingBlockStorage.Read(id);

                CollectionAssert.AreEqual(content, actualContent);
            }
        }

        [TestMethod]
        public void WhenWritingContentLargerThanBlockSize_ThenCanReadContent()
        {
            int id;
            byte[] content = Enumerable.Range(0, 70).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                var actualContent = readingBlockStorage.Read(id);

                CollectionAssert.AreEqual(content, actualContent);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenWritingNullContent_ThenThrows()
        {
            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                savingBlockStorage.Save(null);
            }
        }

        [TestMethod]
        public void WhenOverwritingContent_ThenCanReadUpdatedContent()
        {
            int id;
            byte[] content = new byte[] { 1, 2, 3, 4, 5 };

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                overwritingBlockStorage.Overwrite(id, new byte[] { 7, 8 }, 1);
            }

            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                var actualContent = readingBlockStorage.Read(id);

                CollectionAssert.AreEqual(new byte[] { 1, 7, 8, 4, 5 }, actualContent);
            }
        }

        [TestMethod]
        public void WhenOverwritingContentLargerThanBlockSize_ThenCanReadUpdatedContent()
        {
            int id;
            byte[] content = Enumerable.Range(0, 70).Select(n => (byte)n).ToArray();
            byte[] update = new byte[] { 1, 1, 1, 1, 1 };
            int offset = 29;
            
            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                overwritingBlockStorage.Overwrite(id, update, offset);
            }

            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                var actualContent = readingBlockStorage.Read(id);

                Array.Copy(update, 0, content, offset, update.Length);
                CollectionAssert.AreEqual(content, actualContent);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenReadingUnusedBlock_ThenThrows()
        {
            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                readingBlockStorage.Read(5);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenReadingBlockThatIsOutsideOfRange_ThenThrows()
        {
            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                readingBlockStorage.Read((1024 / 32) + 2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenReadingNegativeBlockId_ThenThrows()
        {
            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                readingBlockStorage.Read(-1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenReadingBlockThatIsNotStartOfSequence_ThenThrows()
        {
            int id = 0;
            try
            {
                using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
                {
                    id = savingBlockStorage.Save(Enumerable.Range(0, 900).Select(n => (byte)n).ToArray());
                }
            }
            catch
            {
                Assert.Fail();
            }

            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                readingBlockStorage.Read(id + 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenRemovingBlock_CannotReadItAfterwards()
        {
            int id = 0;
            try
            {
                using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
                {
                    id = savingBlockStorage.Save(new byte[] { 1, 2, 3, 4, 5 });
                }

                using (var deletingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
                {
                    deletingBlockStorage.Remove(id);
                }
            }
            catch
            {
                Assert.Fail();
            }

            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                readingBlockStorage.Read(id);
            }
        }

        [TestMethod]
        public void WhenWritingContent_ThenCanQueryForUsedIds()
        {
            int id1;
            int id2;

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                id1 = savingBlockStorage.Save(new byte[] { 1, 2, 3, 4, 5 });
                id2 = savingBlockStorage.Save(new byte[] { 7, 8, 9, 10, 11 });

                var ids = savingBlockStorage.GetIds().ToArray();

                Assert.AreEqual(2, ids.Length);
                CollectionAssert.Contains(ids, id1);
                CollectionAssert.Contains(ids, id2);
            }

            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                var ids = readingBlockStorage.GetIds().ToArray();

                Assert.AreEqual(2, ids.Length);
                CollectionAssert.Contains(ids, id1);
                CollectionAssert.Contains(ids, id2);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenOverwritingContentWithInvalidId_ThenThrows()
        {
            int id;
            byte[] content = Enumerable.Range(0, 70).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                overwritingBlockStorage.Overwrite(id + 1, new byte[] { 7, 8 }, 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenOverwritingWithLargerContentSizeThanAllocatedBlocks_ThenThrows()
        {
            int id;
            var content = new byte[] { 1, 2, 3, 4, 5 };
            byte[] update = Enumerable.Range(0, 33).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                overwritingBlockStorage.Overwrite(id, update, 1);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenOverwritingWithNegativeOffset_ThenThrows()
        {
            int id;
            var content = new byte[] { 1, 2, 3, 4, 5 };

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                id = savingBlockStorage.Save(content);
            }

            using (var overwritingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
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

            using (var blockStorage = new BlockStorage(TestStorageName, 128, quota * 1000))
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

            using (var blockStorage = new BlockStorage(TestStorageName, 128, quota * 1000))
            {
            }

            using (var isoStore = IsolatedStorageFile.GetUserStoreForApplication())
            {
                var newFreeSpace = isoStore.AvailableFreeSpace;

                Assert.IsTrue(previousFreeSpace - newFreeSpace < 10 * 1024, "Infrastructure is more than 10 KB");
            }
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenSavingContentLargerThanMaxSize_ThenThrows()
        {
            byte[] content = Enumerable.Range(0, 1025).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                savingBlockStorage.Save(content);
            }
        }

        [TestMethod]
        public void WhenSavingContentLargerThanMaxSize_ThenDoesNotAllocateSpace()
        {
            int id;
            byte[] largeContent = Enumerable.Range(0, 1025).Select(n => (byte)n).ToArray();
            byte[] smallContent = Enumerable.Range(0, 900).Select(n => (byte)n).ToArray();

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                try
                {
                    savingBlockStorage.Save(largeContent);
                    Assert.Fail("Exception not thrown.");
                }
                catch (InvalidOperationException) { }

                id = savingBlockStorage.Save(smallContent);
            }

            using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                var ids = readingBlockStorage.GetIds().ToArray();
                Assert.AreEqual(1, ids.Length);
                CollectionAssert.Contains(ids, id);

                var actualContent = readingBlockStorage.Read(id);
                CollectionAssert.AreEqual(smallContent, actualContent);
            }
        }

        [TestMethod]
        public void WhenWritingContentLargerThanAvailableIsoStorage_ThenThrowsExceptionButDoesNotCorruptState()
        {
            try
            {
                using (var blockStorage = new BlockStorage(TestStorageName, 128, 30000))
                {
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

                using (var blockStorage = new BlockStorage(TestStorageName, 128, 30000))
                {
                    var content = new byte[10000];
                    try
                    {
                        blockStorage.Save(content);
                        Assert.Fail("Exception not thrown.");
                    }
                    catch (IOException) {  }   // This is the expected exception
                }

                using (var readingBlockStorage = new BlockStorage(TestStorageName, 128, 30000))
                {
                    Assert.AreEqual(0, readingBlockStorage.GetIds().Count());
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
        [Ignore]
        public void WhenOpeningSecondInstance_ThenCanReadContent()
        {
            int id;
            var content = new byte[] { 1, 2, 3, 4, 5 };

            using (var savingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
            {
                id = savingBlockStorage.Save(content);

                using (var readingBlockStorage = new BlockStorage(TestStorageName, 32, 1024))
                {
                    Assert.AreEqual(id, readingBlockStorage.GetIds().Single());
                    CollectionAssert.AreEqual(content, readingBlockStorage.Read(id));
                }
            }
        }
    }
}

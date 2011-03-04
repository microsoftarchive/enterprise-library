using System;
using System.IO;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Tests.IsolatedStorage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class FatFixture
    {
        private const int Offset = 16;

        [TestMethod]
        public void WhenFatIsInitializedOnStream_ThenTruncatesStreamAndWritesHeader()
        {
            var stream = new MemoryStream(Enumerable.Range(1, 200).Select(i => (byte)i).ToArray());

            var fat = Fat.Initialize(stream, Offset, 32);

            Assert.AreNotEqual(200, stream.Length);
            Assert.AreEqual(32, fat.MaxBlocks);
            Assert.AreEqual(32, fat.FreeBlocks);
        }

        [TestMethod]
        public void WhenFatIsCreatedOnInitializedStream_ThenReadsHeader()
        {
            var stream = new MemoryStream();

            Fat.Initialize(stream, Offset, 32);

            var fat = new Fat(stream, Offset, 32);

            Assert.AreEqual(32, fat.FreeBlocks);
            Assert.AreEqual(32, fat.MaxBlocks);
            Assert.AreEqual(0, fat.GetCurrentBlockSequenceIds().Count());
        }

        [TestMethod]
        public void WhenRequestsSingleBlock_ThenAllocatesEntryInTheFatStream()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence = fat.AllocateBlockSequence(1);

            CollectionAssert.AreEqual(new[] { 0 }, sequence.ToArray());
            Assert.AreEqual(31, fat.FreeBlocks);

            var storage = stream.ToArray();
            var marker = BitConverter.ToInt32(storage, Fat.HeaderSize + Offset);
            Assert.AreEqual(Fat.LastBlock, marker);
        }

        [TestMethod]
        public void WhenRequestsMultipleBlocks_ThenAllocatesSequentialEntriesInTheFatStream()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence = fat.AllocateBlockSequence(5);

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3, 4 }, sequence.ToArray());
            Assert.AreEqual(27, fat.FreeBlocks);
        }

        [TestMethod]
        public void WhenRequestsSequenceForAllocatedBlocks_ThenReturnsSequence()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence = fat.AllocateBlockSequence(5);

            var retrievedSequence = fat.GetBlocksInSequence(sequence.ElementAt(0));

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3, 4 }, retrievedSequence.ToArray());
        }

        [TestMethod]
        public void WhenLoadingFatFromExistingStream_ThenCanReturnExistingSequence()
        {
            var stream = new MemoryStream();

            int id;

            var fat = Fat.Initialize(stream, Offset, 32);
            var sequence = fat.AllocateBlockSequence(5);
            id = sequence.ElementAt(0);


            var reloadFat = new Fat(stream, Offset, 32);

            CollectionAssert.AreEqual(new[] { 0, 1, 2, 3, 4 }, reloadFat.GetBlocksInSequence(id).ToArray());
            Assert.AreEqual(27, fat.FreeBlocks);
        }

        [TestMethod]
        public void WhenAllocatingSecondSequence_ThenGetsUnavailableBlocks()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            fat.AllocateBlockSequence(5);

            var newSequence = fat.AllocateBlockSequence(10);

            var retrievedSequence = fat.GetBlocksInSequence(newSequence.ElementAt(0));

            CollectionAssert.AreEqual(Enumerable.Range(5, 10).ToArray(), retrievedSequence.ToArray());
            Assert.AreEqual(17, fat.FreeBlocks);
        }

        [TestMethod]
        public void WhenSequenceIsReleased_ThenBlocksBecomeUnavailable()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence = fat.AllocateBlockSequence(5);

            fat.ReleaseBlockSequence(sequence.ElementAt(0));
            Assert.AreEqual(32, fat.FreeBlocks);

            var storage = stream.ToArray();
            var marker = BitConverter.ToInt32(storage, Fat.HeaderSize + Offset);
            Assert.AreEqual(Fat.UnusedBlock, marker);
        }

        [TestMethod]
        public void WhenAllocatingSequenceAfterRelease_ThenReleasedSpaceIsReused()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence1 = fat.AllocateBlockSequence(5);
            var sequence2 = fat.AllocateBlockSequence(5);
            var sequence3 = fat.AllocateBlockSequence(5);

            fat.ReleaseBlockSequence(sequence2.ElementAt(0));


            var sequence4 = fat.AllocateBlockSequence(7);

            CollectionAssert.AreEqual(Enumerable.Range(5, 5).Concat(Enumerable.Range(15, 2)).ToArray(), sequence4.ToArray());
            Assert.AreEqual(15, fat.FreeBlocks);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenRequestingMoreBlocksThanAvailable_ThenThrows()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence1 = fat.AllocateBlockSequence(5);

            fat.AllocateBlockSequence(30);
        }

        [TestMethod]
        public void WhenNew_ThenRetrievesEmptyCurrentSequenceIds()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequences = fat.GetCurrentBlockSequenceIds();

            CollectionAssert.AreEqual(new int[0], sequences.ToArray());
        }

        [TestMethod]
        public void WhenAllocatingMultiBlockSequence_ThenReturnsSequenceId()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence1 = fat.AllocateBlockSequence(10);

            var sequences = fat.GetCurrentBlockSequenceIds();

            CollectionAssert.AreEquivalent(new[] { sequence1.First() }, sequences.ToArray());
        }

        [TestMethod]
        public void WhenAllocatingAndReleasingMultipleSequences_ThenReturnsCurrentSequenceIds()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence1 = fat.AllocateBlockSequence(4);
            var sequence2 = fat.AllocateBlockSequence(5);
            fat.ReleaseBlockSequence(sequence2.First());
            var sequence3 = fat.AllocateBlockSequence(6);
            var sequence4 = fat.AllocateBlockSequence(7);
            var sequence5 = fat.AllocateBlockSequence(8);

            var sequences = fat.GetCurrentBlockSequenceIds();

            CollectionAssert.AreEquivalent(
                new[] { sequence1.First(), sequence3.First(), sequence4.First(), sequence5.First() },
                sequences.ToArray());
        }

        [TestMethod]
        public void WhenAllocatingAndReleasingMultipleSequences_ThenNewInstanceOnSameStreamReturnsCurrentSequenceIds()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence1 = fat.AllocateBlockSequence(4);
            var sequence2 = fat.AllocateBlockSequence(5);
            fat.ReleaseBlockSequence(sequence2.First());
            var sequence3 = fat.AllocateBlockSequence(6);
            var sequence4 = fat.AllocateBlockSequence(7);
            var sequence5 = fat.AllocateBlockSequence(8);

            var newFat = new Fat(stream, Offset, 32);

            var sequences = newFat.GetCurrentBlockSequenceIds();

            CollectionAssert.AreEquivalent(
                new[] { sequence1.First(), sequence3.First(), sequence4.First(), sequence5.First() },
                sequences.ToArray());
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void WhenTableInStreamHasCycles_ThenRequestForSequenceThrows()
        {
            var stream = new MemoryStream();

            Fat.Initialize(stream, Offset, 32);

            stream.Seek(Fat.HeaderSize + Offset, SeekOrigin.Begin);
            stream.Write(BitConverter.GetBytes(1), 0, sizeof(int));
            stream.Write(BitConverter.GetBytes(2), 0, sizeof(int));
            stream.Write(BitConverter.GetBytes(1), 0, sizeof(int));

            var corruptedFat = new Fat(stream, Offset, 32);

            corruptedFat.GetBlocksInSequence(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenRequestingSequenceForUnasignedId_ThenThrows()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            fat.GetBlocksInSequence(0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void WhenRequestingSequenceForNegativeId_ThenThrows()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            fat.GetBlocksInSequence(-1);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenRequestingSequenceForIdOverMaxBlock_ThenThrows()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            fat.GetBlocksInSequence(32);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenRequestingSequenceForIdThatIsNotBeginingOfSequence_ThenThrows()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            var sequence = fat.AllocateBlockSequence(2);

            var retrievedSequence = fat.GetBlocksInSequence(sequence.ElementAt(1));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void WhenInitializingWithNullStream_ThenThrows()
        {
            Fat.Initialize(null, 0, 32);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenInitializingWithNegativeOffset_ThenThrows()
        {
            var stream = new MemoryStream();

            Fat.Initialize(stream, -5, 32);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenInitializingWithNegativeMaxBlocks_ThenThrows()
        {
            var stream = new MemoryStream();

            Fat.Initialize(stream, 0, -32);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenInitializingWithZeroMaxBlocks_ThenThrows()
        {
            var stream = new MemoryStream();

            Fat.Initialize(stream, 0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void WhenReleasingInvalidSequence_ThenThrows()
        {
            var stream = new MemoryStream();

            var fat = Fat.Initialize(stream, Offset, 32);

            fat.ReleaseBlockSequence(0);
        }

        [TestMethod]
        public void WhenStreamCannotExpand_ThenThrowsAndDoesNotCorruptData()
        {
            var stream = new MemoryStream(new byte[300]);

            var fat = Fat.Initialize(stream, 0, 400);
            var allocated = fat.AllocateBlockSequence(1).First();
            try
            {
                fat.AllocateBlockSequence(320);
                Assert.Fail("Exception not thrown.");
            }
            catch (IOException)
            {
            }

            fat = new Fat(stream, 0, 400);
            Assert.AreEqual(allocated, fat.GetCurrentBlockSequenceIds().Single());
        }
    }
}

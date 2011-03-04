using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class Fat
    {
        private readonly Stream fatStream;
        private readonly int fatStreamOffset;
        private readonly int maxBlocks;
        private readonly int[] blockEntries;
        private readonly HashSet<int> allocatedSequences;
        private int freeBlocks;

        private static byte[] FatHeader = new byte[] { 69, 76 };
        public const int HeaderSize = 32;

        public const int UnusedBlock = -2;
        public const int LastBlock = -1;

        public Fat(Stream fatStream, int offset, int maxBlocks)
        {
            CheckFatParameters(fatStream, offset, maxBlocks);

            this.fatStream = fatStream;
            this.fatStreamOffset = offset;
            this.maxBlocks = maxBlocks;

            this.blockEntries = new int[this.maxBlocks];
            this.allocatedSequences = new HashSet<int>(Enumerable.Range(0, this.blockEntries.Length));

            LoadEntries();
        }

        public static Fat Initialize(Stream fatStream, int offset, int maxBlocks)
        {
            CheckFatParameters(fatStream, offset, maxBlocks);

            fatStream.Seek(offset, SeekOrigin.Begin);

            fatStream.Write(FatHeader, 0, 2);
            var maxBlocksBytes = BitConverter.GetBytes(maxBlocks);
            fatStream.Write(maxBlocksBytes, 0, maxBlocksBytes.Length);

            fatStream.SetLength(offset + HeaderSize);

            fatStream.Flush();

            return new Fat(fatStream, offset, maxBlocks);
        }

        private static void CheckFatParameters(Stream fatStream, int offset, int maxBlocks)
        {
            if (fatStream == null)
            {
                throw new ArgumentNullException("fatStream");
            }

            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            if (maxBlocks <= 0)
            {
                throw new ArgumentOutOfRangeException("maxBlocks");
            }
        }

        public IEnumerable<int> AllocateBlockSequence(int blockCount)
        {
            if (blockCount <= 0)
            {
                throw new ArgumentOutOfRangeException("blockCount");
            }

            if (blockCount > this.freeBlocks)
            {
                throw new AllocationException("Avaliable blocks not enough to allocate specified space.");
            }

            var blocks = new List<int>(blockCount);

            for (int i = 0; i < this.maxBlocks; i++)
            {
                if (this.blockEntries[i] == UnusedBlock)
                {
                    blocks.Add(i);

                    if (blocks.Count == blockCount)
                    {
                        return UpdateBlocks(blocks);
                    }
                }
            }

            throw new InvalidDataException("fat is corrupt");
        }

        public IEnumerable<int> GetBlocksInSequence(int sequenceId)
        {
            if (sequenceId < 0)
            {
                throw new ArgumentOutOfRangeException("sequenceId", "Sequence id cannot be negative");
            }

            if (sequenceId >= this.maxBlocks)
            {
                throw new ArgumentException("Invalid sequence id", "sequenceId");
            }

            if (!this.allocatedSequences.Contains(sequenceId))
            {
                throw new ArgumentException("Invalid sequence id", "sequenceId");
            }

            var sequence = new List<int>();

            if (this.blockEntries[sequenceId] == UnusedBlock)
            {
                throw new ArgumentException("Invalid sequence id", "sequenceId");
            }

            do
            {
                if (sequence.Count > this.maxBlocks)
                {
                    // cycle detected
                    throw new InvalidOperationException("fat corrupted");
                }

                if (sequenceId == UnusedBlock)
                {
                    // link to unassigned block
                    throw new InvalidOperationException("fat corrupted");
                }

                sequence.Add(sequenceId);
                sequenceId = this.blockEntries[sequenceId];
            } while (sequenceId != LastBlock);

            return sequence;
        }

        public IEnumerable<int> GetCurrentBlockSequenceIds()
        {
            return new HashSet<int>(this.allocatedSequences);
        }

        public void ReleaseBlockSequence(int sequenceId)
        {
            var sequence = GetBlocksInSequence(sequenceId);
            foreach (var blockId in sequence)
            {
                this.SetNextBlockId(blockId, UnusedBlock);
            }

            this.allocatedSequences.Remove(sequenceId);
        }

        private IEnumerable<int> UpdateBlocks(List<int> blocks)
        {
            int i = 0;

            try
            {
                for (; i < blocks.Count - 1; i++)
                {
                    var blockId = blocks[i];
                    var nextBlockId = blocks[i + 1];

                    SetNextBlockId(blockId, nextBlockId);
                }

                SetNextBlockId(blocks[i], LastBlock);
                this.fatStream.Flush();
                this.allocatedSequences.Add(blocks[0]);
            }
            catch (Exception ex)
            {
                try
                {
                    for (int j = 0; j <= i; j++)
                    {
                        this.SetNextBlockId(blocks[j], UnusedBlock);
                        this.fatStream.Flush();
                    }
                }
                catch { }   // Best effort to restore FAT

                throw new IOException("Cannot write FAT entry", ex);
            }

            return blocks;
        }

        private void SetNextBlockId(int blockId, int nextBlockId)
        {
            var currentNextBlockId = this.blockEntries[blockId];

            this.blockEntries[blockId] = nextBlockId;

            this.fatStream.Seek(this.fatStreamOffset + HeaderSize + (blockId * sizeof(int)), SeekOrigin.Begin);
            this.fatStream.Write(BitConverter.GetBytes(nextBlockId), 0, sizeof(int));

            if (currentNextBlockId != nextBlockId)
            {
                if (nextBlockId == UnusedBlock)
                {
                    this.freeBlocks++;
                }
                else
                {
                    this.freeBlocks--;
                }
            }
        }

        private void LoadEntries()
        {
            byte[] bytes = new byte[sizeof(int)];

            int currentBlock = 0;
            if (this.fatStream.CanRead)
            {
                int readBytes = 0;

                this.fatStream.Seek(this.fatStreamOffset + HeaderSize, SeekOrigin.Begin);

                for (; currentBlock < this.maxBlocks; currentBlock++)
                {
                    readBytes = this.fatStream.Read(bytes, 0, sizeof(int));

                    if (readBytes == 0)
                    {
                        break;
                    }

                    if (readBytes != sizeof(int))
                    {
                    }

                    var entry = BitConverter.ToInt32(bytes, 0);
                    this.blockEntries[currentBlock] = entry;
                    if (entry != UnusedBlock)
                    {

                        if (entry != LastBlock)
                        {
                            this.allocatedSequences.Remove(entry);
                        }
                    }
                    else
                    {
                        this.freeBlocks++;
                        this.allocatedSequences.Remove(currentBlock);
                    }
                }
            }

            this.freeBlocks += this.maxBlocks - currentBlock;
            for (; currentBlock < this.maxBlocks; currentBlock++)
            {
                this.blockEntries[currentBlock] = UnusedBlock;
                this.allocatedSequences.Remove(currentBlock);
            }
        }

        public int MaxBlocks
        {
            get
            {
                return this.maxBlocks;
            }
        }

        public int FreeBlocks
        {
            get
            {
                return this.freeBlocks;
            }
        }

        /// <summary>
        /// Suggested permutations that can move all free blocks to the end of the stream.
        /// </summary>
        /// <returns>Returns an enumearion of Tuples with suggested permutations. Item1 contains the original id. Item2 contains the suggested target id. Item3 specifies if
        /// the original id is head of the sequence.</returns>
        public IEnumerable<Tuple<int,int,bool>> GetCompactSuggestions()
        {
            int freeBlocksIndex = 0;
            int usedBlocksIndex = this.MaxBlocks - 1;

            var suggestions = new List<Tuple<int, int, bool>>();

            while (freeBlocksIndex < usedBlocksIndex)
            {
                while (this.blockEntries[freeBlocksIndex] != UnusedBlock && freeBlocksIndex < usedBlocksIndex)
                    freeBlocksIndex++;

                while (this.blockEntries[usedBlocksIndex] == UnusedBlock && freeBlocksIndex < usedBlocksIndex)
                    usedBlocksIndex--;

                if (freeBlocksIndex >= usedBlocksIndex)
                {
                    break;
                }

                suggestions.Add(new Tuple<int,int,bool>(usedBlocksIndex, freeBlocksIndex, this.allocatedSequences.Contains(usedBlocksIndex)));
                usedBlocksIndex--;
                freeBlocksIndex++;
            }

            return suggestions;
        }

        public void AcceptCompactSuggestion(Tuple<int, int, bool> suggestion)
        {
            if (this.blockEntries[suggestion.Item2] != UnusedBlock)
                throw new ArgumentException("Item2 is not a free block.");

            if (suggestion.Item3 && !this.allocatedSequences.Contains(suggestion.Item1))
                throw new ArgumentException("Item1 is not a sequence head.");

            if (!suggestion.Item3 && this.allocatedSequences.Contains(suggestion.Item1))
                throw new ArgumentException("Item1 is a sequence head.");

            this.SetNextBlockId(suggestion.Item2, this.blockEntries[suggestion.Item1]);
            this.SetNextBlockId(suggestion.Item1, UnusedBlock);

            if (suggestion.Item3)
            {
                this.allocatedSequences.Remove(suggestion.Item1);
                this.allocatedSequences.Add(suggestion.Item2);
            }
            else
            {
                for (int i = 0; i < this.maxBlocks; i++)
                {
                    if (this.blockEntries[i] == suggestion.Item1)
                    {
                        this.SetNextBlockId(i, suggestion.Item2);
                    }
                }
            }
        }

        public int Trim()
        {
            int usedBlocksIndex = this.MaxBlocks - 1;

            while (this.blockEntries[usedBlocksIndex] == UnusedBlock && usedBlocksIndex >= 0)
                usedBlocksIndex--;

            int firstFreeBlock = usedBlocksIndex + 1;

            var suggestedLength = this.fatStreamOffset + HeaderSize + (firstFreeBlock * sizeof(int));
            this.fatStream.SetLength(suggestedLength);

            return firstFreeBlock;
        }
    }
}

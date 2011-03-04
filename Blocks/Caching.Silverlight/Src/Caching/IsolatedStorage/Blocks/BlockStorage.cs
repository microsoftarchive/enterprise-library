using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage
{
    public class BlockStorage : IDisposable
    {
        private const int MinBlockSize = 32;
        private const int MetadataLength = sizeof(int) * 2;

        private readonly string name;
        private readonly int blockSize;
        private readonly long maxSize;

        private IsolatedStorageFile store;
        private IsolatedStorageFileStream fatStream;
        private IsolatedStorageFileStream contentStream;
        private Fat fat;

        public BlockStorage(string name, int blockSize, long maxSize)
        {
            this.name = GetDirectoryName(name);

            if (blockSize < MinBlockSize)
                throw new ArgumentException("blockSize", string.Format(CultureInfo.CurrentCulture, "The minimum block size is {0} bytes.", MinBlockSize));
            if (blockSize > maxSize)
                throw new ArgumentException("blockSize", "The block size cannot be greater than the max size.");

            this.blockSize = blockSize;
            this.maxSize = maxSize;

            this.store = IsolatedStorageFile.GetUserStoreForApplication();
            var maxBlocks = MathHelper.SafeDivision(this.maxSize, this.blockSize);
            if (!store.DirectoryExists(this.name) || !store.FileExists(GetFatFileName(this.name)) || !store.FileExists(GetContentFileName(this.name)))
            {
                this.InitializeNew(maxBlocks);
            }
            else
            {
                // open Read on initialization and then switch to just Write?
                this.fatStream = store.OpenFile(GetFatFileName(this.name), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);

                var buffer = new byte[sizeof(int)];
                this.fatStream.Read(buffer, 0, buffer.Length);
                int previousBlockSize = BitConverter.ToInt32(buffer, 0);

                if (previousBlockSize == this.blockSize)
                {
                    this.fat = new Fat(this.fatStream, sizeof(int), maxBlocks);
                    this.contentStream = store.OpenFile(GetContentFileName(this.name), FileMode.Open, FileAccess.ReadWrite, FileShare.Read);
                }
                else
                {
                    this.fatStream.Dispose();
                    this.fatStream = null;
                    this.store.Dispose();
                    DeleteStorage(this.name);
                    this.store = IsolatedStorageFile.GetUserStoreForApplication();
                    this.InitializeNew(maxBlocks);
                }
            }
        }

        private static string GetDirectoryName(string name)
        {
            if (name == null)
                throw new ArgumentNullException("name");

            name = name.Trim();
            if (name == string.Empty)
                throw new ArgumentException("name");

            Regex nameRegex = new Regex(@"[^\w\d_]");
            name = nameRegex.Replace(name, "_");
            name = "Cache_" + name;
            return name;
        }

        private void InitializeNew(int maxBlocks)
        {
            store.CreateDirectory(this.name);

            this.fatStream = store.OpenFile(GetFatFileName(this.name), FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
            this.fatStream.Write(BitConverter.GetBytes(this.blockSize), 0, sizeof(int));
            this.fat = Fat.Initialize(this.fatStream, sizeof(int), maxBlocks);

            this.contentStream = store.OpenFile(GetContentFileName(this.name), FileMode.Create, FileAccess.ReadWrite, FileShare.Read);
        }

        public static void DeleteStorage(string name)
        {
            name = GetDirectoryName(name);
            using (var store = IsolatedStorageFile.GetUserStoreForApplication())
            {
                if (store.DirectoryExists(name))
                {
                    foreach (string fileName in new[] { GetFatFileName(name), GetContentFileName(name) })
                    {
                        if (store.FileExists(fileName))
                        {
                            store.DeleteFile(fileName);
                        }
                    }

                    store.DeleteDirectory(name);
                }
            }
        }

        public int Save(byte[] content)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }

            //Write content length as the first bytes of the blob.
            int contentLength = content.Length;
            byte[] contentLengthBytes = BitConverter.GetBytes(contentLength);
            byte[] checksumBytes = BitConverter.GetBytes(~contentLength);
            int totalLength = MetadataLength + contentLength;

            var blocks = this.fat.AllocateBlockSequence(MathHelper.SafeDivision(totalLength, blockSize)).ToArray();

            try
            {
                this.WriteContent(contentLengthBytes, blocks, 0, true);
                this.WriteContent(checksumBytes, blocks, sizeof(int), false);
                this.WriteContent(content, blocks, MetadataLength, false);

                this.contentStream.Flush();
            }
            catch (IsolatedStorageException ex)
            {
                this.fat.ReleaseBlockSequence(blocks[0]);

                throw new AllocationException("Available space not enough to store content.", ex);
            }

            return blocks[0];
        }

        /// <summary>
        /// Overwrites a portion of the content.
        /// </summary>
        /// <param name="id">The block id.</param>
        /// <param name="content">The content to overwrite.</param>
        /// <param name="offset">The offset within the current block data where the content will be updated.</param>
        /// <remarks>This method should be used to overwrite bytes when there is a guarantee that it will fit the previous content size.
        /// For performance reasons, this method does not check what was the previous content size. If the data being updated should result in
        /// an updated content length, then this method should not be used, as the data will be corrupt. In this case, the alternative
        /// is to invoke the <see cref="Remove"/> method and then invoke <see cref="Save"/> to re-add it.</remarks>
        public void Overwrite(int id, byte[] content, int offset)
        {
            if (content == null)
            {
                throw new ArgumentNullException("content");
            }
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException("offset");
            }

            offset += MetadataLength;

            var blocks = this.fat.GetBlocksInSequence(id).ToArray();

            if (blocks.Length < MathHelper.SafeDivision(offset + content.Length, this.blockSize))
            {
                throw new InvalidOperationException("The content to overwrite overflows the current allocated disk space. Use overwrite method to overwrite a portion of the content that is of fixed size, otherwise remove the content and re-add it.");
            }

            this.WriteContent(content, blocks, offset, true);

            this.contentStream.Flush();
        }

        public byte[] Read(int id)
        {
            byte[] buffer = new byte[this.blockSize];
            int contentLength = 0;

            var blocks = this.fat.GetBlocksInSequence(id).ToArray();
            if (blocks.Max() * this.blockSize > this.contentStream.Length)
            {
                throw new InvalidDataException("The content file is too short to get the expected data.");
            }

            int currentBlockIndex = 0;
            this.SeekToBlock(blocks[currentBlockIndex]);
            this.contentStream.Read(buffer, 0, MetadataLength);
            contentLength = BitConverter.ToInt32(buffer, 0);

            if ((contentLength != ~(BitConverter.ToInt32(buffer, sizeof(int))))
                || (contentLength + MetadataLength > blocks.Length * this.blockSize))
            {
                throw new InvalidDataException("Metadata is incorrect, possibly due to a corrupt FAT.");
            }

            int blockContentRemaining = Math.Min(this.blockSize - MetadataLength, contentLength);

            byte[] content = new byte[contentLength];
            int currentContentPosition = 0;
            while (currentContentPosition < contentLength)
            {
                int read = this.contentStream.Read(buffer, 0, blockContentRemaining);
                if (read != blockContentRemaining)
                {
                    throw new InvalidDataException("The content file is too short to get the expected data.");
                }

                Array.Copy(buffer, 0, content, currentContentPosition, blockContentRemaining);

                currentContentPosition += blockContentRemaining;
                blockContentRemaining = Math.Min(this.blockSize, contentLength - currentContentPosition);

                if (currentContentPosition < contentLength)
                {
                    currentBlockIndex++;
                    this.SeekToBlock(blocks[currentBlockIndex]);
                }
            }

            return content;
        }

        public void Remove(int id)
        {
            this.fat.ReleaseBlockSequence(id);
        }

        public IEnumerable<int> GetIds()
        {
            return this.fat.GetCurrentBlockSequenceIds();
        }

        public IDictionary<int, int> Compact()
        {
            var ids = new Dictionary<int, int>();

            var candidates = this.fat.GetCompactSuggestions();
            var content = new byte[this.blockSize];
            foreach (var candidate in candidates)
            {
                this.SeekToBlock(candidate.Item1);
                int read = this.contentStream.Read(content, 0, this.blockSize);
                this.SeekToBlock(candidate.Item2);
                this.contentStream.Write(content, 0, read);

                this.fat.AcceptCompactSuggestion(candidate);

                if (candidate.Item3)
                {
                    ids.Add(candidate.Item1, candidate.Item2);
                }
            }

            int firstFreeBlock = this.fat.Trim();
            long suggestedLength = firstFreeBlock * this.blockSize;
            this.contentStream.SetLength(suggestedLength);

            return ids;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.fat = null;
                using (this.fatStream) this.fatStream = null;
                using (this.contentStream) this.contentStream = null;
                using (this.store) this.store = null;
            }
        }

        ~BlockStorage()
        {
            Dispose(false);
        }

        private static string GetContentFileName(string storeName)
        {
            return Path.Combine(storeName, "content.dat");
        }

        private static string GetFatFileName(string storeName)
        {
            return Path.Combine(storeName, "fat.dat");
        }

        private void SeekToBlock(int blockId, int offsetInBlock = 0)
        {
            this.contentStream.Seek(blockId * this.blockSize + offsetInBlock, SeekOrigin.Begin);
        }

        private void WriteContent(byte[] content, int[] blocks, int currentTargetOffset, bool seek)
        {
            int currentBlockIndex = currentTargetOffset / this.blockSize;
            if (seek)
            {
                this.SeekToBlock(blocks[currentBlockIndex], currentTargetOffset % this.blockSize);
            }

            int blockSpace = this.blockSize - (currentTargetOffset % this.blockSize);
            int contentLength = content.Length;
            int currentContentPosition = 0;
            while (currentContentPosition < contentLength)
            {
                if (blockSpace < contentLength - currentContentPosition)
                {
                    this.contentStream.Write(content, currentContentPosition, blockSpace);
                    currentContentPosition += blockSpace;
                    currentBlockIndex++;
                    this.SeekToBlock(blocks[currentBlockIndex]);
                    blockSpace = this.blockSize;
                }
                else
                {
                    this.contentStream.Write(content, currentContentPosition, contentLength - currentContentPosition);
                    currentContentPosition = contentLength;
                }
            }
        }

        public long MaxSize
        {
            get { return this.maxSize; }
        }

        public long UsedSize
        {
            get { return this.MaxSize - (this.fat.FreeBlocks * this.blockSize); }
        }

        public long UsedPhysicalSize
        {
            get { return this.contentStream.Length + this.fatStream.Length; }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging.Properties;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners
{
    /// <summary>
    /// Implements bounded storage on a stream, overwriting older entries.
    /// </summary>
    public class BoundedStreamStorage : IDisposable
    {
        private static readonly byte[] streamHeaderBytes = new byte[] { 15, 16 };
        private static readonly byte[] entryHeaderBytes = new byte[] { 17, 18 };
        private const int streamFullHeaderSize = sizeof(byte) * 2 + sizeof(int) * 4;
        private const int headOffset = sizeof(byte) * 2 + sizeof(int) * 2;
        private const int tailOffset = headOffset + sizeof(int);
        private const int entryFullHeaderSize = sizeof(byte) * 2 + sizeof(int);
        private const int initialHead = -1;
        private const int initialTail = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoundedStreamStorage"/> class with a <see cref="Stream"/>.
        /// </summary>
        /// <remarks>
        /// Requires the stream to be properly initialized for bounded storage.
        /// </remarks>
        /// <param name="stream">The stream to manage.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="stream"/> is null.</exception>
        /// <exception cref="ArgumentException">when <paramref name="stream"/> is not a properly initialized Stream.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by Guard")]
        public BoundedStreamStorage(Stream stream)
        {
            Guard.ArgumentNotNull(stream, "stream");

            this.stream = stream;
            var streamLength = stream.Length;

            this.stream.Seek(0L, SeekOrigin.Begin);

            var headerBytes = new byte[streamHeaderBytes.Length];
            this.stream.Read(headerBytes, 0, streamHeaderBytes.Length);
            CheckHeaderBytes(headerBytes, streamHeaderBytes, "stream");

            this.maxSizeInBytes = ReadIntParameter(this.stream, "maxSizeInBytes");
            this.actualMaxSizeInBytes = ReadIntParameter(this.stream, "effectiveMaxSizeInBytes");

            if (this.maxSizeInBytes < 0)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorInvalidStreamParameter, "maxSizeInBytes"), "stream");
            }
            if (this.actualMaxSizeInBytes < 0 || this.actualMaxSizeInBytes > this.maxSizeInBytes)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorInvalidStreamParameter, "actualMaxSizeInBytes"), "stream");
            }

            this.effectiveMaxSizeInBytes = this.actualMaxSizeInBytes - (int)streamFullHeaderSize;

            this.head = ReadIntParameter(this.stream, "head");
            this.tail = ReadIntParameter(this.stream, "tail");

            if ((this.head < 0 && this.head != initialHead) || this.head >= this.effectiveMaxSizeInBytes)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorInvalidStreamParameter, "head"), "stream");
            }
            if (this.tail < 0 || this.tail >= this.effectiveMaxSizeInBytes)
            {
                throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorInvalidStreamParameter, "head"), "stream");
            }

            this.lockObject = new object();
        }

        private readonly object lockObject;
        private readonly Stream stream;
        private readonly int maxSizeInBytes;
        private readonly int actualMaxSizeInBytes;
        private readonly int effectiveMaxSizeInBytes;
        private int head;
        private int tail;

        /// <summary>
        /// Gets the size of the stream header.
        /// </summary>
        public static int StreamHeaderSize
        {
            get
            {
                return streamFullHeaderSize;
            }
        }

        /// <summary>
        /// Gets the size of the entry header.
        /// </summary>
        public static int EntryHeaderSize
        {
            get
            {
                return entryFullHeaderSize;
            }
        }

        /// <summary>
        /// Gets the max size of the bounded storage as originally requested.
        /// </summary>
        public int MaxSizeInBytes
        {
            get
            {
                return this.maxSizeInBytes;
            }
        }

        /// <summary>
        /// Gets the actual max size of the bounded storage.
        /// </summary>
        public int ActualMaxSizeInBytes
        {
            get
            {
                return this.actualMaxSizeInBytes;
            }
        }

        /// <summary>
        /// Gets the max size that is available for storage of entries.
        /// </summary>
        public int EffectiveMaxSizeInBytes
        {
            get
            {
                return this.effectiveMaxSizeInBytes;
            }
        }

        /// <summary>
        /// Gets the index of the head.
        /// </summary>
        public int Head
        {
            get
            {
                return this.head;
            }
        }

        /// <summary>
        /// Gets the index of the tail.
        /// </summary>
        public int Tail
        {
            get
            {
                return this.tail;
            }
        }

        /// <summary>
        /// Initializes a stream to be used for bounded storage with the specified parameters.
        /// </summary>
        /// <param name="stream">The <see cref="Stream"/> to initialize.</param>
        /// <param name="maxSizeInBytes">The max size in bytes as specified.</param>
        /// <param name="actualMaxSizeInBytes">The actual max size in bytes</param>
        /// <remarks>
        /// The stream is expected to have allow for the header to be written.
        /// </remarks>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by Guard")]
        public static void Initialize(Stream stream, int maxSizeInBytes, int actualMaxSizeInBytes)
        {
            Guard.ArgumentNotNull(stream, "stream");

            stream.Seek(0, SeekOrigin.Begin);
            stream.Write(streamHeaderBytes, 0, streamHeaderBytes.Length);
            stream.Write(BitConverter.GetBytes(maxSizeInBytes), 0, sizeof(int));
            stream.Write(BitConverter.GetBytes(actualMaxSizeInBytes), 0, sizeof(int));
            stream.Write(BitConverter.GetBytes(initialHead), 0, sizeof(int));
            stream.Write(BitConverter.GetBytes(initialTail), 0, sizeof(int));
            stream.Flush();
        }

        /// <summary>
        /// Adds an entry to the storage.
        /// </summary>
        /// <remarks>
        /// Adding a new entry might result in older entries to be deleted.
        /// </remarks>
        /// <param name="entry">The entry to add.</param>
        /// <exception cref="ArgumentNullException">when <paramref name="entry"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">when <paramref name="entry"/> is larger than the effective max size.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:Validate arguments of public methods", MessageId = "0", Justification = "Validated by Guard")]
        public void Add(byte[] entry)
        {
            Guard.ArgumentNotNull(entry, "entry");

            lock (this.lockObject)
            {
                var entrySize = entry.Length;
                var totalSize = entryFullHeaderSize + entrySize;

                if (totalSize > this.effectiveMaxSizeInBytes)
                {
                    throw new ArgumentOutOfRangeException("entry", Resources.ErrorEntryIsLargerThanTheEffectiveMaximumSize);
                }

                var newHead = ComputeNewHead(totalSize);
                if (this.head != newHead)
                {
                    Debug.Assert(newHead >= 0 && newHead < this.effectiveMaxSizeInBytes, "Invalid head value");

                    // make room for the content by moving the current head, effectively discarding older entries
                    var headBytes = BitConverter.GetBytes(newHead);
                    this.stream.Seek(headOffset, SeekOrigin.Begin);
                    this.stream.Write(headBytes, 0, headBytes.Length);
                    this.head = newHead;
                }

                // write the entry in the stream
                this.stream.Seek(streamFullHeaderSize + this.tail, SeekOrigin.Begin);
                this.WrappedWrite(entryHeaderBytes);
                var sizeBytes = BitConverter.GetBytes(entry.Length);
                this.WrappedWrite(sizeBytes);
                this.WrappedWrite(entry);

                // update the tail to include the new entry
                var newTail = (int)(this.stream.Position - streamFullHeaderSize);
                var tailBytes = BitConverter.GetBytes(newTail);
                this.stream.Seek(tailOffset, SeekOrigin.Begin);
                this.stream.Write(tailBytes, 0, tailBytes.Length);
                this.tail = newTail;
            }
        }

        /// <summary>
        /// Returns the entries currently serialized in the storage, in the order in which they were added.
        /// </summary>
        /// <returns>The entries.</returns>
        public IEnumerable<byte[]> RetrieveEntries()
        {
            lock (lockObject)
            {
                var entries = new List<byte[]>();

                if (this.head != initialHead)
                {
                    this.stream.Seek(streamFullHeaderSize + this.head, SeekOrigin.Begin);

                    var headerBytes = new byte[entryHeaderBytes.Length];
                    var entrySizeBytes = new byte[sizeof(int)];
                    int entrySize = 0;
                    long endPosition = this.tail + streamFullHeaderSize;

#if DEBUG
                    int totalSize = 0;
#endif

                    var done = false;
                    while (!done)
                    {
                        this.WrappedRead(headerBytes);
                        CheckHeaderBytes(headerBytes, entryHeaderBytes, "entry");
                        this.WrappedRead(entrySizeBytes);
                        entrySize = BitConverter.ToInt32(entrySizeBytes, 0);
                        var entryBytes = new byte[entrySize];
                        this.WrappedRead(entryBytes);

                        entries.Add(entryBytes);

                        if (this.stream.Position == endPosition)
                        {
                            done = true;
                        }

#if DEBUG
                        totalSize += entryFullHeaderSize + entrySize;
                        Debug.Assert(totalSize <= this.effectiveMaxSizeInBytes, "read more than the total size");
#endif
                    }
                }

                return entries;
            }
        }

        /// <summary>
        /// Flushes buffered data in the stream.
        /// </summary>
        public void Flush()
        {
            this.stream.Flush();
        }

        private static int ReadIntParameter(Stream stream, string valueName)
        {
            var intBuffer = new byte[sizeof(int)];
            var read = stream.Read(intBuffer, 0, intBuffer.Length);

            if (read < sizeof(int))
            {
                throw new ArgumentException(
                    string.Format(CultureInfo.CurrentCulture, Resources.ErrorCannotReadIntegerFromStream, valueName),
                    "stream");
            }

            return BitConverter.ToInt32(intBuffer, 0);
        }

        private void WrappedSeek(int count)
        {
            int preWrapCount;
            int wrapCount;

            ComputeWrappedOperationParameters(count, out preWrapCount, out wrapCount);

            if (wrapCount == -1)
            {
                this.stream.Seek(preWrapCount, SeekOrigin.Current);
            }
            else
            {
                this.stream.Seek(wrapCount + streamFullHeaderSize, SeekOrigin.Begin);
            }
        }

        private void WrappedRead(byte[] bytes)
        {
            int preWrapCount;
            int wrapCount;

            ComputeWrappedOperationParameters(bytes.Length, out preWrapCount, out wrapCount);

            this.stream.Read(bytes, 0, preWrapCount);
            if (wrapCount >= 0)
            {
                this.stream.Seek(streamFullHeaderSize, SeekOrigin.Begin);
                this.stream.Read(bytes, preWrapCount, wrapCount);
            }
        }

        private void WrappedWrite(byte[] bytes)
        {
            int preWrapCount, wrapCount;

            ComputeWrappedOperationParameters(bytes.Length, out preWrapCount, out wrapCount);

            this.stream.Write(bytes, 0, preWrapCount);
            if (wrapCount >= 0)
            {
                this.stream.Seek(streamFullHeaderSize, SeekOrigin.Begin);
                this.stream.Write(bytes, preWrapCount, wrapCount);
            }
        }

        private void ComputeWrappedOperationParameters(int count, out int preWrapCount, out int wrapCount)
        {
            var position = this.stream.Position;

            var endPosition = position + count;

            // need to wrap around?
            if (endPosition >= this.actualMaxSizeInBytes)
            {
                preWrapCount = this.actualMaxSizeInBytes - (int)position;
                wrapCount = (int)endPosition - actualMaxSizeInBytes;
            }
            else
            {
                preWrapCount = count;
                wrapCount = -1;
            }

            Debug.Assert(wrapCount + streamFullHeaderSize <= position, "Wrapped operation overflow");
        }

        private static void CheckHeaderBytes(byte[] headerBytes, byte[] expectedHeaderBytes, string headerName)
        {
            Debug.Assert(headerBytes.Length == expectedHeaderBytes.Length, "Header length mismatch");

            for (int i = 0; i < headerBytes.Length; i++)
            {
                if (headerBytes[i] != expectedHeaderBytes[i])
                {
                    throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, Resources.ErrorHeaderMismatch, headerName));
                }
            }

        }

        private int ComputeNewHead(int newEntrySize)
        {
            if (this.head == initialHead)
            {
                // no entries yet, use first available position
                return 0;
            }

            var remainingSize =
                this.head < this.tail
                    ? this.effectiveMaxSizeInBytes - this.tail + this.head
                    : this.head - this.tail;

            if (newEntrySize <= remainingSize)
            {
                // new entry fits, no need to overwrite existing entries
                return this.head;
            }

            // need to overwrite previous entries

            var requiredSize = newEntrySize - remainingSize;
            var headerBytes = new byte[entryHeaderBytes.Length];
            var entrySizeBytes = new byte[sizeof(int)];

            this.stream.Seek(streamFullHeaderSize + this.head, SeekOrigin.Begin);

            while (true)
            {
                var currentHead = (int)(this.stream.Position - streamFullHeaderSize);

                this.WrappedRead(headerBytes);
                CheckHeaderBytes(headerBytes, entryHeaderBytes, "entry");

                this.WrappedRead(entrySizeBytes);
                var entrySize = BitConverter.ToInt32(entrySizeBytes, 0);

                if (entrySize < 0)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ErrorEntryDataInDiskIsInvalidNegativeSize,
                            currentHead));
                }

                var totalEntrySize = entryFullHeaderSize + entrySize;

                if (totalEntrySize > this.effectiveMaxSizeInBytes)
                {
                    throw new InvalidOperationException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ErrorEntryDataInDiskIsInvalidLongerThanMaximum,
                            currentHead));
                }

                if (totalEntrySize >= requiredSize)
                {
                    // the current entry provides enough room
                    // return the begining of the next entry as the new head, wrapping around the max size
                    return (currentHead + totalEntrySize) % this.effectiveMaxSizeInBytes;
                }
                else
                {
                    // not enough room, try the next one
                    this.WrappedSeek(entrySize);
                    requiredSize -= totalEntrySize;
                }
            }
        }

        /// <summary>
        /// Dispose of the storage before garbage collection.
        /// </summary>
        ~BoundedStreamStorage()
        {
            Dispose(false);
        }

        /// <summary>
        /// Dispose of the storage before garbage collection.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose of the storage before garbage collection.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true"/> if disposing; otherwise, <see langword="false"/>.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.stream.Dispose();
            }
        }
    }
}
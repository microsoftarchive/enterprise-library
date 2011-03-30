using System;
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class InvalidInitializationFixture
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void when_there_is_no_stream_header_then_throws()
        {
            using (var stream = new MemoryStream())
            {
                new BoundedStreamStorage(stream);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void when_stream_header_is_incomplete_then_throws()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte(15);
                new BoundedStreamStorage(stream);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void when_stream_header_is_invalid_then_throws()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte(15);
                stream.WriteByte(20);
                new BoundedStreamStorage(stream);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void when_max_size_is_missing_then_throws()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte(15);
                stream.WriteByte(16);
                new BoundedStreamStorage(stream);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void when_max_size_is_incomplete_then_throws()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte(15);
                stream.WriteByte(16);
                stream.Write(BitConverter.GetBytes(100), 0, 3);
                new BoundedStreamStorage(stream);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void when_max_size_is_negative_then_throws()
        {
            using (var stream = new MemoryStream())
            {
                stream.WriteByte(15);
                stream.WriteByte(16);
                stream.Write(BitConverter.GetBytes(-100), 0, sizeof(int));
                new BoundedStreamStorage(stream);
            }
        }
    }
}

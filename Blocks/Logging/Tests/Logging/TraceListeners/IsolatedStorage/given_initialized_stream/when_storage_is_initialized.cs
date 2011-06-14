//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_initialized_stream
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_storage_is_initialized : Context
    {
        private BoundedStreamStorage storage;

        protected override void Act()
        {
            base.Act();

            this.storage = new BoundedStreamStorage(this.stream);
        }

        [TestMethod]
        public void then_storage_reads_initial_properties()
        {
            Assert.AreEqual(1024, this.storage.MaxSizeInBytes);
            Assert.AreEqual(512, this.storage.ActualMaxSizeInBytes);
            Assert.AreEqual(-1, this.storage.Head);
            Assert.AreEqual(0, this.storage.Tail);
        }
    }
}

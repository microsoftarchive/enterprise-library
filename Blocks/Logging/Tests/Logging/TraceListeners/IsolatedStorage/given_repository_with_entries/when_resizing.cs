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

using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_with_entries
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_resizing : Context
    {
        private const int newSize = 4;

        protected override void Act()
        {
            base.Act();

            this.repository.Resize(newSize);
        }

        [TestMethod]
        public void then_can_retrieve_the_entries()
        {
            var actualEntries = this.repository.RetrieveEntries().ToList();

            Assert.AreEqual(3, actualEntries.Count);
            LogEntryAssert.AreEqual(this.logEntry0, actualEntries[0]);
            LogEntryAssert.AreEqual(this.logEntry1, actualEntries[1]);
            LogEntryAssert.AreEqual(this.logEntry2, actualEntries[2]);
        }

        [TestMethod]
        public void then_max_size_is_read()
        {
            Assert.AreEqual(newSize, this.repository.MaxSizeInKilobytes);
        }

        [TestMethod]
        public void then_effective_max_size_is_read()
        {
            Assert.AreEqual(newSize, this.repository.ActualMaxSizeInKilobytes);
        }
    }
}

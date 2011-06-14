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
using Microsoft.Practices.EnterpriseLibrary.Logging.TraceListeners;
using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_storage_with_wrapped_entries
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_adding_entry_equal_to_effective_max_size : Context
    {
        private byte[] entry;

        protected override void Act()
        {
            this.entry =
                Enumerable.Range(0, this.storage.EffectiveMaxSizeInBytes - BoundedStreamStorage.EntryHeaderSize)
                    .Select(i => (byte)i)
                    .ToArray();

            this.storage.Add(entry);
        }

        [TestMethod]
        public void then_entry_is_added_to_the_repository()
        {
            var entries = this.storage.RetrieveEntries();

            Assert.AreEqual(1, entries.Count());
            CollectionAssert.AreEqual(this.entry, entries.ElementAt(0));
        }
    }
}

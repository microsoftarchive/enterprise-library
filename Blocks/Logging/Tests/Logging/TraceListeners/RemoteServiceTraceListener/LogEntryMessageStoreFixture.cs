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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Logging.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Logging.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.RemoteServiceTraceListener
{
    public class LogEntryMessageStoreFixture
    {
        public const string TestStoreName = "TestStoreName";
        public const int MemoryBufferLimit = 5;
        public static DateTime TestTimeStamp = new DateTime(2000, 2, 3, 5, 6, 7, 8, DateTimeKind.Utc);
        public static LogEntryMessage TestEntry1 = new LogEntryMessage { Message = "TestEntry1", Categories = new[] { "cat1" }, Severity = TraceEventType.Error, TimeStamp = TestTimeStamp.AddDays(10).ToString("o", CultureInfo.InvariantCulture) };
        public static LogEntryMessage TestEntry2 = new LogEntryMessage { Message = "TestEntry2", Categories = new[] { "cat2" }, Severity = TraceEventType.Critical, TimeStamp = TestTimeStamp.AddDays(20).ToString("o", CultureInfo.InvariantCulture) };

        [TestClass]
        public class when_using_in_memory
        {
            private LogEntryMessageStore store;

            [TestInitialize]
            public void Setup()
            {
                this.store = new LogEntryMessageStore(TestStoreName, MemoryBufferLimit, 0);
            }

            [TestMethod]
            public void then_adding_items_allows_getting_them()
            {
                store.Add(TestEntry1);

                var actual = store.GetEntries();
                Assert.AreEqual(1, actual.Length);
                Assert.AreSame(TestEntry1, actual[0]);
            }

            [TestMethod]
            public void then_adding_past_buffer_limit_discards_older_messages()
            {
                var entries = new List<LogEntryMessage>();
                for (int i = 0; i < MemoryBufferLimit + 1; i++)
                {
                    var entry = new LogEntryMessage { Message = i.ToString(), Severity = TraceEventType.Error, TimeStamp = TestTimeStamp.AddHours(i).ToString("o", CultureInfo.InvariantCulture) };
                    entries.Add(entry);
                    store.Add(entry);
                }

                var actual = store.GetEntries();
                Assert.AreEqual(MemoryBufferLimit, actual.Length);

                CollectionAssert.AreEquivalent(entries.Skip(1).ToList(), actual);
            }

            [TestMethod]
            public void then_can_remove_items()
            {
                store.Add(TestEntry1);
                store.Add(TestEntry2);
                store.RemoveUntil(TestEntry2);

                Assert.AreEqual(0, store.GetEntries().Length);
            }

            [TestMethod]
            public void then_remove_inexistent_item_does_not_remove_any()
            {
                store.Add(TestEntry1);
                store.RemoveUntil(TestEntry2);

                Assert.AreEqual(1, store.GetEntries().Length);
            }

            [TestMethod]
            public void then_can_remove_some_items()
            {
                store.Add(TestEntry1);
                store.Add(TestEntry2);

                store.RemoveUntil(TestEntry1);

                var actual = store.GetEntries();
                Assert.AreEqual(1, actual.Length);
                Assert.AreEqual(TestEntry2.Message, actual[0].Message);
            }
        }

        [TestClass]
        public class when_using_iso_storage
        {
            public const int MaxSizeInKilobytes = 10;

            [TestInitialize]
            [TestCleanup]
            public void CleanUp()
            {
                LogEntryMessageStore.DeleteStore(TestStoreName);
            }

            private LogEntryMessageStore CreateInstance(int maxSizeInKilobytes = MaxSizeInKilobytes)
            {
                return new LogEntryMessageStore(TestStoreName, MemoryBufferLimit, maxSizeInKilobytes);
            }

            [TestMethod]
            public void then_adding_items_allows_getting_them_in_other_instances()
            {
                using (var store = CreateInstance())
                {
                    store.Add(TestEntry1);
                }

                using (var store = CreateInstance())
                {
                    var actual = store.GetEntries();
                    Assert.AreEqual(1, actual.Length);
                    Assert.AreEqual(TestEntry1.Message, actual[0].Message);
                }
            }

            [TestMethod]
            public void then_adding_past_buffer_limit_discards_older_messages()
            {
                using (var store = CreateInstance())
                {
                    var entries = new List<LogEntryMessage>();
                    for (int i = 0; i < MemoryBufferLimit + 1; i++)
                    {
                        var entry = new LogEntryMessage { Message = i.ToString(), Severity = TraceEventType.Error, TimeStamp = TestTimeStamp.AddHours(i).ToString("o", CultureInfo.InvariantCulture) };
                        entries.Add(entry);
                        store.Add(entry);
                    }

                    var actual = store.GetEntries();
                    Assert.AreEqual(MemoryBufferLimit, actual.Length);

                    CollectionAssert.AreEquivalent(entries.Skip(1).ToList(), actual);
                }
            }

            [TestMethod]
            public void then_can_remove_items()
            {
                using (var store = CreateInstance())
                {
                    store.Add(TestEntry1);
                }

                using (var store = CreateInstance())
                {
                    store.Add(TestEntry2);
                }

                using (var store = CreateInstance())
                {
                    Assert.AreEqual(2, store.GetEntries().Length);

                    store.RemoveUntil(store.GetEntries()[1]);

                    Assert.AreEqual(0, store.GetEntries().Length);
                }

                using (var store = CreateInstance())
                {
                    Assert.AreEqual(0, store.GetEntries().Length);
                }
            }

            [TestMethod]
            public void then_can_remove_some_items()
            {
                using (var store = CreateInstance())
                {
                    store.Add(TestEntry1);
                    store.Add(TestEntry2);
                }

                using (var store = CreateInstance())
                {
                    store.RemoveUntil(store.GetEntries()[0]);

                    Assert.AreEqual(1, store.GetEntries().Length);
                }

                using (var store = CreateInstance())
                {
                    var actual = store.GetEntries();
                    Assert.AreEqual(1, actual.Length);
                    Assert.AreEqual(TestEntry2.Message, actual[0].Message);
                }
            }

            [TestMethod]
            public void then_items_are_retrieved_in_order()
            {
                using (var store = CreateInstance())
                {
                    store.Add(TestEntry1);
                    store.Add(TestEntry2);
                }

                using (var store = CreateInstance())
                {
                    var actual = store.GetEntries();
                    Assert.AreEqual(2, actual.Length);
                    Assert.AreEqual(TestEntry1.Message, actual[0].Message);
                    Assert.AreEqual(TestEntry2.Message, actual[1].Message);
                }
            }

            [TestMethod]
            public void then_adding__large_items_keeps_them_in_memory_even_when_overflowing()
            {
                var entry1 = new LogEntryMessage { Message = new string('1', MaxSizeInKilobytes * 1024 / 2), Severity = TraceEventType.Error, TimeStamp = TestTimeStamp.AddHours(1).ToString("o", CultureInfo.InvariantCulture) };
                var entry2 = new LogEntryMessage { Message = new string('2', MaxSizeInKilobytes * 1024 / 2), Severity = TraceEventType.Error, TimeStamp = TestTimeStamp.AddHours(2).ToString("o", CultureInfo.InvariantCulture) };
                using (var store = CreateInstance())
                {
                    store.Add(entry1);
                    store.Add(entry2);

                    var actual = store.GetEntries();
                    Assert.AreEqual(entry1.Message, actual[0].Message);
                    Assert.AreEqual(entry2.Message, actual[1].Message);
                }
            }

            [TestMethod]
            public void then_adding_large_items_drops_previous_entries_from_persisted_storage()
            {
                var entry1 = new LogEntryMessage { Message = new string('1', MaxSizeInKilobytes * 1024 / 2), Severity = TraceEventType.Error, TimeStamp = TestTimeStamp.AddHours(1).ToString("o", CultureInfo.InvariantCulture) };
                var entry2 = new LogEntryMessage { Message = new string('2', MaxSizeInKilobytes * 1024 / 2), Severity = TraceEventType.Error, TimeStamp = TestTimeStamp.AddHours(2).ToString("o", CultureInfo.InvariantCulture) };
                using (var store = CreateInstance())
                {
                    store.Add(entry1);
                    store.Add(entry2);
                }

                using (var store = CreateInstance())
                {
                    var actual = store.GetEntries();
                    Assert.AreEqual(1, actual.Length);
                    Assert.AreEqual(entry2.Message, actual[0].Message);
                }
            }

            [TestMethod]
            public void then_can_store_entries_with_extended_properties()
            {
                var entry1 = new LogEntryMessage
                    {
                        Message = "test1",
                        Severity = TraceEventType.Error,
                        TimeStamp = TestTimeStamp.AddHours(1).ToString("o", CultureInfo.InvariantCulture),
                        ExtendedPropertiesKeys = new[] { "key1", "key2" },
                        ExtendedPropertiesValues = new[] { "value1", "value2" }
                    };

                using (var store = CreateInstance())
                {
                    store.Add(entry1);
                }

                using (var store = CreateInstance())
                {
                    var actual = store.GetEntries();
                    Assert.AreEqual(1, actual.Length);
                    Assert.AreEqual(2, actual[0].ExtendedPropertiesKeys.Length);
                    Assert.AreEqual(2, actual[0].ExtendedPropertiesValues.Length);
                    Assert.AreEqual("key1", actual[0].ExtendedPropertiesKeys[0]);
                    Assert.AreEqual("key2", actual[0].ExtendedPropertiesKeys[1]);
                    Assert.AreEqual("value1", actual[0].ExtendedPropertiesValues[0]);
                    Assert.AreEqual("value2", actual[0].ExtendedPropertiesValues[1]);
                }
            }

            [TestMethod]
            public void then_resizing_to_larger_keeps_the_entries()
            {
                using (var store = CreateInstance())
                {
                    store.Add(TestEntry1);
                    store.Add(TestEntry2);
                    Assert.AreEqual(MaxSizeInKilobytes, store.Quota);
                    store.ResizeBackingStore(20);
                    Assert.AreEqual(20, store.Quota);

                    var actual = store.GetEntries();
                    Assert.AreEqual(2, actual.Length);
                    Assert.AreEqual(TestEntry1.Message, actual[0].Message);
                    Assert.AreEqual(TestEntry2.Message, actual[1].Message);
                }

                using (var store = CreateInstance())
                {
                    Assert.AreEqual(20, store.Quota);
                    var actual = store.GetEntries();
                    Assert.AreEqual(2, actual.Length);
                    Assert.AreEqual(TestEntry1.Message, actual[0].Message);
                    Assert.AreEqual(TestEntry2.Message, actual[1].Message);
                }
            }

            [TestMethod]
            public void then_resizing_to_larger_keeps_the_entries_that_would_overflow_on_open()
            {
                var largeEntry = new LogEntryMessage { Message = new string('1', MaxSizeInKilobytes * 1024), Severity = TraceEventType.Error, TimeStamp = TestTimeStamp.AddDays(30).ToString("o", CultureInfo.InvariantCulture) };
                using (var store = CreateInstance())
                {
                    store.Add(TestEntry1);
                    store.Add(TestEntry2);
                    store.ResizeBackingStore(MaxSizeInKilobytes * 2);
                    store.Add(largeEntry);

                    var actual = store.GetEntries();
                    Assert.AreEqual(3, actual.Length);
                    Assert.AreEqual(TestEntry1.Message, actual[0].Message);
                    Assert.AreEqual(TestEntry2.Message, actual[1].Message);
                    Assert.AreEqual(largeEntry.Message, actual[2].Message);
                }

                using (var store = CreateInstance())
                {
                    var actual = store.GetEntries();
                    Assert.AreEqual(3, actual.Length);
                    Assert.AreEqual(TestEntry1.Message, actual[0].Message);
                    Assert.AreEqual(TestEntry2.Message, actual[1].Message);
                    Assert.AreEqual(largeEntry.Message, actual[2].Message);
                }
            }

            [TestMethod]
            public void then_resizing_from_zero_keeps_entries_on_next_run()
            {
                using (var store = CreateInstance(0))
                {
                    store.ResizeBackingStore(MaxSizeInKilobytes);
                    Assert.AreEqual(MaxSizeInKilobytes, store.Quota);
                    store.Add(TestEntry1);
                    store.Add(TestEntry2);

                    var actual = store.GetEntries();
                    Assert.AreEqual(2, actual.Length);
                    Assert.AreEqual(TestEntry1.Message, actual[0].Message);
                    Assert.AreEqual(TestEntry2.Message, actual[1].Message);
                }

                using (var store = CreateInstance(0))
                {
                    Assert.AreEqual(0, store.Quota);
                    var actual = store.GetEntries();
                    Assert.AreEqual(2, actual.Length);
                    Assert.AreEqual(TestEntry1.Message, actual[0].Message);
                    Assert.AreEqual(TestEntry2.Message, actual[1].Message);
                }
            }

            [TestMethod]
            public void then_resizing_from_zero_keeps_entries_on_next_run_and_they_can_be_removed()
            {
                using (var store = CreateInstance(0))
                {
                    store.ResizeBackingStore(MaxSizeInKilobytes);
                    store.Add(TestEntry1);
                    store.Add(TestEntry2);

                    var actual = store.GetEntries();
                    Assert.AreEqual(2, actual.Length);
                    Assert.AreEqual(TestEntry1.Message, actual[0].Message);
                    Assert.AreEqual(TestEntry2.Message, actual[1].Message);
                }

                using (var store = CreateInstance(0))
                {
                    Assert.AreEqual(0, store.Quota);
                    var actual = store.GetEntries();
                    Assert.AreEqual(2, actual.Length);
                    store.RemoveUntil(actual[1]);
                }

                using (var store = CreateInstance(0))
                {
                    Assert.AreEqual(0, store.Quota);
                    var actual = store.GetEntries();
                    Assert.AreEqual(0, actual.Length);
                }
            }

            [TestMethod]
            public void then_resizing_from_zero_does_not_allow_adding_persistently_on_next_run()
            {
                using (var store = CreateInstance(0))
                {
                    store.ResizeBackingStore(MaxSizeInKilobytes);
                }

                using (var store = CreateInstance(0))
                {
                    Assert.AreEqual(0, store.Quota);
                    var actual = store.GetEntries();
                    Assert.AreEqual(0, actual.Length);
                    store.Add(TestEntry1);
                }

                using (var store = CreateInstance(0))
                {
                    Assert.AreEqual(0, store.Quota);
                    var actual = store.GetEntries();
                    Assert.AreEqual(0, actual.Length);
                }
            }
        }
    }
}

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
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport.Asserts;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.TestSupport
{
    public static class LogEntryAssert
    {
        public static void AreEqual(LogEntry expected, LogEntry actual)
        {
            Assert.AreEqual(expected.Message, actual.Message);
            Assert.AreEqual(expected.Title, actual.Title);
            DateTimeAssert.AreEqual(expected.TimeStamp, actual.TimeStamp, 10000);
            Assert.AreEqual(expected.Severity, actual.Severity);
            Assert.AreEqual(expected.EventId, actual.EventId);
            Assert.AreEqual(expected.ErrorMessages, actual.ErrorMessages);
            Assert.AreEqual(expected.ManagedThreadName, actual.ManagedThreadName);
            Assert.AreEqual(expected.Priority, actual.Priority);
            CollectionAssert.AreEqual(expected.Categories.ToArray(), actual.Categories.ToArray());
            CollectionAssert.AreEqual(expected.ExtendedProperties.Keys.ToArray(), expected.ExtendedProperties.Keys.ToArray());
            foreach (var key in expected.ExtendedProperties.Keys)
            {
                Assert.AreEqual(expected.ExtendedProperties[key], actual.ExtendedProperties[key]);
            }
        }
    }
}

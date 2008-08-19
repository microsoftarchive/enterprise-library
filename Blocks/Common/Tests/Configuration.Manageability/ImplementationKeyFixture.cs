//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Tests
{
    [TestClass]
    public class ImplementationKeyFixture
    {
        [TestMethod]
        public void EqualsWorkOnBlankInstances()
        {
            ImplementationKeyComparer comparer = new ImplementationKeyComparer();
            Assert.IsTrue(comparer.Equals(new ImplementationKey(), new ImplementationKey()));
        }

        [TestMethod]
        public void EqualsWorkOnBlankLhsInstance()
        {
            ImplementationKeyComparer comparer = new ImplementationKeyComparer();
            Assert.IsFalse(comparer.Equals(new ImplementationKey(), new ImplementationKey("file", "app", true)));
        }

        [TestMethod]
        public void EqualsWorkOnBlankRhsInstance()
        {
            ImplementationKeyComparer comparer = new ImplementationKeyComparer();
            Assert.IsFalse(comparer.Equals(new ImplementationKey("file", "app", true), new ImplementationKey()));
        }

        [TestMethod]
        public void EqualsWorkOnDifferentNonBlankInstances()
        {
            ImplementationKeyComparer comparer = new ImplementationKeyComparer();
            Assert.IsFalse(comparer.Equals(new ImplementationKey("file", "app", true), new ImplementationKey("file", null, false)));
            Assert.IsFalse(comparer.Equals(new ImplementationKey("file", "app", true), new ImplementationKey("file2", "app", true)));
            Assert.IsFalse(comparer.Equals(new ImplementationKey("file", "app", true), new ImplementationKey("file", "app2", true)));
            Assert.IsFalse(comparer.Equals(new ImplementationKey("file", "app", true), new ImplementationKey("file", "app", false)));
            Assert.IsTrue(comparer.Equals(new ImplementationKey(null, "app", true), new ImplementationKey(null, "app", true)));
            Assert.IsTrue(comparer.Equals(new ImplementationKey("file", "app", true), new ImplementationKey("file", "app", true)));
            Assert.IsTrue(comparer.Equals(new ImplementationKey("file", "app", true), new ImplementationKey("FIle", "app", true)));
            Assert.IsTrue(comparer.Equals(new ImplementationKey("file", "app", true), new ImplementationKey("FIle", "APP", true)));

            StringBuilder sb = new StringBuilder();
            sb.Append("string");
            String string1 = sb.ToString();
            String string2 = sb.ToString();
            Assert.IsTrue(comparer.Equals(new ImplementationKey(string1, "app", true), new ImplementationKey(string2, "app", true)));
            Assert.IsTrue(comparer.Equals(new ImplementationKey("file", string1, true), new ImplementationKey("file", string2, true)));
        }

        [TestMethod]
        public void GetHashCodeWorksOnBlankInstance()
        {
            ImplementationKeyComparer comparer = new ImplementationKeyComparer();
            comparer.GetHashCode(new ImplementationKey());
        }

        [TestMethod]
        public void GetHashCodeWorksOnNonBlankInstance()
        {
            ImplementationKeyComparer comparer = new ImplementationKeyComparer();
            comparer.GetHashCode(new ImplementationKey("file", null, true));
            comparer.GetHashCode(new ImplementationKey(null, "app", true));
            comparer.GetHashCode(new ImplementationKey("file", "app", false));
        }

        [TestMethod]
        public void GetHashCodeIsCaseInsensitive()
        {
            ImplementationKeyComparer comparer = new ImplementationKeyComparer();
            Assert.AreEqual(comparer.GetHashCode(new ImplementationKey("file", "APP", true)), comparer.GetHashCode(new ImplementationKey("FILE", "app", true)));
        }
    }
}
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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests
{
    [TestClass]
    public class ResourceStringLoaderFixture
    {
        [TestMethod]
        public void CanLoadStringFromCallingAssembly()
        {
            string value = ResourceStringLoader.LoadString(typeof(Resources).FullName, "Test");

            Assert.AreEqual("Test", value);
        }

        [TestMethod]
        public void CanLoadStringFromExecutingAssembly()
        {
            string value = ResourceStringLoader.LoadString("Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties.Resources", "ValidationCaption");

            Assert.AreEqual("Validation", value);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExcutingLoadStringWithNullBaseNameThrows()
        {
            ResourceStringLoader.LoadString(null, "ValidationCaption");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ExcutingLoadStringWithNullResourceNameThrows()
        {
            ResourceStringLoader.LoadString("Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties.Resources", null);
        }
    }
}
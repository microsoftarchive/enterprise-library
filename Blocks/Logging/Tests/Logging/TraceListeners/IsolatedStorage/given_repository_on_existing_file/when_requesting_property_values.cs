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

using Microsoft.Silverlight.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Tests.TraceListeners.IsolatedStorage.given_repository_on_existing_file
{
    [TestClass]
    [Tag("IsolatedStorage")]
    public class when_requesting_property_values : Context
    {
        protected override void Act()
        {
            base.Act();
        }

        [TestMethod]
        public void then_max_size_is_read()
        {
            Assert.AreEqual(1, this.repository.MaxSizeInKilobytes);
        }

        [TestMethod]
        public void then_effective_max_size_is_read()
        {
            Assert.AreEqual(1, this.repository.ActualMaxSizeInKilobytes);
        }
    }
}

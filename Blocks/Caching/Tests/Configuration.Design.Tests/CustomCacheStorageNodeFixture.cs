//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration.Design.Tests
{
    [TestClass]
    public class CustomCacheStorageNodeFixture
    {
        [TestMethod]
        public void EnsureCustomCacheStorageNodePropertyCatoriesAndDescriptions()
        {
            Assert.IsTrue(SRAttributesHelper.AssertSRDescription(typeof(CustomCacheStorageNode), "Attributes", Resources.CustomCacheStorageExtensionsDescription));
            Assert.IsTrue(SRAttributesHelper.AssertSRCategory(typeof(CustomCacheStorageNode), "Attributes"));

            Assert.IsTrue(SRAttributesHelper.AssertSRDescription(typeof(CustomCacheStorageNode), "Type", Resources.CustomCacheStorageNodeTypeDescription));
            Assert.IsTrue(SRAttributesHelper.AssertSRCategory(typeof(CustomCacheStorageNode), "Type"));
        }
    }
}
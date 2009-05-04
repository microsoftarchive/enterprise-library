//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests
{
    [TestClass]
    public class SelfNodeFixture
    {
        [TestMethod]
        public void SelfNodeNameReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(SelfNode), "Name"));
        }

        [TestMethod]
        public void SelfNodeDoesntSortChildNodes()
        {
            SelfNode selfNode = new SelfNode();

            Assert.AreEqual(false, selfNode.SortChildren);
        }

        [TestMethod]
        public void SelfNodeHasProperName()
        {
            SelfNode selfNode = new SelfNode();

            Assert.AreEqual("Self", selfNode.Name);
        }
    }
}

//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Tests
{
    [TestClass]
    public class AuthorizationProviderCollectionNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void AuthorizationProviderCollectionNamePropertyIsReadOnly()
        {
            Assert.AreEqual(true, CommonUtil.IsPropertyReadOnly(typeof(AuthorizationProviderCollectionNode), "Name"));
        }

        [TestMethod]
        public void AuthorizationProviderCollectionDefaults()
        {
            AuthorizationProviderCollectionNode authProviderCollection = new AuthorizationProviderCollectionNode();
            Assert.AreEqual("Authorization", authProviderCollection.Name);
        }
    }
}

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

#if  UNIT_TESTS
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Tests
{
    [TestFixture]
    public class CustomProfileProviderNodeFixture : ConfigurationDesignHostTestBase
    {
        [Test]
        public void NodeTest()
        {
            string typeName = "testType";
            CustomProfileProviderNode node = new CustomProfileProviderNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            node.TypeName = typeName;
            Assert.AreEqual(typeName, node.TypeName);

            node.Extensions.Add(new NameValueItem("TEST", "VALUE"));
            Assert.AreEqual("VALUE", node.Extensions["TEST"]);
        }

        [Test]
        public void DataTest()
        {
            CustomProfileProviderData data = new CustomProfileProviderData();
            data.Extensions.Add(new NameValueItem("TEST", "VALUE"));
            data.TypeName = "testType";

            CustomProfileProviderNode node = new CustomProfileProviderNode(data);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            CustomProfileProviderData nodeData = (CustomProfileProviderData)node.ProfileProviderData;

            Assert.AreEqual("VALUE", nodeData.Extensions["TEST"]);
            Assert.AreEqual(data.TypeName, nodeData.TypeName);
        }
    }
}

#endif
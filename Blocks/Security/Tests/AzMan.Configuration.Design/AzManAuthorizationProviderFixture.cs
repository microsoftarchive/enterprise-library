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

using System;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.AzMan.Configuration.Design.Tests
{
    [TestClass]
    public class AzManAuthorizationProviderFixture : ConfigurationDesignHost
    {
        protected override void InitializeCore()
        {
            new SecurityAzManConfigurationDesignManager().Register(ServiceProvider);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PassingNullDataInAzManAuthorizationProviderNodeThrows()
        {
            new AzManAuthorizationProviderNode(null);
        }

        [TestMethod]
        public void AzManAuthorizationProviderNodeDefaults()
        {
            AzManAuthorizationProviderNode azManProviderNode = new AzManAuthorizationProviderNode();
            Assert.AreEqual("AzMan Provider", azManProviderNode.Name);
        }

        [TestMethod]
        public void CanCreateAzManAuthorizationProviderNodeFromData()
        {
            ConfigurationNode createdNode = ServiceHelper.GetNodeCreationService(ServiceProvider).CreateNodeByDataType(typeof(AzManAuthorizationProviderData), new object[] { new AzManAuthorizationProviderData() });
            Assert.IsNotNull(createdNode);
            Assert.AreEqual(typeof(AzManAuthorizationProviderNode), createdNode.GetType());
        }

        [TestMethod]
        public void AzManAuthorizationProviderNodeTest()
        {
            string name = "some name";
            string auditIdentifierPrefix = "pFix";
            string storeLocation = "some store location";
            string scope = "some scope";

            AzManAuthorizationProviderData data = new AzManAuthorizationProviderData();
            data.Name = name;
            data.AuditIdentifierPrefix = auditIdentifierPrefix;
            data.StoreLocation = storeLocation;
            data.Scope = scope;

            AzManAuthorizationProviderNode node = new AzManAuthorizationProviderNode(data);
            Assert.AreEqual(name, node.Name);
            Assert.AreEqual(auditIdentifierPrefix, node.AuditIdentifierPrefix);
            Assert.AreEqual(storeLocation, node.StoreLocation);
            Assert.AreEqual(scope, node.Scope);
        }

        [TestMethod]
        public void AzManAuthorizationProviderNodeDataTest()
        {
            string name = "some name";
            string auditIdentifierPrefix = "pFix";
            string storeLocation = "some store location";
            string scope = "some scope";

            AzManAuthorizationProviderData azManProviderData = new AzManAuthorizationProviderData();
            azManProviderData.Name = name;
            azManProviderData.AuditIdentifierPrefix = auditIdentifierPrefix;
            azManProviderData.StoreLocation = storeLocation;
            azManProviderData.Scope = scope;

            AzManAuthorizationProviderNode azManProviderNode = new AzManAuthorizationProviderNode(azManProviderData);

            AzManAuthorizationProviderData nodeData = (AzManAuthorizationProviderData)azManProviderNode.AuthorizationProviderData;
            Assert.AreEqual(name, nodeData.Name);
            Assert.AreEqual(auditIdentifierPrefix, nodeData.AuditIdentifierPrefix);
            Assert.AreEqual(storeLocation, nodeData.StoreLocation);
            Assert.AreEqual(scope, nodeData.Scope);
        }
    }
}

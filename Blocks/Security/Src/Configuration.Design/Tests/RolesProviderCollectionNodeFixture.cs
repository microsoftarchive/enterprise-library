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
using System;
using System.Diagnostics;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using NUnit.Framework;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design.Tests
{
    [TestFixture]
    public class RolesProviderCollectionNodeFixture : ConfigurationDesignHostTestBase
    {
        public override void SetUp()
        {
            base.SetUp();
            Type nodeType = typeof(CustomAuthenticationProviderNode);
            NodeCreationEntry entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(CustomAuthenticationProviderData), SR.CustomAuthenticationProviderCommandName);
            NodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomRolesProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(CustomRolesProviderData), SR.CustomRolesProviderCommandName);
            NodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomAuthorizationProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(CustomAuthorizationProviderData), SR.CustomAuthorizationProviderCommandName);
            NodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomProfileProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(CustomProfileProviderData), SR.CustomProfileProviderCommandName);
            NodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(CustomSecurityCacheProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(CustomSecurityCacheProviderData), SR.CustomSecurityCacheNodeCommandName);
            NodeCreationService.AddNodeCreationEntry(entry);

            nodeType = typeof(AuthorizationRuleProviderNode);
            entry = NodeCreationEntry.CreateNodeCreationEntryNoMultiples(new AddChildNodeCommand(Host, nodeType), nodeType, typeof(AuthorizationRuleProviderData), SR.AuthorizationRuleProviderCommandName);
            NodeCreationService.AddNodeCreationEntry(entry);
        }

        [Test]
        public void HydrateTest()
        {
            RolesProviderDataCollection dataCollection = new RolesProviderDataCollection();
            CustomRolesProviderData providerData = new CustomRolesProviderData();
            providerData.Name = "provider1";
            providerData.TypeName = typeof(MockRolesProvider).AssemblyQualifiedName;
            dataCollection.Add(providerData);

            RolesProviderCollectionNode node = new RolesProviderCollectionNode(dataCollection);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(1, node.Nodes.Count);
        }

        [Test]
        public void RequiredDefaultProviderTest()
        {
            RolesProviderCollectionNode node = new RolesProviderCollectionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void GetRolesDataTest()
        {
            RolesProviderCollectionNode node = new RolesProviderCollectionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            CustomRolesProviderNode providerNode = new CustomRolesProviderNode();
            node.Nodes.Add(providerNode);
            providerNode.Name = "provider1";
            providerNode.TypeName = typeof(MockRolesProvider).AssemblyQualifiedName;
            RolesProviderDataCollection providers = node.RolesProviderDataCollection;
            Assert.IsNotNull(providers);
            Assert.AreEqual(1, providers.Count);
            RolesProviderData data = providers["provider1"];
            Assert.IsNotNull(data);
            CustomRolesProviderData customData = data as CustomRolesProviderData;
            Assert.IsNotNull(customData);
            Assert.AreEqual(typeof(MockRolesProvider).AssemblyQualifiedName, customData.TypeName);
        }

        private class MockRolesProvider : ConfigurationProvider, IRolesProvider
        {
            public MockRolesProvider()
            {
            }

            public IPrincipal GetRoles(IIdentity identity)
            {
                IPrincipal principal = null;

                return principal;
            }

            public override void Initialize(ConfigurationView configurationView)
            {
                throw new NotImplementedException();
            }
        }
    }
}

#endif

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
    public class AuthorizationProviderCollectionNodeFixture : ConfigurationDesignHostTestBase
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
            AuthorizationProviderDataCollection dataCollection = new AuthorizationProviderDataCollection();
            CustomAuthorizationProviderData providerData = new CustomAuthorizationProviderData();
            providerData.Name = "provider1";
            providerData.TypeName = typeof(MockAuthorizationProvider).AssemblyQualifiedName;
            dataCollection.Add(providerData);

            AuthorizationProviderCollectionNode node = new AuthorizationProviderCollectionNode(dataCollection);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(1, node.Nodes.Count);
        }

        [Test]
        public void NotRequiredDefaultProviderTest()
        {
            AuthorizationProviderCollectionNode node = new AuthorizationProviderCollectionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void GetAuthorizationDataTest()
        {
            AuthorizationProviderCollectionNode node = new AuthorizationProviderCollectionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            CustomAuthorizationProviderNode providerNode = new CustomAuthorizationProviderNode();
            node.Nodes.Add(providerNode);
            providerNode.Name = "provider1";
            providerNode.TypeName = typeof(MockAuthorizationProvider).AssemblyQualifiedName;
            AuthorizationProviderDataCollection providers = node.AuthorizationProviderDataCollection;
            Assert.IsNotNull(providers);
            Assert.AreEqual(1, providers.Count);
            AuthorizationProviderData data = providers["provider1"];
            Assert.IsNotNull(data);
            CustomAuthorizationProviderData customData = data as CustomAuthorizationProviderData;
            Assert.IsNotNull(customData);
            Assert.AreEqual(typeof(MockAuthorizationProvider).AssemblyQualifiedName, customData.TypeName);
        }

        private class MockAuthorizationProvider : ConfigurationProvider, IAuthorizationProvider
        {
            public MockAuthorizationProvider()
            {
            }

            public bool Authorize(IPrincipal principal, string context)
            {
                return false;
            }

            public override void Initialize(ConfigurationView configurationView)
            {
                
            }
        }
    }
}

#endif
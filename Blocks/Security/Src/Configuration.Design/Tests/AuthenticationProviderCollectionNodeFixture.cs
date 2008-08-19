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
    public class AuthenticationProviderCollectionNodeFixture : ConfigurationDesignHostTestBase
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
            AuthenticationProviderDataCollection dataCollection = new AuthenticationProviderDataCollection();
            CustomAuthenticationProviderData providerData = new CustomAuthenticationProviderData();
            providerData.Name = "provider1";
            providerData.TypeName = typeof(MockAuthenticationProvider).AssemblyQualifiedName;
            dataCollection.Add(providerData);
            AuthenticationProviderCollectionNode node = new AuthenticationProviderCollectionNode(dataCollection);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(1, node.AuthenticationProviderDataCollection.Count);
        }

        [Test]
        public void RequiredDefaultProviderTest()
        {
            AuthenticationProviderCollectionNode node = new AuthenticationProviderCollectionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void GetAuthenticationDataTest()
        {
            AuthenticationProviderCollectionNode node = new AuthenticationProviderCollectionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            CustomAuthenticationProviderNode providerNode = new CustomAuthenticationProviderNode();
            node.Nodes.Add(providerNode);
            providerNode.Name = "provider1";
            providerNode.TypeName = typeof(MockAuthenticationProvider).AssemblyQualifiedName;
            AuthenticationProviderDataCollection providers = node.AuthenticationProviderDataCollection;
            Assert.IsNotNull(providers);
            Assert.AreEqual(1, providers.Count);
            AuthenticationProviderData data = providers["provider1"];
            Assert.IsNotNull(data);
            CustomAuthenticationProviderData customData = data as CustomAuthenticationProviderData;
            Assert.IsNotNull(customData);
            Assert.AreEqual(typeof(MockAuthenticationProvider).AssemblyQualifiedName, customData.TypeName);
        }

        private class MockAuthenticationProvider : ConfigurationProvider, IAuthenticationProvider
        {
            public MockAuthenticationProvider(ProviderData providerData, ConfigurationContext context)
            {
            }

            public void Initialize(ProviderData providerData, ConfigurationContext configurationContext)
            {
            }

            public bool Authenticate(object credentials, out IIdentity identity)
            {
                identity = null;
                return false;
            }

            public override void Initialize(ConfigurationView configurationView)
            {
            }
        }

    }
}

#endif
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
    public class SecurityCacheProviderCollectionNodeFixture : ConfigurationDesignHostTestBase
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
            SecurityCacheProviderDataCollection dataCollection = new SecurityCacheProviderDataCollection();
            CustomSecurityCacheProviderData providerData = new CustomSecurityCacheProviderData();
            providerData.Name = "provider1";
            providerData.TypeName = typeof(MockSecurityCacheProvider).AssemblyQualifiedName;
            dataCollection.Add(providerData);

            SecurityCacheProviderCollectionNode node = new SecurityCacheProviderCollectionNode(dataCollection);
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            Assert.AreEqual(1, node.Nodes.Count);
        }

        [Test]
        public void RequiredDefaultProviderTest()
        {
            SecurityCacheProviderCollectionNode node = new SecurityCacheProviderCollectionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            ValidateNodeCommand cmd = new ValidateNodeCommand(Host);
            cmd.Execute(node);
            Assert.AreEqual(0, ValidationErrorsCount);
        }

        [Test]
        public void GetSecurityCacheProvidersDataTest()
        {
            SecurityCacheProviderCollectionNode node = new SecurityCacheProviderCollectionNode();
            CreateHierarchyAndAddToHierarchyService(node, CreateDefaultConfiguration());
            CustomSecurityCacheProviderNode providerNode = new CustomSecurityCacheProviderNode();
            node.Nodes.Add(providerNode);
            providerNode.Name = "provider1";
            providerNode.TypeName = typeof(MockSecurityCacheProvider).AssemblyQualifiedName;

            SecurityCacheProviderDataCollection providers = node.SecurityCacheProviderDataCollection;

            Assert.IsNotNull(providers);

            Assert.AreEqual(1, providers.Count);
            SecurityCacheProviderData data = providers["provider1"];
            Assert.IsNotNull(data);
            CustomSecurityCacheProviderData customData = data as CustomSecurityCacheProviderData;
            Assert.IsNotNull(customData);
            Assert.AreEqual(typeof(MockSecurityCacheProvider).AssemblyQualifiedName, customData.TypeName);
        }

        private class MockSecurityCacheProvider : ConfigurationProvider, ISecurityCacheProvider
        {
            public MockSecurityCacheProvider()
            {
            }

            public void SaveProfile(object profile, IToken token)
            {
            }

            IToken ISecurityCacheProvider.SaveProfile(object profile)
            {
                return null;
            }

            public void ExpireProfile(IToken token)
            {
            }

            public void SaveIdentity(IIdentity identity, IToken token)
            {
            }

            IToken ISecurityCacheProvider.SaveIdentity(IIdentity identity)
            {
                return null;
            }

            public IIdentity GetIdentity(IToken token)
            {
                return null;
            }

            public object GetProfile(IToken token)
            {
                return null;
            }

            public IPrincipal GetPrincipal(IToken token)
            {
                return null;
            }

            public void ExpirePrincipal(IToken token)
            {
            }

            public void ExpireIdentity(IToken token)
            {
            }

            public void SavePrincipal(IPrincipal principal, IToken token)
            {
            }

            IToken ISecurityCacheProvider.SavePrincipal(IPrincipal principal)
            {
                return null;
            }

            public override void Initialize(ConfigurationView configurationView)
            {
                throw new NotImplementedException();
            }
        }
    }
}

#endif

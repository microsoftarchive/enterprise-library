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
using System.Configuration;
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class AuthorizationFactoryFixture
    {
        [TestMethod]
        public void CanCreateDefaultAuthorizationProviderFromConfiguration()
        {
            IAuthorizationProvider provider = AuthorizationFactory.GetAuthorizationProvider();
            Assert.AreEqual(typeof(MockAuthorizationProvider), provider.GetType());
        }

        [TestMethod]
        public void CanCreateAuthorizationProviderFromConfiguration()
        {
            IAuthorizationProvider provider = AuthorizationFactory.GetAuthorizationProvider("provider2");
            Assert.AreEqual(typeof(MockAuthorizationProvider2), provider.GetType());
        }

        [TestMethod]
        public void CanCreateAuthorizationRuleProviderFromConfiguration()
        {
            IAuthorizationProvider provider = AuthorizationFactory.GetAuthorizationProvider("RuleProvider");
            Assert.AreEqual(typeof(AuthorizationRuleProvider), provider.GetType());
            AuthorizationRuleProvider ruleProvider = provider as AuthorizationRuleProvider;
            Assert.IsTrue(ruleProvider.Authorize(new GenericPrincipal(new GenericIdentity("TestUser"), new string[] { "Admin" }), "rule1"));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void TryToCreateAuthorizationProviderFromConfigurationWithNullName()
        {
            AuthorizationFactory.GetAuthorizationProvider(null);
        }

        [TestMethod]
        [ExpectedException(typeof(ConfigurationErrorsException))]
        public void TryToCreateAuthorizationProviderFromConfigurationThatDoesNotExist()
        {
            AuthorizationFactory.GetAuthorizationProvider("provider3");
        }
    }
}

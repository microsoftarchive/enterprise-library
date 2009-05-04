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
using System.Collections.Generic;
using System.Security.Principal;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Security.Tests.Properties;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class AuthorizationRuleProviderFixture
    {
        AuthorizationRuleProvider provider;
        IPrincipal principal;
        const string testRuleName = "rule1";

        [TestInitialize]
        public void TestInitialize()
        {
            Dictionary<string, IAuthorizationRule> rules = new Dictionary<string, IAuthorizationRule>();
            rules.Add(testRuleName, new AuthorizationRuleData(testRuleName, "I:user1"));

            provider = new AuthorizationRuleProvider(rules);

            principal = new GenericPrincipal(new GenericIdentity("user1"), new string[] { "Admin", "Manager" });
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizeInvalidAuthorityException()
        {
            provider.Authorize(null, testRuleName);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizeInvalidNameException()
        {
            provider.Authorize(principal, "");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void AuthorizeNullContextException()
        {
            provider.Authorize(principal, null);
        }

        [TestMethod]
        public void AuthorizeRuleNotFoundTest()
        {
            const string ruleName = "invalidRuleName";
            try
            {
                provider.Authorize(principal, ruleName);
            }
            catch (InvalidOperationException e)
            {
                Assert.AreEqual(string.Format(Resources.AuthorizationRuleNotFoundMsg, ruleName), e.Message);
            }
            catch
            {
                throw;
            }
        }

        [TestMethod]
        public void AuthorizeTest()
        {
            bool result = provider.Authorize(principal, testRuleName);
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AuthorizeFailureTest()
        {
            principal = new GenericPrincipal(new GenericIdentity("bogus user"), new string[0]);
            bool result = provider.Authorize(principal, testRuleName);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void FactoryTest()
        {
            AuthorizationProviderFactory factory = new AuthorizationProviderFactory(ConfigurationSourceFactory.Create());
            IAuthorizationProvider ruleProvider = factory.Create("RuleProvider");
            Assert.IsTrue(ruleProvider.Authorize(principal, testRuleName));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorCallWithNullArgumentsThrows()
        {
            provider = new AuthorizationRuleProvider(null);
        }
    }
}

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

using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using Microsoft.Practices.EnterpriseLibrary.Security.PolicyInjection;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.Practices.Unity;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests.PolicyInjection
{
    [TestClass]
    public class AuthorizationCallHandlerAttributeFixture
    {
        private IUnityContainer container;
        private IAuthorizationProvider authorizationProvider;

        [TestInitialize]
        public void Setup()
        {
            authorizationProvider = new AuthorizationRuleProvider(new Dictionary<string, IAuthorizationRule>());
            container = new UnityContainer();
        }

        [TestMethod]
        public void ShouldCreateHandlerWithReasonableDefaults()
        {
            container.RegisterInstance(authorizationProvider);

            string ruleName = "Some Rule";
            AuthorizationCallHandlerAttribute attribute = new AuthorizationCallHandlerAttribute(ruleName);
            attribute.Order = 500;

            AuthorizationCallHandler handler = GetHandlerFromAttribute(attribute);

            Assert.AreSame(authorizationProvider, handler.AuthorizationProvider);
            Assert.AreEqual(ruleName, handler.OperationName);
            Assert.AreEqual(500, handler.Order);
        }

        [TestMethod]
        public void ShouldBeAbleToSetProviderName()
        {
            container.RegisterInstance("MyRules", authorizationProvider);

            string ruleName = "Some other rule - {namespace}";
            string providerName = "MyRules";
            AuthorizationCallHandlerAttribute attribute = new AuthorizationCallHandlerAttribute(ruleName);
            attribute.ProviderName = providerName;
            AuthorizationCallHandler handler = GetHandlerFromAttribute(attribute);

            Assert.AreSame(authorizationProvider, handler.AuthorizationProvider);
            Assert.AreEqual(ruleName, handler.OperationName);
        }

        AuthorizationCallHandler GetHandlerFromAttribute(HandlerAttribute attribute)
        {
            return (AuthorizationCallHandler)attribute.CreateHandler(container);
        }
    }
}

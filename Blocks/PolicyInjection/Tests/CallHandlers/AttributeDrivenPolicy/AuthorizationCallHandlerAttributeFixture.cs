//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.AttributeDrivenPolicy
{
    [TestClass]
    public class AuthorizationCallHandlerAttributeFixture
    {
        [TestMethod]
        public void ShouldCreateHandlerWithReasonableDefaults()
        {
            string ruleName = "Some Rule";
            AuthorizationCallHandlerAttribute attribute = new AuthorizationCallHandlerAttribute(ruleName);
            AuthorizationCallHandler handler = GetHandlerFromAttribute(attribute);
            Assert.AreEqual(string.Empty, handler.ProviderName);
            Assert.AreEqual(ruleName, handler.OperationName);
        }

        [TestMethod]
        public void ShouldBeAbleToSetProviderName()
        {
            string ruleName = "Some other rule - {namespace}";
            string providerName = "MyRules";
            AuthorizationCallHandlerAttribute attribute = new AuthorizationCallHandlerAttribute(ruleName);
            attribute.ProviderName = providerName;
            AuthorizationCallHandler handler = GetHandlerFromAttribute(attribute);

            Assert.AreEqual(ruleName, handler.OperationName);
            Assert.AreEqual(providerName, handler.ProviderName);
        }

        AuthorizationCallHandler GetHandlerFromAttribute(HandlerAttribute attribute)
        {
            return (AuthorizationCallHandler)attribute.CreateHandler(null);
        }
    }
}

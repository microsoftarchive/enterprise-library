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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.AttributeDrivenPolicy
{
    [TestClass]
    public class ExceptionCallHandlerAttributeFixture
    {
        [TestMethod]
        public void ShouldCreateHandlerWithCorrectPolicy()
        {
            string policy = "Swallow Exceptions";
            ExceptionCallHandlerAttribute attribute = new ExceptionCallHandlerAttribute(policy);
            ExceptionCallHandler handler = (ExceptionCallHandler)attribute.CreateHandler(null);
            Assert.AreEqual(policy, handler.ExceptionPolicyName);
        }
    }
}

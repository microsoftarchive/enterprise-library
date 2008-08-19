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
using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class AnonymousExpressionFixture
    {
        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        public void NotSupportedExceptionTest()
        {
            GenericIdentity identity = new GenericIdentity(String.Empty);
            GenericPrincipal principal = new GenericPrincipal(identity, null);
            AnonymousExpression expression = new AnonymousExpression();
            Assert.IsTrue(expression.Evaluate(principal));
        }

        [TestMethod]
        public void FalseTest()
        {
            GenericIdentity identity = new GenericIdentity("foo");
            Assert.IsTrue(identity.IsAuthenticated);
            AnonymousExpression expression = new AnonymousExpression();
            Assert.IsFalse(expression.Evaluate(identity));
        }

        [TestMethod]
        public void TrueTest()
        {
            GenericIdentity identity = new GenericIdentity(String.Empty);
            Assert.IsFalse(identity.IsAuthenticated);

            AnonymousExpression expression = new AnonymousExpression();
            Assert.IsTrue(expression.Evaluate(identity));
        }
    }
}
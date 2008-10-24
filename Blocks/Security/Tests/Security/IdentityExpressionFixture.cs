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

using System.Security.Principal;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class IdentityExpressionFixture
    {
        IPrincipal principal;

        [TestInitialize]
        public void TestInitialize()
        {
            GenericIdentity identity = new GenericIdentity("foo");
            principal = new GenericPrincipal(identity, null);
        }

        [TestMethod]
        public void IgnoreCaseTest()
        {
            IdentityExpression expression = new IdentityExpression("Foo");
            Assert.IsTrue(expression.Evaluate(principal));
        }

        [TestMethod]
        public void CaseTest()
        {
            IdentityExpression expression = new IdentityExpression("foo");
            Assert.IsTrue(expression.Evaluate(principal));
        }

        [TestMethod]
        public void FalseEvaluationTest()
        {
            IdentityExpression expression = new IdentityExpression("bar");
            Assert.IsFalse(expression.Evaluate(principal));
        }

        [TestMethod]
        public void AnyTest()
        {
            IdentityExpression expression = new IdentityExpression("*");
            Assert.AreEqual(typeof(AnyExpression), expression.Word.GetType());
        }

        [TestMethod]
        public void AnonymousTest()
        {
            IdentityExpression expression = new IdentityExpression("?");
            Assert.AreEqual(typeof(AnonymousExpression), expression.Word.GetType());
        }

        [TestMethod]
        public void WordTest()
        {
            IdentityExpression expression = new IdentityExpression("user1");
            Assert.AreEqual(typeof(WordExpression), expression.Word.GetType());
        }
    }
}

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
    public class OrOperatorFixture
    {
        IPrincipal principal;

        [TestInitialize]
        public void TestInitialize()
        {
            GenericIdentity identity = new GenericIdentity("foo");
            principal = new GenericPrincipal(identity, null);
        }

        [TestMethod]
        public void TrueTrueTest()
        {
            MockExpression leftExpression = new MockExpression(true);
            MockExpression rightExpression = new MockExpression(true);
            OrOperator expression = new OrOperator(leftExpression, rightExpression);
            Assert.IsTrue(expression.Evaluate(principal));
        }

        [TestMethod]
        public void TrueFalseTest()
        {
            MockExpression leftExpression = new MockExpression(true);
            MockExpression rightExpression = new MockExpression(false);
            OrOperator expression = new OrOperator(leftExpression, rightExpression);
            Assert.IsTrue(expression.Evaluate(principal));
        }

        [TestMethod]
        public void FalseFalseTest()
        {
            MockExpression leftExpression = new MockExpression(false);
            MockExpression rightExpression = new MockExpression(false);
            OrOperator expression = new OrOperator(leftExpression, rightExpression);
            Assert.IsFalse(expression.Evaluate(principal));
        }

        [TestMethod]
        public void FalseTrueTest()
        {
            MockExpression leftExpression = new MockExpression(false);
            MockExpression rightExpression = new MockExpression(true);
            OrOperator expression = new OrOperator(leftExpression, rightExpression);
            Assert.IsTrue(expression.Evaluate(principal));
        }
    }
}

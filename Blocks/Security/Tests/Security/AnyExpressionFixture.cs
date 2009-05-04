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
    public class AnyExpressionFixture
    {
        [TestMethod]
        public void PrincipalNotNull()
        {
            AnyExpression expression = new AnyExpression();
            Assert.IsTrue(expression.Evaluate(new GenericPrincipal(new GenericIdentity("foo"), null)));
        }

        [TestMethod]
        public void PrincipalIsNull()
        {
            AnyExpression expression = new AnyExpression();
            Assert.IsFalse(expression.Evaluate((IPrincipal)null));
        }

        [TestMethod]
        public void IdentityNotNull()
        {
            AnyExpression expression = new AnyExpression();
            Assert.IsTrue(expression.Evaluate(new GenericIdentity("foo")));
        }

        [TestMethod]
        public void IdentityIsNull()
        {
            AnyExpression expression = new AnyExpression();
            Assert.IsFalse(expression.Evaluate((IIdentity)null));
        }
    }
}

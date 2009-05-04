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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Tests
{
    [TestClass]
    public class ParserFixture
    {
        [TestMethod]
        public void AndExpressionTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("(R:Role1 and R:Role2)");
            Assert.AreEqual(typeof(AndOperator), expression.GetType());
            AndOperator andOperator = (AndOperator)expression;

            Assert.AreEqual(typeof(RoleExpression), andOperator.Left.GetType());
            Assert.AreEqual(typeof(RoleExpression), andOperator.Right.GetType());
            Assert.AreEqual("Role1", ((RoleExpression)andOperator.Left).Word.Value);
            Assert.AreEqual("Role2", ((RoleExpression)andOperator.Right).Word.Value);
        }

        [TestMethod]
        public void AndExpressionWithoutParentheses()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("R:Role1 and R:Role2");
            Assert.AreEqual(typeof(AndOperator), expression.GetType());
            AndOperator andOperator = (AndOperator)expression;

            Assert.AreEqual(typeof(RoleExpression), andOperator.Left.GetType());
            Assert.AreEqual(typeof(RoleExpression), andOperator.Right.GetType());
            Assert.AreEqual("Role1", ((RoleExpression)andOperator.Left).Word.Value);
            Assert.AreEqual("Role2", ((RoleExpression)andOperator.Right).Word.Value);
        }

        [TestMethod]
        public void MultipleAndExpressionWithParentheses()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("R:Role1 and R:Role2 and I:User1");
            Assert.AreEqual(typeof(AndOperator), expression.GetType());
            AndOperator andOperator = (AndOperator)expression;
            Assert.IsNotNull(andOperator.Left);
            Assert.IsNotNull(andOperator.Right);

            Assert.AreEqual("User1", ((IdentityExpression)andOperator.Right).Word.Value);

            AndOperator leftAndOperator = (AndOperator)andOperator.Left;
            Assert.AreEqual("Role2", ((RoleExpression)leftAndOperator.Right).Word.Value);
        }

        [TestMethod]
        public void IdentityExpressionTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("I:Bob");
            IdentityExpression identityExpression = (IdentityExpression)expression;

            Assert.AreEqual(typeof(WordExpression), identityExpression.Word.GetType());
            Assert.AreEqual("Bob", identityExpression.Word.Value);
        }

        [TestMethod]
        public void NotExpressionTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("Not I:Bob");
            NotOperator notOperator = (NotOperator)expression;
            IdentityExpression identityExpression = (IdentityExpression)notOperator.Expression;

            Assert.AreEqual("Bob", identityExpression.Word.Value);
        }

        [TestMethod]
        public void AndNotTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("R:Managers AND NOT I:Bob");

            AndOperator andOperator = (AndOperator)expression;

            RoleExpression roleExpression = (RoleExpression)andOperator.Left;
            Assert.AreEqual("Managers", roleExpression.Word.Value);

            NotOperator notOperator = (NotOperator)andOperator.Right;

            IdentityExpression identityExpression = (IdentityExpression)notOperator.Expression;
            Assert.AreEqual("Bob", identityExpression.Word.Value);
        }

        [TestMethod]
        public void OrExpressionTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("(R:Role1 or R:Role2)");
            OrOperator andOperator = (OrOperator)expression;

            Assert.IsNotNull(andOperator.Left);
            Assert.IsNotNull(andOperator.Right);
            Assert.AreEqual("Role1", ((RoleExpression)andOperator.Left).Word.Value);
            Assert.AreEqual("Role2", ((RoleExpression)andOperator.Right).Word.Value);
        }

        [TestMethod]
        public void AndOrNotTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse(
                "(R:HumanSR OR R:GeneralManagers) AND NOT R:HRSpecialist");

            AndOperator andOperator = (AndOperator)expression;

            OrOperator orOperator = (OrOperator)andOperator.Left;

            RoleExpression hrRoleExpression = (RoleExpression)orOperator.Left;
            Assert.AreEqual("HumanSR", hrRoleExpression.Word.Value);

            RoleExpression gmRoleExpression = (RoleExpression)orOperator.Right;
            Assert.AreEqual("GeneralManagers", gmRoleExpression.Word.Value);

            NotOperator notOperator = (NotOperator)andOperator.Right;

            RoleExpression specialistRoleExpression = (RoleExpression)notOperator.Expression;
            Assert.AreEqual("HRSpecialist", specialistRoleExpression.Word.Value);
        }

        [TestMethod]
        public void AndOrNotTestWithQuotedStrings()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse(
                "(R:\"Human SR\" OR R:\"General Managers\") AND NOT R:\"HR Specialist\"");

            AndOperator andOperator = (AndOperator)expression;

            OrOperator orOperator = (OrOperator)andOperator.Left;

            RoleExpression hrRoleExpression = (RoleExpression)orOperator.Left;
            Assert.AreEqual("Human SR", hrRoleExpression.Word.Value);

            RoleExpression gmRoleExpression = (RoleExpression)orOperator.Right;
            Assert.AreEqual("General Managers", gmRoleExpression.Word.Value);

            NotOperator notOperator = (NotOperator)andOperator.Right;

            RoleExpression specialistRoleExpression = (RoleExpression)notOperator.Expression;
            Assert.AreEqual("HR Specialist", specialistRoleExpression.Word.Value);
        }

        [TestMethod]
        public void NotAnonymousTest()
        {
            Parser parser = new Parser();
            BooleanExpression expression = parser.Parse("Not I:?");

            NotOperator notOperator = (NotOperator)expression;

            IdentityExpression identityExpression = (IdentityExpression)notOperator.Expression;
        }

        [TestMethod]
        public void MissingRightParenthesisTest()
        {
            ExpectSyntaxError("(R:Role1", 7);
        }

        [TestMethod]
        public void MissingOperandTest()
        {
            ExpectSyntaxError("And And", 0);
        }

        [TestMethod]
        public void RepeatingOperatorTest()
        {
            ExpectSyntaxError("R:Role1 Or OR R:Role2", 9);
        }

        [TestMethod]
        public void UnqualifiedWordTest()
        {
            ExpectSyntaxError("Not User1", 2);
        }

        void ExpectSyntaxError(string expression,
                               int index)
        {
            Parser parser = new Parser();
            try
            {
                parser.Parse(expression);
                Assert.Fail();
            }
            catch (SyntaxException e)
            {
                Assert.AreEqual(index, e.Index);
            }
        }
    }
}

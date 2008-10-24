//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests
{
    [TestClass]
    public class RuleSetNodeFixture
    {
        [TestMethod]
        public void RuleSetHasProperDefaultName()
        {
            RuleSetNode ruleSetNode = new RuleSetNode();

            Assert.AreEqual("Rule Set", ruleSetNode.Name);
        }

        [TestMethod]
        public void RuleSetHasRuleNameAsNodeName()
        {
            ValidationRulesetData ruleData = new ValidationRulesetData("Rule");
            RuleSetNode ruleSetNode = new RuleSetNode(ruleData);
            Assert.AreEqual("Rule", ruleSetNode.Name);
        }
    }
}

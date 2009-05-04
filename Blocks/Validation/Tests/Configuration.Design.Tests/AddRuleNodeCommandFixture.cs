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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Tests
{
    [TestClass]
    public class AddRuleNodeCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void AddRuleSetCommandAddsRuleNode()
        {
            AddRuleSetNodeCommand cmd = new AddRuleSetNodeCommand(ServiceProvider);
            cmd.Execute(ApplicationNode);

            RuleSetNode ruleNode = (RuleSetNode)Hierarchy.FindNodeByType(typeof(RuleSetNode));

            Assert.IsNotNull(ruleNode);
        }

        [TestMethod]
        public void AddRuleSetCommandAddsSelfNode()
        {
            AddRuleSetNodeCommand cmd = new AddRuleSetNodeCommand(ServiceProvider);
            cmd.Execute(ApplicationNode);

            SelfNode selNode = (SelfNode)Hierarchy.FindNodeByType(typeof(SelfNode));

            Assert.IsNotNull(selNode);
        }
    }
}

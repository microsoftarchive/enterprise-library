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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.TestSupport;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests
{
    [TestClass]
    public class AddPolicyCommandFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void PolicyIsAddedWithSensibleDefaultName()
        {
            AddPolicyCommand addPolicyCommand = new AddPolicyCommand(ServiceProvider);
            addPolicyCommand.Execute(ApplicationNode);

            PolicyNode policyNode = ApplicationNode.Hierarchy.FindNodeByType(typeof(PolicyNode)) as PolicyNode;

            Assert.IsNotNull(policyNode);
            Assert.AreEqual("Policy", policyNode.Name);
        }

        [TestMethod]
        public void AddPolicyCommandAddsMatchingRuleContainerNode()
        {
            AddPolicyCommand addPolicyCommand = new AddPolicyCommand(ServiceProvider);
            addPolicyCommand.Execute(ApplicationNode);

            PolicyNode policyNode = ApplicationNode.Hierarchy.FindNodeByType(typeof(PolicyNode)) as PolicyNode;
            MatchingRuleCollectionNode ruleContainer = ApplicationNode.Hierarchy.FindNodeByType(typeof(MatchingRuleCollectionNode)) as MatchingRuleCollectionNode;

            Assert.IsNotNull(policyNode);
            Assert.IsNotNull(ruleContainer);
            Assert.AreEqual(policyNode, ruleContainer.Parent);
        }

        [TestMethod]
        public void AddPolicyCommandAddsCallHandlersContainer()
        {
            AddPolicyCommand addPolicyCommand = new AddPolicyCommand(ServiceProvider);
            addPolicyCommand.Execute(ApplicationNode);

            PolicyNode policyNode = ApplicationNode.Hierarchy.FindNodeByType(typeof(PolicyNode)) as PolicyNode;
            CallHandlersCollectionNode handlersContainer = ApplicationNode.Hierarchy.FindNodeByType(typeof(CallHandlersCollectionNode)) as CallHandlersCollectionNode;

            Assert.IsNotNull(policyNode);
            Assert.IsNotNull(handlersContainer);
            Assert.AreEqual(policyNode, handlersContainer.Parent);
        }
    }
}

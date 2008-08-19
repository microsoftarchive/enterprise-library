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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Tests;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Tests
{
    [TestClass]
    public class MethodSignatureMatchingRuleNodeFixture : ConfigurationDesignHost
    {
        [TestMethod]
        public void MethodSignatureMatchingRuleNodeHasProperDefaultName()
        {
            MethodSignatureMatchingRuleNode ruleNode = new MethodSignatureMatchingRuleNode();
            Assert.AreEqual("Method Signature Matching Rule", ruleNode.Name);
        }

        [TestMethod]
        public void CanCreateMethodSignatureMatchingRuleNodeFromData()
        {
            MethodSignatureMatchingRuleData ruleData = new MethodSignatureMatchingRuleData();
            ruleData.Name = "rule name";
            ruleData.IgnoreCase = false;
            ruleData.Match = "MemberName";
            ruleData.Parameters.Add(new ParameterTypeElement("p1", "ParameterType1"));
            ruleData.Parameters.Add(new ParameterTypeElement("p2", "ParameterType2"));

            MethodSignatureMatchingRuleNode ruleNode = new MethodSignatureMatchingRuleNode(ruleData);
            Assert.AreEqual(ruleData.Name, ruleNode.Name);
            Assert.AreEqual(ruleData.Match, ruleNode.Match);
            Assert.AreEqual(ruleData.IgnoreCase, ruleNode.IgnoreCase);
            Assert.AreEqual(ruleData.Parameters.Count, ruleNode.ParameterTypes.Count);
            Assert.AreEqual(ruleData.Parameters.Get(0).ParameterTypeName, ruleNode.ParameterTypes[0].Type);
            Assert.AreEqual(ruleData.Parameters.Get(1).ParameterTypeName, ruleNode.ParameterTypes[1].Type);
        }

        [TestMethod]
        public void CanCreateRuleDataFromMethodSignatureMatchingRuleNode()
        {
            MethodSignatureMatchingRuleNode ruleNode = new MethodSignatureMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.Match = "MethodName";
            ruleNode.IgnoreCase = true;
            ruleNode.ParameterTypes.Add(new ParameterType("p1", "ParamType1"));
            ruleNode.ParameterTypes.Add(new ParameterType("p2", "ParamType2"));

            MethodSignatureMatchingRuleData ruleData = ruleNode.GetConfigurationData() as MethodSignatureMatchingRuleData;

            Assert.IsNotNull(ruleData);
            Assert.AreEqual(ruleNode.Name, ruleData.Name);
            Assert.AreEqual(ruleNode.Match, ruleData.Match);
            Assert.AreEqual(ruleNode.IgnoreCase, ruleData.IgnoreCase);
            Assert.AreEqual(ruleNode.ParameterTypes[0].Type, ruleData.Parameters.Get(0).ParameterTypeName);
            Assert.AreEqual(ruleNode.ParameterTypes[1].Type, ruleData.Parameters.Get(1).ParameterTypeName);
        }

        [TestMethod]
        public void ParameterTypeWithEmptyValueCausesValidationError()
        {
            MethodSignatureMatchingRuleNode ruleNode = new MethodSignatureMatchingRuleNode();
            ruleNode.Name = "RuleName";
            ruleNode.Match = "*";
            ruleNode.ParameterTypes.Add(new ParameterType("", "System.Int32"));

            ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider, true, false);
            cmd.Execute(ruleNode);
            Assert.IsFalse(cmd.ValidationSucceeded);
        }

        [TestMethod]
        public void DuplicateParameterTypeWithDifferentNamePassesValidation()
        {
            MethodSignatureMatchingRuleNode ruleNode = new MethodSignatureMatchingRuleNode();
            ruleNode.Name = "MethodSignatureRule";
            ruleNode.Match = "*";
            ruleNode.ParameterTypes.Add(new ParameterType("s1", "System.String"));
            ruleNode.ParameterTypes.Add(new ParameterType("s2", "System.String"));

            ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider, true, false);
            cmd.Execute(ruleNode);
            Assert.IsTrue(cmd.ValidationSucceeded);
        }

        [TestMethod]
        public void DuplicateParameterNameCausesValidationError()
        {
            MethodSignatureMatchingRuleNode ruleNode = new MethodSignatureMatchingRuleNode();
            ruleNode.Name = "MyRule";
            ruleNode.Match = "*";
            ruleNode.ParameterTypes.Add(new ParameterType("p1", "System.Int32"));
            ruleNode.ParameterTypes.Add(new ParameterType("p1", "System.String"));

            ValidateNodeCommand cmd = new ValidateNodeCommand(ServiceProvider, true, false);
            cmd.Execute(ruleNode);
            Assert.IsFalse(cmd.ValidationSucceeded);
        }
    }
}
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

using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.Configuration
{
    [TestClass]
    public class DefaultConfigurationFixture
    {
        [TestMethod]
        public void ParameterTypeMatchingRuleDataHasProperDefaultValues()
        {
            new ParameterTypeMatchingRuleData();
        }

        [TestMethod]
        public void ParameterTypeMatchDataHasProperDefaultValues()
        {
            var data = new ParameterTypeMatchData();
            Assert.AreEqual(ParameterKind.InputOrOutput, data.ParameterKind);
        }

        [TestMethod]
        public void AssemblyMatchingRuleDataHasProperDefaultValues()
        {
            new AssemblyMatchingRuleData();
        }

        [TestMethod]
        public void CustomAttributeMatchingRuleDataHasProperDefaultValues()
        {
            new CustomAttributeMatchingRuleData();
        }

        [TestMethod]
        public void MatchingRuleDataHasProperDefaultValues()
        {
            new MatchingRuleData();
        }

        [TestMethod]
        public void MemberNameMatchingRuleDataHasProperDefaultValues()
        {
            new MemberNameMatchingRuleData();
        }
        
        [TestMethod]
        public void MethodSignatureMatchingRuleDataHasProperDefaultValues()
        {
            new MethodSignatureMatchingRuleData();
        }

        [TestMethod]
        public void ParameterTypeElementHasProperDefaultValues()
        {
            new ParameterTypeElement();
        }
        
        [TestMethod]
        public void NamespaceMatchingRuleDataHasProperDefaultValues()
        {
            new NamespaceMatchingRuleData();
        }

        [TestMethod]
        public void PolicyDataHasProperDefaultValues()
        {
            new PolicyData();
        }

        [TestMethod]
        public void PropertyMatchingRuleDataHasProperDefaultValues()
        {
            new PropertyMatchingRuleData();
        }

        [TestMethod]
        public void PropertyMatchDataHasProperDefaultValues()
        {
            var propertyMatchData = new PropertyMatchData();
            Assert.AreEqual(PropertyMatchingOption.GetOrSet, propertyMatchData.MatchOption);
        }

        [TestMethod]
        public void TagAttributeMatchingRuleDataHasProperDefaultValues()
        {
            new TagAttributeMatchingRuleData();
        }

        [TestMethod]
        public void TypeMatchingRuleDataHasProperDefaultValues()
        {
            new TypeMatchingRuleData();
        }
    }
}

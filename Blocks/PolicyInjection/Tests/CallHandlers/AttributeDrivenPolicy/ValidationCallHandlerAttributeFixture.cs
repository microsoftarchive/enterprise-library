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

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.AttributeDrivenPolicy
{
    [TestClass]
    public class ValidationCallHandlerAttributeFixture
    {
        [TestMethod]
        public void ShouldCreateDefaultHandlerFromAttribute()
        {
            ValidationCallHandlerAttribute attribute = new ValidationCallHandlerAttribute();
            ValidationCallHandler handler = GetHandlerFromAttribute(attribute);

            Assert.AreEqual(string.Empty, handler.RuleSet);
            Assert.AreEqual(SpecificationSource.Both, handler.SpecificationSource);
        }

        [TestMethod]
        public void ShouldSetRulesetFromAttribute()
        {
            string ruleset = "Some Ruleset";
            ValidationCallHandlerAttribute attribute = new ValidationCallHandlerAttribute(ruleset);
            ValidationCallHandler handler = GetHandlerFromAttribute(attribute);
            Assert.AreEqual(ruleset, handler.RuleSet);
            Assert.AreEqual(SpecificationSource.Both, handler.SpecificationSource);
        }

        [TestMethod]
        public void ShouldSetSpecificationSourceFromAttribute()
        {
            ValidationCallHandlerAttribute attribute = new ValidationCallHandlerAttribute();
            attribute.SpecificationSource = SpecificationSource.ParameterAttributesOnly;
            ValidationCallHandler handler = GetHandlerFromAttribute(attribute);
            Assert.AreEqual(string.Empty, handler.RuleSet);
            Assert.AreEqual(SpecificationSource.ParameterAttributesOnly, handler.SpecificationSource);
        }

        ValidationCallHandler GetHandlerFromAttribute(ValidationCallHandlerAttribute attribute)
        {
            return (ValidationCallHandler)attribute.CreateHandler(null);
        }
    }
}

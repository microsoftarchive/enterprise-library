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

using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.Configuration
{
    [TestClass]
    public class ValidationCallHandlerFixture : CallHandlerDataFixtureBase
    {
        [TestMethod]
        public void CanDeserializeValidationCallHandlerData()
        {
            ValidationCallHandlerData data = new ValidationCallHandlerData("Logging Handler");
            data.RuleSet = "MyRuleSet";
            data.SpecificationSource = SpecificationSource.Configuration;
            data.Order = 7;

            ValidationCallHandlerData deserialized =
                (ValidationCallHandlerData)SerializeAndDeserializeHandler(data);

            Assert.AreEqual(data.RuleSet, deserialized.RuleSet);
            Assert.AreEqual(data.SpecificationSource, deserialized.SpecificationSource);
            Assert.AreEqual(typeof(ValidationCallHandler), deserialized.Type);
            Assert.AreEqual(data.Order, deserialized.Order);
        }

        [TestMethod]
        public void AssembledValidationCallHandler()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();

            PolicyData policyData = new PolicyData("policy");
            ValidationCallHandlerData data = new ValidationCallHandlerData("FooCallHandler", 2);
            policyData.Handlers.Add(data);
            settings.Policies.Add(policyData);

            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();
            dictConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            ICallHandler handler = CallHandlerCustomFactory.Instance.Create(null, data, dictConfigurationSource, null);
            Assert.IsNotNull(handler);
            Assert.AreEqual(handler.Order, data.Order);
        }

        [TestMethod]
        public void CreatedValidationCallHandlerFromAttributes()
        {
            MethodInfo method = typeof(ValidationMock).GetMethod("ReturnSomething");
            object[] attributes = method.GetCustomAttributes(typeof(ValidationCallHandlerAttribute), false);

            Assert.AreEqual(attributes.Length, 1);

            ValidationCallHandlerAttribute attribute = attributes[0] as ValidationCallHandlerAttribute;

            Assert.IsNotNull(attribute);

            ICallHandler handler = attribute.CreateHandler();

            Assert.IsNotNull(handler);
            Assert.AreEqual(handler.Order, 16);
        }
    }

    class ValidationMock
    {
        [ValidationCallHandler(Order=16)]
        public string ReturnSomething()
        {
            return string.Empty;
        }
    }
}
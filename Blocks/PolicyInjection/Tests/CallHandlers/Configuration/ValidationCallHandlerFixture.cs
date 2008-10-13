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

using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
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

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            var policy = container.Configure<Interception>().AddPolicy("test");
            policy.AddMatchingRule(new AlwaysMatchingRule());
            data.ConfigurePolicy(policy, null);

            var handlers
                = new List<ICallHandler>(
                    container.Resolve<InjectionPolicy>("test")
                        .GetHandlersFor(GetMethodImpl(MethodBase.GetCurrentMethod()), container));

            Assert.AreEqual(1, handlers.Count);

            ICallHandler handler = handlers[0];

            Assert.IsNotNull(handler);
            Assert.AreEqual(handler.Order, data.Order);
        }

        [TestMethod]
        public void ConfiguresCallHandlerAsSingleton()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();

            PolicyData policyData = new PolicyData("policy");
            ValidationCallHandlerData data = new ValidationCallHandlerData("FooCallHandler", 2);
            policyData.Handlers.Add(data);
            settings.Policies.Add(policyData);

            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();
            dictConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            var policy = container.Configure<Interception>().AddPolicy("test");
            policy.AddMatchingRule(new AlwaysMatchingRule());
            data.ConfigurePolicy(policy, null);

            var handlers1
                = new List<ICallHandler>(
                    container.Resolve<InjectionPolicy>("test")
                        .GetHandlersFor(GetMethodImpl(MethodBase.GetCurrentMethod()), container));
            var handlers2
                = new List<ICallHandler>(
                    container.Resolve<InjectionPolicy>("test")
                        .GetHandlersFor(GetMethodImpl(MethodBase.GetCurrentMethod()), container));

            CollectionAssert.AreEquivalent(handlers1, handlers2);
        }

        [TestMethod]
        public void CreatedValidationCallHandlerFromAttributes()
        {
            MethodInfo method = typeof(ValidationMock).GetMethod("ReturnSomething");
            object[] attributes = method.GetCustomAttributes(typeof(ValidationCallHandlerAttribute), false);

            Assert.AreEqual(attributes.Length, 1);

            ValidationCallHandlerAttribute attribute = attributes[0] as ValidationCallHandlerAttribute;

            Assert.IsNotNull(attribute);

            ICallHandler handler = attribute.CreateHandler(null);

            Assert.IsNotNull(handler);
            Assert.AreEqual(handler.Order, 16);
        }

        private static MethodImplementationInfo GetMethodImpl(MethodBase method)
        {
            return new MethodImplementationInfo(null, ((MethodInfo) method));
        }
    }

    class ValidationMock
    {
        [ValidationCallHandler(Order = 16)]
        public string ReturnSomething()
        {
            return string.Empty;
        }
    }
}
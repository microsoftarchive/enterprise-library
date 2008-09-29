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
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.Configuration
{
    [TestClass]
    public class ExceptionCallHandlerDataFixture : CallHandlerDataFixtureBase
    {
        [TestMethod]
        public void CanSerializeExceptionCallHandler()
        {
            ExceptionCallHandlerData handlerData =
                new ExceptionCallHandlerData("CallHandler", "Swallow Exceptions");

            ExceptionCallHandlerData deserializedHandler =
                SerializeAndDeserializeHandler(handlerData) as ExceptionCallHandlerData;

            Assert.IsNotNull(deserializedHandler);
            Assert.AreEqual(handlerData.Name, deserializedHandler.Name);
            Assert.AreEqual(handlerData.ExceptionPolicyName, deserializedHandler.ExceptionPolicyName);
        }

        [TestMethod]
        public void CanCreateHandlerViaAssemblerWithProperData()
        {
            ExceptionCallHandlerData data =
                new ExceptionCallHandlerData("handler", "Swallow Exceptions");
            data.Order = 5;

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            var policy = container.Configure<Interception>().AddPolicy("test");
            policy.AddMatchingRule(new AlwaysMatchingRule());
            data.ConfigurePolicy(policy, null);

            var handlers
                = new List<ICallHandler>(
                    container.Resolve<InjectionPolicy>("test")
                        .GetHandlersFor(MethodInfo.GetCurrentMethod(), container));

            Assert.AreEqual(1, handlers.Count);

            ExceptionCallHandler handler = (ExceptionCallHandler)handlers[0];

            Assert.AreEqual(data.ExceptionPolicyName, handler.ExceptionPolicyName);
            Assert.AreEqual(data.Order, handler.Order);
        }

        [TestMethod]
        public void ConfiguresHandlerAsSingleton()
        {
            ExceptionCallHandlerData data =
                new ExceptionCallHandlerData("handler", "Swallow Exceptions");
            data.Order = 5;

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            var policy = container.Configure<Interception>().AddPolicy("test");
            policy.AddMatchingRule(new AlwaysMatchingRule());
            data.ConfigurePolicy(policy, null);

            var handlers1
                = new List<ICallHandler>(
                    container.Resolve<InjectionPolicy>("test")
                        .GetHandlersFor(MethodInfo.GetCurrentMethod(), container));
            var handlers2
                = new List<ICallHandler>(
                    container.Resolve<InjectionPolicy>("test")
                        .GetHandlersFor(MethodInfo.GetCurrentMethod(), container));

            CollectionAssert.AreEquivalent(handlers1, handlers2);
        }
    }
}
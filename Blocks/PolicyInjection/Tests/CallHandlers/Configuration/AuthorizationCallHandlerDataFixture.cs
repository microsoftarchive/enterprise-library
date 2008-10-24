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
    public class AuthorizationCallHandlerDataFixture : CallHandlerDataFixtureBase
    {
        [TestMethod]
        public void CanDeserializeAuthorizationCallHandlerData()
        {
            AuthorizationCallHandlerData data = new AuthorizationCallHandlerData("Authorization Handler");
            data.AuthorizationProvider = "auhtorizationProvider";
            data.OperationName = "operationName";
            data.Order = 5;

            AuthorizationCallHandlerData deserialized =
                (AuthorizationCallHandlerData)SerializeAndDeserializeHandler(data);

            Assert.AreEqual(data.AuthorizationProvider, deserialized.AuthorizationProvider);
            Assert.AreEqual(data.OperationName, deserialized.OperationName);
            Assert.AreEqual(typeof(AuthorizationCallHandler), deserialized.Type);
            Assert.AreEqual(data.Order, deserialized.Order);
        }

        [TestMethod]
        public void CanCreateHandlerFromDataWithCorrectProperties()
        {
            AuthorizationCallHandlerData data = new AuthorizationCallHandlerData("Auth handler");
            data.AuthorizationProvider = "authorizationProvider";
            data.OperationName = "op";
            data.Order = 7;

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            var policy = container.Configure<Interception>().AddPolicy("test");
            policy.AddMatchingRule(new AlwaysMatchingRule());
            data.ConfigurePolicy(policy, null);

            var handlers
                = new List<ICallHandler>(
                    container.Resolve<InjectionPolicy>("test")
                        .GetHandlersFor(GetMethodImpl(MethodBase.GetCurrentMethod()), container));

            Assert.AreEqual(1, handlers.Count);

            AuthorizationCallHandler handler = (AuthorizationCallHandler)handlers[0];
            Assert.AreEqual(data.AuthorizationProvider, handler.ProviderName);
            Assert.AreEqual(data.OperationName, handler.OperationName);
            Assert.AreEqual(data.Order, handler.Order);
        }

        [TestMethod]
        public void ConfiguresHandlerAsSingleton()
        {
            AuthorizationCallHandlerData data = new AuthorizationCallHandlerData("Auth handler");
            data.AuthorizationProvider = "authorizationProvider";
            data.OperationName = "op";
            data.Order = 7;

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

        private static MethodImplementationInfo GetMethodImpl(MethodBase method)
        {
            return new MethodImplementationInfo(null, ((MethodInfo) method));
        }
    }
}

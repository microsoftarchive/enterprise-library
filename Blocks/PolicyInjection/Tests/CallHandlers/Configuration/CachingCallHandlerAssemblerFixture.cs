using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Tests.ObjectsUnderTest;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.Configuration
{
    /// <summary>
    /// Summary description for CachingCallHandlerAssemblerFixture
    /// </summary>
    [TestClass]
    public class CachingCallHandlerAssemblerFixture
    {
        [TestMethod]
        public void CanCreateCallHandlerViaAssemberWithProperData()
        {
            CachingCallHandlerData data = new CachingCallHandlerData()
            {
                Name = "cache me",
                Order = 37,
                ExpirationTime = new TimeSpan(0, 30, 0)
            };

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            var policy = container.Configure<Interception>().AddPolicy("test");
            policy.AddMatchingRule(new AlwaysMatchingRule());
            data.ConfigurePolicy(policy, null);

            var handlers
                = new List<ICallHandler>(
                    container.Resolve<InjectionPolicy>("test")
                        .GetHandlersFor(MethodInfo.GetCurrentMethod(), container));

            Assert.AreEqual(1, handlers.Count);

            CachingCallHandler handler = (CachingCallHandler)handlers[0];

            Assert.AreEqual(data.Order, handler.Order);
            Assert.AreEqual(data.ExpirationTime, handler.ExpirationTime);
        }

        [TestMethod]
        public void ConfiguresHandlerAsSingleton()
        {
            CachingCallHandlerData data = new CachingCallHandlerData()
            {
                Name = "cache me",
                Order = 37,
                ExpirationTime = new TimeSpan(0, 30, 0)
            };

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

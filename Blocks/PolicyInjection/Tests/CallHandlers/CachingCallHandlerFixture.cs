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

using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Caching;
using System.Xml;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests
{
    [TestClass]
    public class CachingCallHandlerFixture
    {
        DefaultCacheKeyGenerator keyGenerator;
        MethodInfo calculateMethod;
        MethodInfo serializeXmlMethod;
        MethodInfo sometimesReturnsNullMethod;
        IUnityContainer container;

        [TestInitialize]
        public void Setup()
        {
            calculateMethod = typeof(CachingTarget).GetMethod("Calculate");
            serializeXmlMethod = typeof(CachingTarget).GetMethod("SerializeXml");
            sometimesReturnsNullMethod = typeof(CachingTarget).GetMethod("SometimesReturnsNull");
            keyGenerator = new DefaultCacheKeyGenerator();

            container = new UnityContainer();
            container.AddNewExtension<Interception>();
            container.Configure<Interception>()
                .SetDefaultInterceptorFor<CachingTarget>(new TransparentProxyInterceptor());
        }

        [TestMethod]
        public void ShouldAddItemToCache()
        {
            AddCachingPolicies(container);

            CachingTarget target = container.Resolve<CachingTarget>();

            double input = 7;

            ClearCache(input);

            Assert.IsNull(HttpRuntime.Cache.Get(keyGenerator.CreateCacheKey(calculateMethod, input)));

            target.Calculate(input);

            object[] cachedValue = (object[])HttpRuntime.Cache.Get(keyGenerator.CreateCacheKey(calculateMethod, input));
            Assert.IsNotNull(cachedValue);
            Assert.IsTrue(cachedValue[0] is double);
            Assert.AreEqual(14.0, (double)cachedValue[0]);
        }

        [TestMethod]
        public void ShouldReturnValueFromCache()
        {
            AddCachingPolicies(container);

            CachingTarget target = container.Resolve<CachingTarget>();

            double input = 12;
            ClearCache(input);
            HttpRuntime.Cache.Add(keyGenerator.CreateCacheKey(calculateMethod, input), new object[] { 100.0 },
                                  null, Cache.NoAbsoluteExpiration, new TimeSpan(0, 5, 0), CacheItemPriority.Normal,
                                  null);

            double result = target.Calculate(input);

            Assert.AreEqual(100.0, result);
        }

        [TestMethod]
        public void ShouldDefaultToFiveMinuteCacheExpiration()
        {
            CachingCallHandler handler = new CachingCallHandler();
            Assert.AreEqual(new TimeSpan(0, 5, 0), handler.ExpirationTime);
        }

        [TestMethod]
        public void ShouldBeAbleToSetCacheExpiration()
        {
            CachingCallHandler handler = new CachingCallHandler(new TimeSpan(1, 0, 0));

            Assert.AreEqual(new TimeSpan(1, 0, 0), handler.ExpirationTime);
        }

        [TestMethod]
        public void CreateCachingCallHandlerFromConfiguration()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();

            PolicyData policyData = new PolicyData("policy");
            CachingCallHandlerData data = new CachingCallHandlerData("FooCallHandler", 2);
            policyData.MatchingRules.Add(new CustomMatchingRuleData("matchesEverything", typeof(AlwaysMatchingRule)));
            policyData.Handlers.Add(data);
            settings.Policies.Add(policyData);

            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();
            dictConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            settings.ConfigureContainer(container, dictConfigurationSource);

            InjectionFriendlyRuleDrivenPolicy policy = container.Resolve<InjectionFriendlyRuleDrivenPolicy>("policy");

            ICallHandler handler
                = (policy.GetHandlersFor(new MethodImplementationInfo(null, ((MethodInfo) MethodBase.GetCurrentMethod())), container)).ElementAt(0);
            Assert.IsNotNull(handler);
            Assert.AreEqual(handler.Order, data.Order);
        }

        [TestMethod]
        public void ShouldCreateCacheUsingDefaultConfiguration()
        {
            CachingCallHandler handler = new CachingCallHandler();

            Assert.AreEqual(0, handler.Order);
        }

        [TestMethod]
        public void EqualReferenceObjectsProduceSameKeys()
        {
            XmlDocument doc1 = new XmlDocument();
            doc1.LoadXml("<root><child value=\"foo\"/></root>");

            XmlDocument doc2 = doc1;

            string key1 = keyGenerator.CreateCacheKey(serializeXmlMethod, doc1);
            string key2 = keyGenerator.CreateCacheKey(serializeXmlMethod, doc2);

            Assert.AreEqual(key1, key2);
        }

        [TestMethod]
        public void DifferentReferenceObjectsProduceDifferentKeys()
        {
            XmlDocument doc1 = new XmlDocument();
            doc1.LoadXml("<root><child value=\"foo\"/></root>");

            XmlDocument doc2 = new XmlDocument();
            doc2.LoadXml("<root><child value=\"bar\"/></root>");

            string key1 = keyGenerator.CreateCacheKey(serializeXmlMethod, doc1);
            string key2 = keyGenerator.CreateCacheKey(serializeXmlMethod, doc2);

            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        public void EqualValueObjectsProduceSameKeys()
        {
            int x = 314159;
            int y = 314159;

            string key1 = keyGenerator.CreateCacheKey(calculateMethod, x);
            string key2 = keyGenerator.CreateCacheKey(calculateMethod, y);

            Assert.AreEqual(key1, key2);
        }

        [TestMethod]
        public void DifferentValueObjectsProduceDifferentKeys()
        {
            int x = 314159;
            int y = 54321;

            string key1 = keyGenerator.CreateCacheKey(calculateMethod, x);
            string key2 = keyGenerator.CreateCacheKey(calculateMethod, y);

            Assert.AreNotEqual(key1, key2);
        }

        [TestMethod]
        public void CanGenerateCacheKeysForMethodsWithNullParameters()
        {
            string key1 = keyGenerator.CreateCacheKey(serializeXmlMethod, null);
            Assert.IsNotNull(key1);
        }

        [TestMethod]
        public void CanGenerateCacheKeysForMethodsWithNullAndNotNullParameters()
        {
            string key1 = keyGenerator.CreateCacheKey(serializeXmlMethod, null, new XmlDocument());
            Assert.IsNotNull(key1);
        }

        [TestMethod]
        public void ShouldBeAbleToCacheNullReturnValues()
        {
            AddCachingPolicies(container);

            CachingTarget target = container.Resolve<CachingTarget>();

            string result = target.SometimesReturnsNull(false);
            string result2 = target.SometimesReturnsNull(true);
        }

        [TestMethod]
        public void ShouldNotCacheVoidMethods()
        {
            AddCachingPolicies(container);

            CachingTarget target = container.Resolve<CachingTarget>();

            Assert.AreEqual(0, target.Count);
            target.IncrementCount();
            Assert.AreEqual(1, target.Count);
            target.IncrementCount();
            Assert.AreEqual(2, target.Count);
        }

        [TestMethod]
        public void ShouldCacheViaAttributeOnInterface()
        {
            MethodInfo getNewsMethod = typeof(INewsService).GetMethod("GetNews");

            container.RegisterType<INewsService, NewsService>();
            container.Configure<Interception>().SetDefaultInterceptorFor<INewsService>(new TransparentProxyInterceptor());

            INewsService target = container.Resolve<INewsService>();

            ClearCache(getNewsMethod);
            IList news = target.GetNews();

            object[] cachedValue = (object[])(HttpRuntime.Cache.Get(keyGenerator.CreateCacheKey(getNewsMethod)));
            Assert.IsNotNull(cachedValue);
            Assert.AreEqual(1, cachedValue.Length);
            Assert.IsTrue(cachedValue[0] is IList);
            Assert.AreSame(news, cachedValue[0]);
        }

        [TestMethod]
        public void CreatesHandlerProperlyFromAttributes()
        {
            MethodInfo method = typeof(INewsService).GetMethod("GetNews");

            Assert.IsNotNull(method);

            object[] attributes = method.GetCustomAttributes(typeof(CachingCallHandlerAttribute), false);

            Assert.AreEqual(1, attributes.Length);

            CachingCallHandlerAttribute att = attributes[0] as CachingCallHandlerAttribute;
            ICallHandler callHandler = att.CreateHandler(null);

            Assert.IsNotNull(callHandler);
            Assert.AreEqual(6, callHandler.Order);
        }

        static void AddCachingPolicies(IUnityContainer container)
        {
            container.Configure<Interception>()
                .AddPolicy("Caching")
                    .AddMatchingRule(new TypeMatchingRule(typeof(CachingTarget)))
                    .AddCallHandler(new CachingCallHandler());
        }

        void ClearCache(double input)
        {
            HttpRuntime.Cache.Remove(keyGenerator.CreateCacheKey(calculateMethod, input));
        }

        void ClearCache(MethodInfo method,
                        params object[] args)
        {
            HttpRuntime.Cache.Remove(keyGenerator.CreateCacheKey(method, args));
        }
    }

    public class CachingTarget : MarshalByRefObject
    {
        public int Count = 0;

        public double Calculate(double d)
        {
            return d * 2;
        }

        public void IncrementCount()
        {
            ++Count;
        }

        public string SerializeXml(XmlDocument doc)
        {
            return doc.DocumentElement.OuterXml;
        }

        public string SometimesReturnsNull(bool shouldReturnNull)
        {
            if (shouldReturnNull)
            {
                return null;
            }

            return "This is not null";
        }
    }

    public interface INewsService
    {
        [CachingCallHandler(0, 0, 30, Order = 6)]
        IList GetNews();
    }

    public class NewsService : INewsService
    {
        public IList GetNews()
        {
            return new ArrayList(new string[] { "News1", "News2", "News3" });
        }
    }
}

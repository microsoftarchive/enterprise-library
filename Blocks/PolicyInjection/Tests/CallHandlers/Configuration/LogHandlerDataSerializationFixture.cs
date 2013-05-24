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

using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests.Configuration
{
    [TestClass]
    public class LogHandlerDataSerializationFixture : CallHandlerDataFixtureBase
    {
        [TestMethod]
        public void CanDeserializeLogCallHandlerData()
        {
            LogCallHandlerData data = new LogCallHandlerData("Logging Handler");
            data.AfterMessage = "This is the after message";
            data.BeforeMessage = "This is the before message";
            data.EventId = 89;
            data.Categories = new NamedElementCollection<LogCallHandlerCategoryEntry>();
            data.Categories.Add(new LogCallHandlerCategoryEntry("General"));
            data.Categories.Add(new LogCallHandlerCategoryEntry("Type {namespace}.{type}"));
            data.Categories.Add(new LogCallHandlerCategoryEntry("PIAB Logging"));
            data.IncludeCallStack = true;
            data.IncludeCallTime = true;
            data.IncludeParameterValues = true;
            data.LogBehavior = HandlerLogBehavior.After;
            data.Priority = 42;
            data.Severity = TraceEventType.Critical;
            data.Order = 8;

            LogCallHandlerData deserialized =
                (LogCallHandlerData)SerializeAndDeserializeHandler(data);

            Assert.AreEqual(data.Name, deserialized.Name);
            Assert.AreSame(typeof(LogCallHandler), deserialized.Type);
            Assert.AreEqual(data.AfterMessage, data.AfterMessage);
            Assert.AreEqual(data.BeforeMessage, deserialized.BeforeMessage);
            Assert.AreEqual(data.EventId, deserialized.EventId);
            Assert.IsNotNull(deserialized.Categories);
            Assert.AreEqual(data.Categories.Count, deserialized.Categories.Count);
            Assert.AreEqual(data.Categories.Get(0).Name, deserialized.Categories.Get(0).Name);
            Assert.AreEqual(data.Categories.Get(1).Name, deserialized.Categories.Get(1).Name);
            Assert.AreEqual(data.Categories.Get(2).Name, deserialized.Categories.Get(2).Name);
            Assert.AreEqual(data.IncludeCallStack, deserialized.IncludeCallStack);
            Assert.AreEqual(data.IncludeCallTime, deserialized.IncludeCallTime);
            Assert.AreEqual(data.IncludeParameterValues, deserialized.IncludeParameterValues);
            Assert.AreEqual(data.LogBehavior, deserialized.LogBehavior);
            Assert.AreEqual(data.Priority, deserialized.Priority);
            Assert.AreEqual(data.Severity, deserialized.Severity);
            Assert.AreEqual(data.Order, deserialized.Order);
        }

        [TestMethod]
        public void DataDefaultsShouldMatchHandlerDefaults()
        {
            LogCallHandlerData data = new LogCallHandlerData("Log handler");
            Assert.AreEqual(LogCallHandlerDefaults.AfterMessage, data.AfterMessage);
            Assert.AreEqual(LogCallHandlerDefaults.BeforeMessage, data.BeforeMessage);
            Assert.AreEqual(0, data.Categories.Count);
            Assert.AreEqual(LogCallHandlerDefaults.EventId, data.EventId);
            Assert.AreEqual(LogCallHandlerDefaults.IncludeCallStack, data.IncludeCallStack);
            Assert.AreEqual(LogCallHandlerDefaults.IncludeCallTime, data.IncludeCallTime);
            Assert.AreEqual(LogCallHandlerDefaults.IncludeParameters, data.IncludeParameterValues);
            Assert.AreEqual(LogCallHandlerDefaults.LogAfterCall, data.LogBehavior == HandlerLogBehavior.BeforeAndAfter || data.LogBehavior == HandlerLogBehavior.After);
            Assert.AreEqual(LogCallHandlerDefaults.LogBeforeCall, data.LogBehavior == HandlerLogBehavior.BeforeAndAfter || data.LogBehavior == HandlerLogBehavior.Before);
            Assert.AreEqual(LogCallHandlerDefaults.Priority, data.Priority);
            Assert.AreEqual(LogCallHandlerDefaults.Severity, data.Severity);
            Assert.AreEqual(LogCallHandlerDefaults.Order, data.Order);
        }

        [TestMethod]
        public void AssembledCorrectlyLogCallHandler()
        {
            using (var configSource = new FileConfigurationSource("LogCallHandler.config", false))
            {
                Logger.SetLogWriter(new LogWriterFactory(configSource.GetSection).Create(), false);

                PolicyInjectionSettings settings = new PolicyInjectionSettings();

                PolicyData policyData = new PolicyData("policy");
                LogCallHandlerData data = new LogCallHandlerData("fooHandler", 66);
                data.BeforeMessage = "before";
                data.AfterMessage = "after";
                data.IncludeCallTime = true;
                data.EventId = 100;
                data.Categories.Add(new LogCallHandlerCategoryEntry("category1"));
                data.Categories.Add(new LogCallHandlerCategoryEntry("category2"));
                policyData.MatchingRules.Add(new CustomMatchingRuleData("matchesEverything", typeof(AlwaysMatchingRule)));
                policyData.Handlers.Add(data);
                settings.Policies.Add(policyData);

                IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
                settings.ConfigureContainer(container);

                var policy = container.Resolve<RuleDrivenPolicy>("policy");

                LogCallHandler handler = (LogCallHandler)
                                         (policy.GetHandlersFor(GetMethodImpl(MethodBase.GetCurrentMethod()), container)).ElementAt(0);
                Assert.IsNotNull(handler);
                Assert.AreEqual(66, handler.Order);
                Assert.AreEqual("before", handler.BeforeMessage);
                Assert.AreEqual("after", handler.AfterMessage);
                Assert.AreEqual(true, handler.IncludeCallTime);
                Assert.AreEqual(100, handler.EventId);
                Assert.AreEqual(2, handler.Categories.Count);
                CollectionAssert.Contains(handler.Categories, "category1");
                CollectionAssert.Contains(handler.Categories, "category2");
            }
        }

        [TestCleanup]
        public void TestCleanup()
        {
            Logger.Reset();
        }

        private static MethodImplementationInfo GetMethodImpl(MethodBase method)
        {
            return new MethodImplementationInfo(null, ((MethodInfo)method));
        }
    }
}

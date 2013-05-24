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
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.TestSupport;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Logging.PolicyInjection;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests
{
    [TestClass]
    public class LogCallHandlerFixture
    {
        const string beforeMessage = "@@@ Logging before calling target @@@";
        const string afterMessage = "@@@ Logging after calling target @@@";
        const string beginCallTimeMarker = "@@BEGIN CALL TIME@@";
        const string endCallTimeMarker = "@@END CALL TIME@@";

        string preCallTemplate =
            string.Format(
                @"
Call log of parameters for call to {{property(TypeName)}}.{{property(MethodName)}}{{newline}}
Log Category: {{category}}{{newline}}
Event ID: {{eventid}}{{newline}}
Priority: {{priority}}{{newline}}
Severity: {{severity}}{{newline}}
Log message: {{message}}{{newline}}
Call Time: {0}{{property(CallTime)}}{1}{{newline}}
Parameter values:{{newline}}
{{dictionary({{key}} = {{value}}{{newline}})}}
Return value: {{property(ReturnValue)}}{{newline}}
Exception: @@BEGIN EXCEPTION@@{{property(Exception)}}@@END EXCEPTION@@{{newline}}
Call Stack: @@BEGIN CALL STACK@@{{property(CallStack)}}@@END CALL STACK@@{{newline}}",
                beginCallTimeMarker, endCallTimeMarker);

        [TestCleanup]
        public void TestCleanup()
        {
            Logger.Reset();
        }

        [TestMethod]
        public void ShouldLogOnlyToCategoriesGivenInConfig()
        {
            using (var configSource = new FileConfigurationSource("LogCallHandler.config", false))
            {
                Logger.SetLogWriter(new LogWriterFactory(configSource.GetSection).Create(), false);

                using (var eventLog = new EventLogTracker("Application"))
                {
                    using (var injector = new PolicyInjector(configSource))
                    {
                        LoggingTarget target = injector.Create<LoggingTarget>();
                        target.DoSomething(1, "two", 3.0);
                    }

                    Assert.AreEqual(1, eventLog.NewEntries().Select(le => le.Category == "Default Category").Count());
                }
            }
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

                using (var container = new UnityContainer().AddNewExtension<Interception>())
                {
                    settings.ConfigureContainer(container);

                    var policy = container.Resolve<InjectionPolicy>("policy");

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
        }

        private static MethodImplementationInfo GetMethodImpl(MethodBase method)
        {
            return new MethodImplementationInfo(null, ((MethodInfo)method));
        }
    }

    public class LoggingTarget : MarshalByRefObject
    {
        public string DoSomething(int one,
                                  string two,
                                  double three)
        {
            return string.Format("{1}: {0} {2}", one, two, three);
        }

        public void DoSomethingBad()
        {
            throw new ApplicationException("Exception thrown here");
        }

        [LogCallHandler(Order = 9)]
        public void DoSomethingElse(string message) { }
    }
}

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
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.TestSupport.ObjectsUnderTest;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Tests
{
    /// <summary>
    /// Tests for the PerformanceCounterCallHandler. These tests require that
    /// the performance counters are added to your machine before they will
    /// run successfully. A script to do this is forthcoming.
    /// </summary>
    [TestClass]
    public class PerformanceCounterCallHandlerFixture
    {
        public const string TestCategoryName = "Call Handler Unit Tests";
        public const string TestInstanceName = "PerformanceCounterCallHandlerFixture";

        PerformanceCounterCallHandler callHandler;

        [TestMethod]
        public void ShouldRecordOneCallTotal()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetTotalCallCount());
            target.DoSomething();
            Assert.AreEqual(1L, GetTotalCallCount());
        }

        [TestMethod]
        public void ShouldRecordOneCallInstance()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetNumberOfCallsCount(TestInstanceName));
            target.DoSomething();
            Assert.AreEqual(1L, GetNumberOfCallsCount(TestInstanceName));
        }

        [TestMethod]
        public void ShouldBeAbleToDisableTotalCount()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            callHandler.UseTotalCounter = false;
            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetTotalCallCount());
            Assert.AreEqual(0L, GetNumberOfCallsCount(TestInstanceName));
            target.DoSomething();
            Assert.AreEqual(1L, GetNumberOfCallsCount(TestInstanceName));
            Assert.AreEqual(0L, GetTotalCallCount());
        }

        [TestMethod]
        public void ShouldBeAbleToDisableCallCount()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            callHandler.IncrementNumberOfCalls = false;
            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetTotalCallCount());
            Assert.AreEqual(0L, GetNumberOfCallsCount(TestInstanceName));
            target.DoSomething();
            Assert.AreEqual(0L, GetNumberOfCallsCount(TestInstanceName));
            Assert.AreEqual(0L, GetTotalCallCount());
        }

        [TestMethod]
        public void ShouldUpdateCallsPerSecond()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetCallsPerSecondCount(TestInstanceName));
            for (int i = 0; i < 100; ++i)
            {
                target.DoSomething();
            }

            // Timing is too unpredictable to check the actual value, but as long as we get something
            // other than zero we know the counter is updating.
            Assert.IsTrue(GetCallsPerSecondCount(TestInstanceName) > 0);
        }

        [TestMethod]
        public void ShouldBeAbleToDisableCallsPerSecond()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            callHandler.IncrementCallsPerSecond = false;
            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetCallsPerSecondCount(TestInstanceName));
            for (int i = 0; i < 100; ++i)
            {
                target.DoSomething();
            }

            // Timing is too unpredictable to check the actual value, but as long as we get something
            // other than zero we know the counter is updating.
            Assert.AreEqual(0L, GetCallsPerSecondCount(TestInstanceName));
        }

        [TestMethod]
        public void ShouldUpdateNumberOfExceptionsCounter()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            callHandler.IncrementTotalExceptions = true;
            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetNumberOfExceptionsCount(TestInstanceName));

            CauseExceptions(target);

            Assert.AreEqual(20L, GetNumberOfExceptionsCount(TestInstanceName));
        }

        [TestMethod]
        public void ShouldDefaultToDisableExceptionCounter()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetNumberOfExceptionsCount(TestInstanceName));

            CauseExceptions(target);

            Assert.AreEqual(0L, GetNumberOfExceptionsCount(TestInstanceName));
        }

        [TestMethod]
        public void ShouldNotUpdateExceptionsPerSecondByDefault()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetExceptionsPerSecondCount(TestInstanceName));

            CauseExceptions(target);

            Assert.AreEqual(0L, GetExceptionsPerSecondCount(TestInstanceName));
        }

        [TestMethod]
        public void ShouldUpdateExceptionsPerSecond()
        {
            ResetCounters();
            IUnityContainer factory = GetConfiguredContainer();
            callHandler.IncrementExceptionsPerSecond = true;

            MonitorTarget target = factory.Resolve<MonitorTarget>();

            Assert.AreEqual(0L, GetExceptionsPerSecondCount(TestInstanceName));

            CauseExceptions(target);

            Assert.IsTrue(GetExceptionsPerSecondCount(TestInstanceName) > 0);
        }

        [TestMethod]
        public void AssembledProperlyPerfCounterHandler()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();

            PolicyData policyData = new PolicyData("policy");
            PerformanceCounterCallHandlerData data = new PerformanceCounterCallHandlerData("FooCallHandler", 2);
            policyData.MatchingRules.Add(new CustomMatchingRuleData("match everything", typeof(AlwaysMatchingRule)));
            policyData.Handlers.Add(data);
            settings.Policies.Add(policyData);

            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();
            dictConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            IUnityContainer container = new UnityContainer().AddNewExtension<Interception>();
            settings.ConfigureContainer(container);

            RuleDrivenPolicy policy = container.Resolve<RuleDrivenPolicy>("policy");

            ICallHandler handler
                = (policy.GetHandlersFor(GetMethodImpl(MethodBase.GetCurrentMethod()), container)).ElementAt(0);

            Assert.IsNotNull(handler);
            Assert.AreEqual(handler.Order, data.Order);
        }

        [TestMethod]
        public void CreatePerfCounterHandlerFromAttributes()
        {
            MethodInfo method = typeof(MonitorTarget).GetMethod("DoSomethingElse");
            object[] attributes = method.GetCustomAttributes(typeof(PerformanceCounterCallHandlerAttribute), false);

            Assert.AreEqual(1, attributes.Length);

            PerformanceCounterCallHandlerAttribute attr = attributes[0] as PerformanceCounterCallHandlerAttribute;
            ICallHandler handler = attr.CreateHandler(null);

            Assert.IsNotNull(handler);
            Assert.AreEqual(3, handler.Order);
        }

        void CauseExceptions(MonitorTarget target)
        {
            int trigger = 0;
            for (int i = 0; i < 100; ++i)
            {
                try
                {
                    target.ThrowOnZero(trigger);
                }
                catch (ApplicationException) { }
                trigger = (trigger + 1) % 5;
            }
        }

        private IUnityContainer GetConfiguredContainer()
        {
            callHandler = new PerformanceCounterCallHandler(TestCategoryName, TestInstanceName);

            IUnityContainer container = new UnityContainer();
            container.AddNewExtension<Interception>();
            container.Configure<Interception>()
                .AddPolicy("Monitor all methods")
                    .AddMatchingRule(new TypeMatchingRule(typeof(MonitorTarget)))
                    .AddCallHandler(callHandler).Interception
                .SetDefaultInterceptorFor<MonitorTarget>(new TransparentProxyInterceptor());

            return container;
        }

        private static MethodImplementationInfo GetMethodImpl(MethodBase method)
        {
            return new MethodImplementationInfo(null, (MethodInfo) method);
        }

        #region Performance Counter Helper methods

        PerformanceCounter GetNumberOfCallsCounter(string instanceName,
                                                   bool readOnly)
        {
            return GetCounter(PerformanceCounterCallHandler.NumberOfCallsCounterName, instanceName, readOnly);
        }

        PerformanceCounter GetCounter(string counterName,
                                      string instanceName,
                                      bool readOnly)
        {
            try
            {
                return new PerformanceCounter(
                    TestCategoryName,
                    counterName,
                    instanceName,
                    readOnly);
            }
            catch (InvalidOperationException)
            {
                Assert.Inconclusive("In order to run the test, please run RegAssemblies.bat script first as an Administrator.");
                return null;
            }
        }

        PerformanceCounter GetCallsPerSecondCounter(string instanceName,
                                                    bool readOnly)
        {
            return GetCounter(PerformanceCounterCallHandler.CallsPerSecondCounterName, instanceName, readOnly);
        }

        PerformanceCounter GetNumberOfExceptionsCounter(string instanceName,
                                                        bool readOnly)
        {
            return GetCounter(PerformanceCounterCallHandler.TotalExceptionsCounterName, instanceName, readOnly);
        }

        PerformanceCounter GetExceptionsPerSecondCounter(string instanceName,
                                                         bool readOnly)
        {
            return GetCounter(
                PerformanceCounterCallHandler.ExceptionsPerSecondCounterName,
                instanceName,
                readOnly);
        }

        PerformanceCounter GetAverageCallDurationCounter(string instanceName,
                                                         bool readOnly)
        {
            return GetCounter(
                PerformanceCounterCallHandler.AverageCallDurationCounterName,
                instanceName,
                readOnly);
        }

        PerformanceCounter GetAverageCallDurationBaseCounter(string instanceName,
                                                             bool readOnly)
        {
            return GetCounter(
                PerformanceCounterCallHandler.AverageCallDurationBaseCounterName,
                instanceName,
                readOnly);
        }

        PerformanceCounter GetTotalNumberOfCallsCounter(bool readOnly)
        {
            return
                GetNumberOfCallsCounter(
                    PerformanceCounterCallHandler.TotalInstanceName, readOnly);
        }

        void ResetCounters()
        {
            ResetCounters(PerformanceCounterCallHandler.TotalInstanceName);
            ResetCounters(TestInstanceName);
        }

        void ResetCounters(string instanceName)
        {
            GetNumberOfCallsCounter(instanceName, false).RawValue = 0;
            GetCallsPerSecondCounter(instanceName, false).RawValue = 0;
            GetNumberOfExceptionsCounter(instanceName, false).RawValue = 0;
            GetExceptionsPerSecondCounter(instanceName, false).RawValue = 0;
            GetAverageCallDurationCounter(instanceName, false).RawValue = 0;
            GetAverageCallDurationBaseCounter(instanceName, false).RawValue = 0;
        }

        long GetTotalCallCount()
        {
            return GetTotalNumberOfCallsCounter(true).RawValue;
        }

        long GetNumberOfCallsCount(string instanceName)
        {
            return GetNumberOfCallsCounter(instanceName, true).RawValue;
        }

        long GetCallsPerSecondCount(string instanceName)
        {
            return GetCallsPerSecondCounter(instanceName, true).RawValue;
        }

        long GetNumberOfExceptionsCount(string instanceName)
        {
            return GetNumberOfExceptionsCounter(instanceName, true).RawValue;
        }

        long GetExceptionsPerSecondCount(string instanceName)
        {
            return GetExceptionsPerSecondCounter(instanceName, true).RawValue;
        }

        long GetAverageCallDurationCount(string instanceName)
        {
            return GetAverageCallDurationCounter(instanceName, true).RawValue;
        }

        #endregion
    }

    public class MonitorTarget : MarshalByRefObject
    {
        public void DoSomething()
        {
            // Doesn't actually do anything
        }

        [PerformanceCounterCallHandler("category", "fooName", Order = 3)]
        public void DoSomethingElse() { }

        public void ThrowOnZero(int i)
        {
            if (i == 0)
            {
                throw new ApplicationException("time to throw!");
            }
        }
    }
}

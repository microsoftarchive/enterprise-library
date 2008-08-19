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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.MatchingRules;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.RemotingInterception;
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
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetTotalCallCount());
            target.DoSomething();
            Assert.AreEqual(1L, GetTotalCallCount());
        }

        [TestMethod]
        public void ShouldRecordOneCallInstance()
        {
            ResetCounters();
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetNumberOfCallsCount(TestInstanceName));
            target.DoSomething();
            Assert.AreEqual(1L, GetNumberOfCallsCount(TestInstanceName));
        }

        [TestMethod]
        public void ShouldBeAbleToDisableTotalCount()
        {
            ResetCounters();
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            callHandler.UseTotalCounter = false;
            MonitorTarget target = factory.Create<MonitorTarget>();

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
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            callHandler.IncrementNumberOfCalls = false;
            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetTotalCallCount());
            Assert.AreEqual(0L, GetNumberOfCallsCount(TestInstanceName));
            target.DoSomething();
            Assert.AreEqual(0L, GetNumberOfCallsCount(TestInstanceName));
            Assert.AreEqual(0L, GetTotalCallCount());
        }

        [Ignore]
        [TestMethod]
        public void ShouldReplaceTokensInInstanceName()
        {
            string doSomethingInstanceName = "Method DoSomething";
            string doSomethingElseInstanceName = "Method DoSomethingElse";

            ResetCounters(PerformanceCounterCallHandler.TotalInstanceName);
            ResetCounters(doSomethingInstanceName);
            ResetCounters(doSomethingElseInstanceName);

            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            callHandler.InstanceName = "Method {method}";
            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetTotalCallCount());
            Assert.AreEqual(0L, GetNumberOfCallsCount(doSomethingInstanceName));
            Assert.AreEqual(0L, GetNumberOfCallsCount(doSomethingElseInstanceName));

            target.DoSomething();
            target.DoSomethingElse();
            target.DoSomething();
            target.DoSomething();
            target.DoSomethingElse();
            target.DoSomething();

            Assert.AreEqual(6L, GetTotalCallCount());
            Assert.AreEqual(4L, GetNumberOfCallsCount(doSomethingInstanceName));
            Assert.AreEqual(2L, GetNumberOfCallsCount(doSomethingElseInstanceName));
        }

        [TestMethod]
        public void ShouldUpdateCallsPerSecond()
        {
            ResetCounters();
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            MonitorTarget target = factory.Create<MonitorTarget>();

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
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            callHandler.IncrementCallsPerSecond = false;
            MonitorTarget target = factory.Create<MonitorTarget>();

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
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            callHandler.IncrementTotalExceptions = true;
            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetNumberOfExceptionsCount(TestInstanceName));

            CauseExceptions(target);

            Assert.AreEqual(20L, GetNumberOfExceptionsCount(TestInstanceName));
        }

        [TestMethod]
        public void ShouldDefaultToDisableExceptionCounter()
        {
            ResetCounters();
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetNumberOfExceptionsCount(TestInstanceName));

            CauseExceptions(target);

            Assert.AreEqual(0L, GetNumberOfExceptionsCount(TestInstanceName));
        }

        [TestMethod]
        public void ShouldNotUpdateExceptionsPerSecondByDefault()
        {
            ResetCounters();
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetExceptionsPerSecondCount(TestInstanceName));

            CauseExceptions(target);

            Assert.AreEqual(0L, GetExceptionsPerSecondCount(TestInstanceName));
        }

        [TestMethod]
        public void ShouldUpdateExceptionsPerSecond()
        {
            ResetCounters();
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            callHandler.IncrementExceptionsPerSecond = true;

            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetExceptionsPerSecondCount(TestInstanceName));

            CauseExceptions(target);

            Assert.IsTrue(GetExceptionsPerSecondCount(TestInstanceName) > 0);
        }

        [Ignore]
        [TestMethod]
        public void ShouldUpdateAverageCallsPerSecond()
        {
            ResetCounters();
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());

            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetAverageCallDurationCount(TestInstanceName));

            for (int i = 0; i < 500; ++i)
            {
                target.DoSomethingElse();
            }

            // Again, we just want to see change, actual value is too unpredictable
            Assert.IsTrue(GetAverageCallDurationCount(TestInstanceName) > 0);
        }

        [Ignore]
        [TestMethod]
        public void ShouldBeAbleToDisableAverageCallsPerSecond()
        {
            ResetCounters();
            RemotingPolicyInjector factory = new RemotingPolicyInjector(GetPerfMonPolicies());
            callHandler.IncrementAverageCallDuration = false;

            MonitorTarget target = factory.Create<MonitorTarget>();

            Assert.AreEqual(0L, GetAverageCallDurationCount(TestInstanceName));

            for (int i = 0; i < 500; ++i)
            {
                target.DoSomethingElse();
            }

            Assert.AreEqual(0L, GetAverageCallDurationCount(TestInstanceName));
        }

        [TestMethod]
        public void AssembledProperlyPerfCounterHandler()
        {
            PolicyInjectionSettings settings = new PolicyInjectionSettings();

            PolicyData policyData = new PolicyData("policy");
            PerformanceCounterCallHandlerData data = new PerformanceCounterCallHandlerData("FooCallHandler", 2);
            policyData.Handlers.Add(data);
            settings.Policies.Add(policyData);

            DictionaryConfigurationSource dictConfigurationSource = new DictionaryConfigurationSource();
            dictConfigurationSource.Add(PolicyInjectionSettings.SectionName, settings);

            ICallHandler handler = CallHandlerCustomFactory.Instance.Create(null, data, dictConfigurationSource, null);
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
            ICallHandler handler = attr.CreateHandler();

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
                catch (ApplicationException) {}
                trigger = (trigger + 1) % 5;
            }
        }

        PolicySet GetPerfMonPolicies()
        {
            RuleDrivenPolicy policy = new RuleDrivenPolicy("Monitor all methods");
            policy.RuleSet.Add(new TypeMatchingRule(typeof(MonitorTarget)));
            callHandler = new PerformanceCounterCallHandler(TestCategoryName, TestInstanceName);
            policy.Handlers.Add(callHandler);
            return new PolicySet(policy);
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
            return new PerformanceCounter(
                TestCategoryName,
                counterName,
                instanceName,
                readOnly);
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

        [PerformanceCounterCallHandler("category", "fooName", Order=3)]
        public void DoSomethingElse() {}

        public void ThrowOnZero(int i)
        {
            if (i == 0)
            {
                throw new ApplicationException("time to throw!");
            }
        }
    }
}
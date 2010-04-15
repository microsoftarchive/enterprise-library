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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// An <see cref="ICallHandler"/> that updates performance counters when calling the target.
    /// </summary>
    [ConfigurationElementType(typeof(PerformanceCounterCallHandlerData))]
    public class PerformanceCounterCallHandler : ICallHandler
    {
        /// <summary>
        /// Instance name for the "total" instance
        /// </summary>
        public const string TotalInstanceName = "Total";
        /// <summary>
        /// Number of calls counter name
        /// </summary>
        public const string NumberOfCallsCounterName = "# of calls";
        /// <summary>
        /// Calls per second counter name
        /// </summary>
        public const string CallsPerSecondCounterName = "# calls / second";
        /// <summary>
        /// Average call duration counter name
        /// </summary>
        public const string AverageCallDurationCounterName = "average seconds / call";
        /// <summary>
        /// Average call duration base counter name
        /// </summary>
        public const string AverageCallDurationBaseCounterName = "average seconds / call base";
        /// <summary>
        /// Total exceptions counter name
        /// </summary>
        public const string TotalExceptionsCounterName = "# of exceptions";
        /// <summary>
        /// Exceptions per second counter name
        /// </summary>
        public const string ExceptionsPerSecondCounterName = "# exceptions / second";

        private EnterpriseLibraryPerformanceCounterFactory counterFactory;
        private string category;
        private string instanceName;
        private bool useTotalCounter;
        private bool incrementNumberOfCalls;
        private bool incrementCallsPerSecond;
        private bool incrementAverageCallDuration;
        private bool incrementTotalExceptions;
        private bool incrementExceptionsPerSecond;
        private int order = 0;

        /// <summary>
        /// Creates a <see cref="PerformanceCounterCallHandler"/> using the given category
        /// and instance name.
        /// </summary>
        /// <remarks>See the <see cref="PerformanceCounterCallHandlerDefaults"/> for a list
        /// of the default values for each property.</remarks>
        /// <param name="category">Performance counter category to update. This counter category
        /// must be installed separately or the handler will fail.</param>
        /// <param name="counterInstanceName">Counter instance name. This may include replacement
        /// tokens. See the <see cref="MethodInvocationFormatter"/> class for a list of the tokens.</param>
        public PerformanceCounterCallHandler(string category, string counterInstanceName)
        {
            this.category = category;
            this.instanceName = counterInstanceName;
            this.useTotalCounter = PerformanceCounterCallHandlerDefaults.UseTotalCounter;
            this.incrementNumberOfCalls = PerformanceCounterCallHandlerDefaults.IncrementNumberOfCalls;
            this.incrementCallsPerSecond = PerformanceCounterCallHandlerDefaults.IncrementCallsPerSecond;
            this.incrementAverageCallDuration =
                PerformanceCounterCallHandlerDefaults.IncrementAverageCallDuration;
            this.incrementTotalExceptions = PerformanceCounterCallHandlerDefaults.IncrementTotalExceptions;
            this.incrementExceptionsPerSecond =
                PerformanceCounterCallHandlerDefaults.IncrementExceptionsPerSecond;
            counterFactory = new EnterpriseLibraryPerformanceCounterFactory();
        }

        /// <summary>
        /// Creates a <see cref="PerformanceCounterCallHandler"/> using the given settings.
        /// </summary>
        /// <param name="category">Performance counter category to update. This counter category
        /// must be installed separately or the handler will fail.</param>
        /// <param name="instanceName">Counter instance name. This may include replacement
        /// tokens. See the <see cref="MethodInvocationFormatter"/> class for a list of the tokens.</param>
        /// <param name="useTotalCounter">Should a "Total" instance be updated?</param>
        /// <param name="incrementNumberOfCalls">Should the number of calls counter be updated?</param>
        /// <param name="incrementCallsPerSecond">Should the "calls / second" counter be updated?</param>
        /// <param name="incrementAverageCallDuration">Should the "average seconds / call" counter be updated?</param>
        /// <param name="incrementTotalExceptions">Should the "# of exceptions" counter be updated?</param>
        /// <param name="incrementExceptionsPerSecond">Should the "# exceptions / second" counter be updated?</param>
        public PerformanceCounterCallHandler(
            string category,
            string instanceName,
            bool useTotalCounter,
            bool incrementNumberOfCalls,
            bool incrementCallsPerSecond,
            bool incrementAverageCallDuration,
            bool incrementTotalExceptions,
            bool incrementExceptionsPerSecond)
        {
            this.category = category;
            this.instanceName = instanceName;
            this.useTotalCounter = useTotalCounter;
            this.incrementNumberOfCalls = incrementNumberOfCalls;
            this.incrementCallsPerSecond = incrementCallsPerSecond;
            this.incrementAverageCallDuration = incrementAverageCallDuration;
            this.incrementTotalExceptions = incrementTotalExceptions;
            this.incrementExceptionsPerSecond = incrementExceptionsPerSecond;
            counterFactory = new EnterpriseLibraryPerformanceCounterFactory();
        }

        /// <summary>
        /// Creates a <see cref="PerformanceCounterCallHandler"/> using the given settings.
        /// </summary>
        /// <param name="category">Performance counter category to update. This counter category
        /// must be installed separately or the handler will fail.</param>
        /// <param name="instanceName">Counter instance name. This may include replacement
        /// tokens. See the <see cref="MethodInvocationFormatter"/> class for a list of the tokens.</param>
        /// <param name="useTotalCounter">Should a "Total" instance be updated?</param>
        /// <param name="incrementNumberOfCalls">Should the number of calls counter be updated?</param>
        /// <param name="incrementCallsPerSecond">Should the "calls / second" counter be updated?</param>
        /// <param name="incrementAverageCallDuration">Should the "average seconds / call" counter be updated?</param>
        /// <param name="incrementTotalExceptions">Should the "# of exceptions" counter be updated?</param>
        /// <param name="incrementExceptionsPerSecond">Should the "# exceptions / second" counter be updated?</param>
        /// <param name="handlerOrder">Order of the handler.</param>
        public PerformanceCounterCallHandler(
            string category,
            string instanceName,
            bool useTotalCounter,
            bool incrementNumberOfCalls,
            bool incrementCallsPerSecond,
            bool incrementAverageCallDuration,
            bool incrementTotalExceptions,
            bool incrementExceptionsPerSecond,
            int handlerOrder)
        {
            this.category = category;
            this.instanceName = instanceName;
            this.useTotalCounter = useTotalCounter;
            this.incrementNumberOfCalls = incrementNumberOfCalls;
            this.incrementCallsPerSecond = incrementCallsPerSecond;
            this.incrementAverageCallDuration = incrementAverageCallDuration;
            this.incrementTotalExceptions = incrementTotalExceptions;
            this.incrementExceptionsPerSecond = incrementExceptionsPerSecond;
            counterFactory = new EnterpriseLibraryPerformanceCounterFactory();
            this.order = handlerOrder;
        }

        /// <summary>
        /// Should a "Total" instance be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool UseTotalCounter
        {
            get { return useTotalCounter; }
            set { useTotalCounter = value; }
        }

        /// <summary>
        /// Should the number of calls counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementNumberOfCalls
        {
            get { return incrementNumberOfCalls; }
            set { incrementNumberOfCalls = value; }
        }

        /// <summary>
        /// Performance counter category to update.
        /// </summary>
        /// <value>category name</value>
        public string Category
        {
            get { return category; }
            set { category = value; }
        }

        /// <summary>
        /// Counter instance name. This may include replacement
        /// tokens. See the <see cref="MethodInvocationFormatter"/> class for a list of the tokens.
        /// </summary>
        /// <value>instance name.</value>
        public string InstanceName
        {
            get { return instanceName; }
            set { instanceName = value; }
        }

        /// <summary>
        /// Should the "calls / second" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementCallsPerSecond
        {
            get { return incrementCallsPerSecond; }
            set { incrementCallsPerSecond = value; }
        }

        /// <summary>
        /// Should the "average seconds / call" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementAverageCallDuration
        {
            get { return incrementAverageCallDuration; }
            set { incrementAverageCallDuration = value; }
        }

        /// <summary>
        /// Should the "# of exceptions" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementTotalExceptions
        {
            get { return incrementTotalExceptions; }
            set { incrementTotalExceptions = value; }
        }

        /// <summary>
        /// Should the "# exceptions / second" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementExceptionsPerSecond
        {
            get { return incrementExceptionsPerSecond; }
            set { incrementExceptionsPerSecond = value; }
        }

        #region ICallHandler Members
        /// <summary>
        /// Gets or sets the order in which the handler will be executed
        /// </summary>
        public int Order
        {
            get
            {
                return order;
            }
            set
            {
                order = value;
            }
        }

        /// <summary>
        /// Executes the handler. Increments the various counter according to configuration.
        /// </summary>
        /// <param name="input"><see cref="IMethodInvocation"/> describing the current call.</param>
        /// <param name="getNext">delegate to call to get the next handler in the pipeline.</param>
        /// <returns>Return value from target method.</returns>
        public IMethodReturn Invoke(IMethodInvocation input, GetNextHandlerDelegate getNext)
        {
            if (getNext == null) throw new ArgumentNullException("getNext");

            string[] instanceNames = GetInstanceNames(input);

            IncrementItemCounter(incrementNumberOfCalls, NumberOfCallsCounterName, instanceNames);
            IncrementItemCounter(incrementCallsPerSecond, CallsPerSecondCounterName, instanceNames);

            long start = Stopwatch.GetTimestamp();

            IMethodReturn result = getNext()(input, getNext);

            long end = Stopwatch.GetTimestamp();

            IncrementAverageCounter(incrementAverageCallDuration,
                end - start,
                AverageCallDurationCounterName,
                AverageCallDurationBaseCounterName,
                instanceNames);

            if (result.Exception != null)
            {
                IncrementItemCounter(incrementTotalExceptions, TotalExceptionsCounterName, instanceNames);
                IncrementItemCounter(incrementExceptionsPerSecond, ExceptionsPerSecondCounterName,
                    instanceNames);
            }

            return result;
        }

        private void IncrementItemCounter(bool shouldIncrement, string counterName, string[] instances)
        {
            if (shouldIncrement)
            {
                EnterpriseLibraryPerformanceCounter counter = counterFactory.CreateCounter(
                    category, counterName, instances);
                counter.Increment();
            }
        }

        private void IncrementAverageCounter(bool shouldIncrement, long amount,
            string averageCounterName, string baseCounterName, string[] instances)
        {
            if (shouldIncrement)
            {
                EnterpriseLibraryPerformanceCounter counter = counterFactory.CreateCounter(
                    category, averageCounterName, instances);
                EnterpriseLibraryPerformanceCounter baseCounter = counterFactory.CreateCounter(
                    category, baseCounterName, instances);

                counter.IncrementBy(amount);
                baseCounter.Increment();
            }
        }

        private string[] GetInstanceNames(IMethodInvocation input)
        {
            string formattedInstanceName = new MethodInvocationFormatter(input).Format(instanceName);

            if (useTotalCounter)
            {
                return new string[] { TotalInstanceName, formattedInstanceName };
            }
            return new string[] { formattedInstanceName };
        }

        #endregion
    }

    /// <summary>
    /// Defaults for the <see cref="PerformanceCounterCallHandlerDefaults"/> class.
    /// </summary>
    /// <remarks>The default values are:
    /// <list>
    /// <item><term>UseTotalCounter</term><description>true</description></item>
    /// <item><term>IncrementNumberOfCalls</term><description>true</description>></item>
    /// <item><term>IncrementCallsPerSecond</term><description>true</description></item>
    /// <item><term>IncrementAverageCallDuration</term><description>true</description></item>
    /// <item><term>IncrementTotalExceptions</term><description>false</description></item>
    /// <item><term>IncrementExceptionsPerSecond</term><description>false</description></item>
    /// </list>
    /// </remarks>
    public static class PerformanceCounterCallHandlerDefaults
    {
        /// <summary>
        /// Use total counter = true
        /// </summary>
        public const bool UseTotalCounter = true;
        /// <summary>
        /// Increment number of calls counter = true
        /// </summary>
        public const bool IncrementNumberOfCalls = true;
        /// <summary>
        /// Increment calls per second counter = true
        /// </summary>
        public const bool IncrementCallsPerSecond = true;
        /// <summary>
        /// Increment seconds / call counter = true
        /// </summary>
        public const bool IncrementAverageCallDuration = true;
        /// <summary>
        /// Increment total number of exceptions counter = false
        /// </summary>
        public const bool IncrementTotalExceptions = false;
        /// <summary>
        /// Increment exceptions per second counter = false
        /// </summary>
        public const bool IncrementExceptionsPerSecond = false;
    }
}

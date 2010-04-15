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
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers
{
    /// <summary>
    /// An attribute that applies the <see cref="PerformanceCounterCallHandler"/> to the target.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Property | AttributeTargets.Method)]
    public class PerformanceCounterCallHandlerAttribute : HandlerAttribute
    {
        private string categoryName;
        private string instanceName;
        private bool incrementAverageCallDuration;
        private bool incrementCallsPerSecond;
        private bool incrementExceptionsPerSecond;
        private bool incrementNumberOfCalls;
        private bool incrementTotalExceptions;
        private bool useTotalCounter;

        /// <summary>
        /// Creates a new <see cref="PerformanceCounterCallHandlerAttribute"/> with the given 
        /// category and instance names. All other properties start at the default values.
        /// </summary>
        /// <remarks>See the <see cref="PerformanceCounterCallHandlerDefaults"/> class for
        /// the default values.</remarks>
        /// <param name="category">Performance counter category name.</param>
        /// <param name="instanceName">Performance counter instance name. This may contain substitution
        /// tokens; see <see cref="MethodInvocationFormatter"/> for the list of tokens.</param>
        public PerformanceCounterCallHandlerAttribute(string category, string instanceName)
        {
            this.categoryName = category;
            this.instanceName = instanceName;
            this.incrementAverageCallDuration =
                PerformanceCounterCallHandlerDefaults.IncrementAverageCallDuration;
            this.incrementCallsPerSecond =
                PerformanceCounterCallHandlerDefaults.IncrementCallsPerSecond;
            this.incrementExceptionsPerSecond =
                PerformanceCounterCallHandlerDefaults.IncrementExceptionsPerSecond;
            this.IncrementNumberOfCalls =
                PerformanceCounterCallHandlerDefaults.IncrementNumberOfCalls;
            this.incrementTotalExceptions =
                PerformanceCounterCallHandlerDefaults.IncrementTotalExceptions;
            this.useTotalCounter =
                PerformanceCounterCallHandlerDefaults.UseTotalCounter;
        }

        /// <summary>
        /// Performance counter category to update.
        /// </summary>
        /// <value>category name</value>
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
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
        /// Should the "average seconds / call" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementAverageCallDuration
        {
            get { return incrementAverageCallDuration; }
            set { incrementAverageCallDuration = value; }
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
        /// Should the "# exceptions / second" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementExceptionsPerSecond
        {
            get { return incrementExceptionsPerSecond; }
            set { incrementExceptionsPerSecond = value; }
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
        /// Should the "# of exceptions" counter be updated?
        /// </summary>
        /// <value>true or false</value>
        public bool IncrementTotalExceptions
        {
            get { return incrementTotalExceptions; }
            set { incrementTotalExceptions = value; }
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
        /// Derived classes implement this method. When called, it
        /// creates a new call handler as specified in the attribute
        /// configuration.
        /// </summary>
        /// <returns>A new call handler object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1725:ParameterNamesShouldMatchBaseDeclaration", MessageId = "0#", Justification = "Parameter is ignored")]
        public override ICallHandler CreateHandler(IUnityContainer ignored)
        {
            return new PerformanceCounterCallHandler(
                categoryName,
                instanceName,
                useTotalCounter,
                incrementNumberOfCalls,
                incrementCallsPerSecond,
                incrementAverageCallDuration,
                incrementTotalExceptions,
                incrementExceptionsPerSecond,
                Order);
        }
    }
}

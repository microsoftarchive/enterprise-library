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

using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// A <see cref="ConfigurationNode"/> that handles the configuration
    /// for the <see cref="Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.PerformanceCounterCallHandler"/>
    /// call handler.
    /// </summary>
    public class PerformanceCounterCallHandlerNode : CallHandlerNode
    {
        string categoryName;
        bool incrementAverageCallDuration;
        bool incrementCallsPerSecond;
        bool incrementExceptionsPerSecond;
        bool incrementNumberOfCalls;
        bool incrementTotalExceptions;
        string instanceName;
        bool useTotalCounter;

        /// <summary>
        /// Creates a new <see cref="PerformanceCounterCallHandlerNode"/>
        /// with empty configuration data.
        /// </summary>
        public PerformanceCounterCallHandlerNode()
            : this(new PerformanceCounterCallHandlerData(Resources.PerformanceCounterCallHandlerNodeName)) {}

        /// <summary>
        /// Create a new <see cref="PerformanceCounterCallHandlerNode"/>
        /// initialized from the configuration data given in <paramref name="callHandlerData"/>.
        /// </summary>
        /// <param name="callHandlerData">Source of configuration information.</param>
        public PerformanceCounterCallHandlerNode(PerformanceCounterCallHandlerData callHandlerData)
            : base(callHandlerData)
        {
            categoryName = callHandlerData.CategoryName;
            instanceName = callHandlerData.InstanceName;
            useTotalCounter = callHandlerData.UseTotalCounter;
            incrementNumberOfCalls = callHandlerData.IncrementNumberOfCalls;
            incrementAverageCallDuration = callHandlerData.IncrementAverageCallDuration;
            incrementTotalExceptions = callHandlerData.IncrementTotalExceptions;
            incrementExceptionsPerSecond = callHandlerData.IncrementExceptionsPerSecond;
            incrementCallsPerSecond = callHandlerData.IncrementCallsPerSecond;
            useTotalCounter = callHandlerData.UseTotalCounter;
        }

        /// <summary>
        /// Performance counter category to update.
        /// </summary>
        /// <value>Performance counter category to update.</value>
        [SRDescription("CounterCategoryNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }

        /// <summary>
        /// Update the "average call duration" counter.
        /// </summary>
        /// <value>true means update, false means don't.</value>
        [SRDescription("IncrementAverageCallDurationDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool IncrementAverageCallDuration
        {
            get { return incrementAverageCallDuration; }
            set { incrementAverageCallDuration = value; }
        }

        /// <summary>
        /// Update the "calls per second" counter.
        /// </summary>
        /// <value>true means update, false means don't.</value>
        [SRDescription("IncrementCallsPerSecondDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool IncrementCallsPerSecond
        {
            get { return incrementCallsPerSecond; }
            set { incrementCallsPerSecond = value; }
        }

        /// <summary>
        /// Update the "exceptions / second" counter.
        /// </summary>
        /// <value>true means update, false means don't.</value>
        [SRDescription("IncrementExceptionsPerSecondDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool IncrementExceptionsPerSecond
        {
            get { return incrementExceptionsPerSecond; }
            set { incrementExceptionsPerSecond = value; }
        }

        /// <summary>
        /// Update the "Number Of Calls" counter.
        /// </summary>
        /// <value>true means update, false means don't.</value>
        [SRDescription("IncrementNumberOfCallsDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool IncrementNumberOfCalls
        {
            get { return incrementNumberOfCalls; }
            set { incrementNumberOfCalls = value; }
        }

        /// <summary>
        /// Update the "total # of exceptions" counter.
        /// </summary>
        /// <value>true means update, false means don't.</value>
        [SRDescription("IncrementTotalExceptionsDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool IncrementTotalExceptions
        {
            get { return incrementTotalExceptions; }
            set { incrementTotalExceptions = value; }
        }

        /// <summary>
        /// Performance counter instance name to update.
        /// </summary>
        /// <value>
        /// Performance counter instance name to update.
        /// </value>
        [SRDescription("InstanceNameDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public string InstanceName
        {
            get { return instanceName; }
            set { instanceName = value; }
        }

        /// <summary>
        /// Update "Total" instance counters as well as regular instances?
        /// </summary>
        /// <value>true means update the total, false means don't.</value>
        [SRDescription("UseTotalCounterDescription", typeof(Resources))]
        [SRCategory("CategoryGeneral", typeof(Resources))]
        public bool UseTotalCounter
        {
            get { return useTotalCounter; }
            set { useTotalCounter = value; }
        }

        /// <summary>
        /// Convert the data stored into this node into the corresponding
        /// configuration class (<see cref="PerformanceCounterCallHandlerData"/>).
        /// </summary>
        /// <returns>A configuration element to be stores into a configuration source.</returns>
        public override CallHandlerData CreateCallHandlerData()
        {
            PerformanceCounterCallHandlerData callHandlerData = new PerformanceCounterCallHandlerData(Name);
            callHandlerData.CategoryName = categoryName;
            callHandlerData.InstanceName = instanceName;
            callHandlerData.UseTotalCounter = useTotalCounter;
            callHandlerData.IncrementAverageCallDuration = incrementAverageCallDuration;
            callHandlerData.IncrementCallsPerSecond = incrementCallsPerSecond;
            callHandlerData.IncrementExceptionsPerSecond = incrementExceptionsPerSecond;
            callHandlerData.IncrementTotalExceptions = incrementTotalExceptions;
            callHandlerData.IncrementNumberOfCalls = incrementNumberOfCalls;

            return callHandlerData;
        }
    }
}

//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation
{
    /// <summary>
    /// Listens for events raised by block classes and reports them to appropriate instrumentation 
    /// services (EventLog, or PeformanceCounters).
    /// 
    /// </summary>
    public abstract class InstrumentationListener
    {
        private const string DefaultCounterName = "Total";

        IPerformanceCounterNameFormatter nameFormatter;
        bool performanceCountersEnabled;
        bool eventLoggingEnabled;

        /// <summary>
        /// Gets and sets the EventLoggingEnabled property
        /// </summary>
        public bool EventLoggingEnabled
        {
            get { return eventLoggingEnabled; }
            protected set { eventLoggingEnabled = value; }
        }

        /// <summary>
        /// Gets and sets the PerformanceCountersEnabled property
        /// </summary>
        public bool PerformanceCountersEnabled
        {
            get { return performanceCountersEnabled; }
            protected set { performanceCountersEnabled = value; }
        }

        /// <summary>
        /// Base constructor for <see cref="InstrumentationListener"></see>. 
        /// </summary>
        /// <overloads>
        /// Base constructor for <see cref="InstrumentationListener"></see>. 
        /// </overloads>
        /// <param name="performanceCountersEnabled">True if performance counter reporting is enabled</param>
        /// <param name="eventLoggingEnabled">True if event logging is enabled</param>
        /// <param name="nameFormatter">Creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        protected InstrumentationListener(bool performanceCountersEnabled,
                                       bool eventLoggingEnabled,
                                       IPerformanceCounterNameFormatter nameFormatter)
        {
            string[] instanceNames = new string[] { DefaultCounterName };
            Initialize(performanceCountersEnabled, eventLoggingEnabled, nameFormatter, instanceNames);
        }

        /// <summary>
        /// Base constructor for <see cref="InstrumentationListener"></see>. 
        /// </summary>
        /// <param name="instanceName">Unique name for this instance</param>
        /// <param name="performanceCountersEnabled">True if performance counter reporting is enabled</param>
        /// <param name="eventLoggingEnabled">True if event logging is enabled</param>
        /// <param name="nameFormatter">Creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        protected InstrumentationListener(string instanceName,
                                       bool performanceCountersEnabled,
                                       bool eventLoggingEnabled,
                                       IPerformanceCounterNameFormatter nameFormatter)
            : this(CreateDefaultInstanceNames(instanceName), performanceCountersEnabled, eventLoggingEnabled, nameFormatter)
        {
        }

        /// <summary>
        /// Base constructor for <see cref="InstrumentationListener"></see>. 
        /// </summary>
        /// <param name="instanceNames">Unique names for th <see cref="PerformanceCounter"></see> instances to be managed by this listener.</param>
        /// <param name="performanceCountersEnabled">True if performance counter reporting is enabled</param>
        /// <param name="eventLoggingEnabled">True if event logging is enabled</param>
        /// <param name="nameFormatter">Creates unique name for each <see cref="PerformanceCounter"/> instance.</param>
        protected InstrumentationListener(string[] instanceNames,
                                       bool performanceCountersEnabled,
                                       bool eventLoggingEnabled,
                                       IPerformanceCounterNameFormatter nameFormatter)
        {
            Initialize(performanceCountersEnabled, eventLoggingEnabled, nameFormatter, instanceNames);
        }

        /// <summary>
        /// Gets the event source name as defined in the class metadata.
        /// </summary>
        /// <returns>The event source name.</returns>
        protected string GetEventSourceName()
        {
            Type ourType = this.GetType();
            object[] attributes = ourType.GetCustomAttributes(typeof(EventLogDefinitionAttribute), false);
            return ((EventLogDefinitionAttribute)attributes[0]).SourceName;
        }

        private static string[] CreateDefaultInstanceNames(string instanceName)
        {
            return new string[] { DefaultCounterName, instanceName };
        }

        private void Initialize(bool performanceCountersEnabled, bool eventLoggingEnabled, IPerformanceCounterNameFormatter nameFormatter, string[] instanceNames)
        {
            this.performanceCountersEnabled = performanceCountersEnabled;
            this.eventLoggingEnabled = eventLoggingEnabled;
            this.nameFormatter = nameFormatter;

            if (performanceCountersEnabled)
            {
                FormatCounterInstanceNames(nameFormatter, instanceNames);
                CreatePerformanceCounters(instanceNames);
            }
        }

        private void FormatCounterInstanceNames(IPerformanceCounterNameFormatter nameFormatter, string[] instanceNames)
        {
            for (int i = 0; i < instanceNames.Length; i++)
            {
                instanceNames[i] = nameFormatter.CreateName(instanceNames[i]);
            }
        }

        /// <summary>
        /// Initializes the performance counter instances managed by this listener.
        /// </summary>
        /// <param name="instanceNames">Instance names for performance counters.</param>
        protected virtual void CreatePerformanceCounters(string[] instanceNames)
        {
        }

        /// <summary>
        /// Creates a unique name for a specific performance counter instance.
        /// </summary>
        /// <param name="nameSuffix">Instance name for a specific performance counter.</param>
        /// <returns>The created instance name.</returns>
        protected string CreateInstanceName(string nameSuffix)
        {
            return nameFormatter.CreateName(nameSuffix);
        }
    }
}

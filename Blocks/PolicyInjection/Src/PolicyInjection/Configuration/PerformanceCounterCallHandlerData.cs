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

using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration
{
    /// <summary>
    /// A configuration element that stores information for the <see cref="PerformanceCounterCallHandler"/>.
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "PerformanceCounterCallHandlerDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "PerformanceCounterCallHandlerDataDisplayName")]
    public class PerformanceCounterCallHandlerData : CallHandlerData
    {
        private const string CategoryNamePropertyName = "categoryName";
        private const string InstanceNamePropertyName = "instanceName";
        private const string UseTotalCounterPropertyName = "useTotalCounter";
        private const string IncrementNumberOfCallsPropertyName = "incrementNumberOfCalls";
        private const string IncrementCallsPerSecondPropertyName = "incrementCallsPerSecond";
        private const string IncrementAverageCallDurationPropertyName = "incrementAverageCallDuration";
        private const string IncrementTotalExceptionsPropertyName = "incrementTotalExceptions";
        private const string IncrementExceptionsPerSecondPropertyName = "incrementExceptionsPerSecond";

        /// <summary>
        /// Construct a new <see cref="PerformanceCounterCallHandlerData"/>.
        /// </summary>
        public PerformanceCounterCallHandlerData()
        {
            Type = typeof(PerformanceCounterCallHandler);
        }

        /// <summary>
        /// Construct a new <see cref="PerformanceCounterCallHandlerData"/>.
        /// </summary>
        /// <param name="instanceName">Name of the handler section.</param>
        public PerformanceCounterCallHandlerData(string instanceName)
            : base(instanceName, typeof(PerformanceCounterCallHandler))
        {
        }

        /// <summary>
        /// Construct a new <see cref="PerformanceCounterCallHandlerData"/>.
        /// </summary>
        /// <param name="instanceName">Name of the handler section.</param>
        /// <param name="handlerOrder">Order of the handler.</param>
        public PerformanceCounterCallHandlerData(string instanceName, int handlerOrder)
            : base(instanceName, typeof(PerformanceCounterCallHandler))
        {
            this.Order = handlerOrder;
        }

        /// <summary>
        /// Performance counter category name.
        /// </summary>
        /// <value>The "categoryName" config attribute.</value>
        [ConfigurationProperty(CategoryNamePropertyName, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "PerformanceCounterCallHandlerDataCategoryNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PerformanceCounterCallHandlerDataCategoryNameDisplayName")]
        public string CategoryName
        {
            get { return (string)base[CategoryNamePropertyName]; }
            set { base[CategoryNamePropertyName] = value; }
        }

        /// <summary>
        /// Performance counter instance name.
        /// </summary>
        /// <remarks>This string may include substitution tokens. See <see cref="MethodInvocationFormatter"/>
        /// for the list of tokens.</remarks>
        /// <value>The "instanceName" config attribute.</value>
        [ConfigurationProperty(InstanceNamePropertyName, IsRequired = true)]
        [ResourceDescription(typeof(DesignResources), "PerformanceCounterCallHandlerDataInstanceNameDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PerformanceCounterCallHandlerDataInstanceNameDisplayName")]
        public string InstanceName
        {
            get { return (string)base[InstanceNamePropertyName]; }
            set { base[InstanceNamePropertyName] = value; }
        }

        /// <summary>
        /// Increment "Total" counter instance.
        /// </summary>
        /// <value>The "useTotalCounter" config attribute.</value>
        [ConfigurationProperty(UseTotalCounterPropertyName,
            IsRequired = false,
            DefaultValue = PerformanceCounterCallHandlerDefaults.UseTotalCounter)]
        [ResourceDescription(typeof(DesignResources), "PerformanceCounterCallHandlerDataUseTotalCounterDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PerformanceCounterCallHandlerDataUseTotalCounterDisplayName")]
        public bool UseTotalCounter
        {
            get { return (bool)base[UseTotalCounterPropertyName]; }
            set { base[UseTotalCounterPropertyName] = value; }
        }

        /// <summary>
        /// Increment the "total # of calls" counter?
        /// </summary>
        /// <value>The "incrementNumberOfCalls" config attribute.</value>
        [ConfigurationProperty(IncrementNumberOfCallsPropertyName,
            IsRequired = false,
            DefaultValue = PerformanceCounterCallHandlerDefaults.IncrementNumberOfCalls)]
        [ResourceDescription(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementNumberOfCallsDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementNumberOfCallsDisplayName")]
        public bool IncrementNumberOfCalls
        {
            get { return (bool)base[IncrementNumberOfCallsPropertyName]; }
            set { base[IncrementNumberOfCallsPropertyName] = value; }
        }

        /// <summary>
        /// Increment the "calls / second" counter?
        /// </summary>
        /// <value>the "incrementCallsPerSecond" config attribute.</value>
        [ConfigurationProperty(IncrementCallsPerSecondPropertyName,
            IsRequired = false,
            DefaultValue = PerformanceCounterCallHandlerDefaults.IncrementCallsPerSecond)]
        [ResourceDescription(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementCallsPerSecondDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementCallsPerSecondDisplayName")]
        public bool IncrementCallsPerSecond
        {
            get { return (bool)base[IncrementCallsPerSecondPropertyName]; }
            set { base[IncrementCallsPerSecondPropertyName] = value; }
        }

        /// <summary>
        /// Increment "average seconds / call" counter?
        /// </summary>
        /// <value>The "incrementAverageCallDuration" config attribute.</value>
        [ConfigurationProperty(IncrementAverageCallDurationPropertyName,
            IsRequired = false,
            DefaultValue = PerformanceCounterCallHandlerDefaults.IncrementAverageCallDuration)]
        [ResourceDescription(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementAverageCallDurationDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementAverageCallDurationDisplayName")]
        public bool IncrementAverageCallDuration
        {
            get { return (bool)base[IncrementAverageCallDurationPropertyName]; }
            set { base[IncrementAverageCallDurationPropertyName] = value; }
        }

        /// <summary>
        /// Increment "total # of exceptions" counter?
        /// </summary>
        /// <value>The "incrementTotalExceptions" config attribute.</value>
        [ConfigurationProperty(IncrementTotalExceptionsPropertyName,
            IsRequired = false,
            DefaultValue = PerformanceCounterCallHandlerDefaults.IncrementTotalExceptions)]
        [ResourceDescription(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementTotalExceptionsDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementTotalExceptionsDisplayName")]
        public bool IncrementTotalExceptions
        {
            get { return (bool)base[IncrementTotalExceptionsPropertyName]; }
            set { base[IncrementTotalExceptionsPropertyName] = value; }
        }

        /// <summary>
        /// Increment the "exceptions / second" counter?
        /// </summary>
        /// <value>The "incrementExceptionsPerSecond" config attribute.</value>
        [ConfigurationProperty(IncrementExceptionsPerSecondPropertyName,
            IsRequired = false,
            DefaultValue = PerformanceCounterCallHandlerDefaults.IncrementExceptionsPerSecond)]
        [ResourceDescription(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementExceptionsPerSecondDescription")]
        [ResourceDisplayName(typeof(DesignResources), "PerformanceCounterCallHandlerDataIncrementExceptionsPerSecondDisplayName")]
        public bool IncrementExceptionsPerSecond
        {
            get { return (bool)base[IncrementExceptionsPerSecondPropertyName]; }
            set { base[IncrementExceptionsPerSecondPropertyName] = value; }
        }

        /// <summary>
        /// Configures an <see cref="IUnityContainer"/> to resolve the represented call handler by using the specified name.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        /// <param name="registrationName">The name of the registration.</param>
        protected override void DoConfigureContainer(IUnityContainer container, string registrationName)
        {
            container.RegisterType<ICallHandler, PerformanceCounterCallHandler>(
                registrationName,
                new InjectionConstructor(
                    this.CategoryName,
                    this.InstanceName,
                    this.UseTotalCounter,
                    this.IncrementNumberOfCalls,
                    this.IncrementCallsPerSecond,
                    this.IncrementAverageCallDuration,
                    this.IncrementTotalExceptions,
                    this.IncrementExceptionsPerSecond),
                new InjectionProperty("Order", this.Order));
        }
    }
}

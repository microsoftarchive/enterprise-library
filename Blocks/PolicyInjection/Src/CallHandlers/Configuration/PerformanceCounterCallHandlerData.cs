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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.InterceptionExtension;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration
{
    /// <summary>
    /// A configuration element that stores information for the <see cref="PerformanceCounterCallHandler"/>.
    /// </summary>
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
            : base()
        {
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
        public bool IncrementExceptionsPerSecond
        {
            get { return (bool)base[IncrementExceptionsPerSecondPropertyName]; }
            set { base[IncrementExceptionsPerSecondPropertyName] = value; }
        }

        /// <summary>
        /// Adds the call handler represented by this configuration object to <paramref name="policy"/>.
        /// </summary>
        /// <param name="policy">The policy to which the rule must be added.</param>
        /// <param name="configurationSource">The configuration source from which additional information
        /// can be retrieved, if necessary.</param>
        public override void ConfigurePolicy(PolicyDefinition policy, IConfigurationSource configurationSource)
        {
            policy.AddCallHandler<PerformanceCounterCallHandler>(
                new ContainerControlledLifetimeManager(),
                new InjectionConstructor(
                    new InjectionParameter<string>(this.CategoryName),
                    new InjectionParameter<string>(this.InstanceName),
                    new InjectionParameter<bool>(this.UseTotalCounter),
                    new InjectionParameter<bool>(this.IncrementNumberOfCalls),
                    new InjectionParameter<bool>(this.IncrementCallsPerSecond),
                    new InjectionParameter<bool>(this.IncrementAverageCallDuration),
                    new InjectionParameter<bool>(this.IncrementTotalExceptions),
                    new InjectionParameter<bool>(this.IncrementExceptionsPerSecond)),
                new InjectionProperty("Order", new InjectionParameter<int>(this.Order)));
        }
    }
}


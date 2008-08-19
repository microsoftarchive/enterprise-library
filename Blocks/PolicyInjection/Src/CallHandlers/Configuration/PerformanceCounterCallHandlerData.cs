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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration;
using Microsoft.Practices.ObjectBuilder2;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration
{
    /// <summary>
    /// A configuration element that stores information for the <see cref="PerformanceCounterCallHandler"/>.
    /// </summary>
    [Assembler(typeof(PerformanceCounterCallHandlerAssembler))]
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
            set { base[CategoryNamePropertyName] =value; }
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
    }

    /// <summary>
    /// Class used by ObjectBuilder to construct a <see cref="PerformanceCounterCallHandler"/>
    /// from a <see cref="PerformanceCounterCallHandlerData"/> instance.
    /// </summary>
    public class PerformanceCounterCallHandlerAssembler : IAssembler<ICallHandler, CallHandlerData>
    {
        /// <summary>
        /// Builds an instance of the subtype of <typeparamref name="TObject"/> type the receiver knows how to build,  based on 
        /// an a configuration object.
        /// </summary>
        /// <param name="context">The <see cref="IBuilderContext"/> that represents the current building process.</param>
        /// <param name="objectConfiguration">The configuration object that describes the object to build.</param>
        /// <param name="configurationSource">The source for configuration objects.</param>
        /// <param name="reflectionCache">The cache to use retrieving reflection information.</param>
        /// <returns>A fully initialized instance of the <typeparamref name="TObject"/> subtype.</returns>
        public ICallHandler Assemble(IBuilderContext context, CallHandlerData objectConfiguration, IConfigurationSource configurationSource, ConfigurationReflectionCache reflectionCache)
        {
            PerformanceCounterCallHandlerData configuration = (PerformanceCounterCallHandlerData)objectConfiguration;

            PerformanceCounterCallHandler callHandler = new PerformanceCounterCallHandler(
                configuration.CategoryName, configuration.InstanceName, configuration.UseTotalCounter,
                configuration.IncrementNumberOfCalls, configuration.IncrementCallsPerSecond,
                configuration.IncrementAverageCallDuration, configuration.IncrementTotalExceptions,
                configuration.IncrementExceptionsPerSecond);

            return callHandler;
        }
    }
}


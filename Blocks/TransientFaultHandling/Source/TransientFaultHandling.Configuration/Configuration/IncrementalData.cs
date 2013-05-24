#region license
// ==============================================================================
// Microsoft patterns & practices Enterprise Library
// Transient Fault Handling Application Block
// ==============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
// ==============================================================================
#endregion

namespace Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling.Configuration
{
    using System;
    using System.ComponentModel;
    using System.Configuration;
    using Common.Configuration.Design;

    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    /// <summary>
    /// <para>Represents the incremental retry strategy.</para>
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "IncrementalDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "IncrementalDataDisplayName")]
    public class IncrementalData : RetryStrategyData
    {
        private const string MaxRetryCountProperty = "maxRetryCount";
        private const string RetryIncrementProperty = "retryIncrement";
        private const string InitialIntervalProperty = "initialInterval";

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalData"/> class. 
        /// </summary>
        public IncrementalData()
            : base(typeof(Incremental))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="IncrementalData"/> class by using the specified name. 
        /// </summary>
        /// <param name="name">The name.</param>
        public IncrementalData(string name)
            : base(name, typeof(Incremental))
        {
        }

        /// <summary>
        /// Gets or sets the maximum number of retry attempts.
        /// </summary>
        [ConfigurationProperty(MaxRetryCountProperty, IsRequired = false, DefaultValue = 10)]
        [ResourceDescription(typeof(DesignResources), "MaxRetryCountDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MaxRetryCountDisplayName")]
        [ViewModel(TransientFaultHandlingDesignTime.ViewModelTypeNames.DefaultElementConfigurationProperty)]
        [IntegerValidator(MinValue = 0)]
        public int MaxRetryCount
        {
            get { return (int)base[MaxRetryCountProperty]; }
            set { base[MaxRetryCountProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the incremental time value for calculating progressive delay between retry attempts.
        /// </summary>
        [ConfigurationProperty(RetryIncrementProperty, IsRequired = false, DefaultValue = "00:00:01")]
        [ResourceDescription(typeof(DesignResources), "RetryIncrementDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RetryIncrementDisplayName")]
        [ViewModel(TransientFaultHandlingDesignTime.ViewModelTypeNames.TimeSpanElementConfigurationProperty)]
        [TimeSpanValidator(MinValueString = "00:00:00")]
        public TimeSpan RetryIncrement
        {
            get { return (TimeSpan)base[RetryIncrementProperty]; }
            set { base[RetryIncrementProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the initial interval.
        /// </summary>
        [ConfigurationProperty(InitialIntervalProperty, IsRequired = false, DefaultValue = "00:00:01")]
        [ResourceDescription(typeof(DesignResources), "IncrementalDataInitialIntervalDescription")]
        [ResourceDisplayName(typeof(DesignResources), "IncrementalDataInitialIntervalDisplayName")]
        [ViewModel(TransientFaultHandlingDesignTime.ViewModelTypeNames.TimeSpanElementConfigurationProperty)]
        [TimeSpanValidator(MinValueString = "00:00:00")]
        public TimeSpan InitialInterval
        {
            get { return (TimeSpan)base[InitialIntervalProperty]; }
            set { base[InitialIntervalProperty] = value; }
        }

        /// <summary>
        /// Override to hide and avoid saving the type property.
        /// </summary>
        [Browsable(false)]
        [ConfigurationProperty("type", IsRequired = false)]
        [ViewModel(CommonDesignTime.ViewModelTypeNames.ConfigurationPropertyViewModel)]
        public override string TypeName
        {
            get { return null; }
            set { this[typeProperty] = null; }
        }

        /// <summary>
        /// Builds the <see cref="RetryStrategy"/> from the configuration settings.
        /// </summary>
        /// <returns>The <see cref="RetryStrategy"/> for retrying when transient errors occur.</returns>
        public override RetryStrategy BuildRetryStrategy()
        {
            return new Incremental(this.Name, this.MaxRetryCount, this.InitialInterval, this.RetryIncrement, this.FirstFastRetry);
        }

        /// <summary>
        /// Writes the outer tags of this configuration element to the configuration file when implemented in a derived class.
        /// </summary>
        /// <param name="writer">The writer that writes to the configuration file.</param>
        /// <param name="elementName">The name of the <see cref="System.Configuration.ConfigurationElement"/> to be written.</param>
        /// <returns>true if writing was successful; otherwise, false.</returns>
        protected override bool SerializeToXmlElement(System.Xml.XmlWriter writer, string elementName)
        {
            return base.SerializeToXmlElement(writer, WellKnownRetryStrategies.Incremental);
        }
    }
}

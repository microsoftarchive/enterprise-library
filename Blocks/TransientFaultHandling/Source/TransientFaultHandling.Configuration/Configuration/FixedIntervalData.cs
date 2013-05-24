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
    /// <para>Represents the fixed interval retry strategy.</para>
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "FixedIntervalDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "FixedIntervalDataDisplayName")]
    public class FixedIntervalData : RetryStrategyData
    {
        private const string MaxRetryCountProperty = "maxRetryCount";
        private const string RetryIntervalProperty = "retryInterval";

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedIntervalData"/> class.
        /// </summary>
        public FixedIntervalData()
            : base(typeof(FixedInterval))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FixedIntervalData"/> class by using the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        public FixedIntervalData(string name)
            : base(name, typeof(FixedInterval))
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
        /// Gets or sets the interval between retry attempts.
        /// </summary>
        [ConfigurationProperty(RetryIntervalProperty, IsRequired = false, DefaultValue = "00:00:01")]
        [ResourceDescription(typeof(DesignResources), "FixedIntervalDataRetryIntervalDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FixedIntervalDataRetryIntervalDisplayName")]
        [ViewModel(TransientFaultHandlingDesignTime.ViewModelTypeNames.TimeSpanElementConfigurationProperty)]
        [TimeSpanValidator(MinValueString = "00:00:00")]
        public TimeSpan RetryInterval
        {
            get { return (TimeSpan)base[RetryIntervalProperty]; }
            set { base[RetryIntervalProperty] = value; }
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
            return new FixedInterval(this.Name, this.MaxRetryCount, this.RetryInterval, this.FirstFastRetry);
        }

        /// <summary>
        /// Writes the outer tags of this configuration element to the configuration file when implemented in a derived class.
        /// </summary>
        /// <param name="writer">The writer that writes to the configuration file.</param>
        /// <param name="elementName">The name of the <see cref="System.Configuration.ConfigurationElement"/> to be written.</param>
        /// <returns>true if writing was successful; otherwise, false.</returns>
        protected override bool SerializeToXmlElement(System.Xml.XmlWriter writer, string elementName)
        {
            return base.SerializeToXmlElement(writer, WellKnownRetryStrategies.FixedInterval);
        }
    }
}

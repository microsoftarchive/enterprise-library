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

    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    /// <summary>
    /// <para>Represents the exponential backoff retry strategy.</para>
    /// </summary>
    [ResourceDescription(typeof(DesignResources), "ExponentialBackoffDataDescription")]
    [ResourceDisplayName(typeof(DesignResources), "ExponentialBackoffDataDisplayName")]
    [ElementValidation(TransientFaultHandlingDesignTime.ValidatorTypes.ExponentialBackoffValidator)]
    public class ExponentialBackoffData : RetryStrategyData
    {
        private const string MaxRetryCountProperty = "maxRetryCount";
        private const string MinBackoffProperty = "minBackoff";
        private const string MaxBackoffProperty = "maxBackoff";
        private const string DeltaBackoffProperty = "deltaBackoff";

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialBackoffData"/> class. 
        /// </summary>
        public ExponentialBackoffData()
            : base(typeof(ExponentialBackoff))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExponentialBackoffData"/> class. 
        /// </summary>
        /// <param name="name">The name.</param>
        public ExponentialBackoffData(string name)
            : base(name, typeof(ExponentialBackoff))
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
        /// Gets or sets the minimum backoff time.
        /// </summary>
        [ConfigurationProperty(MinBackoffProperty, IsRequired = false, DefaultValue = "00:00:01")]
        [ResourceDescription(typeof(DesignResources), "MinBackoffDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MinBackoffDisplayName")]
        [ViewModel(TransientFaultHandlingDesignTime.ViewModelTypeNames.TimeSpanElementConfigurationProperty)]
        [TimeSpanValidator(MinValueString = "00:00:00")]
        public TimeSpan MinBackoff
        {
            get { return (TimeSpan)base[MinBackoffProperty]; }
            set { base[MinBackoffProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the maximum backoff time.
        /// </summary>
        [ConfigurationProperty(MaxBackoffProperty, IsRequired = false, DefaultValue = "00:00:30")]
        [ResourceDescription(typeof(DesignResources), "MaxBackoffDescription")]
        [ResourceDisplayName(typeof(DesignResources), "MaxBackoffDisplayName")]
        [ViewModel(TransientFaultHandlingDesignTime.ViewModelTypeNames.TimeSpanElementConfigurationProperty)]
        [TimeSpanValidator(MinValueString = "00:00:00")]
        public TimeSpan MaxBackoff
        {
            get { return (TimeSpan)base[MaxBackoffProperty]; }
            set { base[MaxBackoffProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the value that will be used to calculate a random delta in the exponential delay between retries.
        /// </summary>
        [ConfigurationProperty(DeltaBackoffProperty, IsRequired = false, DefaultValue = "00:00:10")]
        [ResourceDescription(typeof(DesignResources), "DeltaBackoffDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DeltaBackoffDisplayName")]
        [ViewModel(TransientFaultHandlingDesignTime.ViewModelTypeNames.TimeSpanElementConfigurationProperty)]
        [TimeSpanValidator(MinValueString = "00:00:00")]
        public TimeSpan DeltaBackoff
        {
            get { return (TimeSpan)base[DeltaBackoffProperty]; }
            set { base[DeltaBackoffProperty] = value; }
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
            return new ExponentialBackoff(this.Name, this.MaxRetryCount, this.MinBackoff, this.MaxBackoff, this.DeltaBackoff, this.FirstFastRetry);
        }

        /// <summary>
        /// Writes the outer tags of this configuration element to the configuration file when implemented in a derived class.
        /// </summary>
        /// <param name="writer">The writer that writes to the configuration file.</param>
        /// <param name="elementName">The name of the <see cref="System.Configuration.ConfigurationElement"/> to be written.</param>
        /// <returns>true if writing was successful; otherwise, false.</returns>
        protected override bool SerializeToXmlElement(System.Xml.XmlWriter writer, string elementName)
        {
            return base.SerializeToXmlElement(writer, WellKnownRetryStrategies.Backoff);
        }
    }
}

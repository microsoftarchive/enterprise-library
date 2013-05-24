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
    using System.Configuration;
    using Common.Configuration;
    using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    /// <summary>
    /// <para>Represents the common configuration data for all rule strategies.</para>
    /// </summary>
    public class RetryStrategyData : NameTypeConfigurationElement
    {
        private const string FirstFastRetryProperty = "firstFastRetry";

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryStrategyData"/> class.
        /// </summary>
        public RetryStrategyData()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryStrategyData"/> class with a retry strategy type.
        /// </summary>
        /// <param name="type">The type of <see cref="RetryStrategy"/>.</param>
        public RetryStrategyData(Type type)
            : this(null, type)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RetryStrategyData"/> class with a name and a retry strategy type.
        /// </summary>
        /// <param name="name">The name of the configured <see cref="RetryStrategy"/>.</param>
        /// <param name="type">The type of <see cref="RetryStrategy"/>.</param>
        public RetryStrategyData(string name, Type type)
            : base(name, type)
        {
        }

        /// <summary>
        /// Gets or sets a value that indicates whether the first retry attempt will be made immediately,
        /// whereas subsequent retries will remain subject to the retry interval.
        /// </summary>
        [ConfigurationProperty(FirstFastRetryProperty, IsRequired = false, DefaultValue = true)]
        [ResourceDescription(typeof(DesignResources), "FirstFastRetryDescription")]
        [ResourceDisplayName(typeof(DesignResources), "FirstFastRetryDisplayName")]
        public bool FirstFastRetry
        {
            get { return (bool)base[FirstFastRetryProperty]; }
            set { base[FirstFastRetryProperty] = value; }
        }

        /// <summary>
        /// Builds the <see cref="RetryStrategy"/> from the configuration settings.
        /// </summary>
        /// <returns>The strategy for retrying when transient errors occur.</returns>
        public virtual RetryStrategy BuildRetryStrategy()
        {
            // Cannot make abstract for serialization reasons.
            throw new NotImplementedException("Must be implemented by subclasses.");
        }
    }
}

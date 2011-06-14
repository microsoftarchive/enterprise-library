//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Silverlight Design-Time Configuration
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Caching.Properties;
using Microsoft.Practices.EnterpriseLibrary.Silverlight.Configuration.Validation;

namespace Microsoft.Practices.EnterpriseLibrary.Caching.Configuration
{
    /// <summary>
    /// <para>Configuration data for an <c>InMemoryCache</c>.</para>
    /// </summary>	
    [ResourceDescription(typeof(CachingResources), "InMemoryCacheDataDescription")]
    [ResourceDisplayName(typeof(CachingResources), "InMemoryCacheDataDisplayName")]
    [Browsable(true)]
    public class InMemoryCacheData : CacheData
    {
        private const string maxItemsBeforeScavengingProperty = "maxItemsBeforeScavenging";
        private const string itemsLeftAfterScavengingProperty = "itemsLeftAfterScavenging";
        private const string expirationPollingIntervalProperty = "expirationPollingInterval";

        private const int maxItemsBeforeScavengingDefaultValue = 200;
        private const int itemsLeftAfterScavengingDefaultValue = 80;
        private const string expirationPollingIntervalDefaultValue = "00:02:00";

        /// <summary>
        /// <para>Initializes a new instance of <see cref="InMemoryCacheData"/> class.</para>
        /// </summary>
        public InMemoryCacheData()
            : base(typeof(InMemoryCache))
        {
        }

        /// <summary>
        /// Gets or sets the maximum number of items in cache before an add causes scavenging to take place.
        /// </summary>	
        [ConfigurationProperty(maxItemsBeforeScavengingProperty,
            DefaultValue = maxItemsBeforeScavengingDefaultValue)]
        [ResourceDescription(typeof(CachingResources), "MaxItemsBeforeScavengingDescription")]
        [ResourceDisplayName(typeof(CachingResources), "MaxItemsBeforeScavengingDisplayName")]
        [Validation(typeof(PositiveValidator))]
        public int MaxItemsBeforeScavenging
        {
            get { return (int)this[maxItemsBeforeScavengingProperty]; }
            set { this[maxItemsBeforeScavengingProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the number of items left in the cache after scavenging has taken place.
        /// </summary>	
        [ConfigurationProperty(itemsLeftAfterScavengingProperty, 
            DefaultValue = itemsLeftAfterScavengingDefaultValue)]
        [ResourceDescription(typeof(CachingResources), "ItemsLeftAfterScavengingDescription")]
        [ResourceDisplayName(typeof(CachingResources), "ItemsLeftAfterScavengingDisplayName")]
        [Validation(typeof(PositiveValidator))]
        public int ItemsLeftAfterScavenging
        {
            get { return (int)this[itemsLeftAfterScavengingProperty]; }
            set { this[itemsLeftAfterScavengingProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the frequency of expiration polling cycle.
        /// </summary>
        [ConfigurationProperty(expirationPollingIntervalProperty, 
            DefaultValue = expirationPollingIntervalDefaultValue)]
        [BaseType(typeof(TimeSpan))]
        [ResourceDescription(typeof(CachingResources), "ExpirationPollingIntervalDescription")]
        [ResourceDisplayName(typeof(CachingResources), "ExpirationPollingIntervalDisplayName")]
        public TimeSpan ExpirationPollingInterval
        {
            get { return TimeSpan.Parse(this[expirationPollingIntervalProperty].ToString()); }
            set { this[expirationPollingIntervalProperty] = value.ToString(); }
        }
    }
}

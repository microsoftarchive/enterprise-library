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
    /// <para>Configuration data for an <c>IsolatedStorageCacheData</c>.</para>
    /// </summary>	
    [ResourceDescription(typeof(CachingResources), "IsolatedStorageCacheDataDescription")]
    [ResourceDisplayName(typeof(CachingResources), "IsolatedStorageCacheDataDisplayName")]
    [Browsable(true)]
    public class IsolatedStorageCacheData : CacheData
    {
        private const string maxSizeInKilobytesProperty = "maxSizeInKilobytes";
        private const string percentOfQuotaUsedBeforeScavengingProperty = "percentOfQuotaUsedBeforeScavenging";
        private const string percentOfQuotaUsedAfterScavengingProperty = "percentOfQuotaUsedAfterScavenging";
        private const string expirationPollingIntervalProperty = "expirationPollingInterval";
        private const string serializerTypeNameProperty = "serializerTypeName";

        private const int maxSizeInKilobytesDefaultValue = 1024;
        private const int percentOfQuotaUsedBeforeScavengingDefaultValue = 80;
        private const int percentOfQuotaUsedAfterScavengingDefaultValue = 60;
        private const string expirationPollingIntervalDefaultValue = "00:02:00";
        private const string serializerTypeNameDefaultValue =
            "Microsoft.Practices.EnterpriseLibrary.Caching.IsolatedStorage.IsolatedStorageCacheEntrySerializer, Microsoft.Practices.EnterpriseLibrary.Caching.Silverlight";

        /// <summary>
        /// <para>Initializes a new instance of <see cref="IsolatedStorageCacheData"/> class.</para>
        /// </summary>
        public IsolatedStorageCacheData()
            :base (typeof(IsolatedStorageCache))
        {
        }

        /// <summary>
        /// Gets or sets the maximum size in bytes before replacing previous cached items.
        /// </summary>	
        [ConfigurationProperty(maxSizeInKilobytesProperty,
            DefaultValue = maxSizeInKilobytesDefaultValue)]
        [ResourceDescription(typeof(CachingResources), "MaxSizeInKilobytesDescription")]
        [ResourceDisplayName(typeof(CachingResources), "MaxSizeInKilobytesDisplayName")]
        [Browsable(true)]
        [Validation(typeof(SizeOverSilverlightDefaultQuotaValidator))]
        public int MaxSizeInKilobytes
        {
            get { return (int)this[maxSizeInKilobytesProperty]; }
            set { this[maxSizeInKilobytesProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the percentage of quota used before scavenging.
        /// </summary>	
        [ConfigurationProperty(percentOfQuotaUsedBeforeScavengingProperty, 
            DefaultValue = percentOfQuotaUsedBeforeScavengingDefaultValue)]
        [ResourceDescription(typeof(CachingResources), "PercentOfQuotaUsedBeforeScavengingDescription")]
        [ResourceDisplayName(typeof(CachingResources), "PercentOfQuotaUsedBeforeScavengingDisplayName")]
        [Browsable(true)]
        [Validation(typeof(PercentageValidator))]
        public int PercentOfQuotaUsedBeforeScavenging
        {
            get { return (int)this[percentOfQuotaUsedBeforeScavengingProperty]; }
            set { this[percentOfQuotaUsedBeforeScavengingProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the percentage of quota used after scavenging.
        /// </summary>	
        [ConfigurationProperty(percentOfQuotaUsedAfterScavengingProperty, 
            DefaultValue = percentOfQuotaUsedAfterScavengingDefaultValue)]
        [ResourceDescription(typeof(CachingResources), "PercentOfQuotaUsedAfterScavengingDescription")]
        [ResourceDisplayName(typeof(CachingResources), "PercentOfQuotaUsedAfterScavengingDisplayName")]
        [Browsable(true)]
        [Validation(typeof(PercentageValidator))]
        public int PercentOfQuotaUsedAfterScavenging
        {
            get { return (int)this[percentOfQuotaUsedAfterScavengingProperty]; }
            set { this[percentOfQuotaUsedAfterScavengingProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the frequency of expiration polling cycle.
        /// </summary>
        [ConfigurationProperty(expirationPollingIntervalProperty, 
            DefaultValue = expirationPollingIntervalDefaultValue)] 
        [BaseType(typeof(TimeSpan))]
        [ResourceDescription(typeof(CachingResources), "ExpirationPollingIntervalDescription")]
        [ResourceDisplayName(typeof(CachingResources), "ExpirationPollingIntervalDisplayName")]
        [Browsable(true)]
        public TimeSpan ExpirationPollingInterval
        {
            get { return TimeSpan.Parse(this[expirationPollingIntervalProperty].ToString()); }
            set { this[expirationPollingIntervalProperty] = value.ToString(); }
        }

        /// <summary>
        /// Gets or sets name of the type used for serializing and deserializing the cache entry.
        /// </summary>
        [ConfigurationProperty(serializerTypeNameProperty, 
            DefaultValue = serializerTypeNameDefaultValue)]
        [ResourceDescription(typeof(CachingResources), "SerializerTypeNameDescription")]
        [ResourceDisplayName(typeof(CachingResources), "SerializerTypeNameDisplayName")]
        [Browsable(true)]
        public string SerializerTypeName
        {
            get { return (string)this[serializerTypeNameProperty]; }
            set { this[serializerTypeNameProperty] = value; }
        }
    }
}

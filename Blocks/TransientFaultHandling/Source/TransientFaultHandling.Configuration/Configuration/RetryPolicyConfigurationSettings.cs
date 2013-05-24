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
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using Common.Configuration;
    using Common.Configuration.Design;
    using Microsoft.Practices.EnterpriseLibrary.TransientFaultHandling;

    /// <summary>
    /// Implements a configuration section that contains retry policy settings.
    /// </summary>
    [ViewModel(TransientFaultHandlingDesignTime.ViewModelTypeNames.RetryPolicyConfigurationSettingsViewModel)]
    [ResourceDescription(typeof(DesignResources), "RetryPolicyConfigurationSettingsDescription")]
    [ResourceDisplayName(typeof(DesignResources), "RetryPolicyConfigurationSettingsDisplayName")]
    public class RetryPolicyConfigurationSettings : SerializableConfigurationSection
    {
        /// <summary>
        /// The name of the configuration section that is represented by this type.
        /// </summary>
        public const string SectionName = "RetryPolicyConfiguration";

        private const string DefaultPolicyProperty = "defaultRetryStrategy";
        private const string DefaultSqlConnectionRetryStrategyProperty = "defaultSqlConnectionRetryStrategy";
        private const string DefaultSqlCommandRetryStrategyProperty = "defaultSqlCommandRetryStrategy";
        private const string DefaultAzureServiceBusRetryStrategyProperty = "defaultAzureServiceBusRetryStrategy";
        private const string DefaultAzureCachingRetryStrategyProperty = "defaultAzureCachingRetryStrategy";
        private const string DefaultAzureStorageRetryStrategyProperty = "defaultAzureStorageRetryStrategy";

        /// <summary>
        /// Gets or sets the name of the default general-purpose retry strategy.
        /// </summary>
        [ConfigurationProperty(DefaultPolicyProperty, IsRequired = true)]
        [Reference(typeof(RetryStrategyCollection), typeof(RetryStrategyData))]
        [ResourceDescription(typeof(DesignResources), "DefaultRetryStrategyDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DefaultRetryStrategyDisplayName")]
        public string DefaultRetryStrategy
        {
            get { return (string)base[DefaultPolicyProperty]; }
            set { base[DefaultPolicyProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the name of a retry strategy dedicated to handling transient conditions with SQL connections.
        /// </summary>
        [ConfigurationProperty(DefaultSqlConnectionRetryStrategyProperty, IsRequired = false)]
        [Reference(typeof(RetryStrategyCollection), typeof(RetryStrategyData))]
        [ResourceDescription(typeof(DesignResources), "DefaultSqlConnectionRetryStrategyDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DefaultSqlConnectionRetryStrategyDisplayName")]
        public string DefaultSqlConnectionRetryStrategy
        {
            get { return (string)base[DefaultSqlConnectionRetryStrategyProperty]; }
            set { base[DefaultSqlConnectionRetryStrategyProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the name of a retry strategy dedicated to handling transient conditions with SQL commands.
        /// </summary>
        [ConfigurationProperty(DefaultSqlCommandRetryStrategyProperty, IsRequired = false)]
        [Reference(typeof(RetryStrategyCollection), typeof(RetryStrategyData))]
        [ResourceDescription(typeof(DesignResources), "DefaultSqlCommandRetryStrategyDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DefaultSqlCommandRetryStrategyDisplayName")]
        public string DefaultSqlCommandRetryStrategy
        {
            get { return (string)base[DefaultSqlCommandRetryStrategyProperty]; }
            set { base[DefaultSqlCommandRetryStrategyProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the name of a retry policy dedicated to handling transient conditions in the Windows Azure Service Bus infrastructure.
        /// </summary>
        [ConfigurationProperty(DefaultAzureServiceBusRetryStrategyProperty, IsRequired = false)]
        [Reference(typeof(RetryStrategyCollection), typeof(RetryStrategyData))]
        [ResourceDescription(typeof(DesignResources), "DefaultAzureServiceBusRetryStrategyDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DefaultAzureServiceBusRetryStrategyDisplayName")]
        public string DefaultAzureServiceBusRetryStrategy
        {
            get { return (string)base[DefaultAzureServiceBusRetryStrategyProperty]; }
            set { base[DefaultAzureServiceBusRetryStrategyProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the name of a retry policy dedicated to handling transient conditions in Windows Azure Caching.
        /// </summary>
        [ConfigurationProperty(DefaultAzureCachingRetryStrategyProperty, IsRequired = false)]
        [Reference(typeof(RetryStrategyCollection), typeof(RetryStrategyData))]
        [ResourceDescription(typeof(DesignResources), "DefaultAzureCachingRetryStrategyDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DefaultAzureCachingRetryStrategyDisplayName")]
        public string DefaultAzureCachingRetryStrategy
        {
            get { return (string)base[DefaultAzureCachingRetryStrategyProperty]; }
            set { base[DefaultAzureCachingRetryStrategyProperty] = value; }
        }

        /// <summary>
        /// Gets or sets the name of a retry policy dedicated to handling transient conditions in Windows Azure Storage.
        /// </summary>
        [ConfigurationProperty(DefaultAzureStorageRetryStrategyProperty, IsRequired = false)]
        [Reference(typeof(RetryStrategyCollection), typeof(RetryStrategyData))]
        [ResourceDescription(typeof(DesignResources), "DefaultAzureStorageRetryStrategyDescription")]
        [ResourceDisplayName(typeof(DesignResources), "DefaultAzureStorageRetryStrategyDisplayName")]
        public string DefaultAzureStorageRetryStrategy
        {
            get { return (string)base[DefaultAzureStorageRetryStrategyProperty]; }
            set { base[DefaultAzureStorageRetryStrategyProperty] = value; }
        }

        /// <summary>
        /// Gets a collection of retry policy definitions represented by the <see cref="RetryStrategyData"/> object instances.
        /// </summary>
        [ConfigurationProperty("", Options = ConfigurationPropertyOptions.IsDefaultCollection)]
        [ConfigurationCollection(typeof(RetryStrategyData))]
        [Command(TransientFaultHandlingDesignTime.CommandTypeNames.WellKnownRetryStrategyElementCollectionCommand, CommandPlacement = CommandPlacement.ContextAdd, Replace = CommandReplacement.DefaultAddCommandReplacement)]
        [ResourceDescription(typeof(DesignResources), "RetryStrategiesDescription")]
        [ResourceDisplayName(typeof(DesignResources), "RetryStrategiesDisplayName")]
        public RetryStrategyCollection RetryStrategies
        {
            get { return (RetryStrategyCollection)base[string.Empty]; }
        }

        /// <summary>
        /// Retrieves the <see cref="RetryPolicyConfigurationSettings"/> section from the configuration source.
        /// </summary>
        /// <param name="configurationSource">The configuration source to get the section from.</param>
        /// <returns>The retry policy section.</returns>
        public static RetryPolicyConfigurationSettings GetRetryPolicySettings(IConfigurationSource configurationSource)
        {
            if (configurationSource == null) throw new ArgumentNullException("configurationSource");
            return (RetryPolicyConfigurationSettings)configurationSource.GetSection(SectionName);
        }

        /// <summary>
        /// Builds the <see cref="RetryManager"/> from the configuration settings.
        /// </summary>
        /// <returns>The retry manager to use when transient errors occur.</returns>
        public RetryManager BuildRetryManager()
        {
            var strategies = this.RetryStrategies.Select(x => x.BuildRetryStrategy()).ToList();

            var defaultStrategies = new Dictionary<string, string>();
            if (!string.IsNullOrWhiteSpace(this.DefaultSqlCommandRetryStrategy))
            {
                defaultStrategies.Add(RetryManagerSqlExtensions.DefaultStrategyCommandTechnologyName, this.DefaultSqlCommandRetryStrategy);
            }

            if (!string.IsNullOrWhiteSpace(this.DefaultSqlConnectionRetryStrategy))
            {
                defaultStrategies.Add(RetryManagerSqlExtensions.DefaultStrategyConnectionTechnologyName, this.DefaultSqlConnectionRetryStrategy);
            }

            if (!string.IsNullOrWhiteSpace(this.DefaultAzureServiceBusRetryStrategy))
            {
                defaultStrategies.Add(RetryManagerServiceBusExtensions.DefaultStrategyTechnologyName, this.DefaultAzureServiceBusRetryStrategy);
            }

            if (!string.IsNullOrWhiteSpace(this.DefaultAzureStorageRetryStrategy))
            {
                defaultStrategies.Add(RetryManagerWindowsAzureStorageExtensions.DefaultStrategyTechnologyName, this.DefaultAzureStorageRetryStrategy);
            }

            if (!string.IsNullOrWhiteSpace(this.DefaultAzureCachingRetryStrategy))
            {
                defaultStrategies.Add(RetryManagerCachingExtensions.DefaultStrategyTechnologyName, this.DefaultAzureCachingRetryStrategy);
            }

            return new RetryManager(
                strategies,
                this.DefaultRetryStrategy,
                defaultStrategies);
        }
    }
}

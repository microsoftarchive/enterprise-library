//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using Microsoft.Practices.EnterpriseLibrary.Caching.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Cache.CachingStore.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="CachingStoreProviderData"/> that
    /// splits policy overrides processing and WMI objects generation, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class CachingStoreProviderDataManageabilityProvider
        : ConfigurationElementManageabilityProviderBase<CachingStoreProviderData>
    {
        /// <summary>
        /// The name of the cache manager property.
        /// </summary>
        public const String CacheManagerPropertyName = "cacheManager";

        /// <summary>
        /// The name of the absolute expiration property.
        /// </summary>
        public const String AbsoluteExpirationPropertyName = "defaultAbsoluteSessionExpirationInMinutes";

        /// <summary>
        /// The name of the sliding expriration property.
        /// </summary>
        public const String SlidingExpirationPropertyName = "defaultSlidingSessionExpirationInMinutes";

        /// <summary>
        /// Initialize an instnace of the <see cref="CachingStoreProviderDataManageabilityProvider"/> class.
        /// </summary>
        public CachingStoreProviderDataManageabilityProvider()
        { }

        /// <summary>
        /// Adds the ADM parts that represent the properties of
        /// a specific instance of the configuration element type managed by the receiver.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationObject">The configuration object instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="elementPolicyKeyName">The key for the element's policies.</param>
        /// <remarks>
        /// Subclasses managing objects that must not create a policy will likely need to include the elements' keys when creating the parts.
        /// </remarks>
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
            CachingStoreProviderData configurationObject,
            IConfigurationSource configurationSource,
            String elementPolicyKeyName)
        {
            CacheManagerSettings cachingConfigurationSection
                = (CacheManagerSettings)configurationSource.GetSection(CacheManagerSettings.SectionName);

            contentBuilder.AddDropDownListPartForNamedElementCollection<CacheManagerDataBase>(Resources.CachingStoreProviderCacheManagerPartName,
                CacheManagerPropertyName,
                cachingConfigurationSection.CacheManagers,
                configurationObject.CacheManager,
                false);

            contentBuilder.AddNumericPart(Resources.CachingStoreProviderAbsoluteExpirationPartName,
                AbsoluteExpirationPropertyName,
                configurationObject.AbsoluteExpiration);

            contentBuilder.AddNumericPart(Resources.CachingStoreProviderSlidingExpirationPartName,
                SlidingExpirationPropertyName,
                configurationObject.SlidingExpiration);
        }

        /// <summary>
        /// Gets the template for the name of the policy associated to the object.
        /// </summary>
        /// <remarks>
        /// Elements that override 
        /// <see cref="ConfigurationElementManageabilityProviderBase{T}.AddAdministrativeTemplateDirectives(AdmContentBuilder, T, IConfigurationSource, String)"/>
        /// to avoid creating a policy must still override this property.
        /// </remarks>
        protected override string ElementPolicyNameTemplate
        {
            get
            {
                return Resources.SecurityCacheProviderPolicyNameTemplate;
            }
        }

        /// <summary>
        /// Overrides the <paramref name="configurationObject"/>'s properties with the Group Policy values from the 
        /// registry.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
        /// configuration element.</param>
        /// <remarks>Subclasses implementing this method must retrieve all the override values from the registry
        /// before making modifications to the <paramref name="configurationObject"/> so any error retrieving
        /// the override values will cancel policy processing.</remarks>
        protected override void OverrideWithGroupPolicies(CachingStoreProviderData configurationObject, IRegistryKey policyKey)
        {
            String cacheManagerOverride = policyKey.GetStringValue(CacheManagerPropertyName);
            int? absoluteExpirationOverride = policyKey.GetIntValue(AbsoluteExpirationPropertyName);
            int? slidingExpirationOverride = policyKey.GetIntValue(SlidingExpirationPropertyName);

            configurationObject.CacheManager = cacheManagerOverride;
            configurationObject.AbsoluteExpiration = absoluteExpirationOverride.Value;
            configurationObject.SlidingExpiration = slidingExpirationOverride.Value;
        }
    }
}

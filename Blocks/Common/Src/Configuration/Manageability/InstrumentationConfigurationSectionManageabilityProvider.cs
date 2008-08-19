//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Properties;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability
{
    /// <summary>
    /// Represents a instrumentation configuration section provider.
    /// </summary>
    public class InstrumentationConfigurationSectionManageabilityProvider
        : ConfigurationSectionManageabilityProviderBase<InstrumentationConfigurationSection>
    {
        /// <summary>
        /// The name of the property to determine if event logging is enabled.
        /// </summary>
        public const String EventLoggingEnabledPropertyName = "eventLoggingEnabled";

        /// <summary>
        /// The name of the property to determine if performance counters is enabled.
        /// </summary>
        public const String PerformanceCountersEnabledPropertyName = "performanceCountersEnabled";

        /// <summary>
        /// The name of the property to determine if wmi is enabled is enabled.
        /// </summary>
        public const String WmiEnabledPropertyName = "wmiEnabled";

        /// <summary>
        /// Initialize a new instance of the <see cref="InstrumentationConfigurationSectionManageabilityProvider"/> class.
        /// </summary>
        /// <param name="subProviders">The sub providers.</param>
        public InstrumentationConfigurationSectionManageabilityProvider(IDictionary<Type, ConfigurationElementManageabilityProvider> subProviders)
            : base(subProviders)
        {
            InstrumentationConfigurationSectionWmiMapper.RegisterWmiTypes();
        }

        /// <summary>
        /// Gets the name of the category that represents the whole configuration section.
        /// </summary>
        protected override string SectionCategoryName
        {
            get { return Resources.InstrumentationSectionCategoryName; }
        }

        /// <summary>
        /// Gets the name of the managed configuration section.
        /// </summary>
        protected override string SectionName
        {
            get { return InstrumentationConfigurationSection.SectionName; }
        }

        /// <summary>
        /// Adds the ADM instructions that describe the policies that can be used to override the configuration
        /// information represented by a configuration section.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationSection">The configuration section instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="sectionKey">The root key for the section's policies.</param>
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
                                                                    InstrumentationConfigurationSection configurationSection,
                                                                    IConfigurationSource configurationSource,
                                                                    String sectionKey)
        {
            contentBuilder.StartPolicy(Resources.InstrumentationSectionPolicyName, sectionKey);
            {
                contentBuilder.AddCheckboxPart(Resources.InstrumentationSectionEventLoggingEnabledPartName,
                                               EventLoggingEnabledPropertyName,
                                               configurationSection.EventLoggingEnabled);

                contentBuilder.AddCheckboxPart(Resources.InstrumentationSectionPerformanceCountersEnabledPartName,
                                               PerformanceCountersEnabledPropertyName,
                                               configurationSection.PerformanceCountersEnabled);

                contentBuilder.AddCheckboxPart(Resources.InstrumentationSectionWmiEnabledPartName,
                                               WmiEnabledPropertyName,
                                               configurationSection.WmiEnabled);
            }
            contentBuilder.EndPolicy();
        }

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the <paramref name="configurationSection"/>.
        /// </summary>
        /// <param name="configurationSection">The configuration section that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjectsForConfigurationSection(InstrumentationConfigurationSection configurationSection,
                                                                          ICollection<ConfigurationSetting> wmiSettings)
        {
            InstrumentationConfigurationSectionWmiMapper.GenerateWmiObjects(configurationSection, wmiSettings);
        }

        /// <summary>
        /// Overrides the <paramref name="configurationSection"/>'s configuration elements' properties 
        /// with the Group Policy values from the registry, if any, and creates the <see cref="ConfigurationSetting"/> 
        /// instances that describe these configuration elements.
        /// </summary>
        /// <param name="configurationSection">The configuration section that must be managed.</param>
        /// <param name="readGroupPolicies"><see langword="true"/> if Group Policy overrides must be applied; otherwise, 
        /// <see langword="false"/>.</param>
        /// <param name="machineKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
        /// configuration section at the machine level, or <see langword="null"/> 
        /// if there is no such registry key.</param>
        /// <param name="userKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
        /// configuration section at the user level, or <see langword="null"/> 
        /// if there is no such registry key.</param>
        /// <param name="generateWmiObjects"><see langword="true"/> if WMI objects must be generated; otherwise, 
        /// <see langword="false"/>.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void OverrideWithGroupPoliciesAndGenerateWmiObjectsForConfigurationElements(InstrumentationConfigurationSection configurationSection,
                                                                                                       bool readGroupPolicies,
                                                                                                       IRegistryKey machineKey,
                                                                                                       IRegistryKey userKey,
                                                                                                       bool generateWmiObjects,
                                                                                                       ICollection<ConfigurationSetting> wmiSettings)
        {
            // no sub elements for this section
        }

        /// <summary>
        /// Overrides the <paramref name="configurationSection"/>'s properties with the Group Policy values from 
        /// the registry.
        /// </summary>
        /// <param name="configurationSection">The configuration section that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides.</param>
        protected override void OverrideWithGroupPoliciesForConfigurationSection(InstrumentationConfigurationSection configurationSection,
                                                                                 IRegistryKey policyKey)
        {
            bool? eventLoggingEnabledOverride = policyKey.GetBoolValue(EventLoggingEnabledPropertyName);
            bool? performanceCountersEnabledOverride = policyKey.GetBoolValue(PerformanceCountersEnabledPropertyName);
            bool? wmiEnabledOverride = policyKey.GetBoolValue(WmiEnabledPropertyName);

            configurationSection.EventLoggingEnabled = eventLoggingEnabledOverride.Value;
            configurationSection.PerformanceCountersEnabled = performanceCountersEnabledOverride.Value;
            configurationSection.WmiEnabled = wmiEnabledOverride.Value;
        }
    }
}
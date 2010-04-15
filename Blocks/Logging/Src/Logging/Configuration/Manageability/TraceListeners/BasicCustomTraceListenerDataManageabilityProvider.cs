//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Base class for <see cref="ConfigurationElementManageabilityProvider"/> implementations that provide manageability
    /// support for custom provider's configuration.
    /// </summary>
    /// <typeparam name="T">The custon provider's configuration element type.</typeparam>
    /// <remarks>
    /// The basic configuration for a custom provider includes the provider type and a collection of attributes.
    /// </remarks>
    public abstract class BasicCustomTraceListenerDataManageabilityProvider<T>
        : CustomProviderDataManageabilityProvider<T>
        where T : BasicCustomTraceListenerData
    {
        /// <summary>
        /// The name of the initial data property.
        /// </summary>
        public const String InitDataPropertyName = "initializeData";

        /// <summary>
        /// The name of the trace output options property.
        /// </summary>
        public const String TraceOutputOptionsPropertyName = "traceOutputOptions";

        /// <summary>
        /// The name of the filter property.
        /// </summary>
        public const String FilterPropertyName = "filter";

        /// <summary>
        /// The name of the attribute property.
        /// </summary>
        public new const String AttributesPropertyName =
            CustomProviderDataManageabilityProvider<CustomTraceListenerData>.AttributesPropertyName;

        /// <summary>
        /// The name of the provider type property.
        /// </summary>
        public new const String ProviderTypePropertyName =
            CustomProviderDataManageabilityProvider<CustomTraceListenerData>.ProviderTypePropertyName;

        /// <summary>
        /// Initialize a new instance of the <see cref="BasicCustomTraceListenerDataManageabilityProvider{T}"/> class.
        /// </summary>
        protected BasicCustomTraceListenerDataManageabilityProvider()
            : base(Resources.TraceListenerPolicyNameTemplate)
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
        /// Subclasses that manage custom provider's configuration objects with additional properties may
        /// override this method to add the corresponding parts.
        /// </remarks>
        /// <seealso cref="ConfigurationElementManageabilityProviderBase{T}.AddAdministrativeTemplateDirectives(AdmContentBuilder, T, IConfigurationSource, String)"/>
        protected override void AddElementAdministrativeTemplateParts(
            AdmContentBuilder contentBuilder,
            T configurationObject,
            IConfigurationSource configurationSource,
            string elementPolicyKeyName)
        {
            base.AddElementAdministrativeTemplateParts(contentBuilder,
                configurationObject,
                configurationSource,
                elementPolicyKeyName);

            contentBuilder.AddEditTextPart(Resources.CustomTraceListenerInitializationPartName,
                InitDataPropertyName,
                configurationObject.InitData,
                1024,
                false);

            contentBuilder.AddTextPart(Resources.TraceListenerTraceOptionsPartName);

            var traceOptionsKey = elementPolicyKeyName + @"\" + TraceOutputOptionsPropertyName;
            AddCheckboxPartsForFlagsEnumeration<TraceOptions>(contentBuilder, traceOptionsKey, configurationObject.TraceOutputOptions);

            contentBuilder.AddDropDownListPartForEnumeration<SourceLevels>(Resources.TraceListenerFilterPartName,
                FilterPropertyName,
                configurationObject.Filter);
        }

        /// <summary>
        /// Overrides the <paramref name="configurationObject"/>'s properties with the Group Policy values from the 
        /// registry.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="policyKey">The <see cref="IRegistryKey"/> which holds the Group Policy overrides for the 
        /// configuration element.</param>
        /// <remarks>Subclasses that manage custom provider's configuration objects with additional properties may
        /// override this method to override these properties.</remarks>
        protected override void OverrideWithGroupPolicies(T configurationObject, IRegistryKey policyKey)
        {
            String initDataOverride = policyKey.GetStringValue(InitDataPropertyName);
            TraceOptions? traceOutputOptionsOverride =
                GetFlagsEnumOverride<TraceOptions>(policyKey, TraceOutputOptionsPropertyName);
            SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);

            base.OverrideWithGroupPolicies(configurationObject, policyKey);

            configurationObject.InitData = initDataOverride;
            configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
            configurationObject.Filter = filterOverride.Value;
        }
    }
}

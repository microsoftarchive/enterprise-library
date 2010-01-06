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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Provides an implementation for <see cref="CustomTraceListenerData"/> that
    /// splits policy overrides processing and WMI objects generation, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class CustomTraceListenerDataManageabilityProvider
        : BasicCustomTraceListenerDataManageabilityProvider<CustomTraceListenerData>
    {
        /// <summary>
        /// The name of the formatter property.
        /// </summary>
        public const String FormatterPropertyName = "formatter";

        /// <summary>
        /// Initialize a new instance of the <see cref="CustomTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
        public CustomTraceListenerDataManageabilityProvider()
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
        protected override void AddElementAdministrativeTemplateParts(AdmContentBuilder contentBuilder,
            CustomTraceListenerData configurationObject,
            IConfigurationSource configurationSource,
            String elementPolicyKeyName)
        {
            base.AddElementAdministrativeTemplateParts(contentBuilder,
                configurationObject,
                configurationSource,
                elementPolicyKeyName);

            LoggingSettings configurationSection = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            contentBuilder.AddDropDownListPartForNamedElementCollection<FormatterData>(Resources.TraceListenerFormatterPartName,
                FormatterPropertyName,
                configurationSection.Formatters,
                configurationObject.Formatter,
                true);
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
        protected override void OverrideWithGroupPolicies(CustomTraceListenerData configurationObject, IRegistryKey policyKey)
        {
            String formatterOverride = policyKey.GetStringValue(FormatterPropertyName);

            base.OverrideWithGroupPolicies(configurationObject, policyKey);

            configurationObject.Formatter = AdmContentBuilder.NoneListItem.Equals(formatterOverride) ? String.Empty : formatterOverride;
        }
    }
}

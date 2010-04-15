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
    /// Represents the behavior required to provide Group Policy updates for a <see cref="TraceListenerData"/>.
    /// </summary>
    public abstract class TraceListenerDataManageabilityProvider<T>
        : ConfigurationElementManageabilityProviderBase<T>
        where T : TraceListenerData
    {
        /// <summary>
        /// Name for the formatter property.
        /// </summary>
        public const String FormatterPropertyName = "formatter";
        /// <summary>
        /// Name for the traceOutputOptions property.
        /// </summary>
        public const String TraceOutputOptionsPropertyName = "traceOutputOptions";
        /// <summary>
        /// Name for the filter property.
        /// </summary>
        public const String FilterPropertyName = "filter";

        /// <summary>
        /// Adds the part to edit the policy overrides for the trace options of a trace listener to the content
        /// built by the <paramref name="contentBuilder"/>.
        /// </summary>
        /// <param name="contentBuilder">The builder for the content where the part will be added.</param>
        /// <param name="parentKey"></param>
        /// <param name="traceOutputOptions">The default value for the part.</param>
        protected internal static void AddTraceOptionsPart(AdmContentBuilder contentBuilder,
            string parentKey,
            TraceOptions traceOutputOptions)
        {
            contentBuilder.AddTextPart(Resources.TraceListenerTraceOptionsPartName);

            var traceOptionsKey = parentKey + @"\" + TraceOutputOptionsPropertyName;

            AddCheckboxPartsForFlagsEnumeration<TraceOptions>(contentBuilder, traceOptionsKey, traceOutputOptions);
        }

        /// <summary>
        /// Adds the part to edit the policy overrides for the filter of a trace listener to the content
        /// built by the <paramref name="contentBuilder"/>.
        /// </summary>
        /// <param name="contentBuilder">The builder for the content where the part will be added.</param>
        /// <param name="filter">The default value for the part.</param>
        protected internal static void AddFilterPart(AdmContentBuilder contentBuilder,
            SourceLevels filter)
        {
            contentBuilder.AddDropDownListPartForEnumeration<SourceLevels>(Resources.TraceListenerFilterPartName,
                FilterPropertyName,
                filter);
        }

        /// <summary>
        /// Adds the part to edit the policy overrides for the formatter of a trace listener to the content
        /// built by the <paramref name="contentBuilder"/>.
        /// </summary>
        /// <param name="contentBuilder">The builder for the content where the part will be added.</param>
        /// <param name="formatterName">The default value for the part.</param>
        /// <param name="configurationSource">The configuration source to use to retrieve the available formatters.</param>
        protected internal static void AddFormattersPart(AdmContentBuilder contentBuilder,
            String formatterName,
            IConfigurationSource configurationSource)
        {
            LoggingSettings configurationSection = (LoggingSettings)configurationSource.GetSection(LoggingSettings.SectionName);

            contentBuilder.AddDropDownListPartForNamedElementCollection<FormatterData>(Resources.TraceListenerFormatterPartName,
                FormatterPropertyName,
                configurationSection.Formatters,
                formatterName,
                true);
        }

        /// <summary>
        /// Gets the template for the name of the policy associated to the object.
        /// </summary>
        protected sealed override string ElementPolicyNameTemplate
        {
            get { return Resources.TraceListenerPolicyNameTemplate; }
        }

        /// <summary>
        /// Utility method that retrieves values from registry keys associated to Group Policy overrides
        /// for the formatter name, giving priority to the machine level key.
        /// </summary>
        /// <param name="policyKey">The registry key that holds the policy overrides.</param>
        /// <returns>The value for the formatter name on <paramref name="policyKey"/>.</returns>
        protected internal static String GetFormatterPolicyOverride(IRegistryKey policyKey)
        {
            String overrideValue = policyKey.GetStringValue(FormatterPropertyName);

            return AdmContentBuilder.NoneListItem.Equals(overrideValue) ? String.Empty : overrideValue;
        }
    }
}

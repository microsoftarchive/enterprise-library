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
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners
{
    /// <summary>
    /// Provides an implementation for <see cref="FormattedEventLogTraceListenerData"/> that
    /// processes policy overrides, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class FormattedEventLogTraceListenerDataManageabilityProvider
        : TraceListenerDataManageabilityProvider<FormattedEventLogTraceListenerData>
    {
        /// <summary>
        /// The name of the log property.
        /// </summary>
        public const String LogPropertyName = "log";

        /// <summary>
        /// The name of the machine property.
        /// </summary>
        public const String MachineNamePropertyName = "machineName";

        /// <summary>
        /// The name of the source property.
        /// </summary>
        public const String SourcePropertyName = "source";

        /// <summary>
        /// Initialize a new instance of the <see cref="FormattedEventLogTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
        public FormattedEventLogTraceListenerDataManageabilityProvider()
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
                                                                      FormattedEventLogTraceListenerData configurationObject,
                                                                      IConfigurationSource configurationSource,
                                                                      String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.EventLogTraceListenerSourcePartName,
                                           SourcePropertyName,
                                           configurationObject.Source,
                                           255,
                                           true);

            contentBuilder.AddEditTextPart(Resources.EventLogTraceListenerLogPartName,
                                           LogPropertyName,
                                           configurationObject.Log,
                                           255,
                                           false);

            contentBuilder.AddEditTextPart(Resources.EventLogTraceListenerMachineNamePartName,
                                           MachineNamePropertyName,
                                           configurationObject.MachineName,
                                           255,
                                           false);

            AddTraceOptionsPart(contentBuilder, elementPolicyKeyName, configurationObject.TraceOutputOptions);

            AddFilterPart(contentBuilder, configurationObject.Filter);

            AddFormattersPart(contentBuilder, configurationObject.Formatter, configurationSource);
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
        protected override void OverrideWithGroupPolicies(FormattedEventLogTraceListenerData configurationObject,
                                                          IRegistryKey policyKey)
        {
            String formatterOverride = GetFormatterPolicyOverride(policyKey);
            String logOverride = policyKey.GetStringValue(LogPropertyName);
            String machineNameOverride = policyKey.GetStringValue(MachineNamePropertyName);
            String sourceOverride = policyKey.GetStringValue(SourcePropertyName);
            TraceOptions? traceOutputOptionsOverride =
                GetFlagsEnumOverride<TraceOptions>(policyKey, TraceOutputOptionsPropertyName);
            SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);

            configurationObject.Formatter = formatterOverride;
            configurationObject.Log = logOverride;
            configurationObject.MachineName = machineNameOverride;
            configurationObject.Source = sourceOverride;
            configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
            configurationObject.Filter = filterOverride.Value;
        }
    }
}

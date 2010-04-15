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
    /// Provides an implementation for <see cref="EmailTraceListenerData"/> that
    /// processes policy overrides, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
    public class EmailTraceListenerDataManageabilityProvider
        : TraceListenerDataManageabilityProvider<EmailTraceListenerData>
    {
        /// <summary>
        /// The name of the from address property.
        /// </summary>
        public const String FromAddressPropertyName = "fromAddress";

        /// <summary>
        /// The name of the smtp port property.
        /// </summary>
        public const String SmtpPortPropertyName = "smtpPort";

        /// <summary>
        /// The name of the smtp server.
        /// </summary>
        public const String SmtpServerPropertyName = "smtpServer";

        /// <summary>
        /// The name of the subject line ender property.
        /// </summary>
        public const String SubjectLineEnderPropertyName = "subjectLineEnder";

        /// <summary>
        /// The name of the subject line starter property.
        /// </summary>
        public const String SubjectLineStarterPropertyName = "subjectLineStarter";

        /// <summary>
        /// The name of the to address property.
        /// </summary>
        public const String ToAddressPropertyName = "toAddress";

        /// <summary>
        /// Initialize a new instance of the <see cref="EmailTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
        public EmailTraceListenerDataManageabilityProvider()
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
            EmailTraceListenerData configurationObject,
            IConfigurationSource configurationSource,
            String elementPolicyKeyName)
        {
            contentBuilder.AddEditTextPart(Resources.EmailTraceListenerFromAddressPartName,
                FromAddressPropertyName,
                configurationObject.FromAddress,
                255,
                true);

            contentBuilder.AddEditTextPart(Resources.EmailTraceListenerToAddressPartName,
                ToAddressPropertyName,
                configurationObject.ToAddress,
                255,
                false);

            contentBuilder.AddNumericPart(Resources.EmailTraceListenerSmtpPortPartName,
                SmtpPortPropertyName,
                configurationObject.SmtpPort);

            contentBuilder.AddEditTextPart(Resources.EmailTraceListenerSmtpServerPartName,
                SmtpServerPropertyName,
                configurationObject.SmtpServer,
                255,
                true);

            contentBuilder.AddEditTextPart(Resources.EmailTraceListenerStarterPartName,
                SubjectLineStarterPropertyName,
                configurationObject.SubjectLineStarter,
                255,
                false);

            contentBuilder.AddEditTextPart(Resources.EmailTraceListenerEnderPartName,
                SubjectLineEnderPropertyName,
                configurationObject.SubjectLineEnder,
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
        protected override void OverrideWithGroupPolicies(EmailTraceListenerData configurationObject, IRegistryKey policyKey)
        {
            String formatterOverride = GetFormatterPolicyOverride(policyKey);
            String fromAddressOverride = policyKey.GetStringValue(FromAddressPropertyName);
            int? smtpPortOverride = policyKey.GetIntValue(SmtpPortPropertyName);
            String smtpServerOverride = policyKey.GetStringValue(SmtpServerPropertyName);
            String subjectLineEnderOverride = policyKey.GetStringValue(SubjectLineEnderPropertyName);
            String subjectLineStarterOverride = policyKey.GetStringValue(SubjectLineStarterPropertyName);
            String toAddressOverride = policyKey.GetStringValue(ToAddressPropertyName);
            TraceOptions? traceOutputOptionsOverride =
                GetFlagsEnumOverride<TraceOptions>(policyKey, TraceOutputOptionsPropertyName);
            SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);

            configurationObject.Formatter = formatterOverride;
            configurationObject.FromAddress = fromAddressOverride;
            configurationObject.SmtpPort = smtpPortOverride.Value;
            configurationObject.SmtpServer = smtpServerOverride;
            configurationObject.SubjectLineEnder = subjectLineEnderOverride;
            configurationObject.SubjectLineStarter = subjectLineStarterOverride;
            configurationObject.ToAddress = toAddressOverride;
            configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
            configurationObject.Filter = filterOverride.Value;
        }
    }
}

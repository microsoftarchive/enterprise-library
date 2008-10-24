//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Properties;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="LoggingExceptionHandlerData"/> that
    /// splits policy overrides processing and WMI objects generation, performing approriate logging of 
    /// policy processing errors.
    /// </summary>
	public class LoggingExceptionHandlerDataManageabilityProvider
		: ConfigurationElementManageabilityProviderBase<LoggingExceptionHandlerData>
	{
        /// <summary>
        /// The name of the event id property.
        /// </summary>
		public const String EventIdPropertyName = "eventId";

        /// <summary>
        /// The name of the formatter type property.
        /// </summary>
		public const String FormatterTypePropertyName = "formatterType";

        /// <summary>
        /// The name of the log category property.
        /// </summary>
		public const String LogCategoryPropertyName = "logCategory";

        /// <summary>
        /// The name of the priority property.
        /// </summary>
		public const String PriorityPropertyName = "priority";

        /// <summary>
        /// The name of the severity property.
        /// </summary>
		public const String SeverityPropertyName = "severity";

        /// <summary>
        /// The name of the title property.
        /// </summary>
		public const String TitlePropertyName = "title";

        /// <summary>
        /// Initialize a new instance of the <see cref="LoggingExceptionHandlerDataManageabilityProvider"/> class.
        /// </summary>
		public LoggingExceptionHandlerDataManageabilityProvider()
		{
			LoggingExceptionHandlerDataWmiMapper.RegisterWmiTypes();
		}

        /// <summary>
        /// Adds the ADM instructions that describe the policies that can be used to override the properties of
        /// a specific instance of the configuration element type managed by the receiver.
        /// </summary>
        /// <param name="contentBuilder">The <see cref="AdmContentBuilder"/> to which the Adm instructions are to be appended.</param>
        /// <param name="configurationObject">The configuration object instance.</param>
        /// <param name="configurationSource">The configuration source from where to get additional configuration
        /// information, if necessary.</param>
        /// <param name="elementPolicyKeyName">The key for the element's policies.</param>
        /// <remarks>
        /// The default implementation for this method creates a policy, using 
        /// <see cref="ConfigurationElementManageabilityProviderBase{T}.ElementPolicyNameTemplate"/> to create the policy name and invoking
        /// <see cref="ConfigurationElementManageabilityProviderBase{T}.AddElementAdministrativeTemplateParts(AdmContentBuilder, T, IConfigurationSource, String)"/>
        /// to add the policy parts.
        /// Subclasses managing objects that must not create a policy must override this method to just add the parts.
        /// </remarks>
        protected override void AddAdministrativeTemplateDirectives(AdmContentBuilder contentBuilder,
			LoggingExceptionHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			// directives are parts of the exception type policy
			AddElementAdministrativeTemplateParts(contentBuilder,
				configurationObject,
				configurationSource,
				elementPolicyKeyName);
		}

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
			LoggingExceptionHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddTextPart(String.Format(CultureInfo.CurrentCulture,
													Resources.HandlerPartNameTemplate,
													configurationObject.Name));

			contentBuilder.AddEditTextPart(Resources.LoggingHandlerTitlePartName,
				elementPolicyKeyName,
				TitlePropertyName,
				configurationObject.Title,
				255,
				true);

			contentBuilder.AddNumericPart(Resources.LoggingHandlerEventIdPartName,
				elementPolicyKeyName,
				EventIdPropertyName,
				configurationObject.EventId);

			contentBuilder.AddDropDownListPartForEnumeration<TraceEventType>(Resources.LoggingHandlerSeverityPartName,
				elementPolicyKeyName,
				SeverityPropertyName,
				configurationObject.Severity);

			contentBuilder.AddNumericPart(Resources.LoggingHandlerPriorityPartName,
				elementPolicyKeyName,
				PriorityPropertyName,
				configurationObject.Priority);

			LoggingSettings loggingConfigurationSection
				= configurationSource.GetSection(LoggingSettings.SectionName) as LoggingSettings;

			contentBuilder.AddDropDownListPartForNamedElementCollection<TraceSourceData>(Resources.LoggingHandlerCategoryPartName,
				elementPolicyKeyName,
				LogCategoryPropertyName,
				loggingConfigurationSection.TraceSources,
				configurationObject.LogCategory,
				false);

			contentBuilder.AddComboBoxPart(Resources.LoggingHandlerFormatterPartName,
				elementPolicyKeyName,
				FormatterTypePropertyName,
				configurationObject.FormatterType.AssemblyQualifiedName,
				255,
				true,
				typeof(TextExceptionFormatter).AssemblyQualifiedName,
				typeof(XmlExceptionFormatter).AssemblyQualifiedName);
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
			get { return null; }
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
        protected override void OverrideWithGroupPolicies(LoggingExceptionHandlerData configurationObject, IRegistryKey policyKey)
		{
			int? eventIdOverride = policyKey.GetIntValue(EventIdPropertyName);
			Type formatterTypeOverride = policyKey.GetTypeValue(FormatterTypePropertyName);
			String logCategoryOverride = policyKey.GetStringValue(LogCategoryPropertyName);
			int? priorityOverride = policyKey.GetIntValue(PriorityPropertyName);
			TraceEventType? severityOverride = policyKey.GetEnumValue<TraceEventType>(SeverityPropertyName);
			String titleOverride = policyKey.GetStringValue(TitlePropertyName);

			configurationObject.EventId = eventIdOverride.Value;
			configurationObject.FormatterType = formatterTypeOverride;
			configurationObject.LogCategory = logCategoryOverride;
			configurationObject.Priority = priorityOverride.Value;
			configurationObject.Severity = severityOverride.Value;
			configurationObject.Title = titleOverride;
		}

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjects(LoggingExceptionHandlerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			LoggingExceptionHandlerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}

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
    /// Provides an implementation for <see cref="FlatFileTraceListenerData"/> that
    /// splits policy overrides processing and WMI objects generation, performing appropriate logging of 
    /// policy processing errors.
    /// </summary>
	public class FlatFileTraceListenerDataManageabilityProvider
		: TraceListenerDataManageabilityProvider<FlatFileTraceListenerData>
	{
        /// <summary>
        /// The name of the file name property.
        /// </summary>
		public const String FileNamePropertyName = "fileName";

        /// <summary>
        /// The name of the footer property.
        /// </summary>
		public const String FooterPropertyName = "footer";

        /// <summary>
        /// The name of the header property
        /// </summary>
		public const String HeaderPropertyName = "header";

        /// <summary>
        /// Initialize a new instance <see cref="FlatFileTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
		public FlatFileTraceListenerDataManageabilityProvider()
		{
			FlatFileTraceListenerDataWmiMapper.RegisterWmiTypes();
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
			FlatFileTraceListenerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddEditTextPart(Resources.FlatFileTraceListenerFileNamePartName,
				FileNamePropertyName,
				configurationObject.FileName,
				255,
				true);

			contentBuilder.AddEditTextPart(Resources.FlatFileTraceListenerHeaderPartName,
				HeaderPropertyName,
				configurationObject.Header,
				512,
				false);

			contentBuilder.AddEditTextPart(Resources.FlatFileTraceListenerFooterPartName,
				FooterPropertyName,
				configurationObject.Footer,
				512,
				false);

			AddTraceOptionsPart(contentBuilder, configurationObject.TraceOutputOptions);

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
        protected override void OverrideWithGroupPolicies(FlatFileTraceListenerData configurationObject, IRegistryKey policyKey)
		{
			String fileNameOverride = policyKey.GetStringValue(FileNamePropertyName);
			String footerOverride = policyKey.GetStringValue(FooterPropertyName);
			String formatterOverride = GetFormatterPolicyOverride(policyKey);
			String headerOverride = policyKey.GetStringValue(HeaderPropertyName);
			TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			SourceLevels? filterOverride = policyKey.GetEnumValue<SourceLevels>(FilterPropertyName);

			configurationObject.FileName = fileNameOverride;
			configurationObject.Footer = footerOverride;
			configurationObject.Formatter = formatterOverride;
			configurationObject.Header = headerOverride;
			configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.Filter = filterOverride.Value;
		}

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjects(FlatFileTraceListenerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			FlatFileTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}

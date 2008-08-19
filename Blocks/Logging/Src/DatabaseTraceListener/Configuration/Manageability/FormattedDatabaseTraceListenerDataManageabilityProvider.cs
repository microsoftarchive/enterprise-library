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
using System.Configuration;
using System.Diagnostics;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Manageability.TraceListeners;
using Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Database.Configuration.Manageability
{
	/// <summary>
	/// Represents the behavior required to provide Group Policy updates and to publish the 
	/// <see cref="ConfigurationSetting"/> instances associated to a <see cref="NamedConfigurationElement"/>.
	/// </summary>
	public class FormattedDatabaseTraceListenerDataManageabilityProvider
		: TraceListenerDataManageabilityProvider<FormattedDatabaseTraceListenerData>
	{
        /// <summary>
        /// The name of the add category stored procedure property.
        /// </summary>
		public const String AddCategoryStoredProcNamePropertyName = "addCategoryStoredProcName";

        /// <summary>
        /// The name of the databse instance property.
        /// </summary>
		public const String DatabaseInstanceNamePropertyName = "databaseInstanceName";

        /// <summary>
        /// The name of the write log stored procedure property.
        /// </summary>
		public const String WriteLogStoredProcNamePropertyName = "writeLogStoredProcName";
		
        /// <summary>
        /// The name of the formatter property.
        /// </summary>
        public new const String FormatterPropertyName =
			TraceListenerDataManageabilityProvider<FormattedDatabaseTraceListenerData>.FormatterPropertyName;
		
        /// <summary>
        /// The name of the trace output options property.
        /// </summary>
        public new const String TraceOutputOptionsPropertyName =
			TraceListenerDataManageabilityProvider<FormattedDatabaseTraceListenerData>.TraceOutputOptionsPropertyName;

        /// <summary>
        /// Initialize a new instance of the <see cref="FormattedDatabaseTraceListenerDataManageabilityProvider"/> class.
        /// </summary>
		public FormattedDatabaseTraceListenerDataManageabilityProvider()
		{
			FormattedDatabaseTraceListenerDataWmiMapper.RegisterWmiTypes();
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
			FormattedDatabaseTraceListenerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			List<AdmDropDownListItem> connectionStrings = new List<AdmDropDownListItem>();
			ConnectionStringsSection connectionStringsSection
				= (ConnectionStringsSection)configurationSource.GetSection("connectionStrings");
			if (connectionStringsSection != null)
			{
				foreach (ConnectionStringSettings connectionString in connectionStringsSection.ConnectionStrings)
				{
					connectionStrings.Add(new AdmDropDownListItem(connectionString.Name, connectionString.Name));
				}
			}
			contentBuilder.AddDropDownListPart(Resources.DatabaseTraceListenerDatabasePartName,
				DatabaseInstanceNamePropertyName,
				connectionStrings,
				configurationObject.DatabaseInstanceName);

			contentBuilder.AddEditTextPart(Resources.DatabaseTraceListenerWriteStoreProcPartName,
				WriteLogStoredProcNamePropertyName,
				configurationObject.WriteLogStoredProcName,
				512,
				true);

			contentBuilder.AddEditTextPart(Resources.DatabaseTraceListenerAddCategoryStoreProcPartName,
				AddCategoryStoredProcNamePropertyName,
				configurationObject.AddCategoryStoredProcName,
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
	    protected override void OverrideWithGroupPolicies(FormattedDatabaseTraceListenerData configurationObject, IRegistryKey policyKey)
		{
			String addCategoryStoredProcNameOverride = policyKey.GetStringValue(AddCategoryStoredProcNamePropertyName);
			String databaseInstanceNameOverride = policyKey.GetStringValue(DatabaseInstanceNamePropertyName);
			String formatterOverride = GetFormatterPolicyOverride(policyKey);
			TraceOptions? traceOutputOptionsOverride = policyKey.GetEnumValue<TraceOptions>(TraceOutputOptionsPropertyName);
			String writeLogStoredProcNameOverride = policyKey.GetStringValue(WriteLogStoredProcNamePropertyName);

			configurationObject.AddCategoryStoredProcName = addCategoryStoredProcNameOverride;
			configurationObject.DatabaseInstanceName = databaseInstanceNameOverride;
			configurationObject.Formatter = formatterOverride;
			configurationObject.TraceOutputOptions = traceOutputOptionsOverride.Value;
			configurationObject.WriteLogStoredProcName = writeLogStoredProcNameOverride;
		}

	    /// <summary>
	    /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
	    /// configurationObject.
	    /// </summary>
	    /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
	    /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
	    protected override void GenerateWmiObjects(FormattedDatabaseTraceListenerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			FormattedDatabaseTraceListenerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}

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
using System.Collections.Specialized;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability.Adm;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability
{
    /// <summary>
    /// Provides an implementation for <see cref="FaultContractExceptionHandlerData"/> that
    /// splits policy overrides processing and WMI objects generation, performing approriate logging of 
    /// policy processing errors.
    /// </summary>
	public class FaultContractExceptionHandlerDataManageabilityProvider
		: ConfigurationElementManageabilityProviderBase<FaultContractExceptionHandlerData>
	{
        /// <summary>
        /// The name of the attributes property.
        /// </summary>
		public const String AttributesPropertyName = "attributes";

        /// <summary>
        /// The name of the exception message property.
        /// </summary>
		public const String ExceptionMessagePropertyName = "exceptionMessage";

        /// <summary>
        /// The name of the fault contract type property.
        /// </summary>
		public const String FaultContractTypePropertyName = "faultContractType";

        /// <summary>
        /// Initialize a new instance of the <see cref="FaultContractExceptionHandlerDataManageabilityProvider"/> clas.
        /// </summary>
		public FaultContractExceptionHandlerDataManageabilityProvider()
		{
			FaultContractExceptionHandlerDataWmiMapper.RegisterWmiTypes();
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
			FaultContractExceptionHandlerData configurationObject,
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
			FaultContractExceptionHandlerData configurationObject,
			IConfigurationSource configurationSource,
			String elementPolicyKeyName)
		{
			contentBuilder.AddTextPart(String.Format(CultureInfo.CurrentCulture,
													Resources.HandlerPartNameTemplate,
													configurationObject.Name));

			contentBuilder.AddEditTextPart(Resources.FaultContractExceptionHandlerExceptionMessagePartName,
				elementPolicyKeyName,
				ExceptionMessagePropertyName,
				configurationObject.ExceptionMessage,
				512,
				true);

			contentBuilder.AddEditTextPart(Resources.FaultContractExceptionHandlerFaultContractTypePartName,
				elementPolicyKeyName,
				FaultContractTypePropertyName,
				configurationObject.FaultContractType,
				512,
				true);

			contentBuilder.AddEditTextPart(Resources.FaultContractExceptionHandlerAttributesPartName,
				elementPolicyKeyName,
				AttributesPropertyName,
				GenerateAttributesString(configurationObject.Attributes),
				1024,
				false);
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
        protected override void OverrideWithGroupPolicies(FaultContractExceptionHandlerData configurationObject, IRegistryKey policyKey)
		{
			String attributesOverride = policyKey.GetStringValue(AttributesPropertyName);
			String exceptionMessageOverride = policyKey.GetStringValue(ExceptionMessagePropertyName);
			String faultContractTypeOverride = policyKey.GetStringValue(FaultContractTypePropertyName);

			configurationObject.ExceptionMessage = exceptionMessageOverride;
			configurationObject.FaultContractType = faultContractTypeOverride;

			configurationObject.PropertyMappings.Clear();
			Dictionary<String, String> attributesDictionary = new Dictionary<string, string>();
			KeyValuePairParser.ExtractKeyValueEntries(attributesOverride, attributesDictionary);
			foreach (KeyValuePair<String, String> kvp in attributesDictionary)
			{
				configurationObject.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData(kvp.Key, kvp.Value));
			}
		}

		private static String GenerateAttributesString(NameValueCollection attributes)
		{
			KeyValuePairEncoder encoder = new KeyValuePairEncoder();

			foreach (String key in attributes.AllKeys)
			{
				encoder.AppendKeyValuePair(key, attributes[key]);
			}

			return encoder.GetEncodedKeyValuePairs();
		}

        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        protected override void GenerateWmiObjects(FaultContractExceptionHandlerData configurationObject, 
			ICollection<ConfigurationSetting> wmiSettings)
		{
			FaultContractExceptionHandlerDataWmiMapper.GenerateWmiObjects(configurationObject, wmiSettings);
		}
	}
}

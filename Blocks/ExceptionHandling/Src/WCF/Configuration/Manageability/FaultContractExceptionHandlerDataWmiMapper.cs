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
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.WCF.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for fault contract exception handler configuration to Wmi.
    /// </summary>
	public static class FaultContractExceptionHandlerDataWmiMapper
	{
        /// <summary>
        /// Generate the Wmi objects for the <see cref="FaultContractExceptionHandlerData"/> configuration.
        /// </summary>
        /// <param name="configurationObject">The configuration for the fault contract exception.</param>
        /// <param name="wmiSettings">The Wmi settings.</param>
		public static void GenerateWmiObjects(FaultContractExceptionHandlerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			string[] attributesArray = GenerateAttributesArray(configurationObject.Attributes);

			wmiSettings.Add(
				new FaultContractExceptionHandlerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.ExceptionMessage,
					configurationObject.FaultContractType,
					attributesArray));
		}

		///<summary>
		/// Save changes from the Wmi objects back to configuration.
		///</summary>
		///<param name="faultContractExceptionHandlerSetting">The settings from Wmi.</param>
		///<param name="sourceElement">The configuration element.</param>
		///<returns>true if the changes were saved; otherwise, false.</returns>
		public static bool SaveChanges(FaultContractExceptionHandlerSetting faultContractExceptionHandlerSetting, ConfigurationElement sourceElement)
		{
			FaultContractExceptionHandlerData element = (FaultContractExceptionHandlerData)sourceElement;

			element.Attributes.Clear();
			element.ExceptionMessage = faultContractExceptionHandlerSetting.ExceptionMessage;
			element.FaultContractType = faultContractExceptionHandlerSetting.FaultContractType;
			foreach (string attribute in faultContractExceptionHandlerSetting.Attributes)
			{
				string[] splittedAttribute = attribute.Split('=');
				element.PropertyMappings.Add(new FaultContractExceptionHandlerMappingData(splittedAttribute[0], splittedAttribute[1]));
			}

			return true;
		}

		private static String[] GenerateAttributesArray(NameValueCollection attributes)
		{
			String[] attributesArray = new String[attributes.Count];
			int i = 0;
			foreach (String key in attributes.AllKeys)
			{
				attributesArray[i++] = KeyValuePairEncoder.EncodeKeyValuePair(key, attributes.Get(key));
			}
			return attributesArray;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(FaultContractExceptionHandlerSetting));
		}
	}
}

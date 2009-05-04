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

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for a replace handler configuration to Wmi.
    /// </summary>
	public static class ReplaceHandlerDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="ExceptionHandlingSettings"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(ReplaceHandlerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new ReplaceHandlerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.ExceptionMessage,
					configurationObject.ReplaceExceptionType.AssemblyQualifiedName));
		}

        /// <summary>
        /// Save the changes in a <see cref="ReplaceHandlerData"/> instance.
        /// </summary>
        /// <param name="replaceHandlerSettingSetting">Changed <see cref="ReplaceHandlerSetting"/> instance.</param>
        /// <param name="sourceElement">Parent <see cref="ConfigurationElement"/>.</param>
		public static bool SaveChanges(ReplaceHandlerSetting replaceHandlerSettingSetting, ConfigurationElement sourceElement)
		{
			ReplaceHandlerData element = (ReplaceHandlerData)sourceElement;

			element.ReplaceExceptionTypeName = replaceHandlerSettingSetting.ReplaceExceptionType;
			element.ExceptionMessage = replaceHandlerSettingSetting.ExceptionMessage;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(ReplaceHandlerSetting));
		}
	}
}

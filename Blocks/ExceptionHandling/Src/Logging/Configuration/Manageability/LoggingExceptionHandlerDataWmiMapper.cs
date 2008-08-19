//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System.Collections.Generic;
using System.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Logging.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for logging exception handler data configuration to Wmi.
    /// </summary>
	public static class LoggingExceptionHandlerDataWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="LoggingExceptionHandlerSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(LoggingExceptionHandlerData configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(
				new LoggingExceptionHandlerSetting(configurationObject,
					configurationObject.Name,
					configurationObject.EventId,
					configurationObject.FormatterType.AssemblyQualifiedName,
					configurationObject.LogCategory,
					configurationObject.Priority,
					configurationObject.Severity.ToString(),
					configurationObject.Title));
		}

        /// <summary>
        /// Save the changes in a <see cref="LoggingExceptionHandlerData"/> instance.
        /// </summary>
        /// <param name="loggingExceptionHandlerSettingSetting">Changed <see cref="LoggingExceptionHandlerSetting"/> instance.</param>
        /// <param name="sourceElement">Parent <see cref="ConfigurationElement"/>.</param>
		public static bool SaveChanges(LoggingExceptionHandlerSetting loggingExceptionHandlerSettingSetting, ConfigurationElement sourceElement)
		{
			LoggingExceptionHandlerData element = (LoggingExceptionHandlerData)sourceElement;

			element.EventId = loggingExceptionHandlerSettingSetting.EventId;
			element.Title = loggingExceptionHandlerSettingSetting.Title;
			element.Priority = loggingExceptionHandlerSettingSetting.Priority;
			element.LogCategory = loggingExceptionHandlerSettingSetting.LogCategory;
			element.FormatterTypeName = loggingExceptionHandlerSettingSetting.FormatterType;

			return true;
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(LoggingExceptionHandlerSetting));
		}
	}
}

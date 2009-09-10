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
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Manageability;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
    /// <summary>
    /// Represensta a mapper for exception handling configuration to Wmi.
    /// </summary>
	public static class ExceptionHandlingSettingsWmiMapper
	{
        /// <summary>
        /// Creates the <see cref="ConfigurationSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateWmiObjects(ExceptionHandlingSettings configurationObject,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			// no section wide settings
		}

        /// <summary>
        /// Creates the <see cref="ExceptionTypeSetting"/> instances that describe the configurationObject.
        /// </summary>
        /// <param name="exceptionType"><see cref="ExceptionTypeData"/> instance with Exception settings.</param>
        /// <param name="parentPolicy"><see cref="ExceptionPolicyData" /> instance with Exception Policy settings.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateExceptionTypeWmiObjects(ExceptionTypeData exceptionType,
			ExceptionPolicyData parentPolicy,
			ICollection<ConfigurationSetting> wmiSettings)
		{
			ExceptionTypeSetting wmiSetting
				= new ExceptionTypeSetting(exceptionType,
					exceptionType.Name,
					exceptionType.Type.AssemblyQualifiedName,
					exceptionType.PostHandlingAction.ToString());
			wmiSetting.Policy = parentPolicy.Name;

			wmiSettings.Add(wmiSetting);
		}

        /// <summary>
        /// Creates the <see cref="ExceptionPolicySetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="policy">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
		public static void GenerateExceptionPolicyDataWmiObjects(ExceptionPolicyData policy,
				ICollection<ConfigurationSetting> wmiSettings)
		{
			wmiSettings.Add(new ExceptionPolicySetting(policy, policy.Name));
		}

		internal static void RegisterWmiTypes()
		{
			ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(ExceptionPolicySetting),
				typeof(ExceptionTypeSetting));
		}
	}
}

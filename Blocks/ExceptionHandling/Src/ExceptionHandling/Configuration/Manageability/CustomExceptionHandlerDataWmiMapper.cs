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

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Configuration.Manageability
{
    /// <summary>
    /// Represents a mapper for custom exception handler configuration to Wmi.
    /// </summary>
    public static class CustomExceptionHandlerDataWmiMapper
    {
        /// <summary>
        /// Creates the <see cref="CustomHandlerSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="data">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        public static void GenerateWmiObjects(CustomHandlerData data,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new CustomHandlerSetting(data,
                                         data.Name,
                                         data.Type.AssemblyQualifiedName,
                                         CustomDataWmiMapperHelper.GenerateAttributesArray(data.Attributes)));
        }

        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(CustomHandlerSetting));
        }

        internal static bool SaveChanges(CustomHandlerSetting customHandlerSetting,
                                         ConfigurationElement sourceElement)
        {
            CustomHandlerData element = (CustomHandlerData)sourceElement;

            element.Attributes.Clear();
            foreach (string attribute in customHandlerSetting.Attributes)
            {
                string[] splittedAttribute = attribute.Split('=');
                element.Attributes.Add(splittedAttribute[0], splittedAttribute[1]);
            }

            return true;
        }
    }
}
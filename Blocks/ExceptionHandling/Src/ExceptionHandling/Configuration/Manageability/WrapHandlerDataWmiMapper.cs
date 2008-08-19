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
    /// Represents a mapper for wrap hanlder configuration to Wmi.
    /// </summary>
    public static class WrapHandlerDataWmiMapper
    {
        /// <summary>
        /// Creates the <see cref="WrapHandlerSetting"/> instances that describe the 
        /// configurationObject.
        /// </summary>
        /// <param name="configurationObject">The configuration object for instances that must be managed.</param>
        /// <param name="wmiSettings">A collection to where the generated WMI objects are to be added.</param>
        public static void GenerateWmiObjects(WrapHandlerData configurationObject,
                                              ICollection<ConfigurationSetting> wmiSettings)
        {
            wmiSettings.Add(
                new WrapHandlerSetting(configurationObject,
                                       configurationObject.Name,
                                       configurationObject.ExceptionMessage,
                                       configurationObject.WrapExceptionType.AssemblyQualifiedName));
        }

        internal static void RegisterWmiTypes()
        {
            ManagementEntityTypesRegistrar.SafelyRegisterTypes(typeof(WrapHandlerSetting));
        }

        /// <summary>
        /// Save the changes in a <see cref="WrapHandlerData"/> instance.
        /// </summary>
        /// <param name="wrapHandlerSettingSetting">Changed <see cref="WrapHandlerSetting"/> instance.</param>
        /// <param name="sourceElement">Parent <see cref="ConfigurationElement"/>.</param>
        public static bool SaveChanges(WrapHandlerSetting wrapHandlerSettingSetting,
                                       ConfigurationElement sourceElement)
        {
            WrapHandlerData element = (WrapHandlerData)sourceElement;

            element.WrapExceptionTypeName = wrapHandlerSettingSetting.WrapExceptionType;
            element.ExceptionMessage = wrapHandlerSettingSetting.ExceptionMessage;

            return true;
        }
    }
}
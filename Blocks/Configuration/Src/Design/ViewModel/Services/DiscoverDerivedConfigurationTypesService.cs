//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Core
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using System.ComponentModel;

namespace Console.Wpf.ViewModel.Services
{
    public class DiscoverDerivedConfigurationTypesService
    {
        private readonly AssemblyLocator assemblyLocator;

        public DiscoverDerivedConfigurationTypesService(AssemblyLocator assemblyLocator)
        {
            this.assemblyLocator = assemblyLocator;
        }

        public IEnumerable<Type> FindAvailableConfigurationElementTypes(Type baseType)
        {
            foreach (var assembly in assemblyLocator.Assemblies)
            {
                var typeList =
                    assembly.GetExportedTypes().Where(
                        t => TypeSpecifiesConfigurationElement(t, baseType))
                        .Select(t => GetDerivedElementType(t));

                foreach (var type in typeList)
                {
                    yield return type;
                }
            }
        }

        private static bool TypeSpecifiesConfigurationElement(Type handlerType, Type baseConfigurationElementType)
        {
            var configAttribute = GetConfigurationElementTypeAttribute(handlerType);
            if (configAttribute != null)
                return baseConfigurationElementType.IsAssignableFrom(configAttribute.ConfigurationType);
            return false;
        }

        private static ConfigurationElementTypeAttribute GetConfigurationElementTypeAttribute(Type handlerType)
        {
            var configAttribute = 
                (from attr in handlerType.GetCustomAttributes(typeof(ConfigurationElementTypeAttribute), true)
                 select (ConfigurationElementTypeAttribute)attr)
                    .FirstOrDefault();
            return configAttribute;
        }

        private static Type GetDerivedElementType(Type handlerType)
        {
            var configAttribute = GetConfigurationElementTypeAttribute(handlerType);
            if (configAttribute == null) return null; //todo: if the caller of this function receives null, it would return null's in its Ienum
            return configAttribute.ConfigurationType;
        }

    }
}

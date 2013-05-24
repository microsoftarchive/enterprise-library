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
using System.Configuration;
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Extensions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services.PlatformProfile;
using Microsoft.Practices.EnterpriseLibrary.Validation;
using Microsoft.Practices.Unity;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services
{
    /// <summary>
    /// Service class used to find configuration classes that are used to configure Enterprise Library providers.
    /// </summary>
    /// <remarks>
    /// In order to get an instance of this class, declare it as a constructor argument on the consuming component or use the <see cref="IUnityContainer"/> to obtain an instance from code.
    /// </remarks>
    public class DiscoverDerivedConfigurationTypesService
    {
        private readonly AssemblyLocator assemblyLocator;
        private readonly Profile profile;

        /// <summary>
        /// This constructor supports the configuration design-time and is not intended to be used directly from your code.
        /// </summary>
        public DiscoverDerivedConfigurationTypesService(AssemblyLocator assemblyLocator)
            : this(assemblyLocator, new Profile())
        {
        }

        /// <summary>
        /// This constructor supports the configuration design-time and is not intended to be used directly from your code.
        /// </summary>
        public DiscoverDerivedConfigurationTypesService(AssemblyLocator assemblyLocator, Profile profile)
        {
            this.assemblyLocator = assemblyLocator;
            this.profile = profile;
        }

        /// <summary>
        /// Check if type is filtered by the profile or not.
        /// </summary>
        /// <param name="type">The actual type.</param>
        /// <returns>True is type should be included.</returns>
        public bool CheckType(Type type)
        {
            return this.profile.Check(type);
        }

        /// <summary>
        /// Finds all the configuration classes that are used to configure a specific Enterprise Library provider base-type, such as <see cref="Validator"/>.
        /// </summary>
        /// <param name="baseType">The Enterprise Library provider base-type, such as <see cref="Validator"/>.</param>
        /// <returns>A list of <see cref="ConfigurationElement"/> types that can be used to configure providers derived of <paramref name="baseType"/>.</returns>
        public IEnumerable<Type> FindAvailableConfigurationElementTypes(Type baseType)
        {
            IEnumerable<Type> typeList =
                assemblyLocator.Assemblies
                    .FilterSelectManySafe(a =>
                                    a.GetExportedTypes()
                                        .Where(t => TypeSpecifiesConfigurationElement(t, baseType))
                                        .FilterSelectSafe(t => GetDerivedElementType(t))).ToArray();

            return typeList;
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
                handlerType.GetCustomAttributes(typeof(ConfigurationElementTypeAttribute), true)
                .Select(attr => (ConfigurationElementTypeAttribute)attr)
                .FirstOrDefault();
            return configAttribute;
        }

        private static Type GetDerivedElementType(Type handlerType)
        {
            var configAttribute = GetConfigurationElementTypeAttribute(handlerType);

            return configAttribute.ConfigurationType;
        }
    }
}

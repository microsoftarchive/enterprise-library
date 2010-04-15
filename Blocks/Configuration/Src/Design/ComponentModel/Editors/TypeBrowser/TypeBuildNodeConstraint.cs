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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.Design;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ComponentModel.Editors
{
    /// <summary>
    /// Represents a constraint on a <see cref="Type"/>.
    /// </summary>
    public class TypeBuildNodeConstraint
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TypeBuildNodeConstraint"/> class.
        /// </summary>
        /// <param name="baseType">The base type (class or interface) from which the constrained type should derive.</param>
        /// <param name="configurationType">The base type from which a type specified by the 
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementTypeAttribute"/>
        /// bound to the constrained type should derive, or <see langword="null"/> if no such constraint is necessary.
        /// </param>
        /// <param name="typeSelectorIncludes">Additional constraints.</param>
        public TypeBuildNodeConstraint(
            Type baseType,
            Type configurationType,
            TypeSelectorIncludes typeSelectorIncludes)
        {
            this.BaseType = baseType;
            this.ConfigurationType = configurationType;
            this.TypeSelectorIncludes = typeSelectorIncludes;

            includeAbstractTypes = IsSet(TypeSelectorIncludes.AbstractTypes);
            includeInterfaces = IsSet(TypeSelectorIncludes.Interfaces);
            includeNonPublicTypes = IsSet(TypeSelectorIncludes.NonpublicTypes);
            includeBaseType = IsSet(TypeSelectorIncludes.BaseType);
        }

        private bool IsSet(TypeSelectorIncludes compareFlag)
        {
            return ((this.TypeSelectorIncludes & compareFlag) == compareFlag);
        }

        /// <summary>
        /// Gets the base type (class or interface) from which the constrained type should derive.
        /// </summary>
        public Type BaseType { get; internal set; }

        /// <summary>
        /// Gets the base type from which a type specified by the 
        /// <see cref="Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ConfigurationElementTypeAttribute"/> 
        /// bound to the constrained type should derive, or <see langword="null"/> if no such constraint is necessary.
        /// </summary>
        public Type ConfigurationType { get; internal set; }

        /// <summary>
        /// Gets the additional constraints.
        /// </summary>
        public TypeSelectorIncludes TypeSelectorIncludes { get; internal set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public bool Matches(Type type)
        {
            if (!this.BaseType.IsAssignableFrom(type))
            {
                var resolvedType = Type.GetType(type.AssemblyQualifiedName, false);
                if (resolvedType == null                                // couldn't resolve it
                    || resolvedType == type                             // resolve to the same type we had, which already failed to match
                    || !this.BaseType.IsAssignableFrom(resolvedType))   // is a different, non-null type but doesn't match
                    return false;

                type = resolvedType;                                    // the new type matches, keep using it
            }

            if (!this.includeInterfaces && type.IsInterface)
                return false;

            if (!includeAbstractTypes && (type.IsAbstract && !type.IsInterface))
                return false;

            if (!this.includeBaseType && (type == this.BaseType))
                return false;

            if (!includeNonPublicTypes && !(type.IsPublic || type.IsNestedPublic))
                return false;

            if (this.ConfigurationType != null)
            {
                var attribute =
                    type.GetCustomAttributes(typeof(ConfigurationElementTypeAttribute), true)
                        .Cast<ConfigurationElementTypeAttribute>()
                        .FirstOrDefault();

                if (attribute == null || this.ConfigurationType != attribute.ConfigurationType)
                    return false;
            }

            return true;
        }

        readonly bool includeBaseType;
        readonly bool includeAbstractTypes;
        readonly bool includeInterfaces;
        readonly bool includeNonPublicTypes;

        /// <summary>
        /// Gets a representation of the constraint.
        /// </summary>
        /// <returns>A string representing the constraint.</returns>
        public string GetDisplayString()
        {
            StringBuilder displayBuilder = new StringBuilder();
            if (BaseType == typeof(object))
            {
                displayBuilder.Append(Resources.TypeBrowserPickAnyType);
            }
            else
            {
                displayBuilder.AppendFormat(CultureInfo.CurrentCulture, Resources.TypeBrowerPickTypeAssignableFrom, BaseType.Name);
            }

            if (ConfigurationType != null)
            {
                displayBuilder.AppendFormat(CultureInfo.CurrentCulture, Resources.TypeBrowserPickTypeWithConfigurationElement, ConfigurationType.Name);
            }

            return displayBuilder.ToString();
        }
    }
}

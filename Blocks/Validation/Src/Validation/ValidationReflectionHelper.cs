//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Globalization;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Validation.Properties;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Validation
{
    internal static class ValidationReflectionHelper
    {
        public static PropertyInfo GetProperty(Type type, string propertyName, bool throwIfInvalid)
        {
            if (string.IsNullOrEmpty(propertyName))
            {
                throw new ArgumentNullException("propertyName");
            }

            PropertyInfo propertyInfo = type.GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);

            if (!IsValidProperty(propertyInfo))
            {
                if (throwIfInvalid)
                {
                    throw new ArgumentException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ExceptionInvalidProperty,
                            propertyName,
                            type.FullName));
                }
                else
                {
                    return null;
                }
            }

            return propertyInfo;
        }

        public static bool IsValidProperty(PropertyInfo propertyInfo)
        {
            return null != propertyInfo				// exists
                    && propertyInfo.CanRead			// and it's readable
                    && propertyInfo.GetIndexParameters().Length == 0;	// and it's not an indexer
        }

        public static FieldInfo GetField(Type type, string fieldName, bool throwIfInvalid)
        {
            if (string.IsNullOrEmpty(fieldName))
            {
                throw new ArgumentNullException("fieldName");
            }

            FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);

            if (!IsValidField(fieldInfo))
            {
                if (throwIfInvalid)
                {
                    throw new ArgumentException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ExceptionInvalidField,
                            fieldName,
                            type.FullName));
                }
                else
                {
                    return null;
                }
            }

            return fieldInfo;
        }

        public static bool IsValidField(FieldInfo fieldInfo)
        {
            return null != fieldInfo;
        }

        public static MethodInfo GetMethod(Type type, string methodName, bool throwIfInvalid)
        {
            if (string.IsNullOrEmpty(methodName))
            {
                throw new ArgumentNullException("methodName");
            }

            MethodInfo methodInfo = type.GetMethod(methodName, BindingFlags.Public | BindingFlags.Instance, null, Type.EmptyTypes, null);

            if (!IsValidMethod(methodInfo))
            {
                if (throwIfInvalid)
                {
                    throw new ArgumentException(
                        string.Format(
                            CultureInfo.CurrentCulture,
                            Resources.ExceptionInvalidMethod,
                            methodName,
                            type.FullName));
                }
                else
                {
                    return null;
                }
            }

            return methodInfo;
        }

        public static bool IsValidMethod(MethodInfo methodInfo)
        {
            return null != methodInfo
                && typeof(void) != methodInfo.ReturnType
                && methodInfo.GetParameters().Length == 0;
        }

        public static T ExtractValidationAttribute<T>(ICustomAttributeProvider attributeProvider, string ruleset)
            where T : BaseValidationAttribute
        {
            if (attributeProvider != null)
            {
                foreach (T attribute in attributeProvider.GetCustomAttributes(typeof(T), false))
                {
                    if (ruleset.Equals(attribute.Ruleset))
                    {
                        return attribute;
                    }
                }
            }

            return null;
        }
    }
}
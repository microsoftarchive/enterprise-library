//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Policy Injection Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.CallHandlers.Configuration.Design
{
    /// <summary>
    /// Validation attribute for validating <see cref="LogCategory"/> instances.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class LogCategoriesAttribute : ValidationAttribute
    {
        /// <summary>
        /// Perform validation on the given <paramref name="instance"/>.
        /// </summary>
        /// <param name="instance">object to validate.</param>
        /// <param name="propertyInfo">The underlying property to validate on the instance.</param>
        /// <param name="errors">list to add errors to.</param>
        protected override void ValidateCore(object instance,
                                             PropertyInfo propertyInfo,
                                             IList<ValidationError> errors)
        {
            List<string> categoryValues = new List<string>();

            object propertyValue = propertyInfo.GetValue(instance, null);
            IEnumerable<LogCategory> listOfLogCategories = propertyValue as IEnumerable<LogCategory>;
            if (listOfLogCategories != null)
            {
                foreach (LogCategory category in listOfLogCategories)
                {
                    if (string.IsNullOrEmpty(category.CategoryName))
                    {
                        errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, Resources.LogCategoriesContainsEmptyMatch));
                        break;
                    }
                    else if (categoryValues.Contains(category.CategoryName))
                    {
                        errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, Resources.LogCategoriesContainsDuplicateMatch));
                        break;
                    }
                    else
                    {
                        categoryValues.Add(category.CategoryName);
                    }
                }
            }
        }
    }
}

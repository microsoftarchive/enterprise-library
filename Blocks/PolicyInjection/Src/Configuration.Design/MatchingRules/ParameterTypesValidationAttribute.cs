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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.PolicyInjection.Configuration.Design.MatchingRules
{
	/// <summary>
	/// Performs validation on a collection of <see cref="ParameterType"/>.
	/// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public sealed class ParameterTypesValidationAttribute : ValidationAttribute
    {
		/// <summary>
		/// Validate the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
		/// </summary>
		/// <param name="instance">
		/// The instance to validate.
		/// </param>
		/// <param name="propertyInfo">
		/// The property containing the value to validate.
		/// </param>
		/// <param name="errors">
		/// The collection to add any errors that occur during the validation.
		/// </param>
		/// <remarks>
		/// The value of property <paramref name="propertyInfo"/> on <paramref name="instance"/> must be an <see cref="IEnumerable{T}"/> 
		/// of <see cref="ParameterType"/>. It may have no elements, each element must contain a value and 
		/// the names of the values cannot be repeated among elements.
		/// </remarks>
		protected override void ValidateCore(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors)
        {
            List<string> parameterNames = new List<string>();
            object propertyValue = propertyInfo.GetValue(instance, null);
            IEnumerable<ParameterType> listOfParameterTypes = propertyValue as IEnumerable<ParameterType>;
            if (listOfParameterTypes != null)
            {
                foreach (ParameterType parameterType in listOfParameterTypes)
                {
                    if(string.IsNullOrEmpty(parameterType.Name))
                    {
                        errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, Resources.ParameterTypeCollectionContainsEmptyName));
                        break;
                    }
                    if (string.IsNullOrEmpty(parameterType.Type))
                    {
                        errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, Resources.ParameterTypeCollectionContainsEmptyType));
                        break;
                    }
                    if (parameterNames.Contains(parameterType.Name))
                    {
                        errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, Resources.ParameterTypeCollectionContainsDuplicateName));
                        break;
                    }
                    else
                    {
                        parameterNames.Add(parameterType.Name);
                    }
                }
            }
        }
    }
}

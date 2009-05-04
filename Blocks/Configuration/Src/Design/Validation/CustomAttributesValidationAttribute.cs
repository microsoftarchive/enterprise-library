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
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Validates the attributes for a custom provider.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
    public sealed class CustomAttributesValidationAttribute : ValidationAttribute
    {
		/// <summary>
		/// Validate the attribute data for the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
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
		protected override void ValidateCore(object instance, System.Reflection.PropertyInfo propertyInfo, IList<ValidationError> errors)
        {
            List<EditableKeyValue> customAttributes = propertyInfo.GetValue(instance, new object[0]) as List<EditableKeyValue>;
            if (customAttributes != null)
            {
                int position = -1;
                List<string> keys = new List<string>();
                foreach (EditableKeyValue item in customAttributes)
                {
                    position++;
                    if (string.IsNullOrEmpty(item.Key))
                    {
                        string errorMessage = string.Format(Resources.Culture, Resources.CustomAttributesKeyNullError, position);
                        errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, errorMessage));
                        continue;
                    }
                    if (keys.Contains(item.Key))
                    {
                        string errorMessage = string.Format(Resources.Culture, Resources.CustomAttributesDuplicateKeyError, item.Key);
                        errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, errorMessage));
                        continue;
                    }

                    keys.Add(item.Key);
                    
                }
            }
        }
    }
}

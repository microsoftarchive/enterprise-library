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
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Specifies a property or event that is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited=true)]
    public sealed class RequiredAttribute : ValidationAttribute
    {
        /// <summary>
        /// Initialize a new instance of the <see cref="RequiredAttribute"/> class
        /// </summary>
        public RequiredAttribute() 
        {
        }

        /// <summary>
        /// Validate the required data for the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
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
		protected override void ValidateCore(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors)
        {	
            object propertyValue = propertyInfo.GetValue(instance, null);
			string valueString = propertyValue as String;
            if (valueString != null)
            {                
                if (string.IsNullOrEmpty(valueString))
                {
                    errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, string.Format(Resources.Culture, Resources.ExceptionValueNullMessage, propertyInfo.Name)));
                }
            }
            else if (!(propertyValue is ValueType))
            {
                if (propertyValue == null)
                {
                    errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, string.Format(Resources.Culture, Resources.ExceptionValueNullMessage, propertyInfo.Name)));
                }
            }
        }
    }
}

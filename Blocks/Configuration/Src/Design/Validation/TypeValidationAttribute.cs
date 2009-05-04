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
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Specifies a type name should be validated to ensure that it is a valid <see cref="Type"/>.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited=true)]
	public sealed class TypeValidationAttribute : ValidationAttribute
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="TypeValidationAttribute"/> class.
        /// </summary>
		public TypeValidationAttribute()
		{
		}

        /// <summary>
        /// Validate the <see cref="Type"/> for the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
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
            string typeName = propertyValue as string;
            // the required attribute will catch this for us
            if (string.IsNullOrEmpty(typeName)) return;
            Type t = Type.GetType(typeName, false, true);
            if (t == null)
            {
                string name = propertyInfo.Name;
                errors.Add(new ValidationError(instance as ConfigurationNode, name, string.Format(CultureInfo.CurrentUICulture, Resources.ExceptionTypeNotValid, typeName)));
            }
        }
	}
}

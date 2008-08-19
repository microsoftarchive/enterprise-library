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
using System.Collections.Generic;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System.Text.RegularExpressions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Configuration.Design.ValidatorNodes
{
    /// <summary>
    /// Specifies a property or event will be validated on whether it is a valid regular expression.
    /// </summary>
    public class ValidRegexAttribute : ValidationAttribute
    {
        /// <summary>
        /// Validate the given <paramref name="instance"/> and the <paramref name="propertyInfo"/> on whether it contains a valid regular expression.
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
            string propertyValue = propertyInfo.GetValue(instance, null) as string;
            if (!String.IsNullOrEmpty(propertyValue))
            {
                try
                {
                    Regex regEx = new Regex(propertyValue);
                }
                catch (ArgumentException ae)
                {
                    errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, string.Format(Resources.InvalidRegularExpressionErrorMessage, propertyInfo.Name, ae.Message)));
                }
            }
        }
    }
}

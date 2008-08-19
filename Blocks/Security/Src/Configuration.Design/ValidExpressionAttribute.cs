//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Security Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Security;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;

namespace Microsoft.Practices.EnterpriseLibrary.Security.Configuration.Design
{
    /// <summary>
    /// Represents an attribute that will validate a property or field using an exrpression.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited=true)]
    public sealed class ValidExpressionAttribute : ValidationAttribute
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
        protected override void ValidateCore(object instance, PropertyInfo propertyInfo, IList<ValidationError> errors)
        {
            string expression = (string)propertyInfo.GetValue(instance, null);
            if (expression != null
                && expression.Length != 0)
            {
                try
                {
                    Parser parser = new Parser();
                    parser.Parse(expression);
                }
                catch (SyntaxException e)
                {
                    errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, e.Message));
                }
            }
        }
    }
}
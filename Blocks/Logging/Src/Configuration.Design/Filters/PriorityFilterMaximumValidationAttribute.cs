//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Logging Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design;
using Microsoft.Practices.EnterpriseLibrary.Logging.Filters;
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Logging.Configuration.Design.Filters
{
    /// <summary>
    /// Represents a validation for the maximum priority of a filter.
    /// </summary>
	[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
	public sealed class PriorityFilterMaximumPriorityValidationAttribute : ValidationAttribute
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
        protected override void ValidateCore(object instance, System.Reflection.PropertyInfo propertyInfo, IList<ValidationError> errors)
        {
			PriorityFilterNode node = instance as PriorityFilterNode;
			if (node != null)
			{
				if (node.MaximumPriority != null && node.MinimumPriority >= node.MaximumPriority)
				{
					string errorMessage = string.Format(CultureInfo.CurrentUICulture, Resources.MaxPrioShouldBeGreaterThanMinPrioError);
                    errors.Add(new ValidationError(node, propertyInfo.Name, errorMessage));
				}
			}
        }
    }
}

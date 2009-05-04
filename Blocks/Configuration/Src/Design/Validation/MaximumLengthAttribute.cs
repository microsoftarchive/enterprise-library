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
using System.Globalization;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Specifies a property or event will be validated on a specific maximum length.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited=true)]
    public sealed class MaximumLengthAttribute : ValidationAttribute
    {
		private readonly int maximumLength;

        /// <summary>
        /// Initialize a new instance of the <see cref="MaximumLengthAttribute"/> class with a maximum length.
        /// </summary>
        /// <param name="maximumLength">
        /// The maximum length.
        /// </param>
        public MaximumLengthAttribute(int maximumLength)
        {
            this.maximumLength = Math.Abs(maximumLength);
        }

		/// <summary>
		/// Gets the maximum length.
		/// </summary>
		/// <value>
		/// The maximum length.
		/// </value>
		public int MaximumLength
		{
			get { return maximumLength; }
		} 

        /// <summary>
        /// Validate the range data for the given <paramref name="instance"/> and the <paramref name="propertyInfo"/>.
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
			string valueString = propertyValue as string;
            if (null != valueString)
            {
				if (valueString.Length > maximumLength)
                {
                    string name = propertyInfo.Name;
                    errors.Add(new ValidationError(instance as ConfigurationNode, name, string.Format(CultureInfo.CurrentUICulture, Resources.MaxLengthExceededErrorMessage, name, maximumLength)));
                }
            }
        }
    }
}

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
using System.IO;
using System.Reflection;
using Microsoft.Practices.EnterpriseLibrary.Common;
using System.Collections.Generic;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Validates that a file can be created or is writable.
    /// <remarks>
    /// This validation assumes that the property is a file.
    /// </remarks>
    /// </summary>
	[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1813:AvoidUnsealedAttributes"), AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, Inherited = true)]
	public class FileValidationAttribute : ValidationAttribute
	{
        /// <summary>
        /// Initialize a new instance of the <see cref="FileValidationAttribute"/> class.
        /// </summary>
		public FileValidationAttribute()
		{
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
            string fileName = propertyValue as string;

            if (string.IsNullOrEmpty(fileName))
            {
                return;
            }
            try
            {
                if (!Path.IsPathRooted(fileName))
                {
                    string contextPath = ContextPath;
                    if (string.IsNullOrEmpty(contextPath)) return; // I have to assume this is ok since I can't get the real path				
                    fileName = Path.Combine(contextPath, fileName);
                }
            }
            catch (ArgumentException e)
            {
                errors.Add(new ValidationError(instance as ConfigurationNode, propertyInfo.Name, e.Message));
                return;
            }
        }

        /// <summary>
        /// Returns the path which should be used to validate relative paths.
        /// </summary>
        /// <value>The path from the current configuration file.</value>
        protected virtual string ContextPath
        {
			get 
			{
				string configFile = ServiceHelper.GetCurrentStorageService(ServiceProvider).ConfigurationFile;
				if (string.IsNullOrEmpty(configFile)) return string.Empty;
				return Path.GetDirectoryName(configFile);
			}			
        }
	}
}

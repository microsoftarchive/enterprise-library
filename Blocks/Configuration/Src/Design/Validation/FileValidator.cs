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
using System.IO;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Services;
using Microsoft.Practices.Unity;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// A <see cref="FileValidator"/> instance that validates whether the value is an existing file.
    /// </summary>
    public class FileValidator : Validator
    {
        /// <summary>
        /// Gets or sets the <see cref="IApplicationModel"/> that is used to obtain contextual information used for validation.
        /// </summary>
        /// <remarks>
        /// This is an Dependency property and will be set when this class is constructed.
        /// </remarks>
        [Dependency]
        public IApplicationModel ApplicationModel { get; set; }

        /// <summary>
        /// Returns the path which should be used to validate relative paths.
        /// </summary>
        /// <value>The path from the current configuration file.</value>
        protected virtual string ContextPath
        {
            get
            {
                string configFile = ApplicationModel.ConfigurationFilePath;
                if (string.IsNullOrEmpty(configFile)) return string.Empty;
                return Path.GetDirectoryName(configFile);
            }
        }

        /// <summary>
        /// Validates whether <paramref name="value"/> is a valid file path then calls <see cref="InnerValidateCore"/> passing a rooted file path.
        /// </summary>
        /// <param name="instance">The instance to validate, this is expected to be a <see cref="Property"/></param>
        /// <param name="value">The value that should be validated.</param>
        /// <param name="results">The collection to add any results that occur during the validation.</param>		
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "f")]
        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var property = instance as Property;
            if (property == null) return;

            var path = value;

            if (string.IsNullOrEmpty(path))
            {
                return;
            }
            try
            {
                //will throw with descriptive exception message to validate whether file path is correct
                FileInfo f = new FileInfo(path);
                
                if (!Path.IsPathRooted(path))
                {
                    string contextPath = ContextPath;
                    path = Path.Combine(contextPath, path);
                }

                InnerValidateCore(property, path, results);
            }
            catch (ArgumentException e)
            {
                results.Add(new PropertyValidationResult(property, e.Message, true));
                return;
            }
        }

        /// <summary>
        /// Determines if the specified path is network path.
        /// </summary>
        /// <param name="path">The path to test.</param>
        /// <returns>Returns <see langword="true"/> if the path </returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison", MessageId = "System.String.StartsWith(System.String,System.StringComparison)"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unc"), System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", MessageId = "UNC")]
        public static bool IsUnc(string path)
        {
            return !string.IsNullOrEmpty(path) && path.StartsWith("\\", StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// When overridden in a derived class performs validation on <paramref name="fileName"/>.
        /// </summary>
        /// <param name="instance">The <see cref="Property"/> instance that declares <paramref name="fileName"/> as a value.</param>
        /// <param name="fileName">A rooted and valid file path</param>
        /// <param name="errors">The collection to add any results that occur during the validation.</param>	
        protected virtual void InnerValidateCore(Property instance, string fileName, IList<ValidationResult> errors)
        {
        }
    }
}

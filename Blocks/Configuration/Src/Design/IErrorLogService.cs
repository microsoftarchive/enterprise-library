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

using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using System;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design
{
    /// <summary>
    /// Provides a service to collect errors while processing commands for nodes.
    /// </summary>
    public interface IErrorLogService
    {
		/// <summary>
		/// When implemented by a class, gets the number of validation errors that occurred.
		/// </summary>
		/// <value>
		/// The number of validation errors that occurred.
		/// </value>
		int ValidationErrorCount { get; }

		/// <summary>
		/// When implemented by a class, gets the number of configuration errors that occurred.
		/// </summary>
		/// <value>
		/// The number of configuration errors that occurred.
		/// </value>
		int ConfigurationErrorCount { get; }

		/// <summary>
		/// When implemented by a class, performs the specified action on each validation error.
		/// </summary>
		/// <param name="action">The Action to perform on each element.</param>
		void ForEachValidationErrors(Action<ValidationError> action);

		/// <summary>
		/// When implemented by a class, performs the specified action on each configuration error.
		/// </summary>
		/// <param name="action">The Action to perform on each element.</param>
		void ForEachConfigurationErrors(Action<ConfigurationError> action);

        /// <summary>
		/// When implemented by a class, clears all the errors in the log.
        /// </summary>
        void ClearErrorLog();

        /// <summary>
		/// When implemented by a class, log a validation error.
        /// </summary>
        /// <param name="validationError">The <see cref="ValidationError"/> to log.</param>
        void LogError(ValidationError validationError);

        /// <summary>
        /// When implemented by a class, log a configuration error.
        /// </summary>
        /// <param name="configurationError">The <see cref="ConfigurationError"/> to log</param>
        void LogError(ConfigurationError configurationError);


        /// <summary>
        /// When implemented by a class, logs the configuration errors from within a <see cref="ConfigurationErrorsException"/>.
        /// </summary>
        /// <param name="configurationErrors">The <see cref="ConfigurationErrorsException"/> that contains errors to log.</param>
        void LogErrors(ConfigurationErrorsException configurationErrors);
    }
}
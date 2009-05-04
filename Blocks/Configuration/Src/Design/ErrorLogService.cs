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
    public class ErrorLogService : IErrorLogService
    {
        private List<ConfigurationError> configurationErrors;
        private List<ValidationError> validationErrors;

		/// <summary>
		/// Initialize a new instance of the <see cref="ErrorLogService"/> class.
		/// </summary>
        public ErrorLogService()
        {
            configurationErrors = new List<ConfigurationError>();
            validationErrors = new List<ValidationError>();
        }

		/// <summary>
		/// Gets the number of validation errors that occurred.
		/// </summary>
		/// <value>
		/// The number of validation errors that occurred.
		/// </value>
		public int ValidationErrorCount { get { return validationErrors.Count; } }

		/// <summary>
		/// Gets the number of configuration errors that occurred.
		/// </summary>
		/// <value>
		/// The number of configuration errors that occurred.
		/// </value>
		public int ConfigurationErrorCount { get { return configurationErrors.Count; } }

		/// <summary>
		/// Performs the specified action on each validation error.
		/// </summary>
		/// <param name="action">The Action to perform on each element.</param>
		public void ForEachValidationErrors(Action<ValidationError> action)
		{
			for (int index = 0; index < validationErrors.Count; index++)
			{
				action(validationErrors[index]);
			}
		}

		/// <summary>
		/// Performs the specified action on each configuration error.
		/// </summary>
		/// <param name="action">The Action to perform on each element.</param>
		public void ForEachConfigurationErrors(Action<ConfigurationError> action)
		{
			for (int index = 0; index < configurationErrors.Count; index++)
			{
				action(configurationErrors[index]);
			}
		}

		/// <summary>
		/// Clears all the errors in the log.
		/// </summary>
        public void ClearErrorLog()
        {
            validationErrors.Clear();
            configurationErrors.Clear();
        }

		/// <summary>
		/// Log a validation error.
		/// </summary>
		/// <param name="validationError">The <see cref="ValidationError"/> to log.</param>
        public void LogError(ValidationError validationError)
        {
            validationErrors.Add(validationError);
        }

		/// <summary>
		/// Log a configuration error.
		/// </summary>
		/// <param name="configurationError">The <see cref="ConfigurationError"/> to log</param>
        public void LogError(ConfigurationError configurationError)
        {
            configurationErrors.Add(configurationError);
        }

        /// <summary>
        /// Log the configuration errors from within a <see cref="ConfigurationErrorsException"/>.
        /// </summary>
        /// <param name="configurationErrors">The <see cref="ConfigurationErrorsException"/> that contains errors to log.</param>
        public void LogErrors(ConfigurationErrorsException configurationErrors)
        {
            foreach (object error in configurationErrors.Errors)
            {
                ConfigurationError configurationError = error as ConfigurationError;
                if (configurationError != null)
                {
                    LogError(configurationError);
                }
                else
                {
                    ConfigurationErrorsException innerException = error as ConfigurationErrorsException;
                    if (innerException != null)
                    {
                        LogErrors(innerException);
                    }
                }
            }
        }

    }
}

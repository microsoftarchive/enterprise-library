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
using System.Runtime.Serialization;
using System.Security.Permissions;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Collects information relevant to a warning or error returned by validation.
    /// </summary>    
    public class ValidationError
    {
        private readonly string message;
        private readonly string propertyName;
        private readonly bool isWarning;

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidationError"/> calss with the invalid object, property name, and error message.
        /// </summary>
        /// <param name="propertyName">
        /// The name of the property that is invalid.
        /// </param>
        /// <param name="errorMessage">
        /// The message that describes the error.
        /// </param>
        public ValidationError(string propertyName, string errorMessage, string elementPath)
        {
            if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "propertyName");
            if (string.IsNullOrEmpty(errorMessage)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "errorMessage");
            if (string.IsNullOrEmpty(elementPath)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "elementPath");

            this.propertyName = propertyName;
            this.message = errorMessage;
            ElementPath = elementPath;
        }

        /// <summary>
        /// Gets the property name of the failed validation.
        /// </summary>
        /// <value>
        /// The property name of the failed validation.
        /// </value>
        public string PropertyName
        {
            get { return this.propertyName; }
        }

        /// <summary>
        /// Gets or sets the message for the error.
        /// </summary>
        /// <value>
        /// The message for the error.
        /// </value>
        public string Message
        {
            get { return this.message; }
        }

        /// <summary>
        /// Returns true if the validation error is a warning.
        /// </summary>
        public bool IsWarning
        {
            get { return this.isWarning; }
        }

        ///<summary>
        /// The <see cref="ElementViewModel.Path"/> reference of the containing element producing the valiation error.
        ///</summary>
        public string ElementPath { get; private set; }

        /// <summary>
        /// Returns the string representatio of the error.
        /// </summary>
        /// <returns>
        /// The string representatio of the error
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentUICulture, Resources.ValidationErrorToString, PropertyName, ElementPath, Message);
        }
    }
}

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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Collects information relevant to a warning or error returned by validation.
    /// </summary>    
    public class ValidationError 
    {
        private readonly ConfigurationNode invalidItem;
        private readonly string message;
        private readonly string propertyName;

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidationError"/> calss with the invalid object, property name, and error message.
        /// </summary>
        /// <param name="invalidItem">
        /// The object that did not validate.
        /// </param>
        /// <param name="propertyName">
        /// The name of the property that is invalid.
        /// </param>
        /// <param name="errorMessage">
        /// The message that describes the error.
        /// </param>
        public ValidationError(ConfigurationNode invalidItem, string propertyName, string errorMessage)
        {
			if (invalidItem == null) throw new ArgumentNullException("invalidItem");
			if (string.IsNullOrEmpty(propertyName)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "propertyName");
			if (string.IsNullOrEmpty(errorMessage)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "errorMessage");


            this.invalidItem = invalidItem;
            this.propertyName = propertyName;
            this.message = errorMessage;
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
        /// Gets or sets the invalid object that that valid validation.
        /// </summary>
        /// <value>
        /// The invalid object that that valid validation.
        /// </value>
        public ConfigurationNode InvalidItem
        {
            get { return this.invalidItem; }            
        }

        /// <summary>
        /// Returns the string representatio of the error.
        /// </summary>
        /// <returns>
        /// The string representatio of the error
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentUICulture, Resources.ValidationErrorToString, propertyName, invalidItem.ToString(), message);
        }       
    }
}

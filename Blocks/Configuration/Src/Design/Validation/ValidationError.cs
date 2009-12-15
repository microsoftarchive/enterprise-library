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
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;
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
        private readonly Property property;
        private readonly bool isWarning;

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidationError"/> calss with the invalid object, property name, and error message.
        /// </summary>
        /// <param name="property">
        /// The invalid property.
        /// </param>
        /// <param name="errorMessage">
        /// The message that describes the error.
        /// </param>
        public ValidationError(Property property, string errorMessage) : this(property, errorMessage, false)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidationError"/> calss with the invalid object, property name, and error message.
        /// </summary>
        /// <param name="property">
        /// The invalid property.
        /// </param>
        /// <param name="errorMessage">
        /// The message that describes the error.
        /// </param>
        /// <param name="isWarning">Sets the error as a warning instead of an error.</param>
        public ValidationError(Property property, string errorMessage, bool isWarning)
        {
            if (property == null) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "property");
            if (string.IsNullOrEmpty(errorMessage)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "errorMessage");

            this.isWarning = isWarning;
            this.property = property;
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
            get { return this.property.DisplayName; }
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
        public string ElementPath
        {
            get
            {
                var elementAssociation = property as IElementAssociation;
                if (elementAssociation == null) return string.Empty;

                return elementAssociation.AssociatedElement.Path;
            }
        }

        public string FriendlyPath
        {
            get
            {
                var elementAssociation = property as IElementAssociation;
                if (elementAssociation == null) return string.Empty;

                return new FriendlyElementPathBuilder(elementAssociation.AssociatedElement).ToString();
            }
        }

        public string ElementName
        {
            get
            {
                var elementAssociation = property as IElementAssociation;
                if (elementAssociation == null) return string.Empty;
                return elementAssociation.ElementName ;
            }
        }


        public bool IsError
        {
            get { return !IsWarning; }
        }

        /// <summary>
        /// Returns the string representatio of the error.
        /// </summary>
        /// <returns>
        /// The string representatio of the error
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentUICulture, Resources.ValidationErrorToString, PropertyName, ElementName, Message);
        }

        private class FriendlyElementPathBuilder
        {
            private readonly ElementViewModel element;
            private const string separator = ".";

            public FriendlyElementPathBuilder(ElementViewModel element)
            {
                this.element = element;
            }

            private string BuildPath()
            {
                var current = element;
                var pathStack = new Stack<string>();
                while(current != null)
                {
                    pathStack.Push(current.Name);
                    current = current.ParentElement;
                }

                return string.Join(separator, pathStack.ToArray());
            }

            public override string ToString()
            {
                return BuildPath();
            }
        }
    }
}

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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    ///<summary>
    /// Validation result associated with a specific <see cref="Property"/> instance.
    ///</summary>
    public class PropertyValidationResult : ValidationResult
    {
        private readonly Property property;

         /// <summary>
        /// Initialize a new instance of the <see cref="ValidationResult"/> class with the invalid object, property name, and error message.
        /// </summary>
        /// <param name="property">
        /// The invalid property.
        /// </param>
        /// <param name="errorMessage">
        /// The message that describes the result.
        /// </param>
        public PropertyValidationResult(Property property, string errorMessage) 
            : this(property, errorMessage, false)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidationResult"/> class with the invalid object, property name, and error message.
        /// </summary>
        /// <param name="property">
        /// The invalid property.
        /// </param>
        /// <param name="errorMessage">
        /// The message that describes the result.
        /// </param>
        /// <param name="isWarning">Sets the result as a warning instead of an error.</param>
        public PropertyValidationResult(Property property, string errorMessage, bool isWarning)
            :base(errorMessage, isWarning)
        {
            if (property == null) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "property");
            this.property = property;

            var propertyAsNotifyPropertyChanged = this.property as INotifyPropertyChanged;
            if (propertyAsNotifyPropertyChanged != null)
            {
                propertyAsNotifyPropertyChanged.PropertyChanged += this.PropertyChangedHandler;
            }
        }

        /// <summary>
        /// Gets the property name of the failed validation.
        /// </summary>
        /// <value>
        /// The property name of the failed validation.
        /// </value>
        public override string PropertyName
        {
            get { return this.property.DisplayName; }
        }

        ///<summary>
        /// The <see cref="ElementViewModel.ElementId"/> reference of the containing element producing the valiation error.
        ///</summary>
        public override Guid ElementId
        {
            get
            {
                var elementAssociation = property as ILogicalPropertyContainerElement;
                if (elementAssociation == null) return Guid.Empty;

                return elementAssociation.ContainingElement.ElementId;
            }
        }

        /// <summary>
        /// Gets a <see cref="string"/> that helps the user locate the <see cref="ElementViewModel"/> associates with this <see cref="ValidationResult"/>.
        /// </summary>
        /// <value>
        /// A <see cref="string"/> that helps the user locate the <see cref="ElementViewModel"/> associates with this <see cref="ValidationResult"/>.
        /// </value>
        public string FriendlyPath
        {
            get
            {
                var elementAssociation = property as ILogicalPropertyContainerElement;
                if (elementAssociation == null) return string.Empty;

                return new FriendlyElementPathBuilder(elementAssociation.ContainingElement).ToString();
            }
        }


        ///<summary>
        /// The name of the element.  If the result
        /// is not element specific, this may return
        /// <see cref="string.Empty"/>
        ///</summary>
        public override string ElementName
        {
            get
            {
                var elementAssociation = property as ILogicalPropertyContainerElement;
                if (elementAssociation == null) return string.Empty;
                return elementAssociation.ContainingElementDisplayName;
            }
        }

        private void PropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            if (string.Equals(e.PropertyName, "ContainingElementDisplayName", StringComparison.Ordinal))
            {
                OnPropertyChanged("ElementName");
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ElementValidationResult"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources. </param>
        protected override void Dispose(bool disposing)
        {
            var propertyAsNotifyPropertyChanged = this.property as INotifyPropertyChanged;
            if (propertyAsNotifyPropertyChanged != null)
            {
                propertyAsNotifyPropertyChanged.PropertyChanged -= this.PropertyChangedHandler;
            }

            base.Dispose(disposing);
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
                while (current != null)
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

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
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Collects information relevant to a warning or error returned by validation.
    /// </summary>
    /// <remarks>
    /// </remarks>
    public abstract class ValidationResult : IDisposable, INotifyPropertyChanged
    {
        private readonly string message;
        private readonly bool isWarning;

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidationResult"/> calss with the invalid object, property name, and error message.
        /// </summary>
        /// <param name="errorMessage">
        /// The message that describes the error.
        /// </param>
        protected ValidationResult(string errorMessage)
            : this(errorMessage, false)
        {
        }

        /// <summary>
        /// Initialize a new instance of the <see cref="ValidationResult"/> calss with the invalid object, property name, and error message.
        /// </summary>
        /// <param name="errorMessage">
        /// The message that describes the error.
        /// </param>
        /// <param name="isWarning">Sets the error as a warning instead of an error.</param>
        protected ValidationResult(string errorMessage, bool isWarning)
        {
            if (string.IsNullOrEmpty(errorMessage)) throw new ArgumentException(Resources.ExceptionStringNullOrEmpty, "errorMessage");

            this.isWarning = isWarning;
            this.message = errorMessage;
        }

        /// <summary>
        /// Gets the property name of the failed validation.
        /// If the validation is not related to a specific property
        /// this may return <see cref="string.Empty"/>
        /// </summary>
        /// <value>
        /// The property name of the failed validation.
        /// </value>
        public virtual string PropertyName
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets or sets the message for the error.
        /// </summary>
        /// <value>
        /// The message for the error.
        /// </value>
        public virtual string Message
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
        /// The <see cref="ElementViewModel"/> reference of the containing element producing the valiation error.
        ///</summary>
        public abstract Guid ElementId
        {
            get;
        }

        ///<summary>
        /// The name of the element.  If the error
        /// is not element specific, this may return
        /// <see cref="string.Empty"/>
        ///</summary>
        public virtual string ElementName
        {
            get { return string.Empty; }
        }


        ///<summary>
        /// Returns true if the element is an error.
        ///</summary>
        public bool IsError
        {
            get { return !IsWarning; }
        }

        /// <summary>
        /// Returns the string representation of the validation result.
        /// </summary>
        /// <returns>
        /// The string representation of the validation result.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.CurrentCulture, Resources.ValidationErrorToString, PropertyName, ElementName, Message);
        }

        #region IDisposable Members

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ValidationResult"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources. </param>
        protected virtual void Dispose(bool disposing)
        {
        }

        /// <summary>
        /// Releases all resources used by the <see cref="ValidationResult"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region INotifyPropertyChanged Members

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Raises the <see cref="PropertyChanged"/> event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }
        }

        #endregion
    }
}

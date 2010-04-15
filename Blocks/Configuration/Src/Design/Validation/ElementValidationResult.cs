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
using System.ComponentModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.Unity.Utility;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{
    /// <summary>
    /// Represents a validation result associated with an instance of <see cref="ElementViewModel"/>.
    /// </summary>
    public class ElementValidationResult : ValidationResult
    {
        private readonly ElementViewModel element;

        ///<summary>
        /// Initializes a new instance of a <see cref="ValidationResult"/> for on a <see cref="ElementViewModel"/>.
        ///</summary>
        ///<param name="element">The <see cref="ElementViewModel"/> instance this <see cref="ElementValidationResult"/> is associated with.</param>
        ///<param name="errorMessage">A message that describes the validation result.</param>
        public ElementValidationResult(ElementViewModel element, string errorMessage)
            : this(element, errorMessage, false)
        {
        }

        ///<summary>
        /// Initializes a new instance of a <see cref="ValidationResult"/> for on a <see cref="ElementViewModel"/>.
        ///</summary>
        ///<param name="element">The <see cref="ElementViewModel"/> instance this <see cref="ElementValidationResult"/> is associated with.</param>
        ///<param name="errorMessage">A message that describes the validation result.</param>
        ///<param name="isWarning"><see langword="true"/> if the validation result is a warning (and shouldn't prohibit saving the configuration); Otherwise <see langword="false"/>.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", Justification = "Validated with Guard class")]
        public ElementValidationResult(ElementViewModel element, string errorMessage, bool isWarning)
            : base(errorMessage, isWarning)
        {
            Guard.ArgumentNotNull(element, "element");

            this.element = element;

            this.element.PropertyChanged += ElementPropertyChangedHandler;
        }

        private void ElementPropertyChangedHandler(object sender, PropertyChangedEventArgs e)
        {
            OnPropertyChanged("ElementName");
        }

        ///<summary>
        /// Gets the name of the element associated with this <see cref="ValidationResult"/>.
        ///</summary>
        ///<remarks>
        ///  If the result is not element specific, this may return <see cref="string.Empty"/>.
        ///</remarks>
        ///<value>
        /// The name of the element associated with this <see cref="ValidationResult"/>.
        ///</value>
        public override string ElementName
        {
            get
            {
                return element.Name;
            }
        }

        ///<summary>
        /// Gets the <see cref="ElementViewModel.ElementId"/> of the element associated with this <see cref="ValidationResult"/>.
        ///</summary>
        /// <value>
        /// The <see cref="ElementViewModel.ElementId"/> of the element associated with this <see cref="ValidationResult"/>.
        /// </value>
        public override Guid ElementId
        {
            get
            {
                return element.ElementId;
            }
        }

        /// <summary>
        /// Releases the unmanaged resources used by the <see cref="ElementValidationResult"/> and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing"><see langword="true"/> to release both managed and unmanaged resources; <see langword="false"/> to release only unmanaged resources. </param>
        protected override void Dispose(bool disposing)
        {
            element.PropertyChanged -= ElementPropertyChangedHandler;
            base.Dispose(disposing);
        }
    }
}

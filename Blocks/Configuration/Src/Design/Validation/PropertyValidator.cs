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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation
{

    /// <summary>
    /// Base class for Validators that validate <see cref="Property"/> values in the designer.
    /// </summary>
    /// <seealso cref="Property"/>
    /// <seealso cref="ElementProperty"/>
    public abstract class PropertyValidator : Validator
    {
        /// <summary>
        /// Casts <paramref name="instance"/> to <see cref="Property"/> and calls <see cref="ValidateCore(Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.Property,string,System.Collections.Generic.IList{Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation.ValidationResult})"/> to perform the actual validation.
        /// </summary>
        /// <param name="instance">The instance of <see cref="Property"/> to validate.</param>
        /// <param name="value">Value to validate</param>
        /// <param name="results">The collection to wich any results that occur during the validation can be added.</param>		
        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var property = instance as Property;
            if (property == null) return;

            if (property.ReadOnly) return;

            ValidateCore(property, value, results);
        }

        /// <summary>
        /// When implemented in a derived class, validates <paramref name="value"/> as part of the Property <paramref name="property"/>.
        /// </summary>
        /// <param name="property">The Property that declares the <paramref name="value"/>.</param>
        /// <param name="value">Value to validate</param>
        /// <param name="results">The collection to wich any results that occur during the validation can be added.</param>		
        protected abstract void ValidateCore(Property property, string value, IList<ValidationResult> results);
    }
}

//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Validation Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.WinForms
{
    /// <summary>
    /// Returns the value from a control.
    /// </summary>
    public class PropertyMappedControlValueAccess : ValueAccess
    {
        string propertyName;

        /// <summary>
        /// Initialize a new instance of the <see cref="PropertyMappedControlValueAccess"/> class with a property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        public PropertyMappedControlValueAccess(string propertyName)
        {
            this.propertyName = propertyName;
        }

        /// <summary>
        /// Gets a hint of the location of the value relative to the object where it was retrieved from.
        /// </summary>
        public override string Key
        {
            get { return propertyName; }
        }

        /// <summary>
        /// Retrieves a value from <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The source for the value.</param>
        /// <param name="value">The value retrieved from the <paramref name="source"/>.</param>
        /// <param name="valueAccessFailureMessage">A message describing the failure to access the value, if any.</param>
        /// <returns><see langword="true"/> when the retrieval was successful; <see langword="false"/> otherwise.</returns>
        /// <remarks>Subclasses provide concrete value accessing behaviors.</remarks>
        public override bool GetValue(object source,
                                      out object value,
                                      out string valueAccessFailureMessage)
        {
            value = null;
            valueAccessFailureMessage = null;

            ValidatedControlItem validatedControlItem = source as ValidatedControlItem;

            if (validatedControlItem == null)
            {
                throw new InvalidOperationException(Resources.ExceptionValueAccessRequiresValidatedControlItem);
            }

            if (!propertyName.Equals(validatedControlItem.SourcePropertyName))
            {
                validatedControlItem = validatedControlItem.ValidationProvider.GetExistingValidatedControlItem(propertyName);
                if (validatedControlItem == null)
                {
                    throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture,
                                                                      Resources.ExceptionValueAccessPropertyNotFound,
                                                                      propertyName));
                }
            }

            return validatedControlItem.GetValue(out value, out valueAccessFailureMessage);
        }
    }
}

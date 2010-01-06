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

using System.Globalization;
using System.Web.UI.WebControls;
using Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Integration.AspNet
{
    /// <summary>
    /// Represents access to a property value.
    /// </summary>
    public class PropertyMappedValidatorValueAccess : ValueAccess
    {
        readonly string propertyName;

        /// <summary>
        /// Initialize a new instance of the <see cref="PropertyMappedValidatorValueAccess"/> class with the property name.
        /// </summary>
        /// <param name="propertyName">The property name.</param>
        public PropertyMappedValidatorValueAccess(string propertyName)
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

            PropertyProxyValidator validator = source as PropertyProxyValidator;

            if (propertyName.Equals(validator.PropertyName))
            {
                return validator.GetValue(out value, out valueAccessFailureMessage);
            }
            else
            {
                foreach (BaseValidator siblingValidator in validator.Page.Validators)
                {
                    PropertyProxyValidator siblingPropertyProxyValidator = siblingValidator as PropertyProxyValidator;

                    // the right source for the value must:
                    // - be a proxy validtor
                    // - belonging to the same naming container as the target of the validation
                    // - mapped to the required property
                    // - with matching source type name
                    if (siblingPropertyProxyValidator != null
                        && propertyName.Equals(siblingPropertyProxyValidator.PropertyName)
                        && validator.NamingContainer == siblingPropertyProxyValidator.NamingContainer
                        && (validator.SourceTypeName != null
                            && validator.SourceTypeName.Equals(siblingPropertyProxyValidator.SourceTypeName)))
                    {
                        return siblingPropertyProxyValidator.GetValue(out value, out valueAccessFailureMessage);
                    }
                }
            }

            valueAccessFailureMessage =
                string.Format(CultureInfo.CurrentCulture, Resources.ErrorNonMappedProperty, propertyName);
            return false;
        }
    }
}

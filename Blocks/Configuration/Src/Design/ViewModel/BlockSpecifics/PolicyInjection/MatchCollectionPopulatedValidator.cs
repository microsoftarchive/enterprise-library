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
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.PolicyInjection
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class MatchCollectionPopulatedValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationResult> results)
        {
            var elementProperty = property as ElementProperty;
            if (elementProperty == null) return;

            var collectionElement = elementProperty.DeclaringElement.ChildElements.Where(e => e.ConfigurationType == elementProperty.PropertyType).OfType<ElementCollectionViewModel>().FirstOrDefault();

            if (collectionElement.ChildElements.Count() == 0)
            {
                results.Add(new PropertyValidationResult(property, string.Format(CultureInfo.CurrentCulture, Resources.ValidationErrorMatchCollectionMustHaveMoreThan1Entry)));
            }
        }
    }
#pragma warning restore 1591
}

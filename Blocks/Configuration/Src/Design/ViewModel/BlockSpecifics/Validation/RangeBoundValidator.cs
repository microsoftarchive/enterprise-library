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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Properties;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Validators;

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics
{
#pragma warning disable 1591
    /// <summary>
    /// This class supports block-specific configuration design-time and is not
    /// intended to be used directly from your code.
    /// </summary>
    public class RangeBoundValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationResult> results)
        {
            var elementProperty = property as ElementProperty;
            if (elementProperty == null) return;

            var lowerBoundTypeProperty = elementProperty.DeclaringElement.Properties.Single(p => p.PropertyName == "LowerBoundType");
            var upperBoundTypeProperty = elementProperty.DeclaringElement.Properties.Single(p => p.PropertyName == "UpperBoundType");


            if (((RangeBoundaryType)lowerBoundTypeProperty.Value) == RangeBoundaryType.Ignore ||
                ((RangeBoundaryType)upperBoundTypeProperty.Value) == RangeBoundaryType.Ignore)
            {
                return;
            }

            IComparable senderBound = (IComparable)property.ConvertFromBindableValue(value);
            IComparable boundToCheck = GetOtherBoundary(elementProperty);
            bool isInvalid;
            if (property.PropertyName == "UpperBound")
            {
                isInvalid = CheckBounds(senderBound, boundToCheck);
            }
            else
            {
                isInvalid = CheckBounds(boundToCheck, senderBound);
            }

            if (isInvalid)
            {
                results.Add(new PropertyValidationResult(property, string.Format(CultureInfo.CurrentCulture, Resources.RangeBoundValidatorErrorMessage)));
            }
        }

        private static IComparable GetOtherBoundary(ElementProperty property)
        {
            string propertyToFetch = property.PropertyName == "UpperBound" ? "LowerBound" : "UpperBound";
            return (IComparable)property.DeclaringElement.Properties.Single(p => p.PropertyName == propertyToFetch).Value;
        }

        private static bool CheckBounds(IComparable upper, IComparable lower)
        {
            return lower.CompareTo(upper) > 0;
        }
    }

#pragma warning restore 1591
}

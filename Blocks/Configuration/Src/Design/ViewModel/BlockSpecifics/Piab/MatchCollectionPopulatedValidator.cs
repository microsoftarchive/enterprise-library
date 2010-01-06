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

namespace Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel.BlockSpecifics.Piab
{
    public class MatchCollectionPopulatedValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationError> errors)
        {
            var elementProperty = property as ElementProperty;
            if (elementProperty == null) return;

            var collectionElement = elementProperty.DeclaringElement.ChildElements.Where(e => e.ConfigurationType == elementProperty.Type).OfType<ElementCollectionViewModel>().FirstOrDefault();

            if (collectionElement.ChildElements.Count == 0)
            {
                errors.Add(new ValidationError(property, string.Format(CultureInfo.CurrentUICulture, "Match collection must have at least one match entry.")));
            }
        }
    }
}

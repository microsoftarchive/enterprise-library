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
using System.Linq;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;

namespace Microsoft.Practices.Unity.Configuration.Design.ViewModel
{
    public class AliasProperty : ElementProperty
    {
        public AliasProperty(IServiceProvider serviceProvider, ElementViewModel parent, PropertyDescriptor declaringProperty) 
            : base(serviceProvider, parent, declaringProperty)
        {
        }

        public override IEnumerable<Validator> GetValidators()
        {
            yield return new RequiredFieldValidator();
            yield return new AliasKeyDuplicateValidator();
        }
    }

    class AliasKeyDuplicateValidator : PropertyValidator
    {
        protected override void ValidateCore(Property property, string value, IList<ValidationResult> errors)
        {
            AliasProperty aliasProperty = property as AliasProperty;
            if (aliasProperty == null) return;

            var owningElement = aliasProperty.DeclaringElement as CollectionElementViewModel;
            if (owningElement == null) return;

            var collection = owningElement.ParentElement as ElementCollectionViewModel;
            if (collection == null) return;

            var allAliasElements =
                collection.ChildElements.Where(e => typeof (AliasElement).IsAssignableFrom(e.ConfigurationType));

            foreach (var aliasElement in allAliasElements)
            {
                if (aliasElement == owningElement) continue;

                if (string.Equals(aliasElement.Property("Alias").Value, value))
                {
                    errors.Add(new PropertyValidationResult(aliasProperty,
                                                   string.Format(CultureInfo.CurrentCulture,
                                                                 DesignResources.DuplicateAliasName, value)));
                    break;
                }
            }
        }
    }
}

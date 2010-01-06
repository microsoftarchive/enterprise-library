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
using System.Linq;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class CollectionCountOneValidator : Validator
    {
        protected override void ValidateCore(object instance, string value, IList<ValidationError> errors)
        {
            var property = instance as ElementProperty;
            if (property == null) throw new ArgumentException("Property was not ElementProperty");

            var collection = property.DeclaringElement
                .ChildElements
                .Where(e => property.DeclaringProperty.Equals(e.DeclaringProperty))
                .OfType<ElementCollectionViewModel>().Single();

            if (collection.ChildElements.Count() == 1)
            {
                errors.Add(new ValidationError(property, "CollectionHasOneItem"));
            }

        }
    }
}

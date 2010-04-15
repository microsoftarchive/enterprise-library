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
        public static readonly string Message = "CollectionHasOneItem";

        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var collection = instance as ElementCollectionViewModel;
            if (collection == null) throw new ArgumentException("instance was not ElementCollectionViewModel");

            if (collection.ChildElements.Count() == 1)
            {
                results.Add(new ElementValidationResult(collection, Message));
            }

        }
    }
}

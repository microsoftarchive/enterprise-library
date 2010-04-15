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
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.Validation;
using Microsoft.Practices.EnterpriseLibrary.Configuration.Design.ViewModel;

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class ErrorProducingValidator : Validator
    {
        public static readonly string ErrorMessage = "Test Validation Error";

        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var property = instance as ElementProperty;
            if (property == null)
            {
                throw new ArgumentException("instance was not ElementProperty");
            }

            results.Add(new PropertyValidationResult(property, ErrorMessage ));
        }
    }

    public class ElementErrorProducingValidator : Validator
    {
        public static readonly string ErrorMessage = "Test Element Validation Error";

        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            var element = instance as ElementViewModel;
            if (element == null) throw new ArgumentException("instance was not ElementViewModel");

            results.Add(new ElementValidationResult(element, ErrorMessage));
        }
    }
}

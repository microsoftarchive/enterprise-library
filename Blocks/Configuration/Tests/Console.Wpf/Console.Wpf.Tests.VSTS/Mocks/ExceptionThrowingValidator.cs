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

namespace Console.Wpf.Tests.VSTS.Mocks
{
    public class ExceptionThrowingValidator : Validator
    {
        protected override void ValidateCore(object instance, string value, IList<ValidationResult> results)
        {
            throw new Exception("SomeException");
        }
    }
}

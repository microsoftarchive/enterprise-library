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
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.Configuration
{
    public class MockValidatorData : ValidatorData
    {
        public MockValidatorData()
        { }

        public MockValidatorData(string name, bool returnFailure)
        {
            base.Name = name;
            this.ReturnFailure = returnFailure;
        }

        public bool ReturnFailure { get; set; }

        protected override Validator DoCreateValidator(Type targetType)
        {
            return new MockValidator(this.ReturnFailure);
        }
    }
}

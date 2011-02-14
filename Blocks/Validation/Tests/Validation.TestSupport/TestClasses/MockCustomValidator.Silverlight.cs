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
using System.Collections.Generic;
using System.Text;
using System.Collections.Specialized;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Validation.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
    public class MockCustomValidator : MockValidator<object>
    {
        public MockCustomValidator(Dictionary<string,string> attributes)
            : base(GetReturnFailure(attributes))
        { }

        private static bool GetReturnFailure(Dictionary<string, string> attributes)
        {
            string returnFailureString;
            if (attributes.TryGetValue("returnFailure", out returnFailureString))
            {
                return bool.Parse(returnFailureString);
            }
            return false;
        }
    }
}

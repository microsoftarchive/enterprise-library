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

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
#if !SILVERLIGHT
    [Common.Configuration.ConfigurationElementType(typeof(Configuration.MockValidatorData))]
#endif
    public class MockValidator : MockValidator<object>
    {
        public MockValidator(bool returnFailure)
            : base(returnFailure)
        { }

        public MockValidator(bool returnFailure, string messageTemplate)
            : base(returnFailure, messageTemplate)
        { }

        public MockValidator(Exception e)
            : base(e)
        {
        }
    }
}

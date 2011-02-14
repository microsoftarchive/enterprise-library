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
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.TestSupport.TestClasses
{
    public class MockValidationInstrumentationProvider : IValidationInstrumentationProvider
    {
        public void NotifyValidationSucceeded(Type typeBeingValidated)
        {
            // Intentional no-op
        }

        public void NotifyValidationFailed(Type typeBeingValidated, ValidationResults validationResult)
        {
            // Intentional no-op
        }

#if SILVERLIGHT
        public void NotifyConfigurationFailure(Exception configurationException)
#else
        public void NotifyConfigurationFailure(System.Configuration.ConfigurationErrorsException configurationException)
#endif
        {
            // Intentional no-op
        }

        public void NotifyConfigurationCalled(Type typeBeingValidated)
        {
            // Intentional no-op
        }

        public void NotifyValidationException(Type typeBeingValidated, string errorMessage, Exception exception)
        {
            // Intentional no-op
        }
    }
}

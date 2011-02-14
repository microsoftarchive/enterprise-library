using System;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation
{
    public class NullValidationInstrumentationProvider : IValidationInstrumentationProvider
    {
        public void NotifyValidationSucceeded(Type typeBeingValidated)
        {
        }

        public void NotifyValidationFailed(Type typeBeingValidated, ValidationResults validationResult)
        {
        }

        public void NotifyConfigurationCalled(Type typeBeingValidated)
        {
        }

        public void NotifyValidationException(Type typeBeingValidated, string errorMessage, Exception exception)
        {
        }

        public void NotifyConfigurationFailure(Exception configurationException)
        {
        }
    }
}

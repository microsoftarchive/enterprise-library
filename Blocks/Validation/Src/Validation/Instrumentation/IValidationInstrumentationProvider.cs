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
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation
{
    ///<summary>
    /// Describes the logical notifications raised by various validation classes to which providers will respond.
    ///</summary>
    public interface IValidationInstrumentationProvider
    {
        ///<summary>
        /// Notifies provider that a validation has succeeded.
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        void NotifyValidationSucceeded(Type typeBeingValidated);

        ///<summary>
        /// Notifies provider that validation has failed.
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        ///<param name="validationResult"></param>
        void NotifyValidationFailed(Type typeBeingValidated, ValidationResults validationResult);

        ///<summary>
        /// Notifies provider that configuration for validation has failed.
        ///</summary>
        ///<param name="configurationException"></param>
#if SILVERLIGHT     //TODO: is this separation needed? Can Desktop use Exception instead?
        void NotifyConfigurationFailure(Exception configurationException);
#else
        void NotifyConfigurationFailure(ConfigurationErrorsException configurationException);
#endif

        ///<summary>
        /// Notifies provider that a configuration based validation has been called.
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        void NotifyConfigurationCalled(Type typeBeingValidated);

        ///<summary>
        /// Notifies provider of a validation exception.
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        ///<param name="errorMessage"></param>
        ///<param name="exception"></param>
        void NotifyValidationException(Type typeBeingValidated, string errorMessage, Exception exception);
    }
}

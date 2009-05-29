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
using System.Configuration;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation
{
    ///<summary>    
    ///</summary>
    public interface IValidationInstrumentationProvider
    {
        ///<summary>
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        void FireValidationSucceeded(Type typeBeingValidated);

        ///<summary>
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        ///<param name="validationResult"></param>
        void FireValidationFailed(Type typeBeingValidated, ValidationResults validationResult);

        ///<summary>
        ///</summary>
        ///<param name="configurationException"></param>
        void FireConfigurationFailure(ConfigurationErrorsException configurationException);

        ///<summary>
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        void FireConfigurationCalled(Type typeBeingValidated);

        ///<summary>
        ///</summary>
        ///<param name="typeBeingValidated"></param>
        ///<param name="errorMessage"></param>
        ///<param name="exception"></param>
        void FireValidationException(Type typeBeingValidated, string errorMessage, Exception exception);
    }
}

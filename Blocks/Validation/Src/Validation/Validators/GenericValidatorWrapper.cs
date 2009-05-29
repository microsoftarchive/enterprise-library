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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
    /// <summary>
    /// Used to provide a generic API over the unknown validators.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class GenericValidatorWrapper<T> : Validator<T>
    {
        private readonly IValidationInstrumentationProvider instrumentationProvider;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="wrappedValidator"></param>
        /// <param name="instrumentationProvider"></param>
        public GenericValidatorWrapper(Validator wrappedValidator, IValidationInstrumentationProvider instrumentationProvider)
            : base(null, null)
        {
            this.WrappedValidator = wrappedValidator;
            this.instrumentationProvider = instrumentationProvider;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="objectToValidate"></param>
        /// <param name="currentTarget"></param>
        /// <param name="key"></param>
        /// <param name="validationResults"></param>
        protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            Type typeBeingValidated = typeof(T);

            instrumentationProvider.FireConfigurationCalled(typeBeingValidated);

            try
            {
                this.WrappedValidator.DoValidate(objectToValidate, currentTarget, key, validationResults);

                if (validationResults.IsValid)
                {
                    instrumentationProvider.FireValidationSucceeded(typeBeingValidated);
                }
                else
                {
                    instrumentationProvider.FireValidationFailed(typeBeingValidated, validationResults);
                }
            }
            catch (ConfigurationErrorsException configurationErrors)
            {
                instrumentationProvider.FireConfigurationFailure(configurationErrors);
                throw;
            }
            catch (Exception ex)
            {
                instrumentationProvider.FireValidationException(typeBeingValidated, ex.Message, ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the message template to use when logging results no message is supplied.
        /// </summary>
        protected override string DefaultMessageTemplate
        {
            get { return null; }
        }

        ///<summary>
        /// Returns the validator wrapped by <see cref="GenericValidatorWrapper{T}"/>
        ///</summary>
        public Validator WrappedValidator { get; private set; }

      
    }
}

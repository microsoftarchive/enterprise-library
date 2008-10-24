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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
    /// <summary>
    /// Validator that wraps other validators and adds instrumentation support.
    /// </summary>
    public sealed class ValidatorWrapper : Validator, IInstrumentationEventProvider
    {
        private ValidationInstrumentationProvider instrumentationProvider;
        private Validator wrappedValidator;

		/// <summary>
		/// <para>Initializes a new instance of the <see cref="ValidatorWrapper"/>.</para>
		/// </summary>
		/// <param name="wrappedValidator">The wrapped <see cref="Validator"/> providing the actual validation.</param>
        public ValidatorWrapper(Validator wrappedValidator)
            :base(null, null)
        {
            this.wrappedValidator = wrappedValidator;
            this.instrumentationProvider = new ValidationInstrumentationProvider();
        }

		/// <summary>
		/// Invokes the validation logic from the wrapped <see cref="Validator"/> and updates the instrumentation informamtion.
		/// </summary>
		/// <param name="objectToValidate">The object to validate.</param>
		/// <param name="currentTarget">The object on the behalf of which the validation is performed.</param>
		/// <param name="key">The key that identifies the source of <paramref name="objectToValidate"/>.</param>
		/// <param name="validationResults">The validation results to which the outcome of the validation should be stored.</param>
		protected internal override void DoValidate(object objectToValidate, object currentTarget, string key, ValidationResults validationResults)
        {
            Type typeBeingValidated = (objectToValidate == null) ? typeof(object) : objectToValidate.GetType();

            instrumentationProvider.FireConfigurationCalledEvent(typeBeingValidated);

            try
            {
                this.wrappedValidator.DoValidate(objectToValidate, currentTarget, key, validationResults);

                if (validationResults.IsValid)
                {
                    instrumentationProvider.FireValidationSucceededEvent(typeBeingValidated);
                }
                else
                {
                    instrumentationProvider.FireValidationFailedEvent(typeBeingValidated, validationResults);
                }
            }
            catch (ConfigurationErrorsException configurationErrors)
            {
                instrumentationProvider.FireConfigurationFailureEvent(configurationErrors);
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
		/// <remarks>
		/// This validator does not issue any messages of its own, so the default message template is <see langword="null"/>.
		/// </remarks>
		protected override string DefaultMessageTemplate
        {
            get { return null; }
        }

        /// <summary>
        /// Only for testing purposes
        /// </summary>
        internal Validator WrappedValidator
        {
            get
            {
                return this.wrappedValidator;
            }
        }

        #region IInstrumentationEventProvider Members
        
        /// <summary>
        /// Returns the object that provides instrumentation services for the <see cref="Validator"/>.
        /// </summary>
        /// <see cref="IInstrumentationEventProvider.GetInstrumentationEventProvider()"/>
        /// <returns>The object that providers intrumentation services. This object may be null if instrumentation services are not created for this instance.</returns>
        public object GetInstrumentationEventProvider()
        {
            return instrumentationProvider;
        }

        #endregion
    }
}

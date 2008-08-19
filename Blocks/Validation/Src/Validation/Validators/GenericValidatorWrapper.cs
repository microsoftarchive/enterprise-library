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
using Microsoft.Practices.EnterpriseLibrary.Common.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.Validation.Instrumentation;
using System.Configuration;

namespace Microsoft.Practices.EnterpriseLibrary.Validation.Validators
{
	/// <summary>
	/// Used to provide a generic API over the unknown validators.
	/// </summary>
	/// <typeparam name="T"></typeparam>
    internal sealed class GenericValidatorWrapper<T> : Validator<T>, IInstrumentationEventProvider
    {
        private ValidationInstrumentationProvider instrumentationProvider;
		private Validator wrappedValidator;

		public GenericValidatorWrapper(Validator wrappedValidator)
			: base(null, null)
		{
			this.wrappedValidator = wrappedValidator;
            this.instrumentationProvider = new ValidationInstrumentationProvider();
		}

		protected override void DoValidate(T objectToValidate, object currentTarget, string key, ValidationResults validationResults)
		{
            Type typeBeingValidated = typeof(T);

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

		protected override string DefaultMessageTemplate
		{
			get { return null; }
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
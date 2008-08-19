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
	/// <summary>
	/// Defines the logical events that can be instrumented for the validation block.
	/// </summary>
	/// <remarks>
	/// The concrete instrumentation is provided by an object bound to the events of the provider. 
	/// The default listener, automatically bound during construction, is <see cref="ValidationInstrumentationListener"/>.
	/// </remarks>
	[InstrumentationListener(typeof(ValidationInstrumentationListener))]
	public class ValidationInstrumentationProvider
	{
		/// <summary>
		/// Occurs when a validation operation is performed.
		/// </summary>
		[InstrumentationProvider("ValidateCalled")]
		public event EventHandler<ValidationEventArgs> validationCalled;

		/// <summary>
		/// Occurs when a validation operation is successful.
		/// </summary>
		[InstrumentationProvider("ValidateSuccess")]
		public event EventHandler<ValidationEventArgs> validationSucceeded;

		/// <summary>
		/// Occurs when a validation operation fails.
		/// </summary>
		[InstrumentationProvider("ValidateFailure")]
		public event EventHandler<ValidationFailedEventArgs> validationFailed;

		/// <summary>
		/// Occurs when an exception is thrown while performing a validation operation.
		/// </summary>
		[InstrumentationProvider("ValidateException")]
		public event EventHandler<ValidationExceptionEventArgs> validationException;

		/// <summary>
		/// Occurs when a failure is detected when accessing the configuration settings for the validation block.
		/// </summary>
		[InstrumentationProvider("ConfigurationFailure")]
		public event EventHandler<ValidationConfigurationFailureEventArgs> configurationFailure;

		internal void FireValidationSucceededEvent(Type typeBeingValidated)
		{
			if (validationSucceeded != null) validationSucceeded(this, new ValidationEventArgs(typeBeingValidated));
		}

		internal void FireValidationFailedEvent(Type typeBeingValidated, ValidationResults validationResult)
		{
			if (validationFailed != null) validationFailed(this, new ValidationFailedEventArgs(typeBeingValidated, validationResult));
		}

		internal void FireConfigurationFailureEvent(ConfigurationErrorsException configurationException)
		{
			if (configurationFailure != null) configurationFailure(this, new ValidationConfigurationFailureEventArgs(configurationException));
		}

		internal void FireConfigurationCalledEvent(Type typeBeingValidated)
		{
			if (validationCalled != null) validationCalled(this, new ValidationEventArgs(typeBeingValidated));
		}

		internal void FireValidationException(Type typeBeingValidated, string errorMessage, Exception exception)
		{
			if (validationException != null) validationException(this, new ValidationExceptionEventArgs(typeBeingValidated, errorMessage, exception));
			if (validationCalled != null) validationCalled(this, new ValidationEventArgs(typeBeingValidated));
		}
	}

	/// <summary>
	/// Base class for validation events.
	/// </summary>
	public class ValidationEventArgs : EventArgs
	{
		Type typeBeingValidated;

		internal ValidationEventArgs(Type typeBeingValidated)
		{
			this.typeBeingValidated = typeBeingValidated;
		}

		/// <summary>
		/// Gets the type being validated.
		/// </summary>
		public Type TypeBeingValidated
		{
			get { return typeBeingValidated; }
		}
	}

	/// <summary>
	/// Provides data for the <see cref="ValidationInstrumentationProvider.validationFailed"/> event.
	/// </summary>
	public class ValidationFailedEventArgs : ValidationEventArgs
	{
		private ValidationResults validationResult;

		internal ValidationFailedEventArgs(Type typeBeingValidated, ValidationResults validationResult)
			: base(typeBeingValidated)
		{
			this.validationResult = validationResult;
		}

		/// <summary>
		/// Gets the result of the failed validation.
		/// </summary>
		public ValidationResults ValidationResult
		{
			get { return validationResult; }
		}
	}

	/// <summary>
	/// Provides data for the <see cref="ValidationInstrumentationProvider.configurationFailure"/> event.
	/// </summary>
	public class ValidationConfigurationFailureEventArgs : EventArgs
	{
		private Exception exception;

		internal ValidationConfigurationFailureEventArgs(Exception exception)
		{
			this.exception = exception;
		}

		/// <summary>
		/// Gets the exception that describes the configuration error.
		/// </summary>
		public Exception Exception
		{
			get { return exception; }
		}
	}

	/// <summary>
	/// Provides data for the <see cref="ValidationInstrumentationProvider.validationException"/> event.
	/// </summary>
	public class ValidationExceptionEventArgs : ValidationEventArgs
	{
		private string errorMessage;
		private Exception exception;

		internal ValidationExceptionEventArgs(Type typeBeingValidated, string errorMessage, Exception exception)
			: base(typeBeingValidated)
		{
			this.errorMessage = errorMessage;
			this.exception = exception;
		}

		/// <summary>
		/// Gets the message that describes the failure.
		/// </summary>
		public string ErrorMessage
		{
			get { return errorMessage; }
		}

		/// <summary>
		/// Gets the exception that caused the failure.
		/// </summary>
		public Exception Exception
		{
			get { return exception; }
		}
	}
}
//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation. All rights reserved.
// Adapted from ACA.NET with permission from Avanade Inc.
// ACA.NET copyright © Avanade Inc. All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections;
using System.Collections.Generic;
using Microsoft.Practices.EnterpriseLibrary.Common.Configuration.ObjectBuilder;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
	/// <summary>
	/// Represents an entry in an <see cref="ExceptionPolicy"/> containing
	/// an exception type as the key and a list of 
	/// <see cref="IExceptionHandler"/> objects as the value.
	/// </summary>
	public sealed class ExceptionPolicyEntry
	{
		private PostHandlingAction postHandlingAction;
		private ICollection<IExceptionHandler> handlers;
		private string policyName = string.Empty;
		private ExceptionHandlingInstrumentationProvider instrumentationProvider;

		/// <summary>
		/// Instantiates a new instance of the 
		/// <see cref="ExceptionPolicyEntry"/> class.
		/// </summary>
		public ExceptionPolicyEntry(PostHandlingAction postHandlingAction, ICollection<IExceptionHandler> handlers)
		{
			if (handlers == null) throw new ArgumentNullException("handlers");

			this.postHandlingAction = postHandlingAction;
			this.handlers = handlers;
		}

		internal string PolicyName
		{
			set { policyName = value; }
		}

		private ExceptionHandlingInstrumentationProvider InstrumentationProvider
		{
			get { return instrumentationProvider; }
			set { instrumentationProvider = value; }
		}

		/// <summary>
		/// Handles all exceptions in the chain.
		/// </summary>
		/// <param name="exceptionToHandle">The <c>Exception</c> to handle.</param>
		/// <returns>Whether or not a rethrow is recommended.</returns>
		public bool Handle(Exception exceptionToHandle)
		{
			if (exceptionToHandle == null) throw new ArgumentNullException("exceptionToHandler");

			Guid handlingInstanceID = Guid.NewGuid();
			Exception chainException = ExecuteHandlerChain(exceptionToHandle, handlingInstanceID);

			if (InstrumentationProvider != null) InstrumentationProvider.FireExceptionHandledEvent();

			return RethrowRecommended(chainException, exceptionToHandle);
		}

		/// <devdoc>
		/// Rethrows the given exception.  Placed in a seperate method for
		/// easier viewing in the stack trace.
		/// </devdoc>
		private Exception IntentionalRethrow(Exception chainException, Exception originalException)
		{
			if (chainException != null)
			{
				throw chainException;
			}

			Exception wrappedException = new ExceptionHandlingException(Resources.ExceptionNullException);
			if (InstrumentationProvider != null)
			{
				InstrumentationProvider.FireExceptionHandlingErrorOccurred(ExceptionUtility.FormatExceptionHandlingExceptionMessage(policyName, wrappedException, chainException, originalException));
			}

			return wrappedException;
		}

		private bool RethrowRecommended(Exception chainException, Exception originalException)
		{
			if (postHandlingAction == PostHandlingAction.None) return false;

			if (postHandlingAction == PostHandlingAction.ThrowNewException)
			{
				throw IntentionalRethrow(chainException, originalException);
			}
			return true;
		}

		private Exception ExecuteHandlerChain(Exception ex, Guid handlingInstanceID)
		{
			string lastHandlerName = String.Empty;
			Exception originalException = ex;

			try
			{
				foreach (IExceptionHandler handler in handlers)
				{
					lastHandlerName = handler.GetType().Name;
					ex = handler.HandleException(ex, handlingInstanceID);
					if (InstrumentationProvider != null) InstrumentationProvider.FireExceptionHandlerExecutedEvent();
				}
			}
			catch (Exception handlingException)
			{
				if (InstrumentationProvider != null)
				{
					InstrumentationProvider.FireExceptionHandlingErrorOccurred(
						ExceptionUtility.FormatExceptionHandlingExceptionMessage(
							policyName,
							new ExceptionHandlingException(string.Format(Resources.Culture, Resources.UnableToHandleException, lastHandlerName), handlingException),
							ex,
							originalException
						));
				}
				throw new ExceptionHandlingException(string.Format(Resources.Culture, Resources.UnableToHandleException, lastHandlerName));
			}

			return ex;
		}

		/// <summary>
        /// Attaches an <see cref="ExceptionHandlingInstrumentationProvider"/> to be used for instrumentation on this instance.
		/// </summary>
        /// <param name="provider">The <see cref="ExceptionHandlingInstrumentationProvider"/> that is attached.</param>
		public void SetInstrumentationProvider(ExceptionHandlingInstrumentationProvider provider)
		{
			this.InstrumentationProvider = provider;
		}
	}
}

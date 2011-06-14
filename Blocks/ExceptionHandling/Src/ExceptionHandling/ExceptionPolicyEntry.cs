//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Exception Handling Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Instrumentation;
using Microsoft.Practices.EnterpriseLibrary.ExceptionHandling.Properties;

namespace Microsoft.Practices.EnterpriseLibrary.ExceptionHandling
{
    /// <summary>
    /// Represents an entry in an exception policy containing
    /// an exception type as the key and a list of 
    /// <see cref="IExceptionHandler"/> objects as the value.
    /// </summary>
    public sealed class ExceptionPolicyEntry
    {
        private readonly PostHandlingAction postHandlingAction;
        private readonly IEnumerable<IExceptionHandler> handlers;
        private string policyName = string.Empty;
        private IExceptionHandlingInstrumentationProvider instrumentationProvider;

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ExceptionPolicyEntry"/> class.
        /// </summary>
        /// <param name="exceptionType">Type of exception this policy refers to.</param>
        /// <param name="postHandlingAction">What to do after the exception is handled.</param>
        /// <param name="handlers">Handlers to execute on the exception.</param>
        public ExceptionPolicyEntry(Type exceptionType, PostHandlingAction postHandlingAction, IEnumerable<IExceptionHandler> handlers)
            : this(exceptionType, postHandlingAction, handlers, new NullExceptionHandlingInstrumentationProvider())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionPolicyEntry"/> class.
        /// </summary>
        /// <param name="exceptionType">Type of exception this policy refers to.</param>
        /// <param name="postHandlingAction">What to do after the exception is handled.</param>
        /// <param name="handlers">Handlers to execute on the exception.</param>
        /// <param name="instrumentationProvider">Instrumentation provider</param>
        public ExceptionPolicyEntry(
            Type exceptionType,
            PostHandlingAction postHandlingAction,
            IEnumerable<IExceptionHandler> handlers,
            IExceptionHandlingInstrumentationProvider instrumentationProvider)
        {
            if (exceptionType == null) throw new ArgumentNullException("exceptionType");
            if (handlers == null) throw new ArgumentNullException("handlers");
            if (instrumentationProvider == null) throw new ArgumentNullException("instrumentationProvider");

            ExceptionType = exceptionType;
            this.postHandlingAction = postHandlingAction;
            this.handlers = handlers;
            this.instrumentationProvider = instrumentationProvider;
        }

        internal string PolicyName
        {
            set { policyName = value; }
        }

        ///<summary>
        /// The type of <see cref="Exception"/> to match this policy entry to.
        ///</summary>
        public Type ExceptionType { get; private set; }

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

            instrumentationProvider.FireExceptionHandledEvent();

            return RethrowRecommended(chainException, exceptionToHandle);
        }

        /// <devdoc>
        /// Rethrows the given exception.  Placed in a separate method for
        /// easier viewing in the stack trace.
        /// </devdoc>
        private Exception IntentionalRethrow(Exception chainException, Exception originalException)
        {
            if (chainException != null)
            {
                throw chainException;
            }

            Exception wrappedException = new ExceptionHandlingException(Resources.ExceptionNullException);
            instrumentationProvider.FireExceptionHandlingErrorOccurred(
                ExceptionUtility.FormatExceptionHandlingExceptionMessage(policyName, wrappedException, chainException, originalException));

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
                    instrumentationProvider.FireExceptionHandlerExecutedEvent();
                }
            }
            catch (Exception handlingException)
            {
                instrumentationProvider.FireExceptionHandlingErrorOccurred(
                    ExceptionUtility.FormatExceptionHandlingExceptionMessage(
                        policyName,
                        new ExceptionHandlingException(string.Format(CultureInfo.CurrentCulture, Resources.UnableToHandleException, lastHandlerName), handlingException),
                        ex,
                        originalException));
                throw new ExceptionHandlingException(string.Format(CultureInfo.CurrentCulture, Resources.UnableToHandleException, lastHandlerName));
            }

            return ex;
        }
    }
}
